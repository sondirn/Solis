using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solis
{
    public class EntityManager
    {
        Dictionary<uint, Entity> _entities;
        List<Entity> _activeEntities;
        List<Entity> _inactiveEntities;
        Queue<Entity> _entitiesToCreate;
        Queue<Entity> _entitiesToDestroy;
        internal uint _entityIdHelper;

        public EntityManager()
        {
            _entityIdHelper = 0;
            _entities = new Dictionary<uint, Entity>();
            _activeEntities = new List<Entity>();
            _inactiveEntities = new List<Entity>();
            _entitiesToCreate = new Queue<Entity>();
            _entitiesToDestroy = new Queue<Entity>();
        }
        public void Update()
        {
            HandleDestroy();
            HandleCreation();
            foreach (Entity entity in _activeEntities)
            {
                entity.Update();
            }
        }

        internal void HandleDestroy()
        {
            if(_entitiesToDestroy.Count > 0)
            {
                for (int i = 0; i < _entitiesToDestroy.Count; i++)
                {
                    var entityToDestroy = _entitiesToDestroy.Dequeue();
                    entityToDestroy.Destroy();
                    if (entityToDestroy.Enabled)
                    {
                        _activeEntities.Remove(entityToDestroy);
                    }
                    else
                    {
                        _inactiveEntities.Remove(entityToDestroy);
                    }
                    _entities.Remove(entityToDestroy.Id);
                }
            }
        }

        internal void HandleCreation()
        {
            if(_entitiesToCreate.Count > 0)
            {
                for (int i = 0; i < _entitiesToCreate.Count; i++)
                {
                    var entityToCreate = _entitiesToCreate.Dequeue();
                    _entities.Add(entityToCreate.Id, entityToCreate);
                    _activeEntities.Add(entityToCreate);
                    entityToCreate.OnAddedToScene();
                }
            }
        }

        public Entity CreateEntity(Entity entity)
        {
            _entitiesToCreate.Enqueue(entity);
            return entity;
        }

        public Entity CreateEntity(string name = "NoName")
        {
            var entity = new Entity(name);
            _entitiesToCreate.Enqueue(entity);
            return entity;
        }

        public void DestroyEntity(uint id)
        {
            var entityToDestroy = _entities[id];
            _entitiesToDestroy.Enqueue(entityToDestroy);
        }

        public void DestroyEntity(Entity entity)
        {
            _entitiesToDestroy.Enqueue(entity);
        }

        public Entity DeactivateEntity(Entity entity)
        {
            if (_activeEntities.Contains(entity))
            {
                _activeEntities.Remove(entity);
            }
            if (!_inactiveEntities.Contains(entity))
            {
                _inactiveEntities.Add(entity);
            }

            return entity;
        }

        public Entity DeactivateEntity(uint id)
        {
            var entity = _entities[id];
            if (_activeEntities.Contains(entity))
            {
                _activeEntities.Remove(entity);
            }
            if (!_inactiveEntities.Contains(entity))
            {
                _inactiveEntities.Add(entity);
            }
            return entity;
        }

        public int EntityCount()
        {
            return _entities.Count;
        }
    }
}
