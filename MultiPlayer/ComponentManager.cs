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
        public List<IStartable> StartableComponents { get; private set; }
        public List<IDrawable> DrawableComponents { get; private set; }

        public ComponentManager()
        {
            Components = new Dictionary<Type, LinkedList<T>>();
            UpdateableComponents = new List<IUpdateable>();
            StartableComponents = new List<IStartable>();
            DrawableComponents = new List<IDrawable>();
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
            if (component is IStartable)
            {
                StartableComponents.Add(component as IStartable);

                if (started)
                    (component as IStartable).Start();
            }
            if (component is IDrawable)
                DrawableComponents.Add(component as IDrawable);
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
            var toRemove = new List<int>();

            for (var i = 0; i < UpdateableComponents.Count; i++)
            {
                var updateableComponent = UpdateableComponents[i];
                updateableComponent.Update(step);

                if (updateableComponent is IDestroyable)
                {
                    var destroyable = updateableComponent as IDestroyable;
                    if (destroyable.ShouldRemove)
                        toRemove.Add(i);
                }
            }

            for (var i = toRemove.Count - 1; i >= 0; --i)
            {
                var removeAt = toRemove[i];

                var component = UpdateableComponents[removeAt];
                (component as IHearsDestroy)?.OnDestroy();

                UpdateableComponents.RemoveAt(removeAt);

                //TODO improve this. It's not as nice as I'd like
                if (component is IDrawable)
                    DrawableComponents.Remove(component as IDrawable);
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
            DrawableComponents.ForEach(d => d.Draw());
        }

        public object ParentObject { get; set; }
    }

    public class ComponentManager : ComponentManager<object>
    {

    }
}
