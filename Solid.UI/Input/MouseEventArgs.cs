using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.UI.Input
{
	public sealed class MouseEventArgs : EventArgs
	{
		private MouseEventArgs()
		{

		}

		public int X { get; private set; }

		public int Y { get; private set; }

		public int WheelDelta { get; private set; }

		public MouseButton Button { get; private set; }

		public MouseEventType Type { get; private set; }

		public static MouseEventArgs CreateWheel(int x, int y, int delta)
		{
			return new MouseEventArgs()
			{
				Type = MouseEventType.Wheel,
				WheelDelta = delta,
				X = x,
				Y = y,
			};
		}

		public static MouseEventArgs CreateMovement(int x, int y)
		{
			return new MouseEventArgs()
			{
				Type = MouseEventType.Move,
				X = x,
				Y = y,
			};
		}

		public static MouseEventArgs CreateLeave(int x, int y)
		{
			return new MouseEventArgs()
			{
				Type = MouseEventType.Leave,
				X = x,
				Y = y,
			};
		}

		public static MouseEventArgs CreateEnter(int x, int y)
		{
			return new MouseEventArgs()
			{
				Type = MouseEventType.Enter,
				X = x,
				Y = y,
			};
		}

		public static MouseEventArgs CreateButtonDown(int x, int y, MouseButton button)
		{
			return new MouseEventArgs()
			{
				Type = MouseEventType.ButtonDown,
				X = x,
				Y = y,
				Button = button
			};
		}

		public static MouseEventArgs CreateButtonUp(int x, int y, MouseButton button)
		{
			return new MouseEventArgs()
			{
				Type = MouseEventType.ButtonUp,
				X = x,
				Y = y,
				Button = button
			};
		}
	}
}
