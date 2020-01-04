using System;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorEditForm
	public sealed class ColorF
		: Form
	{
		#region Fields (static)
		internal static ColorF That;

		internal static bool reallyDesignMode = true;
		#endregion Fields (static)


		#region Properties
		internal ColorControl ColorControl
		{
			get { return colorControl; }
		}
		#endregion Properties


		#region cTor
		public ColorF()
		{
			reallyDesignMode = false;	// -> cTor doesn't run in DesignMode.
			That = this;				// -> req'd before InitializeComponent() by SwatchIo.Read()

			InitializeComponent();

			_tconsole.Interval = 3500;
			_tconsole.Tick += tick_console;

#if DEBUG
			ColorControl.InitialColor(System.Drawing.Color.White); // <- STANDALONE PROGRAM START - TEST ONLY <<-
#endif
		}
		#endregion cTor


		#region Console
		Timer _tconsole = new Timer();
		bool _busy;

		internal void Print(string text, bool force = false)
		{
			if (force || !_busy)
			{
				la_console.Text = text;

				if (force)
				{
					_busy = true;
					_tconsole.Stop();
					_tconsole.Start(); // ie. restart
				}
			}
		}

		void tick_console(object sender, EventArgs e)
		{
			_busy = false;
			_tconsole.Stop();
		}
		#endregion Console


		#region Handlers (override)
		protected override void OnKeyUp(KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.B:
					if (!ColorControl.IsTextboxFocused(Controls))
					{
						e.Handled = e.SuppressKeyPress = true;

						var args = new MouseEventArgs(MouseButtons.Right, 0, 0,0, 0);
						ColorControl.mouseup_colorbox(null, args);
					}
					break;
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.PageUp:
					if (!ColorControl.IsTextboxFocused(Controls))
					{
						e.Handled = e.SuppressKeyPress = true;
						ColorControl.colorslider.ChangeValue_key(+1);
					}
					break;

				case Keys.PageDown:
					if (!ColorControl.IsTextboxFocused(Controls))
					{
						e.Handled = e.SuppressKeyPress = true;
						ColorControl.colorslider.ChangeValue_key(-1);
					}
					break;
			}
		}
		#endregion Handlers (override)



		#region Designer
		ColorControl colorControl;

		Button bu_Okay;
		Button bu_Cancel;
		Label la_console;


		void InitializeComponent()
		{
			this.la_console = new System.Windows.Forms.Label();
			this.bu_Okay = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.colorControl = new creaturevisualizer.ColorControl();
			this.SuspendLayout();
			// 
			// la_console
			// 
			this.la_console.BackColor = System.Drawing.SystemColors.Info;
			this.la_console.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.la_console.ForeColor = System.Drawing.SystemColors.InfoText;
			this.la_console.Location = new System.Drawing.Point(91, 301);
			this.la_console.Margin = new System.Windows.Forms.Padding(0);
			this.la_console.Name = "la_console";
			this.la_console.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this.la_console.Size = new System.Drawing.Size(298, 17);
			this.la_console.TabIndex = 3;
			// 
			// bu_Okay
			// 
			this.bu_Okay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Okay.Location = new System.Drawing.Point(90, 270);
			this.bu_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Okay.Name = "bu_Okay";
			this.bu_Okay.Size = new System.Drawing.Size(300, 29);
			this.bu_Okay.TabIndex = 0;
			this.bu_Okay.Text = "Book em Dano";
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(3, 270);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(83, 49);
			this.bu_Cancel.TabIndex = 1;
			this.bu_Cancel.Text = "Cancel";
			// 
			// colorControl
			// 
			this.colorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.colorControl.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colorControl.Location = new System.Drawing.Point(0, 0);
			this.colorControl.Margin = new System.Windows.Forms.Padding(0);
			this.colorControl.Name = "colorControl";
			this.colorControl.Size = new System.Drawing.Size(494, 322);
			this.colorControl.TabIndex = 2;
			// 
			// ColorF
			// 
			this.AcceptButton = this.bu_Okay;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(494, 322);
			this.Controls.Add(this.la_console);
			this.Controls.Add(this.bu_Okay);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.colorControl);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ColorF";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Color";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
