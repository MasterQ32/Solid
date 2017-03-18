using Solid.UI.Skinning;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using Solid.Layout;
using System.IO;
using System.ComponentModel;

namespace Solid.UI.Demo
{
	class UIDemoProgram : System.Windows.Forms.Form, IGraphicsObjectFactory
	{
		static void Main(string[] args)
		{
			Application.Run(new UIDemoProgram());
		}

		UserInterface ui;
		private readonly VirtualGraphics graphics;


		public UIDemoProgram()
		{
			this.graphics = new VirtualGraphics(this, this.CreateGraphics());

			this.ui = new UserInterface();
			this.ui.Skin = Skin.Load(this, "skin.sml");
			this.ui.Input = new WindowsFormsInput(this);

			var form = Form.Load("userinterface.sml", this);
			form.ViewModel = new GameViewModel();

			this.ui.CurrentForm = form;

			var timer = new Timer() { Interval = 25 };
			timer.Tick += (s, e) => { this.ui.Update(this.graphics); this.Invalidate(); };
			timer.Start();

			this.DoubleBuffered = true;
			this.ClientSize = new System.Drawing.Size(800, 600);
		}

		protected override void OnResize(EventArgs e)
		{
			this.ui.Update(this.graphics);
			this.Invalidate();
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			e.Graphics.Clear(Color.Wheat);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			this.graphics.graphics = e.Graphics;
			this.ui.Draw(this.graphics);
		}

		public IBrush CreateBrush(string spec)
		{
			if (Path.GetExtension(spec) != "")
				return new VirtualBrush(new Bitmap(spec));
			else
				return new VirtualBrush(new SolidBrush(System.Drawing.Color.FromName(spec)));
		}

		public IFont CreateFont(string spec)
		{
			var converter = TypeDescriptor.GetConverter(typeof(Font));
			var font = (Font)converter.ConvertFromString(spec) ?? this.Font;
			return new VirtualFont(this.graphics.graphics, font);
		}

		public IPicture CreatePicture(string value)
		{
			return new VirtualPicture(new Bitmap(value));
		}
	}

	sealed class VirtualPicture : IPicture
	{
		private readonly Bitmap bitmap;

		public VirtualPicture(Bitmap bitmap)
		{
			this.bitmap = bitmap;
		}

		public int Height => this.bitmap.Height;

		public int Width => this.bitmap.Width;
		
		public Image Image => this.bitmap;
	}

	sealed class VirtualGraphics : IGraphics
	{
		private readonly System.Windows.Forms.Form form;
		internal Graphics graphics;

		public VirtualGraphics(System.Windows.Forms.Form form, Graphics g)
		{
			this.form = form;
			this.graphics = g;
		}

		public Layout.Size ScreenSize => new Layout.Size(this.form.ClientSize.Width, this.form.ClientSize.Height);

		private static float Clamp(float v, float min, float max)
		{
			if (v < min) return min;
			if (v > max) return max;
			return v;
		}

		private static Color ConvertColor(Skinning.Color c)
		{
			return Color.FromArgb(
				(int)Clamp(255.0f * c.A, 0, 255),
				(int)Clamp(255.0f * c.R, 0, 255),
				(int)Clamp(255.0f * c.G, 0, 255),
				(int)Clamp(255.0f * c.B, 0, 255));
		}

		private static RectangleF ConvertRectangle(UI.Rectangle rect)
		{
			return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
		}

		public void Clear(Skinning.Color color)
		{
			this.graphics.Clear(ConvertColor(color));
		}

		public void DrawBrush(IBrush iBrush, UI.Rectangle target)
		{
			var brush = (VirtualBrush)iBrush;

			if (brush.Bitmap != null)
			{
				graphics.DrawImage(brush.Bitmap, ConvertRectangle(target));
			}
			else
			{
				graphics.FillRectangle(
					brush.Brush,
					ConvertRectangle(target));
			}
		}

		public void DrawString(IFont iFont, Skinning.Color color, UI.Rectangle target, string text)
		{
			var font = (VirtualFont)iFont;

			graphics.DrawString(
				text,
				font.Font,
				new SolidBrush(ConvertColor(color)),
				ConvertRectangle(target));
		}

		public Layout.Size MeasureString(IFont iFont, string text, float? maxWidth = default(float?))
		{
			var font = (VirtualFont)iFont;

			SizeF size;
			if (maxWidth != null)
				size = graphics.MeasureString(text, font.Font, (int)maxWidth);
			else
				size = graphics.MeasureString(text, font.Font);

			return new Layout.Size(size.Width, size.Height);
		}

		public void ResetScissor()
		{
			graphics.ResetClip();
		}

		public void SetScissor(UI.Rectangle rect)
		{
			graphics.SetClip(ConvertRectangle(rect));
		}

		public void DrawPicture(IPicture picture, Rectangle rectangle)
		{
			var vp = (VirtualPicture)picture;
			graphics.DrawImage(vp.Image, ConvertRectangle(rectangle));
		}
	}

	sealed class VirtualBrush : IBrush
	{
		public VirtualBrush(Bitmap bitmap)
		{
			this.Bitmap = bitmap;
		}

		public VirtualBrush(Brush brush)
		{
			this.Brush = brush;
		}

		public Bitmap Bitmap { get; private set; }
		public Brush Brush { get; private set; }
	}

	sealed class VirtualFont : IFont
	{
		private readonly Graphics graphics;

		public VirtualFont(Graphics g, Font font)
		{
			this.graphics = g;
			this.Font = font;
		}

		public Font Font { get; private set; }

		public Layout.Size Measure(string text, float? maxWidth = default(float?))
		{
			Layout.Size size;
			if (maxWidth != null)
				size = Convert(graphics.MeasureString(text, this.Font, (int)maxWidth));
			else
				size = Convert(graphics.MeasureString(text, this.Font));
			if(size.Height < this.Font.GetHeight(graphics))
				size.Height = this.Font.GetHeight(graphics);
			return size;
		}

		static Layout.Size Convert(System.Drawing.SizeF s)
		{
			return new Layout.Size(s.Width, s.Height);
		}
	}
}
