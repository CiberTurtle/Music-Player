using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// Using a modified version of Monofoxe's input system
namespace MusicPlayer
{
	public class Input
	{
		#region Mouse
		public static Vector2Int windowMousePosition = Vector2Int.zero;

		public static bool isMouseInWindow = false;

		static List<Inputs> _mouseButtons = new List<Inputs>();
		static List<Inputs> _oldMouseButtons = new List<Inputs>();

		public static int scroll = 0;
		static int _mouseWheelAdditionPrev = 0;
		#endregion

		#region Keyboard
		public static string keyboardString = string.Empty;

		public static Keys keyboardKey = Keys.None;

		public static Keys keyboardLastKey = Keys.None;

		public static char keyboardLastChar = ' ';

		static StringBuilder _keyboardBuffer = new StringBuilder();
		static Keys _keyboardLastKeyBuffer = Keys.None;
		static List<Keys> _currentKeys = new List<Keys>();
		static List<Keys> _oldKeys = new List<Keys>();
		#endregion

		const int _keyboardMaxCode = 1000;
		const int _mouseMaxCode = 2000;

		static bool _mouseCleared, _keyboardCleared = false;

		public static void Update()
		{
			_mouseCleared = false;
			_keyboardCleared = false;

			#region Mouse
			MouseState mouseState = Mouse.GetState();

			if (new Rect(0, 0, Main.graphics.PreferredBackBufferWidth, Main.graphics.PreferredBackBufferHeight).Contains(mouseState.Position))
				windowMousePosition = mouseState.Position;
			else
				windowMousePosition = Vector2Int.zero;

			_oldMouseButtons = _mouseButtons;
			_mouseButtons = new List<Inputs>();

			if (mouseState.LeftButton == ButtonState.Pressed)
				_mouseButtons.Add(Inputs.MouseLeft);

			if (mouseState.RightButton == ButtonState.Pressed)
				_mouseButtons.Add(Inputs.MouseRight);

			if (mouseState.MiddleButton == ButtonState.Pressed)
				_mouseButtons.Add(Inputs.MouseMiddle);

			scroll = _mouseWheelAdditionPrev - mouseState.ScrollWheelValue;
			_mouseWheelAdditionPrev = mouseState.ScrollWheelValue;
			#endregion

			#region Keyboard
			keyboardString = _keyboardBuffer.ToString();
			_keyboardBuffer.Clear();

			if (keyboardString.Length > 0)
				keyboardLastChar = keyboardString[keyboardString.Length - 1];

			keyboardLastKey = _keyboardLastKeyBuffer;

			_oldKeys.Clear();
			_oldKeys.AddRange(_currentKeys);
			_currentKeys.Clear();
			_currentKeys.AddRange(Keyboard.GetState().GetPressedKeys());

			if (_currentKeys.Count > 0)
				keyboardKey = _currentKeys[_currentKeys.Count - 1];
			else
				keyboardKey = Keys.None;
			#endregion
		}

		#region Button Checks
		public static bool CheckButton(Inputs button, int index = 0)
		{
			var buttonCode = (int)button;

			return
				buttonCode < _keyboardMaxCode && !_keyboardCleared && _currentKeys.Contains((Keys)button)
				|| buttonCode < _mouseMaxCode && !_mouseCleared && _mouseButtons.Contains(button);
		}

		public static bool CheckButtonPress(Inputs button, int index = 0)
		{
			var buttonCode = (int)button;

			return
				buttonCode < _keyboardMaxCode && !_keyboardCleared && _currentKeys.Contains((Keys)button) && !_oldKeys.Contains((Keys)button)
				|| buttonCode < _mouseMaxCode && !_mouseCleared && _mouseButtons.Contains(button) && !_oldMouseButtons.Contains(button);
		}

		public static bool CheckButtonRelease(Inputs button, int index = 0)
		{
			var buttonCode = (int)button;

			return
				buttonCode < _keyboardMaxCode && !_keyboardCleared && !_currentKeys.Contains((Keys)button) && _oldKeys.Contains((Keys)button)
				|| buttonCode < _mouseMaxCode && !_mouseCleared && !_mouseButtons.Contains(button) && _oldMouseButtons.Contains(button);
		}
		#endregion

		#region Mouse
		public static void ClearMouseInput() => _mouseCleared = true;
		#endregion

		#region Keyboard
		public static bool KeyboardCheckAnyKey() => !_keyboardCleared && _currentKeys.Count > 0;

		public static bool KeyboardCheckAnyKeyPress() => !_keyboardCleared && _currentKeys.Count > 0 && _oldKeys.Count == 0;

		public static bool KeyboardCheckAnyKeyRelease() => !_keyboardCleared && _currentKeys.Count == 0 && _oldKeys.Count > 0;

		public static void ClearKeyboardInput() => _keyboardCleared = true;

		public static void TextInput(TextInputEventArgs args)
		{
			_keyboardBuffer.Append(args.Character);
			_keyboardLastKeyBuffer = args.Key;
		}
		#endregion

		public static void ClearInput()
		{
			ClearMouseInput();
			ClearKeyboardInput();
		}
	}
}