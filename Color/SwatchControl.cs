using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSwatchPanel
	sealed class SwatchControl
		: UserControl
	{
		#region Events
		public event SwatchSelectedEventHandler SwatchSelected;
		#endregion Events


		#region Fields (static)
		internal static string _path;

		const int MaxSwatches = 175;

		const  int _tile     =  12; // x/y tile pixels

		const int _x         =   5; // x-start of 1st tile location in pixels
		const int _y         =   5; // y-start of 1st tile location in pixels

		const int _horitiles =   7; // count of tiles in a row
		#endregion Fields (static)


		#region Fields
		string SwatchFile = "NWN2 Toolset" + Path.DirectorySeparatorChar + "CustomSwatches.xml";

		readonly DragForm _dragger = new DragForm();

		List<Swatch> _swatchlist;

		Bitmap _graphic;

		Swatch[] _swatcharray = new Swatch[MaxSwatches];
		Swatch _lastswatch;

		int _id;
		int _id1;

		int _count;
		int _blank;

		bool _highlight;

//		bool _track;
		#endregion Fields


		#region cTor
		public SwatchControl()
		{
			InitializeComponent();

			_path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			_path = Path.Combine(_path, SwatchFile);

			if (!File.Exists(_path))
			{
				_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				_path = Path.Combine(_path, SwatchFile);
			}

			if (File.Exists(_path))
			{
				_swatchlist = ColorSwatchXml.ReadSwatches(_path);
			}
/*			else
			{
//				ColorSwatchXml.CreateCustomSwatchesFile();
//				_swatchlist = ColorSwatchXml.ReadSwatches("ColorSwatches.xml", true);

				// TODO: "CustomSwatches.xml was not found in either the Local or Roaming directories."
			} */
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnPaint(PaintEventArgs e)
		{
//			ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(0,0, _outerWidth, _outerHeight));

			if (_graphic != null || (_graphic = CreateGraphic()) != null)
			{
				e.Graphics.DrawImage(_graphic, 0,0);

				if (_highlight)
					e.Graphics.DrawRectangle(Pens.Yellow, _swatcharray[_id1].Rect);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
//			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
			{
				if (InsideGrid(e.X, e.Y))
				{
					switch (e.Button)
					{
						case MouseButtons.Left:
						{
							Swatch swatch = GetColorSwatch(e.X, e.Y);

							if (swatch.Color != Color.Empty && swatch.Rect.Contains(e.X, e.Y))
								Cursor = Cursors.Hand;

							break;
						}

						case MouseButtons.Right:
							_id = GetSwatchId(e.X, e.Y);
							break;
					}
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_dragger.Hide();

			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
			{
				if (InsideGrid(e.X, e.Y))
				{
					Swatch swatch = GetColorSwatch(e.X, e.Y);

					if (swatch.Color != Color.Empty && swatch.Rect.Contains(e.X, e.Y))
					{
						switch (e.Button)
						{
							case MouseButtons.Left:
								if (SwatchSelected != null)
									SwatchSelected(new ColorEventArgs(swatch.Color));
								break;

							case MouseButtons.Right:
								context.Show(this, new Point(e.X, e.Y));
								break;
						}
					}
				}
//				else _track = false;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
//			if (_track)
//			{
//				_dragger.Location = PointToScreen(new Point(e.X - _dragger.CursorXDifference, e.Y - _dragger.CursorYDifference));
//			}
//			else
			if (InsideGrid(e.X, e.Y))
			{
				Swatch swatch = GetColorSwatch(e.X, e.Y);

				if (swatch.Color != Color.Empty && swatch.Rect.Contains(e.X, e.Y))
				{
					if (!_lastswatch.Equals(swatch))
						colorTip.Active = false; // wtf

					if (swatch.Description != null && !swatch.Description.Equals(colorTip.GetToolTip(this)))
						colorTip.SetToolTip(this, swatch.Description);

					colorTip.Active = true; // wtf

					_lastswatch = swatch;
					Cursor = Cursors.Hand;
				}
				else
				{
					Cursor = Cursors.Default;
					colorTip.Active = false;
				}
			}
			else
				Cursor = Cursors.Default;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			_dragger.Hide();

			base.OnMouseLeave(e);
		}

		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			ToggleEmptySwatchState(true);

			base.OnDragEnter(drgevent);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			ToggleEmptySwatchState(false);

			base.OnDragLeave(e);
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			drgevent.Effect = DragDropEffects.Move;

			base.OnDragOver(drgevent);
		}

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			var color = (Color)drgevent.Data.GetData(typeof(Color));
			AddColor(color);

			drgevent.Effect = DragDropEffects.None;

			base.OnDragDrop(drgevent);
		}
		#endregion Handlers (override)


		#region Handlers
		void click_delete(object sender, EventArgs e)
		{
			if (_id != -1)
			{
				using (Graphics graphics = Graphics.FromImage(_graphic))
				{
					int valid = _id + (_count - 1) - _id - _blank;

					for (int id = _id; id <= valid; ++id)
					{
						if (id + 1 < _swatcharray.Length - 1)
						{
							Swatch swatch = _swatcharray[id];
							swatch.Color       = _swatcharray[id + 1].Color;
							swatch.Description = _swatcharray[id + 1].Description;
							_swatcharray[id] = swatch; // effin structs
						}
						else // clean the last valid swatch ->
						{
							Swatch swatch = _swatcharray[id];
							swatch.Color = Color.Empty;
							swatch.Description = String.Empty;
							_swatcharray[id] = swatch; // effin structs

							_id1 = id;
						}
//						if (_swatcharray[id].Color == Color.Empty)
//							_id1 = id;

						DrawSwatch(graphics, id);
						InvalidateSwatch(_swatcharray[id].Location); // is that redundant
					}

					++_blank;
				}

				_id = -1;
				ColorSwatchXml.WriteSwatches(SwatchFile, _swatcharray);
			}
		}

		void click_relabel(object sender, EventArgs e)
		{
			using (var f = new SwatchDialog(_swatcharray[_id]))
			{
				f.StartPosition = FormStartPosition.CenterParent;
				f.ShowInTaskbar = false;

				if (f.ShowDialog() == DialogResult.OK)
				{
					_swatcharray[_id].Description = f.ColorDescription;
					ColorSwatchXml.WriteSwatches(SwatchFile, _swatcharray);
				}
			}
		}
		#endregion Handlers


		#region Methods
		Bitmap CreateGraphic()
		{
			if (_swatchlist != null)
			{
				var b = new Bitmap(Width, Height);

				using (Graphics graphics = Graphics.FromImage(b))
				{
					int x = _x;
					int y = _y;

					int id = 0;
					for (; id != _swatchlist.Count && id != MaxSwatches; ++id)
					{
						Swatch swatch = _swatchlist[id];
						swatch.Location = new Point(x,y);
						_swatcharray[id] = swatch; // effin structs

						DrawSwatch(graphics, id);
						UpdatePositions(id, ref x, ref y);
					}

					_count = _id1 = id; // TODO: '_id1' shall always equal '_count+1' (ie. delete '_id1')

					_blank = 0;
					for (; id != MaxSwatches; ++id)
					{
						++_blank;
						_swatcharray[id] = new Swatch(new Point(x,y));

						DrawSwatch(graphics, id);
						UpdatePositions(id, ref x, ref y);
					}

					return b;
				}
			}
			return null;
		}

		void DrawSwatch(Graphics graphics, int id)
		{
			Swatch swatch = _swatcharray[id];

			int x = swatch.Location.X;
			int y = swatch.Location.Y;

			Color color = swatch.Color;
			if (color == Color.Empty)
				color = BackColor;

			using (var brush = new SolidBrush(color))
				graphics.FillRectangle(brush, x,y, 10,10);

			graphics.DrawRectangle(Pens.Black, x,y, 10,10);
		}

		void UpdatePositions(int id, ref int x, ref int y)
		{
			if ((id + 1) % 7 == 0) // wrap to next row
			{
				x = _x; y += _tile;
			}
			else
				x += _tile;
		}

		bool InsideGrid(int x, int y)
		{
			var rect = new Rectangle(_x, _y, Width - _x * 2, Height - _y * 2);
			return rect.Contains(x,y);
		}

		Swatch GetColorSwatch(int x, int y)
		{
			int id = GetSwatchId(x,y);
			if (id >= MaxSwatches)
				id  = MaxSwatches - 1;

			return _swatcharray[id];
		}

		int GetSwatchId(int x, int y)
		{
			return ((y - _y) / _tile) * _horitiles + ((x - _x) / _tile);
		}

		void AddColor(Color color)
		{
			int id = _id1;

			if (!DoesColorAlreadyExist(color) && _blank > 0)
			{
				using (var f = new SwatchDialog(color))
				{
					f.StartPosition = FormStartPosition.CenterParent;
					f.ShowInTaskbar = false;

					if (f.ShowDialog() == DialogResult.OK && _graphic != null)
					{
						int x = _swatcharray[_id1].Location.X;
						int y = _swatcharray[_id1].Location.Y;

						using (Graphics graphics = Graphics.FromImage(_graphic))
						{
							using (var brush = new SolidBrush(color))
								graphics.FillRectangle(brush, x,y, 10,10);

							graphics.DrawRectangle(Pens.Black, x,y, 10,10);
						}

						--_blank;

						Swatch swatch = _swatcharray[_id1];
						swatch.Color       = color;
						swatch.Description = f.ColorDescription;
						_swatcharray[_id1] = swatch;

						++_id1;

						ColorSwatchXml.WriteSwatches(SwatchFile, _swatcharray);
					}
				}
			}

/*			bool flag = false;
			int nextEmptySwatchIndex = m_nextEmptySwatchIndex;

			if (DoesColorAlreadyExist(c))
			{
				MessageBox.Show(base.Parent,
								RMManager.GetString(GetType(), "ES_ColorSwatchPanel_AddColor_MessageBox"),
								RMManager.GetString(GetType(), "ES_Generic_MessageBox_Caption"),
								MessageBoxButtons.OK,
								MessageBoxIcon.Hand);
			}
			else if (m_numberOfEmptySwatches <= 0)
			{
				MessageBox.Show(base.Parent,
								RMManager.GetString(GetType(), "ES_ColorSwatchPanel_AddColor_MessageBox"),
								RMManager.GetString(GetType(), "ES_Generic_MessageBox_Caption"),
								MessageBoxButtons.OK,
								MessageBoxIcon.Hand);
			}
			else
			{
				using (var addNewColorSwatchForm = new SwatchDialog(c))
				{
					addNewColorSwatchForm.StartPosition = FormStartPosition.CenterParent;
					addNewColorSwatchForm.ShowInTaskbar = false;
					if (addNewColorSwatchForm.ShowDialog() == DialogResult.OK && m_swatchBitmap != null)
					{
						int x = m_swatchArray[m_nextEmptySwatchIndex].Location.X;
						int y = m_swatchArray[m_nextEmptySwatchIndex].Location.Y;
						using (Graphics graphics = Graphics.FromImage(m_swatchBitmap))
						{
							using (var brush = new SolidBrush(c))
							{
								graphics.FillRectangle(brush, x,y, 10,10);
							}
							graphics.DrawRectangle(Pens.Black, x,y, 10,10);
						}

						--m_numberOfEmptySwatches;

						m_swatchArray[m_nextEmptySwatchIndex].Color = c;
						m_swatchArray[m_nextEmptySwatchIndex].Description = addNewColorSwatchForm.ColorDescription;

						++m_nextEmptySwatchIndex;
						flag = true;
					}
				}
			}

			if (flag)
				ColorSwatchXml.WriteSwatches(CustomSwatchesFile, m_swatchArray); */

			_highlight = false;
			InvalidateSwatch(_swatcharray[id].Location);
		}

		bool DoesColorAlreadyExist(object color)
		{
			for (int i = 0; i != _swatcharray.Length; ++i)
			{
				if (_swatcharray[i].Color.Equals(color))
					return true;
			}
			return false;
		}

		void ToggleEmptySwatchState(bool isActive)
		{
			if (_blank > 0)
			{
				_highlight = isActive;
				InvalidateSwatch(_swatcharray[_id1].Location);
			}
			else
				_highlight = false;
		}

		void InvalidateSwatch(Point pt)
		{
			var rect = new Rectangle(pt, new Size(11,11)); // why 11.
			Invalidate(rect);
		}
		#endregion Methods



		#region Designer
		IContainer components;

		ToolTip colorTip; // TODO
		MenuItem itDelete;
		MenuItem itRelabel;
		ContextMenu context;


		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}


		void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.colorTip = new System.Windows.Forms.ToolTip(this.components);
			this.itDelete = new System.Windows.Forms.MenuItem();
			this.itRelabel = new System.Windows.Forms.MenuItem();
			this.context = new System.Windows.Forms.ContextMenu();
			this.SuspendLayout();
			// 
			// colorTip
			// 
			this.colorTip.Active = false;
			// 
			// itDelete
			// 
			this.itDelete.Index = 0;
			this.itDelete.Text = "delete";
			this.itDelete.Click += new System.EventHandler(this.click_delete);
			// 
			// itRelabel
			// 
			this.itRelabel.Index = 1;
			this.itRelabel.Text = "relabel";
			this.itRelabel.Click += new System.EventHandler(this.click_relabel);
			// 
			// contextMenu
			// 
			this.context.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.itRelabel,
			this.itDelete});
			// 
			// SwatchControl
			// 
			this.AllowDrop = true;
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "SwatchControl";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}


	internal delegate void SwatchSelectedEventHandler(ColorEventArgs e);
}
