using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.UI.Demo
{
	class UIDemoProgram : GameWindow
	{
		static void Main(string[] args)
		{
			using (var demo = new UIDemoProgram())
			{
				demo.Run(60, 60);
			}
		}

		UserInterface ui;
		
		public UIDemoProgram() : 
			base(
				1280, 720,
				GraphicsMode.Default,
				"Solid UI Demonstration",
				GameWindowFlags.Default,
				DisplayDevice.Default,
				3, 3,
				GraphicsContextFlags.Debug | GraphicsContextFlags.ForwardCompatible)
		{
			this.ui = UserInterface.Load("userinterface.sml");
		}

		protected override void OnLoad(EventArgs e)
		{
			this.ui.InitializeOpenGL();
		}

		protected override void OnResize(EventArgs e)
		{
			GL.Viewport(0, 0, this.Width, this.Height);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			this.ui.Update(new Layout.Size(this.Width, this.Height));
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GL.ClearColor(Color4.CornflowerBlue);
			GL.ClearDepth(1.0f);
			GL.Clear(ClearBufferMask.ColorBufferBit);

			this.ui.Draw();

			this.SwapBuffers();
		}
	}
}
