Skin
{
	Button : Style {
		Default : Texture(Source = "button-normal.png");
		Hovered : Texture(Source = "button-hovered.png");
		Active  : Texture(Source = "button-clicked.png");
	}
	Panel : Style {
		Default : TextureBox(EdgeWidth = 10) {
			TopLeft      : Texture(Source = "panel.png", Rect = "0;0;10;10");
			TopRight     : Texture(Source = "panel.png", Rect = "90;0;10;10");
			BottomLeft   : Texture(Source = "panel.png", Rect = "0;90;10;10");
			BottomRight  : Texture(Source = "panel.png", Rect = "90;90;10;10");
			
			TopMiddle    : Texture(Source = "panel.png", Rect = "10;0;80;10");
			MiddleLeft   : Texture(Source = "panel.png", Rect = "0;10;10;80");
			MiddleRight  : Texture(Source = "panel.png", Rect = "90;10;10;80");
			BottomCenter : Texture(Source = "panel.png", Rect = "10;90;80;10");
			
			Center       : Texture(Source = "panel.png", Rect = "10;10;80;80");
		}
	}
	Crosshair : Style {
		Default : Texture(Source = "red_cross.png");
	}
}