namespace Solis
{
    public abstract class VirtualInput
    {
        public enum OverlapBehavior
        {
            CancelOut,
            TakeOlder,
            TakeNewer
        }

        protected VirtualInput()
        {
            Input._virtualInputs.Add(this);
        }

        public void Deregister()
        {
            Input._virtualInputs.Remove(this);
        }

        public abstract void Update();
    }

    public abstract class VirtualInputNode
    {
        public virtual void Update()
        {
        }
    }
}