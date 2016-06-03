Skin
{
	Button : Style(
		Width=48, Height=48, 
		Padding="0;0;0;10",
		Background = "button-normal.png",
		BackgroundHovered = "button-hovered.png");
	Label : Style(
		Font="Courier New;12pt",
		FontColor="1;1;1");
	Panel : Style(
		Background = "panel.png");
}

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