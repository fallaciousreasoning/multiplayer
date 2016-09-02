namespace MultiPlayer.Core.Animation
{
    public interface IAccessor
    {
        Entity Entity { get; set; }
        object Get();

        void Set(object value, object relativeTo);
    }
}
