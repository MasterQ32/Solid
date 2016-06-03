using System;

namespace Solid.UI.Input
{
	[Flags]
	public enum MouseButton
	{
		None = 0,
		Left = 1,
		Right = 2,
		Middle = 4,
		NavBack = 8,
		NavForward = 16,
	}
}