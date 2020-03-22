using System.Collections.Generic;

namespace Solis
{
    public class ComponentManager
    {
        private Entity Entity;
        private List<Component> _components = new List<Component>();
        private List<IUpdatable> _updatableComponents = new List<IUpdatable>();
        private Queue<Component> _componentsToAdd = new Queue<Component>();
        private Queue<Component> _componentsToRemove = new Queue<Component>();

        public ComponentManager(Entity entity)
        {
            Entity = entity;
        }

        public void Update()
        {
            HandleComponentsToBeDestroyed();
            HandleComponentsToBeAdded();
            foreach (IUpdatable updatableComponent in _updatableComponents)
            {
                updatableComponent.Update();
            }
        }

        public void HandleComponentsToBeAdded()
        {
            if (_componentsToAdd.Count > 0)
            {
                for (int i = 0; i < _componentsToAdd.Count; i++)
                {
                    var componentToAdd = _componentsToAdd.Dequeue();
                    _components.Add(componentToAdd);
                    if (componentToAdd is IUpdatable)
                        _updatableComponents.Add(componentToAdd as IUpdatable);
                    componentToAdd.OnAddedToEntity();
                }
            }
        }

        public void HandleComponentsToBeDestroyed()
        {
            if (_componentsToRemove.Count > 0)
            {
                for (int i = 0; i < _componentsToRemove.Count; i++)
                {
                    var componentToDestroy = _componentsToRemove.Dequeue();
                    _components.Remove(componentToDestroy);
                    if (componentToDestroy is IUpdatable && _updatableComponents.Contains(componentToDestroy as IUpdatable))
                        _updatableComponents.Remove(componentToDestroy as IUpdatable);
                    componentToDestroy.OnRevmoedFromEntity();
                }
            }
        }

        public void Add(Component component)
        {
            _componentsToAdd.Enqueue(component);
        }

        public void Remove(Component component)
        {
            _componentsToRemove.Enqueue(component);
        }

        public T GetComponent<T>() where T : Component
        {
            for (int i = 0; i < _components.Count; i++)
            {
                var component = _components[i];
                if (component is T)
                    return component as T;
            }
            return null;
        }

        public void RemoveAllComponents()
        {
            _components.Clear();
            _updatableComponents.Clear();
            _componentsToAdd.Clear();
            _componentsToRemove.Clear();
        }
    }
}