using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
//using System.IO;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSwatchPanel
	sealed class SwatchPanel
		: UserControl
	{
		#region Events
		public event SwatchSelectedEventHandler SwatchSelected;
		#endregion Events


		#region Fields (static)
		const string CustomSwatchesFile = "CustomSwatches.xml";
		#endregion Fields (static)


		#region Fields
		readonly DragForm _dragger = new DragForm();

		readonly Rectangle _swatchRegion = new Rectangle(0,0, 164,263);

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
		public SwatchPanel()
		{
			InitializeComponent();

			_startX = _swatchRegion.X + 6;
			_startY = _swatchRegion.Y + 6;
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

//			if (File.Exists(CustomSwatchesFile))
//			{
//				_swatches = ColorSwatchXml.ReadSwatches(CustomSwatchesFile, false);
//			}
//			else
//			{
//				CreateCustomSwatchesFile();
//				_swatches = ColorSwatchXml.ReadSwatches("ColorSwatches.xml", true);
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
				e.Graphics.DrawImage(_swatchB,
									 new Rectangle(new Point(0,0),
												   new Size(_swatchB.Width,
															_swatchB.Height)));

			if (_paintActiveBlankSwatch)
				e.Graphics.DrawRectangle(Pens.Yellow, _swatchArray[_nextSwatchId].Rect);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
//			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
			{
				if (IsCursorWithinSwatchGridBorders(e.X, e.Y))
				{
					switch (e.Button)
					{
						case MouseButtons.Left:
						{
							ColorSwatch swatch = GetColorSwatch(e.X, e.Y);

							if (swatch.Color != Color.Empty
								&& DoesCursorIntersectWithSwatch(swatch, e.X, e.Y))
							{
								Cursor = Cursors.Hand;
							}
							break;
						}

						case MouseButtons.Right:
							_currentSwatchId = GetSwatchId(e.X, e.Y);
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
				if (IsCursorWithinSwatchGridBorders(e.X, e.Y))
				{
					ColorSwatch swatch = GetColorSwatch(e.X, e.Y);

					if (swatch.Color != Color.Empty
						&& DoesCursorIntersectWithSwatch(swatch, e.X, e.Y))
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
			if (IsCursorWithinSwatchGridBorders(e.X, e.Y))
			{
				ColorSwatch swatch = GetColorSwatch(e.X, e.Y);

				if (swatch.Color != Color.Empty
					&& DoesCursorIntersectWithSwatch(swatch, e.X, e.Y))
				{
					if (!_lastSwatch.Equals(swatch))
						colorTip.Active = false; // wtf

					if (swatch.Description != null && !swatch.Description.Equals(colorTip.GetToolTip(this)))
						colorTip.SetToolTip(this, swatch.Description);

					colorTip.Active = true; // wtf

					_lastSwatch = swatch;
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
							ColorSwatch swatch = _swatchArray[val2];
							swatch.Color       = _swatchArray[val2 + 1].Color;
							swatch.Description = _swatchArray[val2 + 1].Description;
							_swatchArray[val2] = swatch; // effin structs
						}
						else
						{
							ColorSwatch swatch = _swatchArray[val2];
							swatch.Color = Color.Empty;
							swatch.Description = String.Empty;
							_swatchArray[val2] = swatch; // effin structs
						}

						if (_swatchArray[val2].Color == Color.Empty)
							_nextSwatchId = val2;

						DrawSwatch(graphics, val2);

						Rectangle rect = _swatchArray[val2].Rect;
						rect.Inflate(1,1);
						Invalidate(rect);
					}

					++_blankSwatches;
				}

				_currentSwatchId = -1;
				ColorSwatchXml.WriteSwatches(CustomSwatchesFile, _swatchArray);
			}
		}

		void click_relabel(object sender, EventArgs e)
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
		#endregion Handlers


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

		void DrawSwatch(Graphics graphics, int id)
		{
			ColorSwatch swatch = _swatchArray[id];

			int x = swatch.Location.X;
			int y = swatch.Location.Y;

			Color color = swatch.Color;
			if (color == Color.Empty)
				color = BackColor;

			using (var brush = new SolidBrush(color))
				graphics.FillRectangle(brush, x,y, 10,10);

			graphics.DrawRectangle(Pens.Black, x,y, 10,10);
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
					for (int id = 0; id != _swatches.Count; ++id)
					{
						ColorSwatch swatch = _swatches[id];
						swatch.Location = new Point(x,y);
						_swatchArray[val++] = swatch; // effin structs

						DrawSwatch(graphics, id);
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

						_swatchArray[val] = new ColorSwatch(new Point(x,y));

						DrawSwatch(graphics, val++);
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
			var rect = new Rectangle(_startX, _startY, _outerWidth - 12, _outerHeight - 12);
			return rect.Contains(x,y);
		}

		ColorSwatch GetColorSwatch(int x, int y)
		{
			return _swatchArray[GetSwatchId(x,y)];
		}

		int GetSwatchId(int x, int y)
		{
			return ((y - 6) / 12) * _hori + ((x - 6) / 12);
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
								graphics.FillRectangle(brush, x,y, 10,10);

							graphics.DrawRectangle(Pens.Black, x,y, 10,10);
						}

						--_blankSwatches;

						ColorSwatch swatch = _swatchArray[_nextSwatchId];
						swatch.Color       = color;
						swatch.Description = f.ColorDescription;
						_swatchArray[_nextSwatchId] = swatch;

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
			var rect = new Rectangle(swatchPoint, new Size(11,11)); // why.
			Invalidate(rect);
		}
		#endregion Methods


		#region Methods (static)
		static bool DoesCursorIntersectWithSwatch(ColorSwatch swatch, int x, int y)
		{
			return swatch.Rect.Contains(x,y);
		}
		#endregion Methods (static)



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
			this.itDelete.Text = "itDelete";
			this.itDelete.Click += new System.EventHandler(this.click_delete);
			// 
			// itRelabel
			// 
			this.itRelabel.Index = 1;
			this.itRelabel.Text = "itRelabel";
			this.itRelabel.Click += new System.EventHandler(this.click_relabel);
			// 
			// contextMenu
			// 
			this.context.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
			this.itRelabel,
			this.itDelete});
			// 
			// SwatchPanel
			// 
			this.AllowDrop = true;
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "SwatchPanel";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}


	internal delegate void SwatchSelectedEventHandler(ColorEventArgs e);
}
