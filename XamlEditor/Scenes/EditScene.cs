using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.InputMethods;
using SharpDX.Direct3D9;
using XamlEditor.Annotations;
using XamlEditor.Scene.Components;
using XamlEditor.Scene.Messages;
using XamlEditor.Scene.Systems;

namespace XamlEditor.Scenes
{
    public class EditScene : MultiPlayer.Scene, INotifyPropertyChanged
    {
        public UpdateNotifier UpdateNotifier;

        public event ChildEvent ChildAdded;
        public event ComponentEvent ComponentAdded;
        public event ChildEvent ComponentRemoved;
        public event ComponentEvent ScriptRemoved;

        public Entity AddEntity(Entity entity, Transform parent = null)
        {
            if (!entity.HasComponent<Transform>())
                entity.Add(new Transform());

            entity.Get<Transform>().Parent = parent;

            Engine.AddEntity(entity);

            return entity;
        }

        public void SelectGameObject(Entity entity)
        {
            this.Engine.MessageHub.SendMessage(new SelectMessage(entity));
        }

        public EditScene(IMouse mouse, IKeyboard keyboard)
            : base(mouse, keyboard)
        {
            UpdateNotifier = new UpdateNotifier();
            UpdateNotifier.ChildAdded += ChildAdded;
            UpdateNotifier.ChildRemoved += ComponentRemoved;
            UpdateNotifier.ComponentAdded += ComponentAdded;
            UpdateNotifier.ComponentRemoved += ScriptRemoved;
        }

        public override void Start()
        {
            base.Start();

            Engine.AddSystem(new UpdateNotifier());
            Engine.AddSystem(new DraggerSystem());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
