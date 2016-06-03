Widget
{
	Panel(VerticalAlignment=Center, HorizontalAlignment=Center, Margin=16)
	{
		DockPanel(Margin=16) {
			Panel(DockPanel.Dock=Top) {
				Label(HorizontalAlignment=Left, Text=[DisplayText], Margin=10);
			}
			Table(Rows=4, Columns=3, Padding=8)
			{
				Button(Table.Row=0, Table.Column=0, ClickCommand=[Input1]) {
					Label(Text="1");
				}
				Button(Table.Row=0, Table.Column=1, ClickCommand=[Input2]) {
					Label(Text="2");
				}
				Button(Table.Row=0, Table.Column=2, ClickCommand=[Input3]) {
					Label(Text="3");
				}
				
				Button(Table.Row=1, Table.Column=0, ClickCommand=[Input4]) {
					Label(Text="4");
				}
				Button(Table.Row=1, Table.Column=1, ClickCommand=[Input5]) {
					Label(Text="5");
				}
				Button(Table.Row=1, Table.Column=2, ClickCommand=[Input6]) {
					Label(Text="6");
				}
				
				Button(BindingSource=[LowerRow], Table.Row=2, Table.Column=0, ClickCommand=[Input7]) {
					Label(Text="7");
				}
				Button(BindingSource=[LowerRow], Table.Row=2, Table.Column=1, ClickCommand=[Input8]) {
					Label(Text="8");
				}
				Button(BindingSource=[LowerRow], Table.Row=2, Table.Column=2, ClickCommand=[Input9]) {
					Label(Text="9");
				}
				
				Button(Table.Row=3, Table.Column=0, ClickCommand=[Clear]) {
					
				}
				Button(Table.Row=3, Table.Column=1, ClickCommand=[Input0]) {
					Label(Text="0");
				}
				Button(Table.Row=3, Table.Column=2, ClickCommand=[Check]) {
					
				}
			}
		}
	}

	StackPanel(Template = "pressed-number.sml", Items = [PressedNumbers], Direction=Vertical, HorizontalAlignment=Right, Width=200);
}


Image(VerticalAlignment=Center, HorizontalAlignment=Center, Source = "red_cross.png", IsTouchable=False);
Image(VerticalAlignment=Center, HorizontalAlignment=Center, Source = "green_checkmark.png", IsTouchable=False);