using SharpFont;
using System.Drawing;

namespace Solid.UI
{
	/// <summary>
	/// Loads UI resources.
	/// </summary>
	public interface IResourceLoader
	{
		/// <summary>
		/// Loads a texture by an abstract resource name.
		/// </summary>
		/// <param name="resourceName"></param>
		/// <returns></returns>
		Texture LoadTexture(string resourceName);

		/// <summary>
		/// Loads a bitmap by an abstract resource name.
		/// </summary>
		/// <param name="resourceName"></param>
		/// <returns></returns>
		Bitmap LoadBitmap(string resourceName);

		/// <summary>
		/// Loads a font face by an abstract resource name.
		/// </summary>
		/// <param name="resourceName"></param>
		/// <returns></returns>
		Face LoadFont(string resourceName);
	}
}