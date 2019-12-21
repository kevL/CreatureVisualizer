using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.DragForm
	sealed class DragForm
		: Form
	{
		#region Fields
		Size _size;
		#endregion Fields


		#region Properties
		internal int CursorXDifference
		{ get; set; }

		internal int CursorYDifference
		{ get; set; }
		#endregion Properties


		internal DragForm()
		{
			ShowInTaskbar = false;
			TopMost = true;
			FormBorderStyle = FormBorderStyle.FixedToolWindow;
			Size = new Size(0,0);

			ShowForm();

			Hide();
		}


		internal void ChangeSize(Size size)
		{
			_size = size;
			Size = size + new Size(6, 22);
			Region = new Region(new Rectangle(new Point(3, 19), size)); // TODO: <- test that
		}

		internal void UpdateLocation(Point newLocation)
		{
			Location = new Point(newLocation.X - 3, newLocation.Y - 19);
		}

		/// <summary>
		/// kL_add: Cf Sano.Utility.NativeMethods.Window.
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="nCmdShow"></param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern IntPtr ShowWindow(IntPtr hwnd, int nCmdShow);

		internal void ShowForm()
		{
			Opacity = 0.5;
			ShowWindow(Handle, 4);
		}

		internal Point GetCurrentLocation()
		{
			return new Point(Location.X +  3,
							 Location.Y + 19);
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(Pens.Black,
									 0,0,
									 _size.Width - 1, _size.Height - 1);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 132)
			{
				m.Result = (IntPtr)(-1);
			}
			else
				base.WndProc(ref m);
		}



		#region Designer
		void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// DragForm
			// 
			this.ClientSize = new System.Drawing.Size(292, 274);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "DragForm";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
