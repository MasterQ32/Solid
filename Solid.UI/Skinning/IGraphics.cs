using Solid.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.UI.Skinning
{
	public interface IGraphics
	{
		/// <summary>
		/// Returns the screen size of the graphics object.
		/// </summary>
		Size ScreenSize { get; }

		/// <summary>
		/// Draws the given picture.
		/// </summary>
		/// <param name="picture"></param>
		/// <param name="rectangle"></param>
		void DrawPicture(IPicture picture, Rectangle rectangle);

		/// <summary>
		/// Clears full the virtual screen.
		/// </summary>
		/// <param name="color"></param>
		/// <remarks>This function is not affected by the current scissor.</remarks>
		void Clear(Color color);

		/// <summary>
		/// Draws a specific brush.
		/// </summary>
		/// <param name="brush"></param>
		/// <param name="target"></param>
		void DrawBrush(IBrush brush, Rectangle target);

		/// <summary>
		/// Draws a given text with a specified font.
		/// </summary>
		/// <param name="font"></param>
		/// <param name="target"></param>
		/// <param name="text"></param>
		void DrawString(IFont font, Color color, Rectangle target, string text);

		/// <summary>
		/// Sets the area on the virtual screen we want to draw on.
		/// </summary>
		/// <param name="rect"></param>
		void SetScissor(Rectangle rect);

		/// <summary>
		/// Disables scissoring
		/// </summary>
		void ResetScissor();
	}
}
