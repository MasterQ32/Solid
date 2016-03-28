namespace Solid.UI
{
	using OpenTK;
	using OpenTK.Input;
	using System;

	public class InputSource
	{
		public event EventHandler<KeyboardKeyEventArgs> KeyDown;
		public event EventHandler<KeyPressEventArgs> KeyPress;
		public event EventHandler<KeyboardKeyEventArgs> KeyUp;

		public event EventHandler<MouseButtonEventArgs> MouseDown;
		public event EventHandler<MouseMoveEventArgs> MouseMove;
		public event EventHandler<MouseButtonEventArgs> MouseUp;
		public event EventHandler<MouseWheelEventArgs> MouseWheel;

		public void SendKeyDown(KeyboardKeyEventArgs e) => this.KeyDown?.Invoke(this, e);

		public void SendKeyPress(KeyPressEventArgs e) => this.KeyPress?.Invoke(this, e);

		public void SendKeyUp(KeyboardKeyEventArgs e) => this.KeyUp?.Invoke(this, e);

		public void SendMouseDown(MouseButtonEventArgs e) => this.MouseDown?.Invoke(this, e);

		public void SendMouseMove(MouseMoveEventArgs e) => this.MouseMove?.Invoke(this, e);

		public void SendMouseUp(MouseButtonEventArgs e) => this.MouseUp?.Invoke(this, e);

		public void SendMouseWheel(MouseWheelEventArgs e) => this.MouseWheel?.Invoke(this, e);
	}
}