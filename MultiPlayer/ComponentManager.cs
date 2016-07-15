using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiPlayer.GameComponents;

namespace MultiPlayer
{
    public class ComponentManager<T> : IUpdateable, IStartable, IDrawable, IKnowsParent
    {
        private bool started;

        private List<T> delayedAdd = new List<T>(); 

        public Dictionary<Type, LinkedList<T>> Components { get; private set; } 
        public List<IUpdateable> UpdateableComponents { get; private set; }
        public List<ILateUpdateable> LateUpdateableComponents { get; private set; }
        public List<IStartable> StartableComponents { get; private set; }
        public List<IDestroyable> DestroyableComponents { get; private set; }
        public List<IDrawable> DrawableComponents { get; private set; }

        public ComponentManager()
        {
            Components = new Dictionary<Type, LinkedList<T>>();
            UpdateableComponents = new List<IUpdateable>();
            LateUpdateableComponents = new List<ILateUpdateable>();
            StartableComponents = new List<IStartable>();
            DrawableComponents = new List<IDrawable>();
            DestroyableComponents = new List<IDestroyable>();
        }

        public void DelayedAdd(T component)
        {
            if (started)
                delayedAdd.Add(component);
            else Add(component);
        }

        protected virtual void Add(T component)
        {
            if (!Components.ContainsKey(component.GetType()))
                Components.Add(component.GetType(), new LinkedList<T>());

            Components[component.GetType()].AddLast(component);

            if (component is IKnowsParent)
                (component as IKnowsParent).ParentObject = this;

            if (component is IUpdateable)
                UpdateableComponents.Add(component as IUpdateable);

            if (component is ILateUpdateable)
                LateUpdateableComponents.Add(component as ILateUpdateable);

            if (component is IStartable)
            {
                StartableComponents.Add(component as IStartable);

                if (started)
                    (component as IStartable).Start();
            }
            if (component is IDrawable)
                DrawableComponents.Add(component as IDrawable);
            if (component is IDestroyable)
            {
                var destroyable = component as IDestroyable;

                //Reset the should remove flag in case this object has been removed from somewhere.
                destroyable.ShouldRemove = false;
                DestroyableComponents.Add(destroyable);
            }
        }

        public IEnumerable<K> GetComponents<K>() where K : T
        {
            var t = typeof (K);
            var result = Components.ContainsKey(t) ? Components[t] : new LinkedList<T>();
            return from component in result select (K) component;
        }

        public K GetComponent<K>() where K : T
        {
            return GetComponents<K>().FirstOrDefault();
        }

        public void Update(float step)
        {
            var toRemove = new List<IDestroyable>();

            for (var i = 0; i < UpdateableComponents.Count; i++)
            {
                var updateableComponent = UpdateableComponents[i];
                updateableComponent.Update(step);

                if (updateableComponent is IDestroyable)
                {
                    var destroyable = updateableComponent as IDestroyable;
                    if (destroyable.ShouldRemove)
                        UpdateableComponents.RemoveAt(i--);
                }
            }

            for (var i = 0; i < LateUpdateableComponents.Count; i++)
            {
                var updateableComponent = LateUpdateableComponents[i];
                updateableComponent.LateUpdate(step);

                if (updateableComponent is IDestroyable)
                {
                    var destroyable = updateableComponent as IDestroyable;
                    if (destroyable.ShouldRemove)
                    {
                        LateUpdateableComponents.RemoveAt(i--);
                    }
                }
            }

            for (var i = 0; i < DestroyableComponents.Count; ++i)
            {
                var destroyable = DestroyableComponents[i];
                if (destroyable.ShouldRemove)
                {
                    DestroyableComponents.RemoveAt(i--);
                    toRemove.Add(destroyable);
                }
            }

            for (var i = toRemove.Count - 1; i >= 0; --i)
            {
                var remove = toRemove[i];
                
                (remove as IHearsDestroy)?.OnDestroy();

                //TODO improve this. It's not as nice as I'd like
                if (remove is IDrawable)
                    DrawableComponents.Remove(remove as IDrawable);
            }

            delayedAdd.ForEach(Add);
            delayedAdd.Clear();
        }

        public virtual void Start()
        {
            StartableComponents.ForEach(c => c.Start());

            started = true;
        }

        public virtual void Draw()
        {
            if (!Visible) return;

            DrawableComponents.ForEach(d => d.Draw());
        }

        public object ParentObject { get; set; }


        /// <summary>
        /// Indicates whether the GameObject should be rendered
        /// </summary>
        public bool Visible { get; set; } = true;
    }

    public class ComponentManager : ComponentManager<object>
    {

    }
}
