using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Solis
{
    public static class Input
    {
        public const float DEFAULT_DEADZONE = 0.1f;

        internal static Vector2 _resolutionScale;
        internal static Point _resolutionOffset;
        private static KeyboardState _previousKeyboardState;
        private static KeyboardState _currentKeyboardState;
        private static MouseState _previousMouseState;
        private static MouseState _currentMouseState;
        private static int _maxSupportdGamePads;

        public static int MaxSupportedGamePads
        {
            get { return _maxSupportdGamePads; }
            set
            {
#if FNA
                _maxSupportedGamePads = Mathf.Clamp(value,1,8);
#else
                _maxSupportdGamePads = Mathf.Clamp(value, 1, GamePad.MaximumGamePadCount);
#endif
            }
        }

        static Input()
        {
            _previousKeyboardState = new KeyboardState();
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = new MouseState();
            _currentMouseState = Mouse.GetState();
        }

        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// meant to simulate keyboard input
        /// </summary>
        public static void SetCurrentKeyboardState(KeyboardState state)
        {
            _currentKeyboardState = state;
        }

        #region keyboard

        public static KeyboardState PreviousKeyboardState => _previousKeyboardState;
        public static KeyboardState CurrentKeyboardState => _currentKeyboardState;

        public static bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return !_currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys keyA, Keys keyB)
        {
            return IsKeyPressed(keyA) || IsKeyPressed(keyB);
        }

        public static bool IsKeyDown(Keys keyA, Keys keyB)
        {
            return IsKeyDown(keyA) || IsKeyDown(keyB);
        }

        #endregion keyboard
    }
}