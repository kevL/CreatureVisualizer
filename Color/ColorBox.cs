using System;
using System.Drawing;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorBox
	sealed class ColorBox
		: UserControl
	{
		#region Properties
		bool _isActive;
		internal bool IsActive
		{
			get { return _isActive; }
			set
			{
				_isActive = value;
				Invalidate();
			}
		}
		#endregion Properties


		#region cTor
		public ColorBox()
		{
			InitializeComponent();
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnPaint(PaintEventArgs e)
		{
			using (var brush = new SolidBrush(BackColor))
				e.Graphics.FillRectangle(brush, ClientRectangle);

			// Muahaha
			e.Graphics.DrawLine(Pens.Black, 1,         0, 1,         Height);
			e.Graphics.DrawLine(Pens.Black, Width - 2, 0, Width - 2, Height);

			if (IsActive)
			{
				e.Graphics.DrawLine(Pens.Black, 0,         0, 0,         Height);
				e.Graphics.DrawLine(Pens.Black, Width - 1, 0, Width - 1, Height);
			}
			else
			{
				e.Graphics.DrawLine(SystemPens.Control, 0,         0, 0,         Height);
				e.Graphics.DrawLine(SystemPens.Control, Width - 1, 0, Width - 1, Height);
			}
		}
		#endregion Handlers (override)



		#region Designer
		void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// ColorBox
			// 
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorBox";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
