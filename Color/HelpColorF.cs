using System;
using System.Windows.Forms;


namespace creaturevisualizer
{
	sealed class HelpColorF
		: Form
	{
		#region Fields (static)
		static string Info;
		#endregion Fields (static)


		#region Fields
		string L = Environment.NewLine;
		#endregion Fields


		#region cTor
		internal HelpColorF()
		{
			InitializeComponent();

			if (Info == null)
			{
				Info = String.Empty;

				Info += "COLORFIELD"                        + L;
				Info += "----------"                        + L;
				Info += "LMB click - select color point"    + L;
				Info += "LMB drag  - move color point"      + L;
				Info += "Left      - move point left"       + L;
				Info += "Right     - move point right"      + L;
				Info += "Up        - move point up"         + L;
				Info += "Down      - move point down"       + L;
				Info += "Home      - move point left+up"    + L;
				Info += "End       - move point left+down"  + L;
				Info += "PageUp    - move point right+up"   + L;
				Info += "PageDown  - move point right+down" + L + L;

				Info += "COLORSLIDER"                                                         + L;
				Info += "-----------"                                                         + L;
				Info += "LMB click       - select a value for the current colorspace control" + L;
				Info += "LMB drag        - move the value of the current colorspace control"  + L;
				Info += "Subtract/Insert - increase the value"                                + L;
				Info += "Add/Delete      - decrease the value"                                + L + L;

				Info += "COLORBOXES"                             + L;
				Info += "----------"                             + L;
				Info += "RMB click - toggle the active colorbox" + L;
				Info += "[o]       - toggle the active colorbox" + L + L;

				Info += "TEXTBOXES (focused, but not the hexbox)"                  + L;
				Info += "---------"                                                + L;
				Info += "Mousewheel - increase/decrease the value"                 + L;
				Info += "Add        - increase the value"                          + L;
				Info += "Subtract   - decrease the value"                          + L;
				Info += "Spacebar   - select the colorspace control if applicable" + L + L;

				Info += "SWATCHES"                                        + L;
				Info += "--------"                                        + L;
				Info += "LMB click - select color of the active colorbox" + L;
				Info += "RMB click - open the context menu"               + L + L;

				Info += "Note that altering the swatches from the context menu will" + L
					  + "immediately rewrite the swatch file at"                     + L + L;
				Info += @"%localappdata%\NWN2 Toolset\CustomSwatches.xml"            + L;
			}

			tb_help.Text = Info;

			tb_help.SelectionStart  =
			tb_help.SelectionLength = 0;
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Close();
		}
		#endregion Handlers (override)



		#region Designer
		TextBox tb_help;


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.tb_help = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tb_help
			// 
			this.tb_help.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tb_help.Location = new System.Drawing.Point(0, 0);
			this.tb_help.Margin = new System.Windows.Forms.Padding(0);
			this.tb_help.Multiline = true;
			this.tb_help.Name = "tb_help";
			this.tb_help.ReadOnly = true;
			this.tb_help.Size = new System.Drawing.Size(447, 554);
			this.tb_help.TabIndex = 0;
			this.tb_help.WordWrap = false;
			// 
			// HelpColorF
			// 
			this.ClientSize = new System.Drawing.Size(447, 554);
			this.Controls.Add(this.tb_help);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.KeyPreview = true;
			this.Name = "HelpColorF";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Help";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
