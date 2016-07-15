using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;
using MultiPlayer.Core;

namespace Editor.Actions
{
    public class ActionManager
    {
        private readonly Stack<IEditorAction> undoStack = new Stack<IEditorAction>();
        private readonly Stack<IEditorAction> redoStack = new Stack<IEditorAction>();

        private readonly MapInfo mapInfo;
        private readonly GameObject scene;
        private readonly PrefabFactory prefabs;

        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;

        public ActionManager(MapInfo mapInfo, GameObject scene, PrefabFactory prefabs)
        {
            this.mapInfo = mapInfo;
            this.scene = scene;
            this.prefabs = prefabs;
        }

        internal void Do(IEditorAction action)
        {
            if (action is IKnowsMapInfo)
                (action as IKnowsMapInfo).Info = mapInfo;
            if (action is IKnowsScene)
                (action as IKnowsScene).Scene = scene;
            if (action is IKnowsPrefabManager)
                (action as IKnowsPrefabManager).Prefabs = prefabs;

            action.Do();
            undoStack.Push(action);

            redoStack.Clear();
        }

        internal void Undo()
        {
            if (!CanUndo) throw new InvalidOperationException("Nothing to undo!");

            var action = undoStack.Pop();
            action.Undo();

            redoStack.Push(action);
        }

        internal void Redo()
        {
            if (!CanRedo) throw new InvalidOperationException("Nothing to undo!");

            var action = redoStack.Pop();
            action.Do();

            undoStack.Push(action);
        }
    }
}
