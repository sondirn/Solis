using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Solis
{
    public class EntityManager
    {
        /// <summary>
        /// Dictionary for quick access to entities by Id
        /// </summary>
        private Dictionary<uint, Entity> _entities;

        /// <summary>
        /// List for all active entities
        /// </summary>
        private List<Entity> _activeEntities;

        /// <summary>
        /// List for all inactive entities
        /// </summary>
        private List<Entity> _inactiveEntities;

        /// <summary>
        /// A queue that stores entities that need to be added
        /// </summary>
        private Queue<Entity> _entitiesToCreate;

        /// <summary>
        /// A queue that stores entities that need to be destroyed
        /// </summary>
        private Queue<Entity> _entitiesToDestroy;

        /// <summary>
        /// Variable that assists in assigning ID's to each entity
        /// </summary>
        internal uint _entityIdHelper;

        /// <summary>
        /// Default constructor for Entity Manager
        /// </summary>
        public EntityManager()
        {
            _entityIdHelper = 0;
            _entities = new Dictionary<uint, Entity>();
            _activeEntities = new List<Entity>();
            _inactiveEntities = new List<Entity>();
            _entitiesToCreate = new Queue<Entity>();
            _entitiesToDestroy = new Queue<Entity>();
        }

        /// <summary>
        /// Update method, here is where Entity qeues to be added/destroyed are processed. Also all active entities are updated here
        /// </summary>
        public void Update()
        {
            HandleDestroy();
            HandleCreation();
            foreach (Entity entity in _activeEntities)
            {
                entity.Update();
            }
        }

        /// <summary>
        /// Function that handles entities in the EntitiestoDestroy queue
        /// </summary>
        internal void HandleDestroy()
        {
            if (_entitiesToDestroy.Count > 0)
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

        /// <summary>
        /// Function that handles entities in the EntitiesToCreate Queue
        /// </summary>
        internal void HandleCreation()
        {
            if (_entitiesToCreate.Count > 0)
            {
                for (int i = 0; i < _entitiesToCreate.Count; i++)
                {
                    var entityToCreate = _entitiesToCreate.Dequeue();
                    _entities.Add(entityToCreate.Id, entityToCreate);
                    _activeEntities.Add(entityToCreate);
                    entityToCreate.SetEnabled();
                    entityToCreate.OnAddedToScene();
                }
            }
        }

        /// <summary>
        /// Creates an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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

        public Entity CreateEntity(string name, Vector2 position)
        {
            var entity = new Entity(name, position);
            _entitiesToCreate.Enqueue(entity);
            return entity;
        }

        /// <summary>
        /// Destroys an entity
        /// </summary>
        /// <param name="id"></param>
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

        public Entity GetEntity(uint id)
        {
            return _entities[id];
        }

        public int EntityCount()
        {
            return _entities.Count;
        }
    }
}