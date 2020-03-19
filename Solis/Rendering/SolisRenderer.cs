using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Solis
{
    public class SolisRenderer
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public static Exception NULL_RENDER = new Exception("Renderer can not be null, please instantiate an instance of a renderer");

        public SolisRenderer()
        {
            Graphics = SolisCore.Instance.GraphicsManager;
        }

        public virtual void Initialize()
        {
            SpriteBatch = new SpriteBatch(SolisCore.Instance.GraphicsDevice);
        }

        public virtual void LoadContent()
        {
            SpriteBatch = new SpriteBatch(SolisCore.Instance.GraphicsDevice);
        }

        public virtual void UnloadContent()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime)
        {
        }
    }
}