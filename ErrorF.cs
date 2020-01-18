using System;
using System.Windows.Forms;


namespace creaturevisualizer
{
	sealed class ErrorF
		: Form
	{
		#region cTor
		internal ErrorF(string error)
		{
			InitializeComponent();

			TopMost = CreatureVisualizerPreferences.that.StayOnTop;

			la_error.Text = error;
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
		Label la_error;


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.la_error = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// la_error
			// 
			this.la_error.Dock = System.Windows.Forms.DockStyle.Fill;
			this.la_error.Location = new System.Drawing.Point(0, 0);
			this.la_error.Margin = new System.Windows.Forms.Padding(0);
			this.la_error.Name = "la_error";
			this.la_error.Padding = new System.Windows.Forms.Padding(3, 5, 0, 0);
			this.la_error.Size = new System.Drawing.Size(292, 74);
			this.la_error.TabIndex = 0;
			// 
			// ErrorF
			// 
			this.ClientSize = new System.Drawing.Size(292, 74);
			this.Controls.Add(this.la_error);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ErrorF";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Error";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
