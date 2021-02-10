using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MusicPlayer
{
	public class StackLayout : IDisposable
	{
		readonly Vector2 _position;
		readonly int _sizePerElement;
		readonly bool _isHorizontal;

		Vector2 _offset;
		bool _isFirstElement = true;

		public StackLayout(Vector2 position, int sizePerElement, bool isHorizontal)
		{
			_position = position;
			_offset = position;
			_sizePerElement = sizePerElement;
			_isHorizontal = isHorizontal;

			Main.sb.Begin();
		}

		public Vector2 AddElement(int elementSize = -1)
		{
			if (elementSize < 0) elementSize = _sizePerElement;

			if (!_isFirstElement)
			{
				switch (_isHorizontal)
				{
					case true:
						_offset.X += elementSize;
						break;
					case false:
						_offset.Y += elementSize;
						break;
				}
			}
			_isFirstElement = false;

			return _offset;
		}

		public void Dispose()
		{
			Main.sb.End();
		}
	}
}