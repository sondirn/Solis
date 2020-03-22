namespace Solis
{
    public class Component
    {
        public Entity ParentEntity;

        public virtual void Initialize() { }

        public virtual void OnAddedToEntity()
        {
        }

        public virtual void OnRevmoedFromEntity()
        {
        }
    }
}