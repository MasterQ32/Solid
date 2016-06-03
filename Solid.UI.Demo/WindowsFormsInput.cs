namespace Solid.UI.Demo
{
	using Solid.UI.Input;
	using Form = System.Windows.Forms.Form;

	public sealed class WindowsFormsInput : InputSource
	{
		private Form form;
		
		public WindowsFormsInput(Form form)
		{
			this.form = form;
			this.form.MouseMove += Form_MouseMove;
			this.form.MouseDown += Form_MouseDown;
			this.form.MouseUp += Form_MouseUp;
		}

		private void Form_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			switch (e.Button)
			{
				case System.Windows.Forms.MouseButtons.Left:
					this.SendMouseUp(MouseEventArgs.CreateButtonUp(e.X, e.Y, MouseButton.Left));
					break;
			}
			form.Invalidate();
		}

		private void Form_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			switch(e.Button)
			{
				case System.Windows.Forms.MouseButtons.Left:
					this.SendMouseDown(MouseEventArgs.CreateButtonDown(e.X, e.Y, MouseButton.Left));
					break;
			}
			form.Invalidate();
		}

		private void Form_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.SendMouseMove(MouseEventArgs.CreateMovement(e.X, e.Y));
			form.Invalidate();
		}
	}
}