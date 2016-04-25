using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Solid.UI.Skinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

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
			this.ui.Input = new GameWindowInput(this);
			this.ui.ViewModel = new GameViewModel();
		}

		protected override void OnLoad(EventArgs e)
		{
			GL.DebugMessageCallback(this.Callback, IntPtr.Zero);

			this.ui.InitializeOpenGL();
			this.ui.Skin = Skin.Load("skin.sml");
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);
		}

		private void Callback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
		{
			var msg = Marshal.PtrToStringAnsi(message, length);

			switch (severity)
			{
				case DebugSeverity.DebugSeverityHigh:
				{
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				}
				case DebugSeverity.DebugSeverityMedium:
				{
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					break;
				}
				case DebugSeverity.DebugSeverityLow:
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				}
				default:
				case DebugSeverity.DebugSeverityNotification:
				{
					Console.ForegroundColor = ConsoleColor.White;
					break;
				}
			}
			
			Console.WriteLine("[{0}] {1}", source, msg);

			if (severity == DebugSeverity.DebugSeverityHigh)
				System.Diagnostics.Debugger.Break();
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
