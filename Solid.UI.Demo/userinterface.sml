Widget
{
	Panel(Width=300, Height=500, VerticalAlignment=Center, HorizontalAlignment=Center)
	{
		StackPanel(Direction=Vertical, Margin=16)
		{
			Widget(Background="Button", Margin=8, Height=32);
			Widget(Background="Button", Margin=8, Height=32);
			Widget(Background="Button", Margin=8, Height=32);
			Widget(Background="Panel", Margin=8, Height=32);
		}
	}
}