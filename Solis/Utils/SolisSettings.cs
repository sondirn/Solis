namespace Solis
{
    public class SolisSettings
    {
        public WINDOW_MODE WindowMode { get; set; }
        public bool Vsync { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }

        public bool AllowWindowAdjusting { get; set; }
        public bool IsMouseVisible { get; set; }
        public bool IsFixedTimeStep { get; set; }
        public float TargetFrameRate { get; set; }

        public enum WINDOW_MODE
        {
            FULLSCREEN,
            WINDOWED,
            BORDERLESS
        }
    }
}