using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solis
{
    public class ComponentManager
    {
        Entity Entity;
        List<Component> _components = new List<Component>();
        List<IUpdatable> _updatableComponents = new List<IUpdatable>();
        List<Component> _componentsToAdd = new List<Component>();
        List<Component> _componentsToRemove = new List<Component>();

        public void Update()
        {
            //handle any removals
            if(_componentsToRemove.Count > 0)
            {
                foreach (var componentToBeRemoved in _componentsToRemove)
                {
                    _components.Remove(componentToBeRemoved);
                }
                _componentsToRemove.Clear();
            }

            if(_componentsToAdd.Count > 0)
            {
                foreach (var componentToBeAdded in _componentsToAdd)
                {
                    if (componentToBeAdded is IUpdatable)
                        _updatableComponents.Add(componentToBeAdded as IUpdatable);
                    if (componentToBeAdded is DrawableComponent)
                    {
                            
                    }
                       
                    _components.Add(componentToBeAdded);
                    componentToBeAdded.OnAddedToEntity();
                }

                _componentsToAdd.Clear();
            }
        }

        public void Add(Component component)
        {
            _componentsToAdd.Add(component);
        }

        public void Remove(Component component)
        {
            if (_componentsToAdd.Contains(component))
            {
                _componentsToAdd.Remove(component);
                return;
            }

            _componentsToRemove.Add(component);
        }

        public void RemoveAllComponents()
        {
            
        }

        public void HandleRemove(Component component)
        {
            if(component is DrawableComponent)
            {

            }
               
        }
        
    }
}
