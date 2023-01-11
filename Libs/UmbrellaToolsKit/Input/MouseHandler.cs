using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UmbrellaToolsKit.Input
{
    public class MouseHandler
    {
        private static MouseHandler _instance;

        internal ButtonState _buttonLeftLastState = ButtonState.Released;
        internal ButtonState _buttonRightLastState = ButtonState.Released;

        internal static MouseHandler GetStatus
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MouseHandler();
                    GameManagement.OnGameUpdateData += _instance.setInputData;
                }
                return _instance;
            }
        }

        public static bool ButtonLeftPressed
        {
            get
            {
                var status = GetStatus;
                return Mouse.GetState().LeftButton == ButtonState.Pressed;
            }
        }
        public static bool ButtonLeftReleased
        {
            get
            {
                var status = GetStatus;
                return Mouse.GetState().LeftButton == ButtonState.Released;
            }
        }

        public static bool ButtonRightPressed
        {
            get
            {
                var status = GetStatus;
                return Mouse.GetState().RightButton == ButtonState.Pressed;
            }
        }
        public static bool ButtonRightReleased
        {
            get
            {
               var status = GetStatus;
                return Mouse.GetState().RightButton == ButtonState.Released;
            }
        }

        public static bool ButtonLeftPressing => ButtonLeftPressed && GetStatus._buttonLeftLastState == ButtonState.Pressed;
        public static bool ButtonRightPressing => ButtonRightPressed && GetStatus._buttonRightLastState == ButtonState.Pressed;

        public static bool ButtonLeftOneClick => ButtonLeftPressed && GetStatus._buttonLeftLastState == ButtonState.Released;
        public static bool ButtonRightOneClick => ButtonRightPressed && GetStatus._buttonRightLastState == ButtonState.Released;

        public static Vector2 Position => Mouse.GetState().Position.ToVector2();
        
        internal void setInputData()
        {
            _buttonLeftLastState = Mouse.GetState().LeftButton;
            _buttonRightLastState = Mouse.GetState().RightButton;
        }
    }
}
