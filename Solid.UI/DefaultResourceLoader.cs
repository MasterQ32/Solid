using System;
using System.Drawing;
using SharpFont;

namespace Solid.UI
{
	public sealed class DefaultResourceLoader : IResourceLoader
	{
		public Bitmap LoadBitmap(string resourceName)
		{
			return new Bitmap(resourceName);
		}

		public Face LoadFont(string resourceName)
		{
			return new Face(UserInterface.FontLibrary, resourceName, 0);
		}

		public Texture LoadTexture(string resourceName)
		{
			return new Texture(resourceName);
		}
	}
}