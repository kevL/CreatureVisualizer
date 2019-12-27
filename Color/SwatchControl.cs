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

		const int MaxTiles = 175;

		const  int _tile = 12; // x/y tile pixels

		const int _x =  5; // x-start of 1st tile location in pixels
		const int _y =  5; // y-start of 1st tile location in pixels

		const int _horitiles =  7; // count of tiles in a row
		const int _verttiles = 25; // count of tiles in a col
		#endregion Fields (static)


		#region Fields
		string SwatchFile = "NWN2 Toolset" + Path.DirectorySeparatorChar
						  + "CustomSwatches.xml";

		readonly Dragger _dragger = new Dragger();

		List<Swatch> _fileSwatches; // the swatches in the XML file

		Swatch[] _tiles = new Swatch[MaxTiles]; // the tiles of the table

		Bitmap _graphic;

		int _id; // for context
		int _firstBlankId;

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
			if (_graphic != null || (_graphic = CreateGraphic()) != null)
			{
				e.Graphics.DrawImage(_graphic, 0,0);

				if (_highlight)
					e.Graphics.DrawRectangle(Pens.White, _tiles[_firstBlankId].Rect);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_dragger.Hide();

			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
			{
				int id = GetTileId(e.X, e.Y);
				if (id != -1)
				{
					Swatch tile = _tiles[id];

					if (tile.Color != Color.Empty && tile.Rect.Contains(e.X, e.Y))
					{
						switch (e.Button)
						{
							case MouseButtons.Left:
								if (SwatchSelected != null)
									SwatchSelected(new ColorEventArgs(tile.Color));
								break;

							case MouseButtons.Right:
								_id = id;
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

			int id = GetTileId(e.X, e.Y);
			if (id != -1)
			{
				Swatch tile = _tiles[id];

				if (tile.Color != Color.Empty && tile.Rect.Contains(e.X, e.Y))
				{
					if (!tile.Description.Equals(colorTip.GetToolTip(this)))
						colorTip.SetToolTip(this, tile.Description);

					colorTip.Active = true;
					return;
				}
			}
			colorTip.Active = false;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			_dragger.Hide();
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
		void click_context_delete(object sender, EventArgs e)
		{
			using (Graphics graphics = Graphics.FromImage(_graphic))
			{
				Swatch swatch, swatch1;
				bool done = false;

				for (int id = _id; id != MaxTiles; ++id)
				{
					swatch  = _tiles[id];
					swatch1 = _tiles[id + 1];

					if (swatch1.Color != Color.Empty)
					{
						swatch.Color       = swatch1.Color;
						swatch.Description = swatch1.Description;
						_tiles[id] = swatch; // effin structs
					}
					else
					{
						swatch.Color       = Color.Empty;
						swatch.Description = Swatch.NoLabel;
						_tiles[id] = swatch; // effin structs

						_firstBlankId = id;
						done = true;
					}

					DrawSwatch(graphics, id);
					InvalidateSwatch(_tiles[id].Location); // is that redundant

					if (done) break;
				}
			}
//			SwatchIo.Write(SwatchFile, _tiles);
		}

		void click_context_relabel(object sender, EventArgs e)
		{
			Swatch swatch = _tiles[_id];
			using (var f = new SwatchDialog(swatch))
			{
				if (f.ShowDialog(this) == DialogResult.OK
					&& f.Description != swatch.Description)
				{
					swatch.Description = f.Description;
					_tiles[_id] = swatch; // effin structs

//					SwatchIo.Write(SwatchFile, _tiles);
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
					for (; id != _fileSwatches.Count && id != MaxTiles; ++id)
					{
						Swatch swatch = _fileSwatches[id];
						swatch.Location = new Point(x,y);
						_tiles[id] = swatch; // effin structs

						DrawSwatch(graphics, id);
						UpdatePositions(id, ref x, ref y);
					}

					_firstBlankId = id;

					for (; id != MaxTiles; ++id)
					{
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

		int GetTileId(int x, int y)
		{
			if (   x > _x && x < _x + _horitiles * _tile
				&& y > _y && y < _y + _verttiles * _tile)
			{
				return (x - _x) / _tile
					 + (y - _y) / _tile * _horitiles;
			}
			return -1;
		}

		void ColorSwatch(Color color)
		{
			if (_graphic != null && !ColorExists(color))
			{
				using (var f = new SwatchDialog(color))
				{
					if (f.ShowDialog(this) == DialogResult.OK)
					{
						Swatch swatch = _tiles[_firstBlankId];

						using (Graphics graphics = Graphics.FromImage(_graphic))
						{
							using (var brush = new SolidBrush(color))
								graphics.FillRectangle(brush, swatch.Rect);

							graphics.DrawRectangle(Pens.Black, swatch.Rect);
						}

						swatch.Color       = color;
						swatch.Description = f.Description;
						_tiles[_firstBlankId] = swatch;

						InvalidateSwatch(_tiles[_firstBlankId].Location);

						++_firstBlankId;

//						SwatchIo.Write(SwatchFile, _tiles);
					}
				}
			}
			_highlight = false;
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
			if (_firstBlankId != MaxTiles)
			{
				_highlight = highlight;
				InvalidateSwatch(_tiles[_firstBlankId].Location);
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

		ToolTip colorTip;
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
			this.itDelete.Click += new System.EventHandler(this.click_context_delete);
			// 
			// itRelabel
			// 
			this.itRelabel.Index = 1;
			this.itRelabel.Text = "relabel";
			this.itRelabel.Click += new System.EventHandler(this.click_context_relabel);
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
