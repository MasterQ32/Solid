namespace Solid.UI
{
	using OpenTK;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using OpenTK.Input;

	public class GameWindowInput : InputSource
	{
		private GameWindow current = null;

		public GameWindowInput()
		{

		}

		public GameWindowInput(GameWindow window) : 
			this()
		{
			this.Bind(window);
		}

		public void Bind(GameWindow window)
		{
			if (this.current != null)
				this.Unbind();
			this.current = window;

			this.current.KeyDown += Current_KeyDown;
			this.current.KeyPress += Current_KeyPress;
			this.current.KeyUp += Current_KeyUp;

			this.current.MouseDown += Current_MouseDown;
			this.current.MouseMove += Current_MouseMove;
			this.current.MouseUp += Current_MouseUp;
			this.current.MouseWheel += Current_MouseWheel;
		}

		private void Current_KeyUp(object sender, KeyboardKeyEventArgs e) => this.SendKeyUp(e);

		private void Current_KeyPress(object sender, KeyPressEventArgs e) => this.SendKeyPress(e);

		private void Current_KeyDown(object sender, KeyboardKeyEventArgs e) => this.SendKeyDown(e);

		private void Current_MouseWheel(object sender, MouseWheelEventArgs e) => this.SendMouseWheel(e);

		private void Current_MouseMove(object sender, MouseMoveEventArgs e) => this.SendMouseMove(e);

		private void Current_MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs e) => this.SendMouseDown(e);

		private void Current_MouseUp(object sender, OpenTK.Input.MouseButtonEventArgs e) => this.SendMouseUp(e);

		public void Unbind()
		{
			if (this.current == null)
				throw new InvalidOperationException("No game window is currently bound.");


			this.current.KeyDown -= Current_KeyDown;
			this.current.KeyPress -= Current_KeyPress;
			this.current.KeyUp -= Current_KeyUp;

			this.current.MouseDown -= Current_MouseDown;
			this.current.MouseMove -= Current_MouseMove;
			this.current.MouseUp -= Current_MouseUp;
			this.current.MouseWheel -= Current_MouseWheel;

			this.current = null;
		}
	}
}
