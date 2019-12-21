using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
//using System.IO;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSwatchPanel
	sealed class ColorSwatchPanel
		: UserControl
	{
		#region Events
		internal event ColorSwatchSelectedHandler ColorSwatchSelected;
		#endregion Events


		#region Fields (static)
		public static string CustomSwatchesFile = "CustomSwatches.xml";
		#endregion Fields (static)


		#region Fields
		DragForm _dragger;

		readonly Rectangle _swatchRegion = new Rectangle(0,0, 164,263);
		readonly Size _swatchSize = new Size(10,10);

		List<ColorSwatch> _swatches;

		Bitmap _swatchB;

		ColorSwatch[] _swatchArray;
		ColorSwatch _lastSwatch;

		int _currentSwatchId;
		int _nextSwatchId;

		int _startX;
		int _startY;

		int _totalSwatches;
		int _blankSwatches;
		int _hori;
		int _vert;

		int _outerWidth;
		int _outerHeight;

		bool _paintActiveBlankSwatch;

//		bool _track;
		#endregion Fields


		#region cTor
		public ColorSwatchPanel()
		{
			InitializeComponent();

			_startX = _swatchRegion.X + 6;
			_startY = _swatchRegion.Y + 6;

			_dragger = new DragForm();
			AllowDrop = true;
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

//			if (File.Exists(CustomSwatchesFile))
//			{
//				m_swatches = ColorSwatchXml.ReadSwatches(CustomSwatchesFile, false);
//			}
//			else
//			{
//				CreateCustomSwatchesFile();
//				m_swatches = ColorSwatchXml.ReadSwatches("ColorSwatches.xml", true);
//			}
			_swatches = null;


			_outerWidth  = Width  - (Width  - 12) % 12;
			_outerHeight = Height - (Height - 12) % 12;
			_hori = (_outerWidth  - 12) / 12;
			_vert = (_outerHeight - 12) / 12;

			_swatchArray = new ColorSwatch[_hori * _vert];
		}

		protected override void OnPaint(PaintEventArgs e)
		{
//			ControlPaint.DrawBorder3D(e.Graphics, new Rectangle(0,0, _outerWidth, _outerHeight));

			if (_swatchB != null || (_swatchB = BuildSwatchBitmap()) != null)
			{
				e.Graphics.DrawImage(_swatchB,
									 new Rectangle(new Point(0,0),
												   new Size(_swatchB.Width,
															_swatchB.Height)));
			}

			if (_paintActiveBlankSwatch)
			{
				Rectangle region = _swatchArray[_nextSwatchId].Region;
				e.Graphics.DrawRectangle(Pens.Yellow, region);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (IsCursorWithinSwatchGridBorders(e.X, e.Y))
			{
				ColorSwatch colorSwatch = GetColorSwatch(e.X, e.Y);
				if (e.Button == MouseButtons.Left)
				{
					if (DoesCursorIntersectWithSwatch(colorSwatch, e.X, e.Y) && colorSwatch.Color != Color.Empty)
					{
						Cursor = Cursors.Hand;
					}
				}
				else if (e.Button == MouseButtons.Right)
				{
					_currentSwatchId = GetColorSwatchIndex(e.X, e.Y);
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
//			if (_track)
//			{
//				_dragger.Location = PointToScreen(new Point(e.X - _dragger.CursorXDifference, e.Y - _dragger.CursorYDifference));
//			}
//			else
			if (IsCursorWithinSwatchGridBorders(e.X, e.Y))
			{
				ColorSwatch colorSwatch = GetColorSwatch(e.X, e.Y);
				if (DoesCursorIntersectWithSwatch(colorSwatch, e.X, e.Y) && colorSwatch.Color != Color.Empty)
				{
					if (!_lastSwatch.Equals(colorSwatch))
					{
						colorTip.Active = false;
					}

					if (colorSwatch.Description != null && !colorSwatch.Description.Equals(colorTip.GetToolTip(this)))
					{
						colorTip.SetToolTip(this, colorSwatch.Description);
					}

					colorTip.Active = true;
					_lastSwatch = colorSwatch;
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

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_dragger.Hide();

			if (IsCursorWithinSwatchGridBorders(e.X, e.Y))
			{
				ColorSwatch colorSwatch = GetColorSwatch(e.X, e.Y);
				if (DoesCursorIntersectWithSwatch(colorSwatch, e.X, e.Y) && colorSwatch.Color != Color.Empty)
				{
					if (e.Button == MouseButtons.Right)
					{
						contextMenu.Show(this, new Point(e.X, e.Y));
					}
					else
						OnColorSwatchSelected(new ColorSelectedEventArgs(colorSwatch.Color));
				}
			}
//			else _track = false;
		}

		void OnColorSwatchSelected(ColorSelectedEventArgs e)
		{
			if (ColorSwatchSelected != null)
				ColorSwatchSelected(this, e);
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


		#region Methods
		void CreateCustomSwatchesFile()
		{
			// TODO: using etc.

/*			var streamReader = new StreamReader(SanoResources.GetFileResource("ColorSwatches.xml")); // TODO ->>
			if (streamReader != null)
			{
				StreamWriter streamWriter = null;
				try
				{
					string directoryName = Path.GetDirectoryName(CustomSwatchesFile);
					if (!Directory.Exists(directoryName))
						Directory.CreateDirectory(directoryName);

					streamWriter = new StreamWriter(CustomSwatchesFile);
					streamWriter.Write(streamReader.ReadToEnd());
					streamWriter.Flush();
				}
				catch (DirectoryNotFoundException)
				{}
//				catch (Exception)
//				{}
				finally
				{
					streamReader.Close();
					streamWriter.Close();
				}
			} */
		}

		void DrawColorSwatch(Graphics g, int swatchIndex)
		{
			PaintSwatch(g, swatchIndex, _swatchArray[swatchIndex].Color);
			g.DrawRectangle(Pens.Black, _swatchArray[swatchIndex].Location.X, _swatchArray[swatchIndex].Location.Y, 10, 10);
		}

		void DrawEmptyColorSwatch(Graphics g, int swatchIndex)
		{
			PaintSwatch(g, swatchIndex, BackColor);
			g.DrawRectangle(Pens.DarkGray, _swatchArray[swatchIndex].Location.X, _swatchArray[swatchIndex].Location.Y, 10, 10);
		}

		void PaintSwatch(Graphics g, int swatchIndex, Color color)
		{
			using (var brush = new SolidBrush(color))
			{
				g.FillRectangle(brush, _swatchArray[swatchIndex].Location.X, _swatchArray[swatchIndex].Location.Y, 10, 10);
			}
		}

		Bitmap BuildSwatchBitmap()
		{
			if (_swatches != null) // kL
			{
				int x = _startX;
				int y = _startY;

				int col = 0;
				int row = 0;

				int val = 0;

				var b = new Bitmap(_outerWidth, _outerHeight);

				using (Graphics graphics = Graphics.FromImage(b))
				{
					for (int i = 0; i != _swatches.Count; ++i)
					{
						_swatchArray[val++] = _swatches[i];
						_swatchArray[val].Location = new Point(x,y);
						_swatchArray[val].Size     = _swatchSize;

						DrawColorSwatch(graphics, i);
						UpdatePositions(ref x, ref y, ref col, ref row);

						if (y + 10 > _outerHeight)
							return b;

						++_totalSwatches; // TODO: why are you not increasing total
					}

					_blankSwatches = 0;

					do
					{
						if (++_blankSwatches == 1)
							_nextSwatchId = _totalSwatches;

						_swatchArray[val] = new ColorSwatch(new Point(x,y), _swatchSize);

						DrawEmptyColorSwatch(graphics, val++);
						UpdatePositions(ref x, ref y, ref col, ref row);

						++_totalSwatches;
					}
					while (y + 10 <= _outerHeight);

					return b;
				}
			}
			return null;
		}

		void UpdatePositions(ref int x, ref int y, ref int col, ref int row)
		{
			if (x + 24 > _outerWidth)
			{
				x = _startX;
				y += 12;
				col = 0;
				++row;
			}
			else
			{
				++col;
				x += 12;
			}
		}

		bool IsCursorWithinSwatchGridBorders(int x, int y)
		{
			int width = _outerWidth - 12;
			int height = _outerHeight - 12;
			var rect = new Rectangle(_startX, _startY, width, height);
			return new Rectangle(x, y, 1, 1).IntersectsWith(rect);
		}

		ColorSwatch GetColorSwatch(int x, int y)
		{
			return _swatchArray[GetColorSwatchIndex(x, y)];
		}

		int GetColorSwatchIndex(int x, int y)
		{
			return GetSwatchRowIndex(y) * _hori + GetSwatchColumnIndex(x);
		}

		void AddColor(Color color)
		{
			int id = _nextSwatchId;

			if (!DoesColorAlreadyExist(color) && _blankSwatches > 0)
			{
				using (var f = new ColorSwatchF(color))
				{
					f.StartPosition = FormStartPosition.CenterParent;
					f.ShowInTaskbar = false;
					if (f.ShowDialog() == DialogResult.OK && _swatchB != null)
					{
						int x = _swatchArray[_nextSwatchId].Location.X;
						int y = _swatchArray[_nextSwatchId].Location.Y;
						using (Graphics graphics = Graphics.FromImage(_swatchB))
						{
							using (var brush = new SolidBrush(color))
							{
								graphics.FillRectangle(brush, x,y, 10,10);
							}
							graphics.DrawRectangle(Pens.Black, x,y, 10,10);
						}

						--_blankSwatches;

						_swatchArray[_nextSwatchId].Color       = color;
						_swatchArray[_nextSwatchId].Description = f.ColorDescription;

						++_nextSwatchId;

						ColorSwatchXml.WriteSwatches(CustomSwatchesFile, _swatchArray);
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
				using (var addNewColorSwatchForm = new ColorSwatchF(c))
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
								graphics.FillRectangle(brush, x, y, 10, 10);
							}
							graphics.DrawRectangle(Pens.Black, x, y, 10, 10);
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

			_paintActiveBlankSwatch = false;
			InvalidateSwatch(_swatchArray[id].Location);
		}

		bool DoesColorAlreadyExist(object color)
		{
			for (int i = 0; i != _swatchArray.Length; ++i)
			{
				if (_swatchArray[i].Color.Equals(color))
					return true;
			}
			return false;
		}

		void DeleteSwatch()
		{
			if (_currentSwatchId != -1)
			{
				using (Graphics graphics = Graphics.FromImage(_swatchB))
				{
					int val1 = _currentSwatchId + (_totalSwatches - 1) - _currentSwatchId - _blankSwatches;
					int val2 = 0;

					for (val2 = _currentSwatchId; val2 <= val1; ++val2)
					{
						if (val2 + 1 < _swatchArray.Length - 1)
						{
							_swatchArray[val2].Color = _swatchArray[val2 + 1].Color;
							_swatchArray[val2].Description = _swatchArray[val2 + 1].Description;
						}
						else
						{
							_swatchArray[val2].Color = Color.Empty;
							_swatchArray[val2].Description = String.Empty;
						}

						if (_swatchArray[val2].Color == Color.Empty)
						{
							DrawEmptyColorSwatch(graphics, val2);
							_nextSwatchId = val2;
						}
						else
						{
							DrawColorSwatch(graphics, val2);
						}

						Rectangle region = _swatchArray[val2].Region;
						region.Inflate(1, 1);
						Invalidate(region);
					}

					++_blankSwatches;
				}

				_currentSwatchId = -1;
				ColorSwatchXml.WriteSwatches(CustomSwatchesFile, _swatchArray);
			}
		}

		void ToggleEmptySwatchState(bool isActive)
		{
			if (_blankSwatches > 0)
			{
				_paintActiveBlankSwatch = isActive;
				InvalidateSwatch(_swatchArray[_nextSwatchId].Location);
			}
			else
				_paintActiveBlankSwatch = false;
		}

		void InvalidateSwatch(Point swatchPoint)
		{
			var rect = new Rectangle(swatchPoint, _swatchSize);
			rect.Inflate(1, 1);
			Invalidate(rect);
		}

		void deleteSwatchMenuItem_Click(object sender, EventArgs e)
		{
			DeleteSwatch();
		}

		void renameSwatchMenuItem_Click(object sender, EventArgs e)
		{
			using (var f = new ColorSwatchF(_swatchArray[_currentSwatchId]))
			{
				f.StartPosition = FormStartPosition.CenterParent;
				f.ShowInTaskbar = false;

				if (f.ShowDialog() == DialogResult.OK)
				{
					_swatchArray[_currentSwatchId].Description = f.ColorDescription;
					ColorSwatchXml.WriteSwatches(CustomSwatchesFile, _swatchArray);
				}
			}
		}
		#endregion Methods


		#region Methods (static)
		static bool DoesCursorIntersectWithSwatch(ColorSwatch swatch, int x, int y)
		{
			var location = new Point(x, y);
			return new Rectangle(location, new Size(1,1)).IntersectsWith(swatch.Region);
		}

		static int GetSwatchColumnIndex(int x)
		{
			return (x - 6) / 12;
		}

		static int GetSwatchRowIndex(int y)
		{
			return (y - 6) / 12;
		}
		#endregion Methods (static)



		#region Designer
		IContainer components;

		ToolTip colorTip; // TODO
		MenuItem deleteSwatchMenuItem;
		MenuItem renameSwatchMenuItem;
		ContextMenu contextMenu;


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
			this.deleteSwatchMenuItem = new System.Windows.Forms.MenuItem();
			this.renameSwatchMenuItem = new System.Windows.Forms.MenuItem();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.SuspendLayout();
			// 
			// colorTip
			// 
			this.colorTip.Active = false;
			// 
			// deleteSwatchMenuItem
			// 
			this.deleteSwatchMenuItem.Index = 1;
			this.deleteSwatchMenuItem.Click += new System.EventHandler(this.deleteSwatchMenuItem_Click);
			// 
			// renameSwatchMenuItem
			// 
			this.renameSwatchMenuItem.Index = 0;
			this.renameSwatchMenuItem.Click += new System.EventHandler(this.renameSwatchMenuItem_Click);
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.renameSwatchMenuItem,
			this.deleteSwatchMenuItem});
			// 
			// ColorSwatchPanel
			// 
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSwatchPanel";
			this.ResumeLayout(false);

		}
/*		void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(creaturevisualizer.ColorSwatchPanel));
			colorTip = new System.Windows.Forms.ToolTip(components);
			deleteSwatchMenuItem = new System.Windows.Forms.MenuItem();
			renameSwatchMenuItem = new System.Windows.Forms.MenuItem();
			contextMenu = new System.Windows.Forms.ContextMenu();
			SuspendLayout();
			colorTip.Active = false;
			deleteSwatchMenuItem.Index = 1;
			resources.ApplyResources(deleteSwatchMenuItem, "deleteSwatchMenuItem");
			deleteSwatchMenuItem.Click += new System.EventHandler(deleteSwatchMenuItem_Click);
			renameSwatchMenuItem.Index = 0;
			resources.ApplyResources(renameSwatchMenuItem, "renameSwatchMenuItem");
			renameSwatchMenuItem.Click += new System.EventHandler(renameSwatchMenuItem_Click);
			contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[2]
			{
				renameSwatchMenuItem,
				deleteSwatchMenuItem
			});
			base.Name = "ColorSwatchPanel";
			ResumeLayout(false);
		} */
		#endregion Designer
	}



	internal delegate void ColorSwatchSelectedHandler(object sender, ColorSelectedEventArgs e);
}
