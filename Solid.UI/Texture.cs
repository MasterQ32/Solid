using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;

namespace Solid.UI
{
	public sealed class Texture : IDisposable
	{
		private readonly int id;

		private Texture()
		{
			this.id = GL.GenTexture();

			GL.BindTexture(TextureTarget.Texture2D, this.id);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public Texture(Bitmap source) :
			this()
		{
			GL.BindTexture(TextureTarget.Texture2D, this.id);

			var data = source.LockBits(
				new System.Drawing.Rectangle(0, 0, source.Width, source.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				PixelInternalFormat.Rgba,
				source.Width, source.Height,
				0,
				PixelFormat.Bgra,
				PixelType.UnsignedByte,
				data.Scan0);
			source.UnlockBits(data);
			this.Width = source.Width;
			this.Height = source.Height;
		}

		public Texture(string fileName) :
			this(new Bitmap(fileName))
		{

		}

		public void Bind()
		{
			GL.BindTexture(TextureTarget.Texture2D, this.id);
		}

		public void Dispose()
		{
			GL.DeleteTexture(this.id);
		}

		public int Width { get; private set; }

		public int Height { get; private set; }
	}
}