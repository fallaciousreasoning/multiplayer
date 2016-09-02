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
using MultiPlayer.Core.Systems;
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
        public event ChildEvent ChildRemoved;
        public event ComponentEvent ComponentRemoved;
        public event ComponentEvent ComponentChanged;

        public Entity AddEntity(Entity entity, Transform parent = null)
        {
            entity.FriendlyName = entity.FriendlyName ?? "GameObject";

            if (!entity.HasComponent<Transform>())
                entity.Add(new Transform());


            entity.Get<Transform>().Parent = parent;

            entity.Add(new Dragger());
            entity.Add(new TransformWatcher());

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
            UpdateNotifier.ChildRemoved += ChildRemoved;
            UpdateNotifier.ComponentAdded += ComponentAdded;
            UpdateNotifier.ComponentRemoved += ComponentRemoved;
            UpdateNotifier.ComponentChanged += ComponentChanged;
        }

        public override void Start()
        {
            base.Start();

            Engine.AddSystem(new SpriteRenderer());
            Engine.AddSystem(new CameraSystem());

            Engine.AddSystem(UpdateNotifier);
            Engine.AddSystem(new DraggerSystem());
            Engine.AddSystem(new TransformWatcherSystem());

            var test1 = new Entity();
            test1.Add(new Transform());
            test1.Add(new MultiPlayer.Core.Components.Sprite() { Texture = TextureUtil.CreateTexture(64, 64, Color.Black)});

            var camera = new Entity();
            camera.Add(new Camera());
            camera.Add(new Transform());

            AddEntity(test1);
            AddEntity(camera);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
