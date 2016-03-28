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
				Button(Table.Row=0, Table.Column=0, Click=[Input1]) {
					Label(Text="1");
				}
				Button(Table.Row=0, Table.Column=1, Click=[Input2]) {
					Label(Text="2");
				}
				Button(Table.Row=0, Table.Column=2, Click=[Input3]) {
					Label(Text="3");
				}
				
				Button(Table.Row=1, Table.Column=0, Click=[Input4]) {
					Label(Text="4");
				}
				Button(Table.Row=1, Table.Column=1, Click=[Input5]) {
					Label(Text="5");
				}
				Button(Table.Row=1, Table.Column=2, Click=[Input6]) {
					Label(Text="6");
				}
				
				Button(Table.Row=2, Table.Column=0, Click=[Input7]) {
					Label(Text="7");
				}
				Button(Table.Row=2, Table.Column=1, Click=[Input8]) {
					Label(Text="8");
				}
				Button(Table.Row=2, Table.Column=2, Click=[Input9]) {
					Label(Text="9");
				}
				
				Button(Table.Row=3, Table.Column=0) {
					Label(Text="*");
				}
				Button(Table.Row=3, Table.Column=1, Click=[Input0]) {
					Label(Text="0");
				}
				Button(Table.Row=3, Table.Column=2) {
					Label(Text="#");
				}
			}
		}
	}
}