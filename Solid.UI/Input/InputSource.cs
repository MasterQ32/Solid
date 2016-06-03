namespace Solid.UI.Input
{
	using System;

	public class InputSource
	{
		public event EventHandler<TextInputEventArgs> TextInput;

		public event EventHandler<KeyboardEventArgs> KeyDown;
		public event EventHandler<KeyboardEventArgs> KeyUp;

		public event EventHandler<MouseEventArgs> MouseDown;
		public event EventHandler<MouseEventArgs> MouseMove;
		public event EventHandler<MouseEventArgs> MouseUp;
		public event EventHandler<MouseEventArgs> MouseWheel;

		public void SendTextInput(TextInputEventArgs e) => this.TextInput?.Invoke(this, e);

		public void SendKeyDown(KeyboardEventArgs e) => this.KeyDown?.Invoke(this, e);

		public void SendKeyUp(KeyboardEventArgs e) => this.KeyUp?.Invoke(this, e);

		public void SendMouseDown(MouseEventArgs e) => this.MouseDown?.Invoke(this, e);

		public void SendMouseMove(MouseEventArgs e) => this.MouseMove?.Invoke(this, e);

		public void SendMouseUp(MouseEventArgs e) => this.MouseUp?.Invoke(this, e);

		public void SendMouseWheel(MouseEventArgs e) => this.MouseWheel?.Invoke(this, e);
	}
}