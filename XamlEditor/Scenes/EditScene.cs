using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.Core.InputMethods;
using Runner;
using XamlEditor.Annotations;
using XamlEditor.GameComponents;

namespace XamlEditor.Scenes
{
    public class EditScene : Scene, INotifyPropertyChanged
    {
        public GameObject EditRoot
        {
            get { return editRoot; }
            private set
            {
                if (Equals(value, editRoot)) return;
                editRoot = value;
                OnPropertyChanged();
            }
        }

        public GameObject SceneRoot
        {
            get { return sceneRoot; }
            private set
            {
                if (Equals(value, sceneRoot)) return;
                sceneRoot = value;
                OnPropertyChanged();
            }
        }

        public UpdateNotifier UpdateNotifier;
        private GameObject editRoot;
        private GameObject sceneRoot;

        private GameObject selectedGameObject;
        private readonly Dictionary<GameObject, GameObject> sceneToEditMap = new Dictionary<GameObject, GameObject>();

        public event ChildEvent ChildAdded;
        public event ScriptEvent ScriptAdded;
        public event ChildEvent ChildRemoved;
        public event ScriptEvent ScriptRemoved;

        public GameObject AddGameObject(string prefabName, GameObject parent = null)
        {
            if (parent == null)
                parent = SceneRoot;

            var gameObject = PrefabFactory.Build(prefabName);
            return AddGameObject(gameObject, parent);
        }

        public GameObject AddGameObject(GameObject gameObject, GameObject parent = null)
        {
            if (parent == null)
                parent = SceneRoot;

            gameObject.DelayedAdd(UpdateNotifier);

            var editNode = BuildEditNode(gameObject);

            sceneToEditMap.Add(gameObject, editNode);

            parent.DelayedAdd(gameObject);
            EditRoot.DelayedAdd(editNode);

            return gameObject;
        }

        private GameObject BuildEditNode(GameObject toEdit)
        {
            var dragger = new Dragger()
            {
                Drag = toEdit,
            };

            return GameObjectFactory
                .New()
                .With(new StickTo()
                {
                    To = toEdit
                })
                .With(new EditSelector())
                .With(new Sprite()
                {
                    Texture = TextureUtil.CreateTexture(32, 32, Color.Red)
                })
                .With(dragger)
                .Create();
        }

        public void SelectGameObject(GameObject gameObject)
        {
            selectedGameObject?.GetComponent<EditSelector>()?.Deselect();

            var editNode = sceneToEditMap[gameObject];
            editNode.GetComponent<EditSelector>()?.Select();
            selectedGameObject = editNode;
        }

        public EditScene(IMouse mouse, IKeyboard keyboard)
            : base(mouse, keyboard)
        {
            UpdateNotifier = new UpdateNotifier();
            UpdateNotifier.ChildAdded += ChildAdded;
            UpdateNotifier.ChildRemoved += ChildRemoved;
            UpdateNotifier.ScriptAdded += ScriptAdded;
            UpdateNotifier.ScriptRemoved += ScriptRemoved;
        }

        public override void Start()
        {
            base.Start();

            PrefabInitializer.AddRunnerGamePrefabs(PrefabFactory);

            SceneRoot = GameObjectFactory.New().With(UpdateNotifier).Create();

            EditRoot = BuildEditNode(SceneRoot);

            Add(SceneRoot);
            Add(EditRoot);

            //var platform = PrefabFactory.Instantiate("platform");
            //platform.Transform.Position = new Vector2();
            //platform.Transform.Scale = new Vector2(5, 3);

            //SceneRoot.DelayedAdd(platform);
            AddGameObject("platform");
            AddGameObject(GameObjectFactory
                .New()
                .WithChild(
                    GameObjectFactory
                    .New()
                    .Create()
                )
                .Create());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
