using Microsoft.Xna.Framework;
using Solis.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solis.Sandbox
{
    public class SandboxCore : SolisCore
    {
        public SandboxCore() : base(gameName: "SolisSandbox")
        {
            ClearColor = Color.Black;
            SetScene(new Solis.Scenes.Scene("Test SandboxScene", new SolisRenderer(GraphicsManager)));
            
        }
    }
}
