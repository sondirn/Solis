using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solis.Rendering
{
    public class SolisRenderer
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public static Exception NULL_RENDER = new Exception("Renderer can not be null, please instantiate an instance of a renderer");

        public SolisRenderer(GraphicsDeviceManager deviceManager)
        {
            Graphics = deviceManager;
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
            SpriteBatch.Begin();

            SpriteBatch.End();
        }
    }
}
