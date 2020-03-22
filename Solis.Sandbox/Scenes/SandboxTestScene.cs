using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solis.Sandbox.Scenes
{
    class SandboxTestScene : Scene
    {
        SpriteBatch spriteBatch;
        Entity testEntity;
        bool texturesLoaded;
        public SandboxTestScene(string sceneName) : base(sceneName)
        {
            Renderer = new SolisRenderer();
            texturesLoaded = false;
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(SolisCore.Instance.GraphicsDevice);
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Input.IsKeyPressed(Keys.Space))
            {
                Content.LoadAsyncTextures(new string[] { "T_NoTexture", "T_DefaultTexture" }, textures);
                Content.LoadAsyncSong("S_Viking", test);
            }
            if (Input.IsKeyPressed(Keys.Enter))
            {
                GC.Collect();
            }
            if (Input.IsKeyPressed(Keys.L))
            {
                Console.WriteLine("Currently {0} assets loaded", Content.AssetCount());
                Console.WriteLine("Currently {0} Entities exist", EntityCount());
            }
            if (Input.IsKeyPressed(Keys.K))
            {
                Console.WriteLine("Created Entity: {0}",CreateEntity("TestEntity").Name);
            }

        }

        public void test(Song song)
        {
            var songs = Content.GetAssetSong("S_Viking");
            MediaPlayer.Play(songs);
            MediaPlayer.IsRepeating = true;
        }

        public void textures()
        {
            texturesLoaded = true;
        }


        
        public override void Draw(GameTime gameTime)
        {
            if(spriteBatch != null)
            {
                base.Draw(gameTime);
                spriteBatch.Begin();

                if (texturesLoaded)
                {
                    spriteBatch.Draw(Content.GetAssetTexture("T_NoTexture"), new Rectangle(0, 0, 64, 64), Color.White);
                    spriteBatch.Draw(Content.GetAssetTexture("T_DefaultTexture"), new Vector2(100, 100), Color.White);
                }

                spriteBatch.End();
            }

        }
    }       
}
