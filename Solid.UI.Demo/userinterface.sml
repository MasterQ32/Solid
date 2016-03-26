Widget(Background = "paper_background.png", Skin="skin.sml")
{
	Widget(Width=300, Height=500, VerticalAlignment=Center, HorizontalAlignment=Center)
	{
		Widget(Background="wood_background.png", Margin=16)
		{
			StackPanel(Direction=Vertical)
			{
				Widget(Background=Silver, Margin=8, Height=32);
				Widget(Background=Silver, Margin=8, Height=32);
				Widget(Background=Silver, Margin=8, Height=32);
			}
		}
	}
}