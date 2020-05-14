using System;
using System.Drawing;
using System.Windows.Forms;


namespace creaturevisualizer
{
	sealed class HelpF
		: Form
	{
		#region Fields (static)
		static string Info;
		#endregion Fields (static)


		#region Fields
		string L = Environment.NewLine;
		#endregion Fields


		#region cTor
		internal HelpF()
		{
			InitializeComponent();

			if (Info == null)
			{
				Info = String.Empty;

				// TEMPORARILY DISABLE ALL SAVE OPERATIONS ->
//				Info += "IMPORTANT: When saving a blueprint from the visualizer the output will be based on the"    + L;
//				Info += "creature that is currently displayed in the visualizer. This creature is not necessarily"  + L;
//				Info += "identical to your currently selected blueprint or instance, if changes were done to the"   + L;
//				Info += "latter in the toolset. [F5] Refresh reinstantiates the creature in the visualizer so that" + L;
//				Info += "it is identical to the selected blueprint or instance in the toolset as long as the item-" + L;
//				Info += "processing toggles under the Options menu are all active."                                 + L + L;

				Info += "MENUS" + L;
				Info += "-----" + L;
				Info += "Model" + L;
				Info += "- Refresh [F5] - loads current properties of the currently selected blueprint or instance" + L;
				Info += "- RefreshProtocol" + L;
				Info += "  user-invoked - loads the blueprint or instance only on [F5] Refresh" + L;
				Info += "  on focus - loads the blueprint or instance whenever the visualizer takes focus" + L;
				Info += "  on appearance changed - loads the blueprint or instance whenever its appearance changes" + L;
//				Info += "- save to Module ... [Ctrl+M] - opens a save file dialog at the Module directory" + L;
//				Info += "- save to Campaign ... [Ctrl+G] - opens a save file dialog at the Campaign directory" + L;
//				Info += "- save to file ... [Ctrl+E] - opens a save file dialog at the Override directory" + L;
				Info += "Options" + L;
				Info += "- display equipped body-items - toggles display of equipped items (non hand held)" + L;
				Info += "- display equipped held-items - toggles display of equipped right/left hand items" + L;
//				Info += "- process equipped body-items - toggles display (and output) of equipped items (non hand held)" + L;
//				Info += "- process equipped held-items - toggles display (and output) of equipped right/left hand items" + L;
//				Info += "- process inventory - toggles whether or not to output a creature's inventory items" + L;
				Info += "View" + L;
				Info += "- control panel [Ctrl+P] - toggles on/off the control panel" + L;
				Info += "- mini panel [F7] - toggles on/off quick controls in the display panel" + L;
				Info += "- cycle panel [F8] - cycles the control panel through the cardinal compass points" + L;
				Info += "- stay on top [Ctrl+T] - toggles whether or not the visualizer stays on top of the toolset" + L;
				Info += "Help" + L;
				Info += "- help [F1] - this dialog" + L;
				Info += "- about [F2] - author, version, and build date" + L + L;

				Info += "MOUSE"                             + L;
				Info += "-----"                             + L;
				Info += "LMB+Alt  - pan up/down"            + L;
				Info += "LMB+Ctrl - pan left/right"         + L;
				Info += "RMB      - pan up/down/left/right" + L;
				Info += "RMB+Ctrl - pan polar"              + L + L;

				Info += "BUTTON"                                                       + L;
				Info += "------"                                                       + L;
				Info += "press Ctrl/Shift to increase/decrease rate by a factor of 10" + L + L;

				Info += "TEXT"                                                          + L;
				Info += "----"                                                          + L;
				Info += "use mousewheel or keypad [+/-] to increase/decrease the value" + L;
				Info += "press Ctrl/Shift to increase/decrease rate by a factor of 10"  + L + L;

				Info += "COLORS"                                                     + L;
				Info += "------"                                                     + L;
				Info += "Left-click a color-swatch to open the color-picker dialog." + L;
				Info += "Accept the dialog to enable a color."                       + L + L;
				Info += "LMB - toggles the color off/on"                             + L;
				Info += "RMB - resets the color to default and disables it"          + L + L;
				Info += "Note that [F1] opens Help in the Color picker."             + L + L;

				Info += "PREFS"                                                       + L;
				Info += "-----"                                                       + L;
				Info += @"%localappdata%\NWN2 Toolset\Plugins\CreatureVisualizer.xml" + L;
			}

			rt_help.Text = Info;

			rt_help.SelectionStart  =
			rt_help.SelectionLength = 0;
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnLoad(EventArgs e)
		{
			rt_help.AutoWordSelection = false;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Close();
		}
		#endregion Handlers (override)


		#region Handlers
		void paint_panel(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine(System.Drawing.Pens.DimGray, 0,0, pa_help.Width, 0);
			e.Graphics.DrawLine(System.Drawing.Pens.DimGray, 0,0, 0, pa_help.Height);
		}
		#endregion Handlers



		#region Designer
		RichTextBox rt_help;
		Panel pa_help;


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.rt_help = new System.Windows.Forms.RichTextBox();
			this.pa_help = new System.Windows.Forms.Panel();
			this.pa_help.SuspendLayout();
			this.SuspendLayout();
			// 
			// rt_help
			// 
			this.rt_help.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rt_help.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rt_help.HideSelection = false;
			this.rt_help.Location = new System.Drawing.Point(5, 5);
			this.rt_help.Margin = new System.Windows.Forms.Padding(0);
			this.rt_help.Name = "rt_help";
			this.rt_help.ReadOnly = true;
			this.rt_help.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rt_help.Size = new System.Drawing.Size(587, 369);
			this.rt_help.TabIndex = 0;
			this.rt_help.Text = "";
			// 
			// pa_help
			// 
			this.pa_help.Controls.Add(this.rt_help);
			this.pa_help.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pa_help.Location = new System.Drawing.Point(0, 0);
			this.pa_help.Margin = new System.Windows.Forms.Padding(0);
			this.pa_help.Name = "pa_help";
			this.pa_help.Padding = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this.pa_help.Size = new System.Drawing.Size(592, 374);
			this.pa_help.TabIndex = 1;
			this.pa_help.Paint += new System.Windows.Forms.PaintEventHandler(this.paint_panel);
			// 
			// HelpF
			// 
			this.ClientSize = new System.Drawing.Size(592, 374);
			this.Controls.Add(this.pa_help);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.KeyPreview = true;
			this.Name = "HelpF";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Help";
			this.pa_help.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
