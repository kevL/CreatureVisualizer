using System;
using System.Collections.Generic;
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
		public event SwatchSelectedEvent SwatchSelected;
		#endregion Events


		#region Fields (static)
		internal static string _path;

		const int MaxTiles = 175;

		const  int _tile = 12; // x/y tile pixels

		const int _x = 5; // x-start of 1st tile location in pixels
		const int _y = 5; // y-start of 1st tile location in pixels

		const int _horitiles =  7; // count of tiles in a row
		const int _verttiles = 25; // count of tiles in a col
		#endregion Fields (static)


		#region Fields
		string SwatchFile = "NWN2 Toolset" + Path.DirectorySeparatorChar
						  + "CustomSwatches.xml";

		List<Swatch> _fileSwatches; // the swatches in the XML file

		Swatch[] _tiles = new Swatch[MaxTiles]; // the tiles of the table

		Bitmap _graphic;

		int _sel = -1;
		int _firstBlankId;
		#endregion Fields


		#region cTor
		public SwatchControl()
		{
			InitializeComponent();

			if (!ColorF.reallyDesignMode)
			{
				_path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); // local userdir
				_path = Path.Combine(_path, SwatchFile);

				if (!File.Exists(_path))
				{
					_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // roaming userdir
					_path = Path.Combine(_path, SwatchFile);
				}

				if (File.Exists(_path))
				{
					_fileSwatches = SwatchIo.Read(_path);

					if (_fileSwatches.Count < MaxTiles)
						_firstBlankId = _fileSwatches.Count;
					else
						_firstBlankId = MaxTiles;

					CreateGraphic();
				}
//				else
//				{
//					SwatchIo.Create();
//					_fileSwatches = SwatchIo.ReadSwatches("ColorSwatches.xml", true);
//				}
			}
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnPaint(PaintEventArgs e)
		{
			if (_graphic != null)
			{
				e.Graphics.DrawImage(_graphic, 0,0);

				if (_sel != -1)
					e.Graphics.DrawRectangle(Pens.White, _tiles[_sel].Rect);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (_graphic != null) ClearSelector();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (_graphic != null
				&& (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
				&& ClientRectangle.Contains(e.X, e.Y))
			{
				int id = GetTileId(e.X, e.Y);
				if (id != -1)
				{
					Swatch tile = _tiles[id];

					if (tile.Color != Color.Empty && tile.Contains(e.X, e.Y))
					{
						switch (e.Button)
						{
							case MouseButtons.Left:
								if (SwatchSelected != null)
									SwatchSelected(new ColorEventArgs(tile.Color));

								goto case MouseButtons.Right;

							case MouseButtons.Right:
								InvalidateSwatch(_tiles[_sel = id]);
								break;
						}
					}
				}

				if (e.Button == MouseButtons.Right)
				{
					context.MenuItems.Clear();

					if (_firstBlankId != MaxTiles)
						context.MenuItems.Add(itAppend);

					if (_sel != -1)
					{
						if (_firstBlankId != MaxTiles)
							context.MenuItems.Add("-");

						context.MenuItems.Add(itDelete);
						context.MenuItems.Add(itRelabel);
					}
					context.Show(this, new Point(e.X, e.Y));
				}
			}
		}


		string _desc;

		protected override void OnMouseMove(MouseEventArgs e)
		{
			int id = GetTileId(e.X, e.Y);
			if (id != -1)
			{
				Swatch tile = _tiles[id];

				if (tile.Color != Color.Empty && tile.Contains(e.X, e.Y)
					&& tile.Description != _desc)
				{
					ColorF.That.Print(_desc = tile.Description);
				}
			}
		}
		#endregion Handlers (override)


		#region Handlers
		void click_context_delete(object sender, EventArgs e)
		{
			using (Graphics graphics = Graphics.FromImage(_graphic))
			{
				--_firstBlankId;

				Swatch swatch, swatch1;
				int id = _sel; _sel = -1;

				for (; id != MaxTiles; ++id)
				{
					swatch  = _tiles[id];

					if (id + 1 != MaxTiles)
					{
						swatch1 = _tiles[id + 1];

						swatch.Color       = swatch1.Color;
						swatch.Description = swatch1.Description;
					}
					else
					{
						swatch.Color       = Color.Empty;
						swatch.Description = Swatch.NoLabel;
					}
					_tiles[id] = swatch; // effin structs

					DrawSwatch(graphics, swatch);
					InvalidateSwatch(swatch);

					if (swatch.Color == Color.Empty)
						break;
				}
			}
//			SwatchIo.Write(SwatchFile, _tiles);
		}

		void click_context_relabel(object sender, EventArgs e)
		{
			Swatch swatch = _tiles[_sel];
			using (var f = new SwatchDialog(swatch))
			{
				if (f.ShowDialog(this) == DialogResult.OK
					&& f.Description != swatch.Description)
				{
					swatch.Description = f.Description;
					_tiles[_sel] = swatch; // effin structs

//					SwatchIo.Write(SwatchFile, _tiles);
				}
			}
		}

		void click_context_append(object sender, EventArgs e)
		{
			ClearSelector();

			Color color = ColorF.That.ColorControl.GetActiveColorbox().BackColor;

			int id = ColorExists(color);
			if (id == _firstBlankId)
			{
				using (var f = new SwatchDialog(color))
				{
					if (f.ShowDialog(this) == DialogResult.OK)
					{
						++_firstBlankId;

						Swatch swatch = _tiles[id];

						using (Graphics graphics = Graphics.FromImage(_graphic))
						{
							using (var brush = new SolidBrush(color))
								graphics.FillRectangle(brush, swatch.Rect);

							graphics.DrawRectangle(Pens.Black, swatch.Rect);
						}

						swatch.Color       = color;
						swatch.Description = f.Description;
						_tiles[id] = swatch;

						InvalidateSwatch(_tiles[_sel = id]);

//						SwatchIo.Write(SwatchFile, _tiles);
					}
				}
			}
			else
				InvalidateSwatch(_tiles[_sel = id]);
		}
		#endregion Handlers


		#region Methods
		void CreateGraphic()
		{
			if (_fileSwatches != null)
			{
				_graphic = new Bitmap(Width, Height);

				using (Graphics graphics = Graphics.FromImage(_graphic))
				{
					int x = _x;
					int y = _y;

					int id = 0;
					for (; id != _firstBlankId; ++id)
					{
						Swatch swatch = _fileSwatches[id];
						swatch.Location = new Point(x,y);
						_tiles[id] = swatch; // effin structs

						DrawSwatch(graphics, swatch);
						UpdatePositions(id, ref x, ref y);
					}

					for (; id != MaxTiles; ++id)
					{
						DrawSwatch(graphics, (_tiles[id] = new Swatch(new Point(x,y))));
						UpdatePositions(id, ref x, ref y);
					}
				}
			}
			else
				_graphic = null;
		}

		void DrawSwatch(Graphics graphics, Swatch swatch)
		{
			Color color = swatch.Color;
			if (color == Color.Empty)
				color = BackColor;

			int x = swatch.Location.X;
			int y = swatch.Location.Y;

			using (var brush = new SolidBrush(color))
				graphics.FillRectangle(brush, x,y, Swatch.width, Swatch.height);

			graphics.DrawRectangle(Pens.Black, x,y, Swatch.width, Swatch.height);
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

		int ColorExists(Color color)
		{
			Color color0;
			for (int id = 0; id != _tiles.Length; ++id)
			{
				color0 = _tiles[id].Color;
				if (   color0.A == color.A
					&& color0.R == color.R
					&& color0.G == color.G
					&& color0.B == color.B)
				{
					return id;
				}
			}
			return _firstBlankId;
		}

		internal void SelectSwatch(Color color)
		{
			ClearSelector();

			int id = ColorExists(color);
			if (id != _firstBlankId)
				InvalidateSwatch(_tiles[_sel = id]);
		}

		void ClearSelector()
		{
			if (_sel != -1)
			{
				int id = _sel; _sel = -1;
				InvalidateSwatch(_tiles[id]);
			}
		}

		void InvalidateSwatch(Swatch swatch)
		{
			var rect = new Rectangle(swatch.Location.X, swatch.Location.Y,
									 Swatch.width + 1,  Swatch.height + 1); // why +1. because .net
			Invalidate(rect);
		}
		#endregion Methods



		#region Designer
		MenuItem itDelete;
		MenuItem itRelabel;
		MenuItem itAppend;
		ContextMenu context;


		void InitializeComponent()
		{
			this.itDelete = new System.Windows.Forms.MenuItem();
			this.itRelabel = new System.Windows.Forms.MenuItem();
			this.itAppend = new System.Windows.Forms.MenuItem();
			this.context = new System.Windows.Forms.ContextMenu();
			this.SuspendLayout();
			// 
			// itDelete
			// 
			this.itDelete.Index = -1;
			this.itDelete.Text = "delete";
			this.itDelete.Click += new System.EventHandler(this.click_context_delete);
			// 
			// itRelabel
			// 
			this.itRelabel.Index = -1;
			this.itRelabel.Text = "relabel";
			this.itRelabel.Click += new System.EventHandler(this.click_context_relabel);
			// 
			// itAppend
			// 
			this.itAppend.Index = -1;
			this.itAppend.Text = "append color";
			this.itAppend.Click += new System.EventHandler(this.click_context_append);
			// 
			// SwatchControl
			// 
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "SwatchControl";
			this.Size = new System.Drawing.Size(93, 309);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}


	internal delegate void SwatchSelectedEvent(ColorEventArgs e);
}
