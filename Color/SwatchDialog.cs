using System;
using System.Drawing;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.AddNewColorSwatchForm
	sealed class SwatchDialog
		: Form
	{
		#region Fields
		Color _color;
		#endregion Fields


		#region Properties
		internal string Description
		{
			get { return tb_Description.Text; }
		}
		#endregion Properties


		#region cTor
		/// <summary>
		/// add swatch
		/// </summary>
		/// <param name="color"></param>
		internal SwatchDialog(Color color)
		{
			InitializeComponent();

			Text = "swatch";

			_color = color;
		}

		/// <summary>
		/// relabel swatch
		/// </summary>
		/// <param name="swatch"></param>
		internal SwatchDialog(Swatch swatch)
		{
			InitializeComponent();

			Text = "relabel";
			tb_Description.Text = swatch.Description;

			tb_Description.SelectionLength = 0;
			tb_Description.SelectionStart = tb_Description.Text.Length;

			_color = swatch.Color;
		}
		#endregion cTor


		#region Handlers
		void click_ok(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(tb_Description.Text))
				tb_Description.Text = Swatch.NoLabel;

			Close();
		}

		void keyup_description(object sender, KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.Return:
					click_ok(null, EventArgs.Empty);
					break;

				case Keys.Escape:
					Close();
					break;
			}
		}

		void paint_color(object sender, PaintEventArgs e)
		{
			using (var brush = new SolidBrush(_color))
				e.Graphics.FillRectangle(brush, ClientRectangle);
		}
		#endregion Handlers



		#region Designer
		TextBox tb_Description;
		Button bu_Ok;
		Panel pa_Color;
		Button bu_Cancel;


		void InitializeComponent()
		{
			this.pa_Color = new System.Windows.Forms.Panel();
			this.tb_Description = new System.Windows.Forms.TextBox();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Ok = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pa_Color
			// 
			this.pa_Color.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.pa_Color.BackgroundImage = global::CreatureVisualizer.Properties.Resources.checkers;
			this.pa_Color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_Color.Location = new System.Drawing.Point(5, 23);
			this.pa_Color.Margin = new System.Windows.Forms.Padding(0);
			this.pa_Color.Name = "pa_Color";
			this.pa_Color.Size = new System.Drawing.Size(162, 62);
			this.pa_Color.TabIndex = 1;
			this.pa_Color.Paint += new System.Windows.Forms.PaintEventHandler(this.paint_color);
			// 
			// tb_Description
			// 
			this.tb_Description.Dock = System.Windows.Forms.DockStyle.Top;
			this.tb_Description.Location = new System.Drawing.Point(0, 0);
			this.tb_Description.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Description.Name = "tb_Description";
			this.tb_Description.Size = new System.Drawing.Size(172, 20);
			this.tb_Description.TabIndex = 0;
			this.tb_Description.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tb_Description.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyup_description);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(4, 88);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(82, 25);
			this.bu_Cancel.TabIndex = 2;
			this.bu_Cancel.Text = "Cancel";
			// 
			// bu_Ok
			// 
			this.bu_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bu_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Ok.Location = new System.Drawing.Point(87, 88);
			this.bu_Ok.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Ok.Name = "bu_Ok";
			this.bu_Ok.Size = new System.Drawing.Size(82, 25);
			this.bu_Ok.TabIndex = 3;
			this.bu_Ok.Text = "Ok";
			this.bu_Ok.Click += new System.EventHandler(this.click_ok);
			// 
			// SwatchDialog
			// 
			this.AcceptButton = this.bu_Ok;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(172, 115);
			this.Controls.Add(this.bu_Ok);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.pa_Color);
			this.Controls.Add(this.tb_Description);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SwatchDialog";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
