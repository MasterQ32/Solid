namespace Solid.Layout
{
	using static System.Math;

	public class Table : Widget
	{
		public static readonly SolidProperty RowProperty = SolidProperty.Register<Widget, int>("Table.Row");
		public static readonly SolidProperty ColumnProperty = SolidProperty.Register<Widget, int>("Table.Column");

		public static readonly SolidProperty RowsProperty = SolidProperty.Register<Table, int>("Rows");
		public static readonly SolidProperty ColumnsProperty = SolidProperty.Register<Table, int>("Columns");

		public override Size SizeHint
		{
			get
			{
				if (this.Children.Count == 0)
					return base.SizeHint;

				var size = this.DeclaredSize;

				bool modifyX = (size.Width <= 0);
				bool modifyY = (size.Height <= 0);

				Size cellSize = new Size(0, 0);

				foreach (var child in this.Children)
				{
					var ws = child.WidgetSize;
					cellSize.Width = Max(cellSize.Width, ws.Width);
					cellSize.Height = Max(cellSize.Height, ws.Height);
				}

				if (modifyX)
				{
					size.Width = this.Columns * cellSize.Width;
				}
				if (modifyY)
				{
					size.Height = this.Rows * cellSize.Height;
				}

				size.Width += this.Columns * (this.Padding.Left + this.Padding.Right);
				size.Height += this.Rows * (this.Padding.Top + this.Padding.Bottom);

				return size;
			}
		}

		protected override void OnLayout()
		{
			// Trivial
			if (this.Children.Count == 0)
				return;

			var cellSize = new Size(
				this.Size.Width / this.Columns, 
				this.Size.Height / this.Rows);
			foreach (var child in this.Children)
			{
				var c = ColumnProperty.GetValue<int>(child);
				var r = RowProperty.GetValue<int>(child);

				if (c >= this.Columns) continue;
				if (r >= this.Rows) continue;

				var cellPosition = new Point(
					c * cellSize.Width + this.Padding.Left,
					r * cellSize.Height + this.Padding.Top);

				var cellInternalSize = new Size(
					cellSize.Width - this.Padding.Horizontal,
					cellSize.Height - this.Padding.Horizontal);

				child.ApplyAlignment(cellPosition, cellInternalSize);
				child.SetupLayout();
			}
		}

		public int Rows
		{
			get { return Get<int>(RowsProperty); }
			set { Set(RowsProperty, value); }
		}

		public int Columns
		{
			get { return Get<int>(ColumnsProperty); }
			set { Set(ColumnsProperty, value); }
		}
	}
}