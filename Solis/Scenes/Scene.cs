using Microsoft.Xna.Framework;
using Solis.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solis.Scenes
{
    public class Scene
    {
        public SolisRenderer Renderer;
        public string SceneName;
        public Scene(string sceneName, SolisRenderer renderer)
        {
            initialized = false;
            SolisCore.Instance.SetScene(this);
            SceneName = sceneName;
            Renderer = renderer;
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
            Renderer.Update(gameTime);
        }
        public virtual void Draw(GameTime gameTime) 
        {
            Renderer.Draw(gameTime);
        }

        private bool initialized;

        public void Run(GameTime gameTime)
        {
            if(!initialized)
            {
                Initialize();
                LoadContent();
            }
            Update(gameTime);
        }

        public void InjectRenderer(SolisRenderer renderer)
        {
            renderer = null;
            Renderer = renderer;
        }
    }
}
