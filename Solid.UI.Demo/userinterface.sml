Widget
{
	Panel(Width=300, VerticalAlignment=Center, HorizontalAlignment=Right, Margin=16)
	{
		StackPanel(Direction=Vertical, Margin=16)
		{
			Button(Margin=8, Width=48, Height=48);
			Button(Margin=8, Width=48, Height=48);
			Button(Margin=8, Width=48, Height=48);
			Button(Background="Panel", Margin=8, Height=32);
		}
	}
	Panel(VerticalAlignment=Center, HorizontalAlignment=Left, Margin=16)
	{
		Table(Rows=4, Columns=3, Margin=16)
		{
			Button(Table.Row=0, Table.Column=0, Margin=8, Width=48, Height=48);
			Button(Table.Row=0, Table.Column=1, Margin=8, Width=48, Height=48);
			Button(Table.Row=0, Table.Column=2, Margin=8, Width=48, Height=48);
			
			Button(Table.Row=1, Table.Column=0, Margin=8, Width=48, Height=48);
			Button(Table.Row=1, Table.Column=1, Margin=8, Width=48, Height=48);
			Button(Table.Row=1, Table.Column=2, Margin=8, Width=48, Height=48);
			
			Button(Table.Row=2, Table.Column=0, Margin=8, Width=48, Height=48);
			Button(Table.Row=2, Table.Column=1, Margin=8, Width=48, Height=48);
			Button(Table.Row=2, Table.Column=2, Margin=8, Width=48, Height=48);
			
			Button(Table.Row=3, Table.Column=0, Margin=8, Width=48, Height=48);
			Button(Table.Row=3, Table.Column=1, Margin=8, Width=48, Height=48);
			Button(Table.Row=3, Table.Column=2, Margin=8, Width=48, Height=48);
		}
	}
}