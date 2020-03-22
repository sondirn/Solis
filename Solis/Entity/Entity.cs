using Microsoft.Xna.Framework;
using System;

namespace Solis
{
    public class Entity
    {
        /// <summary>
        /// Holds Components, handles removal and adding components
        /// </summary>
        public ComponentManager Components;

        /// <summary>
        /// Unique ID to this entity
        /// </summary>
        public readonly uint Id;

        /// <summary>
        /// Reference to the scene that is occupied by this entity
        /// </summary>
        public Scene Scene;

        /// <summary>
        /// Interval for when this entity should update, 1 is every frame, 2 every other etc.
        /// </summary>
        public uint UpdateInterval;

        /// <summary>
        /// Boolean for if the entity should be enabled.
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// Boolean for if the Entity is destroyed
        /// </summary>
        public bool IsDestroyed;

        /// <summary>
        /// String that holds the name of the entity
        /// </summary>
        public string Name;

        /// <summary>
        /// Default Component every entity has, this holds positional data
        /// </summary>
        ///
        public Transform Transform;

        /// <summary>
        /// Constructor for entity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Position"></param>
        public Entity(string name, Vector2 Position)
        {
            Scene = SolisCore.Instance.CurrentScene;
            Id = Scene.EntityManager._entityIdHelper++;
            Name = string.Format("{0}_{1}", Id, name);
            Components = new ComponentManager(this);
            Transform = AddComponent(new Transform(Position));
        }

        public Entity() : this("NoName", Vector2.Zero)
        {
        }

        public Entity(string name) : this(name, Vector2.Zero)
        {
        }

        /// <summary>
        /// This is called when the entity is added to the scene
        /// </summary>
        public virtual void OnAddedToScene() { }

        /// <summary>
        /// This is called when an entity is removed from the scene
        /// </summary>
        public virtual void OnRemovedFromScene() { }

        /// <summary>
        /// This is your update method, it is called if the entity is set to active
        /// </summary>
        public virtual void Update()
        {
            Components.Update();
        }

        public virtual void OnEnabled()
        {

        }

        public virtual void OnDisabled()
        {

        }

        /// <summary>
        /// This adds a component to the entity to be handled by the component manager
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public T AddComponent<T>(T component) where T : Component
        {
            component.ParentEntity = this;
            component.Initialize();
            Components.Add(component);
            return component;
        }

        public T AddComponent<T>() where T: Component, new()
        {
            var component = new T();
            component.ParentEntity = this;
            component.Initialize();
            Components.Add(component);
            return component;
        }

        public T GetComponent<T>() where T : Component => Components.GetComponent<T>();

        public bool RemoveComponent<T>() where T : Component
        {
            var comp = GetComponent<T>();
            if(comp != null)
            {
                RemoveComponent(comp);
                return true;
            }
            return false;
        }

        void RemoveComponent(Component component) => Components.Remove(component);

        public void RemoveAllComponents() => Components.RemoveAllComponents();

        public T GetOrCreateComponent<T>() where T : Component, new()
        {
            var component = Components.GetComponent<T>();
            if (component == null)
                component = AddComponent<T>();
            return component;
        }

        /// <summary>
        /// this is called when you want to destroy the entity, not to confuse when the entity is removed from the scene.
        /// </summary>
        public virtual void Destroy() { }

        public void SetEnabled()
        {
            Enabled = true;
            OnEnabled();
        }

        public void SetDisabled()
        {
            Enabled = false;
            OnDisabled();
        }
    }
}