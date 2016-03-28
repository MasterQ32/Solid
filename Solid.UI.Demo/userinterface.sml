Widget
{
	Panel(Width=300, VerticalAlignment=Center, HorizontalAlignment=Right, Margin=16, Padding=16)
	{
		StackPanel(Direction=Vertical)
		{
			Button(Margin=8, Width=48, Height=48);
			Button(Margin=8, Width=48, Height=48);
			Button(Margin=8, Width=48, Height=48);
			Button(Background="Panel", Margin=8, Height=32);
		}
	}
	Panel(VerticalAlignment=Center, HorizontalAlignment=Left, Margin=16)
	{
		Table(Rows=4, Columns=3, Margin=16, Padding=8)
		{
			Button(Table.Row=0, Table.Column=0, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="1");
			}
			Button(Table.Row=0, Table.Column=1, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="2");
			}
			Button(Table.Row=0, Table.Column=2, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="3");
			}
			
			Button(Table.Row=1, Table.Column=0, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="4");
			}
			Button(Table.Row=1, Table.Column=1, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="5");
			}
			Button(Table.Row=1, Table.Column=2, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="6");
			}
			
			Button(Table.Row=2, Table.Column=0, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="7");
			}
			Button(Table.Row=2, Table.Column=1, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="8");
			}
			Button(Table.Row=2, Table.Column=2, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="9");
			}
			
			Button(Table.Row=3, Table.Column=0, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="*");
			}
			Button(Table.Row=3, Table.Column=1, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="0");
			}
			Button(Table.Row=3, Table.Column=2, Width=48, Height=48, Padding="4;0;0;10") {
				Label(Text="#");
			}
		}
	}
}