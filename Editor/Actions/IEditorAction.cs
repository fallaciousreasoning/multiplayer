namespace Editor.Actions
{
    internal interface IEditorAction
    {
        void Do();
        void Undo();
    }
}
