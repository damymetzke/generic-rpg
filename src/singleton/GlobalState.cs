using Godot;

class GlobalState : Node
{
    public class Item<T>
    {
        public delegate void OnChange(T oldValue, T newValue);
        private OnChange onChange = (T oldValue, T newValue) => { };

        private T under;

        public T Under
        {
            get { return under; }
            set
            {
                onChange.Invoke(under, value);
                under = value;
            }
        }

        public void RegisterChange(OnChange callback)
        {
            onChange += callback;
        }
    }

    public enum EInputState
    {
        GAMEPLAY,
        DEBUG_CONSOLE,
        MENU
    }

    public Item<EInputState> inputState;

}