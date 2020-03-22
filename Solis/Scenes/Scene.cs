using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;

namespace Solis
{
    public class Scene
    {
        public SolisRenderer Renderer;
        public string SceneName;
        public SolisContentManager Content;
        internal EntityManager EntityManager;

        public Scene(string sceneName, SolisRenderer renderer)
        {
            initialized = false;
            SolisCore.Instance.SetScene(this);
            SceneName = sceneName;
            Content = Content = new SolisContentManager
            {
                RootDirectory = SolisCore.Content.RootDirectory
            };
            EntityManager = new EntityManager();
        }

        public Scene(string sceneName)
        {
            initialized = false;
            Content = new SolisContentManager
            {
                RootDirectory = SolisCore.Content.RootDirectory
            };
            SolisCore.Instance.SetScene(this);
            SceneName = sceneName;
            EntityManager = new EntityManager();
        }

        public Scene()
        {
            initialized = false;
            Content = new SolisContentManager
            {
                RootDirectory = SolisCore.Content.RootDirectory
            };
            SolisCore.Instance.SetScene(this);
            SceneName = "NoName";
            EntityManager = new EntityManager();
        }

        public virtual void Initialize()
        {
            if (Renderer != null)
                Renderer.Initialize();
            else
                throw new Exception("Please create a renderer");
        }

        public virtual void LoadContent()
        {
            Renderer.LoadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            EntityManager.Update();
            Renderer.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (Renderer != null)
                Renderer.Draw(gameTime);
        }

        private bool initialized;

        public void Run(GameTime gameTime)
        {
            if (!initialized)
            {
                Initialize();
                LoadContent();
                initialized = true;
            }
            Update(gameTime);
        }

        public void InjectRenderer(SolisRenderer renderer)
        {
            renderer = null;
            Renderer = renderer;
        }

        public virtual void Unload()
        {
            MediaPlayer.Stop();
        }

        public Entity CreateEntity(Entity entity)
        {
            return EntityManager.CreateEntity(entity);
        }

        public Entity CreateEntity(string name = "NoName") => EntityManager.CreateEntity(name);

        public Entity GetEntity(uint id) => EntityManager.GetEntity(id);

        public int EntityCount()
        {
            return EntityManager.EntityCount();
        }
    }
}