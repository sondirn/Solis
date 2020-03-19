using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solis.Utils;
using System;
using System.Diagnostics;
using System.IO;
using static Solis.SolisSettings;

namespace Solis
{
    public class SolisCore : Game
    {
        #region members

        /// <summary>
        /// sets this core object as a service
        /// </summary>
        public new static GameServiceContainer Services => ((Game)_instance).Services;

        /// <summary>
        /// Accessors for core variables
        /// </summary>
        public static SolisCore Instance => _instance;

        internal static SolisCore _instance;

        /// <summary>
        /// Clear color of background
        /// </summary>
        public static Color ClearColor;

        /// <summary>
        /// GraphicsDeviceManager
        /// </summary>
        public GraphicsDeviceManager GraphicsManager;

        /// <summary>
        /// Window title
        /// </summary>
        public string GameName;

        /// <summary>
        /// Settings for game
        /// </summary>
        public SolisSettings GameSettings;

        /// <summary>
        /// Current Scene of the game
        /// </summary>
        public Scene CurrentScene;

        public new static SolisContentManager Content;

        private Scene _previousScene;

        #endregion members

        #region Constructor

        /// <summary>
        /// Constructor for core game container
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isFullScreen"></param>
        /// <param name="gameName"></param>
        /// <param name="contentDirectory"></param>
        ///

        public SolisCore(bool isFullScreen = false, string gameName = "SolisEngine", string contentDirectory = "Content")
        {
            _instance = this;
            Content = new SolisContentManager
            {
                RootDirectory = contentDirectory
            };
            GameName = gameName;
            CheckSettingsFile();

            CreateGraphicsDeviceManager();

            Window.ClientSizeChanged += OnGraphicsDeviceReset;
            Window.OrientationChanged += OnOrientationChanged;

            base.Content.RootDirectory = contentDirectory;
            //apply settings
            ClearColor = Color.CornflowerBlue;
            IsFixedTimeStep = GameSettings.IsFixedTimeStep;
            Window.AllowUserResizing = GameSettings.AllowWindowAdjusting;
            IsMouseVisible = GameSettings.IsMouseVisible;
        }

        #endregion Constructor

        #region Listeners

        /// <summary>
        /// handler when orientation is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///

        private void OnOrientationChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// handler when game is resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGraphicsDeviceReset(object sender, EventArgs e)
        {
            Console.WriteLine("GameResized");
        }

        public new static void Exit()
        {
            ((Game)_instance).Exit();
        }

        #endregion Listeners

        #region overrides

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            DebugUpdate(gameTime);
            CurrentScene.Run(gameTime);
            Input.Update();
            if (_previousScene != null)
            {
                _previousScene.Content.Dispose();
                _previousScene = null;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(ClearColor);
            CurrentScene.Draw(gameTime);
        }

        #endregion overrides

        #region Debug Injection

#if DEBUG

        //Debug Specific Variables
        private TimeSpan _frameCounterElapsedTime = new TimeSpan();

#endif

        [Conditional("DEBUG")]
        private void DebugUpdate(GameTime gameTime)
        {
#if DEBUG
            _frameCounterElapsedTime += gameTime.ElapsedGameTime;
            if (_frameCounterElapsedTime >= TimeSpan.FromSeconds(1))
            {
                int framerate = (int)(1 / (float)gameTime.ElapsedGameTime.TotalSeconds);
                var totalMemory = (GC.GetTotalMemory(false) / 1048576f).ToString("F");
                Window.Title = string.Format("{0} {1} fps - {2} MB", GameName, framerate, totalMemory);
                _frameCounterElapsedTime -= TimeSpan.FromSeconds(1);
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif
        }

        #endregion Debug Injection

        #region Helper Methods

        /// <summary>
        /// Checks if Settings file exists, if not then it will create a new settings file
        /// </summary>
        private void CheckSettingsFile()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var folderPath = Path.Combine(path, "My Games", GameName);

            if (File.Exists(folderPath + @"\" + "Settings.xml"))
            {
                GameSettings = SolisXML.FromXML<SolisSettings>(folderPath + @"\" + "Settings.xml");
            }
            else
            {
                Directory.CreateDirectory(folderPath);
                CreateSettingsFile();
            }
        }

        private void CreateSettingsFile()
        {
            // Create New Settings File
            int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            GameSettings = new SolisSettings
            {
                WindowMode = WINDOW_MODE.FULLSCREEN,
                Vsync = true,
                WindowWidth = w,
                WindowHeight = h,
                AllowWindowAdjusting = false,
                IsMouseVisible = true,
                IsFixedTimeStep = false
            };
            SaveSettings();
            Console.WriteLine("Created initial default settings file");
        }

        private void CreateGraphicsDeviceManager()
        {
            switch (GameSettings.WindowMode)
            {
                case WINDOW_MODE.FULLSCREEN:
                    GraphicsManager = new GraphicsDeviceManager(this)
                    {
                        PreferredBackBufferWidth = GameSettings.WindowWidth,
                        PreferredBackBufferHeight = GameSettings.WindowHeight,
                        IsFullScreen = true,
                        SynchronizeWithVerticalRetrace = GameSettings.Vsync
                    };
                    break;

                case WINDOW_MODE.WINDOWED:
                    GraphicsManager = new GraphicsDeviceManager(this)
                    {
                        PreferredBackBufferWidth = GameSettings.WindowWidth,
                        PreferredBackBufferHeight = GameSettings.WindowHeight,
                        IsFullScreen = false,
                        SynchronizeWithVerticalRetrace = GameSettings.Vsync
                    };
                    break;

                case WINDOW_MODE.BORDERLESS:
                    int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    GraphicsManager = new GraphicsDeviceManager(this)
                    {
                        PreferredBackBufferWidth = w,
                        PreferredBackBufferHeight = h,
                        IsFullScreen = false,
                        SynchronizeWithVerticalRetrace = GameSettings.Vsync,
                    };
                    Window.IsBorderless = true;
                    break;
            }

            GraphicsManager.DeviceReset += OnGraphicsDeviceReset;
            GraphicsManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
        }

        public void SaveSettings()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var folderPath = Path.Combine(path, "My Games", GameName);
            SolisXML.ToXML(GameSettings, folderPath + @"\" + "Settings.xml");
            Console.WriteLine("GameSettings were saved");
        }

        public void SetScene(Scene scene)
        {
            CurrentScene = scene;
        }

        public void ChangeScene(Scene newScene)
        {
            CurrentScene.Unload();
            CurrentScene = newScene;
        }

        #endregion Helper Methods
    }
}