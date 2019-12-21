using System;
using System.Drawing;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorBox
	sealed class ColorBox
		: UserControl
	{
		#region Fields
		DragForm _dragger = new DragForm();
		#endregion Fields


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
			if (IsActive)
			{
				if (GradientService.IsBright(BackColor))
				{
					e.Graphics.DrawString("a", Font, Brushes.Black, 2,2);
				}
				else
				{
					using (var font = new Font(Font, FontStyle.Bold))
					{
						e.Graphics.DrawString("a", font, Brushes.White, 2,2);
					}
				}
			}
		}

//		protected override void OnLoad(EventArgs e)
//		{
//			_dragger = new DragForm();
//		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left)
			{
				_dragger.UpdateLocation(Parent.PointToScreen(Location));
				_dragger.CursorXDifference = Cursor.Position.X - _dragger.Location.X;
				_dragger.CursorYDifference = Cursor.Position.Y - _dragger.Location.Y;
				_dragger.BackColor = BackColor;
				_dragger.ChangeSize(Size);
				_dragger.ShowForm();

				DoDragDrop(BackColor, DragDropEffects.Move);
			}
		}

		protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
		{
			gfbevent.UseDefaultCursors = false;
			Cursor.Current = Cursors.Hand;

			base.OnGiveFeedback(gfbevent);
		}

		protected override void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent)
		{
			if (qcdevent.Action == DragAction.Cancel || qcdevent.Action == DragAction.Drop)
			{
				_dragger.Hide();
			}
			else
				_dragger.Location = new Point(Cursor.Position.X - _dragger.CursorXDifference, Cursor.Position.Y - _dragger.CursorYDifference);

			base.OnQueryContinueDrag(qcdevent);
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			object data = drgevent.Data.GetData(typeof(Color));
			if (data != null && !((Color)data).Equals(BackColor))
				drgevent.Effect = DragDropEffects.Move;

			base.OnDragOver(drgevent);
		}

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			base.OnDragDrop(drgevent);

			BackColor = (Color)drgevent.Data.GetData(typeof(Color));
			drgevent.Effect = DragDropEffects.None;
		}
		#endregion Handlers (override)



		#region Designer
		protected override void Dispose(bool disposing)
		{
			if (disposing && _dragger != null)
				_dragger.Dispose();

			base.Dispose(disposing);
		}


		void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// ColorBox
			// 
			this.AllowDrop = true;
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorBox";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
