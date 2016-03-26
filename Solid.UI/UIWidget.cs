using OpenTK.Graphics;
using Solid.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.UI
{
    public class UIWidget : Widget
    {
		public static readonly SolidProperty BackgroundProperty = SolidProperty.Register<UIWidget, Color4>(nameof(Background), new SolidPropertyMetadata()
		{
			DefaultValue = Color4.Transparent,
			InheritFromHierarchy = true,
		});

		public static readonly SolidProperty ForegroundProperty = SolidProperty.Register<UIWidget, Color4>(nameof(Foreground), new SolidPropertyMetadata()
		{
			DefaultValue = Color4.Black,
			InheritFromHierarchy = true,
		});
		
		public virtual void Draw()
		{

		}

		public Color4 Background
		{
			get { return Get<Color4>(BackgroundProperty); }
			set { Set(BackgroundProperty, value); }
		}

		public Color4 Foreground
		{
			get { return Get<Color4>(BackgroundProperty); }
			set { Set(BackgroundProperty, value); }
		}
	}
}
