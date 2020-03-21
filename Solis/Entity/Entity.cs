﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solis
{
    public class Entity
    {
        public ComponentManager Components;
        public readonly uint Id;
        public Scene Scene;
        public uint UpdateInterval;
        public bool Enabled { get; set; }
        public bool IsDestroyed;
        public string Name;
        public Transform Transform;
        public Entity(string name, Vector2 Position)
        {
            Components = new ComponentManager();
            Transform = AddCompnoent(new Transform(Position));
        }

        public Entity() : this("NoName", Vector2.Zero)
        {

        }

        public Entity(string name) : this(name, Vector2.Zero)
        {

        }

        public virtual void OnAddedToScene() { }
        public virtual void OnRemovedFromScene() { }

        public virtual void Update() => Components.Update();

        public T AddCompnoent<T>(T component) where T : Component
        {
            component.ParentEntity = this;
            Components.Add(component);
            component.OnStart();
            return component;
        }

        
    }
}