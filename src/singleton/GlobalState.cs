using Godot;

class GlobalState : Node
{
    public class Item<T>
    {
        public delegate void OnChange(T oldValue, T newValue);
        private OnChange onChange = (T oldValue, T newValue) => { };

        private T under;

        public T Get()
        {
            return under;
        }

        public void Set(T value)
        {
            if (under.Equals(value))
            {
                return;
            }
            onChange.Invoke(under, value);
            under = value;
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

    public Item<EInputState> inputState = new Item<EInputState>();
}