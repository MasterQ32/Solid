using Solid.Markup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solid.Layout.Demo
{
	public partial class Form1 : Form
	{
		LayoutDocument layout;

		public Form1()
		{
			InitializeComponent();
		}

		private void RenderWidgets(Graphics g, Widget widget)
		{
			var drawable = widget as ColorWidget;
			drawable?.Paint(g);

			foreach (var child in widget.Children)
			{
				RenderWidgets(g, child);
			}
		}

		private void panelDemo_Paint(object sender, PaintEventArgs e)
		{
			if (this.layout == null)
			{
				e.Graphics.DrawString(
					"Please enter some layout code below...",
					this.Font,
					Brushes.Black,
					8.0f, 8.0f);
				return;
			}
			e.Graphics.Clear(this.panelDemo.BackColor);
			RenderWidgets(e.Graphics, this.layout.Root);
		}

		private void panelDemo_Resize(object sender, EventArgs e)
		{
			this.layout?.Update(new Solid.Layout.Size(this.panelDemo.ClientSize.Width - 1, this.panelDemo.ClientSize.Height - 1));
			this.panelDemo.Invalidate();
		}

		private void buttonReload_Click(object sender, EventArgs e)
		{
			var code = this.textBoxCode.Text;

			try
			{
				var document = Parser.Parse(code);

				var mapper = new LayoutMapper();
				mapper.RegisterType<ColorWidget>();
				mapper.RegisterConverter<Color, ColorConverter>();

				this.layout = mapper.Instantiate(document);

				panelDemo_Resize(sender, e);

				this.panelDemo.Invalidate();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString());
			}
		}
	}
}
