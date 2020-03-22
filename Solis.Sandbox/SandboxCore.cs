using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Solis.Sandbox.Scenes;
using System;

namespace Solis.Sandbox
{
    public class SandboxCore : SolisCore
    {
        public SandboxCore() : base(gameName: "SolisSandbox")
        {
            ClearColor = Color.Black;
            SetScene(new SandboxTestScene("testScene"));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Input.IsKeyPressed(Keys.M))
            {
                ChangeScene(new SandboxTestScene("new scene"));
                GC.Collect();
            }
        }
    }
}