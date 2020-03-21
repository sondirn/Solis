namespace Solis
{
    public class Component
    {
        public Entity ParentEntity;
        public virtual void OnStart()
        {
        }

        public virtual void OnAddedToEntity()
        {
        }

    }
}