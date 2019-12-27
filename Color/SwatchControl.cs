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

		const  int _tile      = 12; // x/y tile pixels

		const int _x          =  5; // x-start of 1st tile location in pixels
		const int _y          =  5; // y-start of 1st tile location in pixels

		const int _horitiles  =  7; // count of tiles in a row
		#endregion Fields (static)


		#region Fields
		string SwatchFile = "NWN2 Toolset" + Path.DirectorySeparatorChar
						  + "CustomSwatches.xml";

		readonly Dragger _dragger = new Dragger();

		List<Swatch> _fileSwatches; // the swatches in the XML file

		Swatch[] _tiles = new Swatch[MaxSwatches]; // the tiles of the table

		Bitmap _graphic;

		Swatch _lastover;

		int _idcontext;
		int _idfirstblank;

		int _countAssignedTiles;
		int _countBlankTiles;

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
				_fileSwatches = SwatchIo.Read(_path);
			}
/*			else
			{
//				SwatchIo.Create();
//				_swatchlist = SwatchIo.ReadSwatches("ColorSwatches.xml", true);

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
					e.Graphics.DrawRectangle(Pens.Yellow, _tiles[_idfirstblank].Rect);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
//			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Right)
				_idcontext = GetSwatchId(e.X, e.Y);

/*			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
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
							_idcontext = GetSwatchId(e.X, e.Y);
							break;
					}
				}
			} */
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
					if (!_lastover.Equals(swatch))
						colorTip.Active = false; // wtf

					if (swatch.Description != null && !swatch.Description.Equals(colorTip.GetToolTip(this)))
						colorTip.SetToolTip(this, swatch.Description);

					colorTip.Active = true; // wtf

					_lastover = swatch;
//					Cursor = Cursors.Hand;
				}
				else
				{
//					Cursor = Cursors.Default;
					colorTip.Active = false;
				}
			}
//			else
//				Cursor = Cursors.Default;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			_dragger.Hide();

			base.OnMouseLeave(e);
		}

		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			SetHighlight(true);

			base.OnDragEnter(drgevent);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			SetHighlight(false);

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
			ColorSwatch(color);

			drgevent.Effect = DragDropEffects.None;

			base.OnDragDrop(drgevent);
		}
		#endregion Handlers (override)


		#region Handlers
		void click_delete(object sender, EventArgs e)
		{
			if (_idcontext != -1)
			{
				using (Graphics graphics = Graphics.FromImage(_graphic))
				{
					int valid = _idcontext + (_countAssignedTiles - 1) - _idcontext - _countBlankTiles;

					for (int id = _idcontext; id <= valid; ++id)
					{
						if (id + 1 < _tiles.Length - 1)
						{
							Swatch swatch = _tiles[id];
							swatch.Color       = _tiles[id + 1].Color;
							swatch.Description = _tiles[id + 1].Description;
							_tiles[id] = swatch; // effin structs
						}
						else // clean the last valid swatch ->
						{
							Swatch swatch = _tiles[id];
							swatch.Color = Color.Empty;
							swatch.Description = String.Empty;
							_tiles[id] = swatch; // effin structs

							_idfirstblank = id;
						}
//						if (_swatcharray[id].Color == Color.Empty)
//							_id1 = id;

						DrawSwatch(graphics, id);
						InvalidateSwatch(_tiles[id].Location); // is that redundant
					}

					++_countBlankTiles;
				}

				_idcontext = -1;
				SwatchIo.Write(SwatchFile, _tiles);
			}
		}

		void click_relabel(object sender, EventArgs e)
		{
			using (var f = new SwatchDialog(_tiles[_idcontext]))
			{
				f.StartPosition = FormStartPosition.CenterParent;
				f.ShowInTaskbar = false;

				if (f.ShowDialog() == DialogResult.OK)
				{
					Swatch swatch = _tiles[_idcontext];
					swatch.Description = f.ColorDescription;
					_tiles[_idcontext] = swatch; // effin structs

					SwatchIo.Write(SwatchFile, _tiles);
				}
			}
		}
		#endregion Handlers


		#region Methods
		Bitmap CreateGraphic()
		{
			if (_fileSwatches != null)
			{
				var b = new Bitmap(Width, Height);

				using (Graphics graphics = Graphics.FromImage(b))
				{
					int x = _x;
					int y = _y;

					int id = 0;
					for (; id != _fileSwatches.Count && id != MaxSwatches; ++id)
					{
						Swatch swatch = _fileSwatches[id];
						swatch.Location = new Point(x,y);
						_tiles[id] = swatch; // effin structs

						DrawSwatch(graphics, id);
						UpdatePositions(id, ref x, ref y);
					}

					_countAssignedTiles = _idfirstblank = id; // TODO: '_id1' shall always equal '_count+1' (ie. delete '_id1')

					_countBlankTiles = 0;
					for (; id != MaxSwatches; ++id)
					{
						++_countBlankTiles;
						_tiles[id] = new Swatch(new Point(x,y));

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
			Swatch swatch = _tiles[id];

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

			return _tiles[id];
		}

		int GetSwatchId(int x, int y)
		{
			return ((y - _y) / _tile) * _horitiles + ((x - _x) / _tile);
		}

		void ColorSwatch(Color color)
		{
			int id = _idfirstblank;

			if (!ColorExists(color) && _countBlankTiles > 0)
			{
				using (var f = new SwatchDialog(color))
				{
					f.StartPosition = FormStartPosition.CenterParent;
					f.ShowInTaskbar = false;

					if (f.ShowDialog() == DialogResult.OK && _graphic != null)
					{
						int x = _tiles[_idfirstblank].Location.X;
						int y = _tiles[_idfirstblank].Location.Y;

						using (Graphics graphics = Graphics.FromImage(_graphic))
						{
							using (var brush = new SolidBrush(color))
								graphics.FillRectangle(brush, x,y, 10,10);

							graphics.DrawRectangle(Pens.Black, x,y, 10,10);
						}

						--_countBlankTiles;

						Swatch swatch = _tiles[_idfirstblank];
						swatch.Color       = color;
						swatch.Description = f.ColorDescription;
						_tiles[_idfirstblank] = swatch;

						++_idfirstblank;

						SwatchIo.Write(SwatchFile, _tiles);
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
				SwatchIo.WriteSwatches(CustomSwatchesFile, m_swatchArray); */

			_highlight = false;
			InvalidateSwatch(_tiles[id].Location);
		}

		bool ColorExists(object color)
		{
			for (int i = 0; i != _tiles.Length; ++i)
			{
				if (_tiles[i].Color.Equals(color))
					return true;
			}
			return false;
		}

		void SetHighlight(bool highlight)
		{
			if (_countBlankTiles > 0)
			{
				_highlight = highlight;
				InvalidateSwatch(_tiles[_idfirstblank].Location);
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
