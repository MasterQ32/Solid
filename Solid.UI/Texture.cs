using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.IO;

namespace Solid.UI
{
	public sealed class Texture : IDisposable
	{
		private readonly int id;

		public Texture()
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
			var data = source.LockBits(
				new System.Drawing.Rectangle(0, 0, source.Width, source.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			this.Load(source.Width, source.Height, PixelInternalFormat.Rgba, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
			source.UnlockBits(data);
		}

		public Texture(string fileName) :
			this(new Bitmap(fileName))
		{

		}

		public Texture(Stream source) :
			this(new Bitmap(source))
		{

		}

		public void SetWrap(TextureWrapMode wrap)
		{
			this.Bind();

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)wrap);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrap);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrap);

			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public void Load(
			int width,
			int height,
			PixelInternalFormat internalFormat,
			PixelFormat format, 
			PixelType type, 
			IntPtr scan0)
		{
			GL.BindTexture(TextureTarget.Texture2D, this.id);
			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				internalFormat,
				width, height,
				0,
				format,
				PixelType.UnsignedByte,
				scan0);
			this.Width = width;
			this.Height = height;
			GL.BindTexture(TextureTarget.Texture2D, 0);
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

		public Layout.Size Size => new Layout.Size(this.Width, this.Height);
	}
}