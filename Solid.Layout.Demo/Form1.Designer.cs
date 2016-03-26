namespace Solid.Layout.Demo
{
	partial class Form1
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.panelDemo = new System.Windows.Forms.Panel();
			this.buttonReload = new System.Windows.Forms.Button();
			this.textBoxCode = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// panelDemo
			// 
			this.panelDemo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panelDemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.panelDemo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelDemo.Location = new System.Drawing.Point(12, 12);
			this.panelDemo.Name = "panelDemo";
			this.panelDemo.Size = new System.Drawing.Size(642, 315);
			this.panelDemo.TabIndex = 0;
			this.panelDemo.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDemo_Paint);
			this.panelDemo.Resize += new System.EventHandler(this.panelDemo_Resize);
			// 
			// buttonReload
			// 
			this.buttonReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonReload.Location = new System.Drawing.Point(579, 333);
			this.buttonReload.Name = "buttonReload";
			this.buttonReload.Size = new System.Drawing.Size(75, 23);
			this.buttonReload.TabIndex = 1;
			this.buttonReload.Text = "Reload";
			this.buttonReload.UseVisualStyleBackColor = true;
			this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
			// 
			// textBoxCode
			// 
			this.textBoxCode.AcceptsReturn = true;
			this.textBoxCode.AcceptsTab = true;
			this.textBoxCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxCode.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxCode.Location = new System.Drawing.Point(12, 333);
			this.textBoxCode.Multiline = true;
			this.textBoxCode.Name = "textBoxCode";
			this.textBoxCode.Size = new System.Drawing.Size(561, 191);
			this.textBoxCode.TabIndex = 2;
			this.textBoxCode.Text = resources.GetString("textBoxCode.Text");
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(666, 536);
			this.Controls.Add(this.textBoxCode);
			this.Controls.Add(this.buttonReload);
			this.Controls.Add(this.panelDemo);
			this.Name = "Form1";
			this.Text = "Solid.Layout";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panelDemo;
		private System.Windows.Forms.Button buttonReload;
		private System.Windows.Forms.TextBox textBoxCode;
	}
}

