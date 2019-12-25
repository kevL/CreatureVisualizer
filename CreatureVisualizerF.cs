using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.DirectX;

using OEIShared.NetDisplay;
using OEIShared.OEIMath;
using OEIShared.UI.Input;


namespace creaturevisualizer
{
	sealed partial class CreatureVisualizerF
		: Form
	{
		#region Fields (static)
		internal const string TITLE = "Creature Visualizer";

		const int BDI = 22; // minipanel Button DImensions x/y
		#endregion Fields (static)


		#region Fields
		/// <summary>
		/// The ElectronPanel panel.
		/// </summary>
		CreatureVisualizerP _panel;

		MenuItem _itStayOnTop;
		MenuItem _itRefreshOnFocus;
		MenuItem _itControlPanel;
		MenuItem _itMiniPanel;
		MenuItem _itCyclePanel;

		Timer _t1 = new Timer();
		Button _repeater;
		bool _firstrepeat;

		/// <summary>
		/// Compass direction that the controlpanel is docked at.
		/// </summary>
		CpDir _dir;

		int _pa_Gui_w, _pa_Gui_h,
			_pa_Con_w, _pa_Con_h;

		Button _i = new Button(), _o = new Button(), // in/out
			   _u = new Button(), _d = new Button(), // up/down
			   _l = new Button(), _r = new Button(); // left/right
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor. Creates and adds menuitems and adds the NetDisplay panel. Also
		/// attempts to create and render a blueprint instance.
		/// </summary>
		internal CreatureVisualizerF()
		{
			InitializeComponent();

			Text = TITLE;

			_panel = new CreatureVisualizerP(this);
			_panel.Dock = DockStyle.Fill;
			_panel.BorderStyle = BorderStyle.FixedSingle;
			Controls.Add(_panel);

			_panel.BringToFront();


			int x = CreatureVisualizerPreferences.that.x;
			if (x != Int32.MinValue)
			{
				int y = CreatureVisualizerPreferences.that.y;
				if (checklocation(x,y))
				{
					StartPosition = FormStartPosition.Manual;
					SetDesktopLocation(x,y);
				}
				ClientSize = new Size(CreatureVisualizerPreferences.that.w,
									  CreatureVisualizerPreferences.that.h);
			}
			else
				ClientSize = new Size(ClientSize.Width - pa_con.Width,	// the ControlPanel starts non-visible
									  ClientSize.Height);				// but let it show in the designer


			_t1.Tick += tick;

			CreateMainMenu();


			SuspendLayout();
			CreateButtons();
			ResumeLayout(false);

			_pa_Con_w = pa_con.Width;
			_pa_Con_h = pa_con.Height;

			tb_camera_height  .Text = CreatureVisualizerP.POS_OFF_Zd.Z                 .ToString("N2");
			tb_light_intensity.Text = CreatureVisualizerPreferences.that.LightIntensity.ToString("N2");


			if (CreatureVisualizerP.ColorDiffuse != null)
			{
				cb_light_diffuse.Enabled = true;
				cb_light_diffuse.Checked = CreatureVisualizerP.ColorCheckedDiffuse;
			}

			if (CreatureVisualizerP.ColorSpecular != null)
			{
				cb_light_specular.Enabled = true;
				cb_light_specular.Checked = CreatureVisualizerP.ColorCheckedSpecular;
			}

			if (CreatureVisualizerP.ColorAmbient != null)
			{
				cb_light_ambient.Enabled = true;
				cb_light_ambient.Checked = CreatureVisualizerP.ColorCheckedAmbient;
			}


			// Preferences ->
			if (!CreatureVisualizerPreferences.that.StayOnTop)
				_itStayOnTop.PerformClick();

			if (!CreatureVisualizerPreferences.that.RefreshOnFocus)
				_itRefreshOnFocus.PerformClick();

			_dir = (CpDir)CreatureVisualizerPreferences.that.ControlPanelDirection;

			if (CreatureVisualizerPreferences.that.ShowControls)
				_itControlPanel.PerformClick();

			if (!CreatureVisualizerPreferences.that.ShowMinipanel)
				_itMiniPanel.PerformClick();

			tc1.SelectedIndex = CreatureVisualizerPreferences.that.TabPageCurrent;

			cb_char_female.Checked = CreatureVisualizerPreferences.that.char_Female;


			ActiveControl = _panel;
		}


		/// <summary>
		/// Checks if the initial location is onscreen.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		bool checklocation(int x, int y)
		{
			x += 100; y += 50;

			Screen[] screens = Screen.AllScreens;
			foreach (var screen in screens)
			{
				if (screen.WorkingArea.Contains(x,y))
					return true;
			}
			return false;
		}


		/// <summary>
		/// Instantiates the MainMenu.
		/// </summary>
		void CreateMainMenu()
		{
			Menu = new MainMenu();

			Menu.MenuItems.Add("&Instance");
			Menu.MenuItems.Add("&Options");
			Menu.MenuItems.Add("&Help");

			// Instance ->
			Menu.MenuItems[0].MenuItems.Add("&refresh", instanceclick_Refresh);
			Menu.MenuItems[0].MenuItems[0].Shortcut = Shortcut.F5;

			Menu.MenuItems[0].MenuItems.Add("-");

			_itRefreshOnFocus = Menu.MenuItems[0].MenuItems.Add("refresh on foc&us", instanceclick_RefreshOnFocus);
			_itRefreshOnFocus.Shortcut = Shortcut.F6;
			_itRefreshOnFocus.Checked = true;

			// Options ->
			_itControlPanel = Menu.MenuItems[1].MenuItems.Add("control &panel", optionsclick_ControlPanel);
			_itControlPanel.Shortcut = Shortcut.CtrlP;

			_itMiniPanel = Menu.MenuItems[1].MenuItems.Add("&mini panel", optionsclick_MiniPanel);
			_itMiniPanel.Shortcut = Shortcut.CtrlM;
			_itMiniPanel.Checked = true;

			Menu.MenuItems[1].MenuItems.Add("-");

			_itCyclePanel = Menu.MenuItems[1].MenuItems.Add("&cycle panel", optionsclick_CyclePanel);
			_itCyclePanel.Shortcut = Shortcut.F8;
			_itCyclePanel.Enabled = false;

			Menu.MenuItems[1].MenuItems.Add("-");

			_itStayOnTop = Menu.MenuItems[1].MenuItems.Add("stay on &top", optionsclick_StayOnTop);
			_itStayOnTop.Shortcut = Shortcut.CtrlT;
			_itStayOnTop.Checked = TopMost = true;

			// Help ->
			Menu.MenuItems[2].MenuItems.Add("&help", helpclick_Help);
			Menu.MenuItems[2].MenuItems[0].Shortcut = Shortcut.F1;

			Menu.MenuItems[2].MenuItems.Add("&about", helpclick_About);
			Menu.MenuItems[2].MenuItems[1].Shortcut = Shortcut.F2;
		}


		/// <summary>
		/// Creates buttons for the MiniPanel.
		/// </summary>
		void CreateButtons()
		{
			_i = ButtonFactory(_i, "+");
			_i.Click += click_bu_camera_distneg;
			_o = ButtonFactory(_o, "-");
			_o.Click += click_bu_camera_distpos;

			_l = ButtonFactory(_l, "l");
			_l.Click += click_bu_camera_yawneg;
			_r = ButtonFactory(_r, "r");
			_r.Click += click_bu_camera_yawpos;

			_u = ButtonFactory(_u, "u");
			_u.Click += click_bu_camera_zpos;
			_d = ButtonFactory(_d, "d");
			_d.Click += click_bu_camera_zneg;

			Controls.Add(_i);
			Controls.Add(_o);
			Controls.Add(_l);
			Controls.Add(_r);
			Controls.Add(_u);
			Controls.Add(_d);

			_i.BringToFront();
			_o.BringToFront();
			_l.BringToFront();
			_r.BringToFront();
			_u.BringToFront();
			_d.BringToFront();
		}

		Button ButtonFactory(Button b, string text)
		{
			b.MouseDown += mousedown_EnableRepeater;
			b.MouseUp   += mouseup_DisableRepeater;
			b.Text   = text;
			b.Width  =
			b.Height = BDI;

			return b;
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnActivated(EventArgs e)
		{
			if (_itRefreshOnFocus.Checked && WindowState != FormWindowState.Minimized)
				_panel.CreateInstance();
		}

		protected override void OnResize(EventArgs e)
		{
			switch (WindowState)
			{
				case FormWindowState.Normal:
					if (!_toggle
						&& (_itControlPanel == null || !_itControlPanel.Checked))
					{
						_pa_Gui_w = ClientSize.Width;
						_pa_Gui_h = ClientSize.Height;
					}
					LayoutButtons();
					break;

				case FormWindowState.Maximized:
					if (_itControlPanel != null && _itControlPanel.Checked)
					{
						switch (_dir)
						{
							case CpDir.n:
								_dir = CpDir.e;
								UpdatePanel();
								CreatureVisualizerPreferences.that.ControlPanelDirection = (int)_dir;
								return;

							case CpDir.s:
								_dir = CpDir.w;
								UpdatePanel();
								CreatureVisualizerPreferences.that.ControlPanelDirection = (int)_dir;
								return;
						}
					}
					LayoutButtons();
					break;
			}

			base.OnResize(e);
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			CreatureVisualizerPreferences.that.x = DesktopLocation.X;
			CreatureVisualizerPreferences.that.y = DesktopLocation.Y;

			// store Width and Height as if the controlpanel is closed ->
			CreatureVisualizerPreferences.that.w = pa_gui.Width;
			CreatureVisualizerPreferences.that.h = pa_gui.Height;


			_t1.Dispose();
			_t1 = null;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.Escape:
					_panel.Focus();
					break;
			}
		}
		#endregion Handlers (override)


		#region Handlers
		void selectedindexchanged_TabControl(object sender, EventArgs e)
		{
			CreatureVisualizerPreferences.that.TabPageCurrent = tc1.SelectedIndex;
		}
		#endregion Handlers


		#region Handlers (menu)
		void instanceclick_Refresh(object sender, EventArgs e)
		{
			_panel.CreateInstance();
		}

		void instanceclick_RefreshOnFocus(object sender, EventArgs e)
		{
			CreatureVisualizerPreferences.that.RefreshOnFocus =
			_itRefreshOnFocus.Checked = !_itRefreshOnFocus.Checked;
		}


		bool _toggle;

		void optionsclick_ControlPanel(object sender, EventArgs e)
		{
			if (_itControlPanel.Checked = !_itControlPanel.Checked)
			{
				_toggle = true;

				switch (WindowState)
				{
					case FormWindowState.Normal:
					{
						int w,h;
						switch (_dir)
						{
							default: //case CpDir.n:
								pa_con.Dock = DockStyle.Top;
								w = _pa_Gui_w;
								h = _pa_Gui_h + _pa_Con_h;
								break;

							case CpDir.e:
								pa_con.Dock = DockStyle.Right;
								w = _pa_Gui_w + _pa_Con_w;
								h = _pa_Gui_h;
								break;

							case CpDir.s:
								pa_con.Dock = DockStyle.Bottom;
								w = _pa_Gui_w;
								h = _pa_Gui_h + _pa_Con_h;
								break;

							case CpDir.w:
								pa_con.Dock = DockStyle.Left;
								w = _pa_Gui_w + _pa_Con_w;
								h = _pa_Gui_h;
								break;
						}
						ClientSize = new Size(w,h);

						goto case FormWindowState.Maximized;
					}

					case FormWindowState.Maximized:
						pa_con.Visible = _itCyclePanel.Enabled = true;
						LayoutButtons();

						CreatureVisualizerPreferences.that.ShowControls = true;
						break;
				}

				_toggle = false;
			}
			else // panel closed ->
			{
				switch (WindowState)
				{
					case  FormWindowState.Normal:
					{
						int w,h;
						switch (_dir)
						{
							case CpDir.n: case CpDir.s:
								w = ClientSize.Width;
								h = ClientSize.Height - _pa_Con_h;
								break;
	
							default: // case CpDir.e: case CpDir.w:
								w = ClientSize.Width - _pa_Con_w;
								h = ClientSize.Height;
								break;
						}
						ClientSize = new Size(w,h);

						goto case FormWindowState.Maximized;
					}

					case FormWindowState.Maximized:
						pa_con.Visible = _itCyclePanel.Enabled = false;
						LayoutButtons();

						CreatureVisualizerPreferences.that.ShowControls = false;
						_panel.Focus();
						break;
				}
			}
		}

		/// <summary>
		/// [F8] cycles the controlpanel though its docking directions.
		/// </summary>
		void UpdatePanel()
		{
			if (WindowState != FormWindowState.Minimized)
			{
				_pa_Gui_w = pa_gui.Width;
				_pa_Gui_h = pa_gui.Height;

//				la_dx.Text = ClientSize.Height.ToString();
//				la_dy.Text = _pa_Gui_h.ToString();
//				la_dz.Text = _pa_Con_h.ToString();

				_toggle = true;

				int w,h;
				switch (_dir)
				{
					default: //case CpDir.n:
						pa_con.Dock = DockStyle.Top;
						w = _pa_Gui_w;
						h = _pa_Gui_h + _pa_Con_h;
						break;

					case CpDir.e:
						pa_con.Dock = DockStyle.Right;
						w = _pa_Gui_w + _pa_Con_w;
						h = _pa_Gui_h;
						break;

					case CpDir.s:
						pa_con.Dock = DockStyle.Bottom;
						w = _pa_Gui_w;
						h = _pa_Gui_h + _pa_Con_h;
						break;

					case CpDir.w:
						pa_con.Dock = DockStyle.Left;
						w = _pa_Gui_w + _pa_Con_w;
						h = _pa_Gui_h;
						break;
				}
				ClientSize = new Size(w,h);

				LayoutButtons();
				_toggle = false;
			}
		}

		void optionsclick_MiniPanel(object sender, EventArgs e)
		{
			_i.Visible = _o.Visible = _u.Visible =
			_d.Visible = _l.Visible = _r.Visible =
			CreatureVisualizerPreferences.that.ShowMinipanel = (_itMiniPanel.Checked = !_itMiniPanel.Checked);
		}

		void optionsclick_CyclePanel(object sender, EventArgs e)
		{
			if (_itControlPanel.Checked)
			{
				switch (WindowState)
				{
					case FormWindowState.Normal:
						switch (_dir)
						{
							case CpDir.n: _dir = CpDir.e; break;
							case CpDir.e: _dir = CpDir.s; break;
							case CpDir.s: _dir = CpDir.w; break;
							case CpDir.w: _dir = CpDir.n; break;
						}
						UpdatePanel();

						CreatureVisualizerPreferences.that.ControlPanelDirection = (int)_dir;
						break;

					case FormWindowState.Maximized:
						switch (_dir)
						{
							case CpDir.n: case CpDir.w: _dir = CpDir.e; break;
							case CpDir.e: case CpDir.s: _dir = CpDir.w; break;
						}
						UpdatePanel();

						CreatureVisualizerPreferences.that.ControlPanelDirection = (int)_dir;
						break;
				}
			}
		}

		void optionsclick_StayOnTop(object sender, EventArgs e)
		{
			CreatureVisualizerPreferences.that.StayOnTop =
			TopMost = (_itStayOnTop.Checked = !_itStayOnTop.Checked);
		}


		void helpclick_Help(object sender, EventArgs e)
		{
			using (var f = new HelpF())
				f.ShowDialog(this);
		}

		void helpclick_About(object sender, EventArgs e)
		{
			string text = "Creature Visualizer"
						+ Environment.NewLine
						+ "- toolset plugin for Neverwinter Nights 2"
						+ Environment.NewLine + Environment.NewLine;

			var ass = Assembly.GetExecutingAssembly();
			var an = ass.GetName();
			text += an.Version.Major + "."
				  + an.Version.Minor + "."
				  + an.Version.Build + "."
				  + an.Version.Revision;
#if DEBUG
			text += " debug";
#else
//			text += " release";
			text += " beta";
#endif

			text += Environment.NewLine
				  + String.Format(CultureInfo.CurrentCulture,
								  "{0:yyyy MMM d} {0:HH}:{0:mm}:{0:ss} UTC",
								  ass.GetLinkerTime());

			MessageBox.Show(this, text, "About");
		}
		#endregion Handlers (menu)


		#region Handlers (timer)
		void mousedown_EnableRepeater(object sender, MouseEventArgs e)
		{
			_repeater = sender as Button;

			if (_t1 != null)
			{
				_firstrepeat = true;

				_t1.Interval = 233; // TODO: use System DoubleClick period or keyboard repeat-delay stuff
				_t1.Start();
			}
		}

		void mouseup_DisableRepeater(object sender, MouseEventArgs e)
		{
			if (_t1 != null) _t1.Stop();
		}

		void tick(object sender, EventArgs e)
		{
			if (_repeater != null)
			{
				_repeater.PerformClick();

				if (_firstrepeat)
				{
					_firstrepeat = false;
					_t1.Interval = 89;
				}
			}
		}
		#endregion Handlers (timer)


		#region Handlers (camera)
		internal static Vector3 Offset;

		void click_bu_camera_zpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 delta = grader(CreatureVisualizerP.off_zpos);
				_panel.CameraPosition += delta;
				Offset                += delta;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_zneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 delta = grader(CreatureVisualizerP.off_zneg);
				_panel.CameraPosition += delta;
				Offset                += delta;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_ypos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 delta = grader(CreatureVisualizerP.off_ypos);
				_panel.CameraPosition += delta;
				Offset                += delta;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_yneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 delta = grader(CreatureVisualizerP.off_yneg);
				_panel.CameraPosition += delta;
				Offset                += delta;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_xpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 delta = grader(CreatureVisualizerP.off_xpos);
				_panel.CameraPosition += delta;
				Offset                += delta;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_xneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 delta = grader(CreatureVisualizerP.off_xneg);
				_panel.CameraPosition += delta;
				Offset                += delta;
				PrintCameraPosition();
			}
		}


		internal void click_bu_camera_distpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				((ModelViewerInputCameraReceiverState)_panel.Receiver.CameraState).Distance += grader(0.1F);
				_panel.UpdateCamera();
				PrintCameraPosition();
			}
		}

		internal void click_bu_camera_distneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				((ModelViewerInputCameraReceiverState)_panel.Receiver.CameraState).Distance -= grader(0.1F);
				_panel.UpdateCamera();
				PrintCameraPosition();
			}
		}


		void click_bu_camera_pitchpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.RaiseCameraPolar();
				PrintCameraPosition();
			}
		}

		void click_bu_camera_pitchneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.LowerCameraPolar();
				PrintCameraPosition();
			}
		}

		void click_bu_camera_yawpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.Receiver.CameraAngleXY += grader((float)Math.PI / 64F); // FocusTheta

				_panel.CameraPosition += CreatureVisualizerP.POS_OFF_Zd + Offset;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_yawneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.Receiver.CameraAngleXY -= grader((float)Math.PI / 64F); // FocusTheta
				_panel.CameraPosition += CreatureVisualizerP.POS_OFF_Zd + Offset;
				PrintCameraPosition();
			}
		}


		void click_bu_camera_zreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Offset.Z = 0F;
				_panel.CameraPosition = new Vector3(_panel.CameraPosition.X,
													_panel.CameraPosition.Y,
													CreatureVisualizerP.POS_START_CAMERA.Z);
				PrintCameraPosition();
			}
		}

		void click_bu_camera_xyreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Offset.X = Offset.Y = 0F;
				_panel.CameraPosition = new Vector3(CreatureVisualizerP.POS_START_CAMERA.X,
													CreatureVisualizerP.POS_START_CAMERA.Y,
													_panel.CameraPosition.Z);
				PrintCameraPosition();
			}
		}

		void click_bu_camera_resetdist(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				((ModelViewerInputCameraReceiverState)_panel.Receiver.CameraState).Distance = CreatureVisualizerP.DIST_START;
				_panel.UpdateCamera();
				PrintCameraPosition();
			}
		}

		void click_bu_camera_resetpolar(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.Receiver.CameraAngleXY = (float)Math.PI /  2F;
				_panel.Receiver.CameraAngleYZ = (float)Math.PI / 32F;
				_panel.CameraPosition += CreatureVisualizerP.POS_OFF_Zd;
				PrintCameraPosition();
			}
		}


		void click_bu_camera_focus(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				((ModelViewerInputCameraReceiverState)_panel.Receiver.CameraState).FocusPoint = _panel.Model.Position;
				Offset.X = Offset.Y = Offset.Z = 0F;
				_panel.UpdateCamera();
				PrintCameraPosition();

				//NetDisplayManager.Instance.MoveCamera  (NetDisplayWindow, ChangeType, Vector3);		// <- Position
				//NetDisplayManager.Instance.RotateCamera(NetDisplayWindow, ChangeType, RHQuaternion);	// <- Orientation

				//public static RHMatrix OEIShared.OEIMath.RHMatrix.LookAtRH(Vector3 cCameraPosition, Vector3 cCameraTarget, Vector3 cCameraUp)
				//P_1 = RHQuaternion.RotationMatrix(RHMatrix.LookAtRH(Vector3.Empty, focusPoint, v2));
			}
		}

		//OEIShared.UI.Input.FPSInputCameraReceiver
/*		public void OnMouseWheel(object sender, EPMouseEventArgs eArgs)
		{
			MousePanel mousePanel = sender as MousePanel;
			if (mousePanel != null)
			{
				ElectronPanel electronPanel = mousePanel.Parent as ElectronPanel;
				if (electronPanel != null && mousePanel.WheelDelta != 0)
				{
					Vector3 vector = Vector3.Empty;
					vector.Y = (float)mousePanel.WheelDelta * 3f / 50f;
					mousePanel.ClearWheelDelta();
					vector = RHMatrix.RotationQuaternion(electronPanel.CameraOrientation).TransformCoordinate(vector);
					electronPanel.CameraPosition += vector;
					eArgs.Handled = true;
				}
			}
		} */


		void textchanged_tb_camera_baseheight(object sender, EventArgs e)
		{
			float result;
			if (Single.TryParse(tb_camera_height.Text, out result)
				&& result > -100F && result < 100F)
			{
				CreatureVisualizerP.POS_OFF_Zd = new Vector3(0F,0F, result);
				_panel.UpdateCamera();
			}
			else if (result <= -100F)
				tb_camera_height.Text = (-99.99F).ToString("N2");	// refire^
			else if (result >=  100F)
				tb_camera_height.Text = 99.99F.ToString("N2");		// refire^
		}

		void keydown_tb_camera_baseheight(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Enter:
					_panel.Focus();
					break;

//				case Keys.Oemplus:
				case Keys.Add:
				{
					float z = CreatureVisualizerP.POS_OFF_Zd.Z;
					z += grader(0.1F);
					tb_camera_height.Text = z.ToString("N2");

					e.Handled = e.SuppressKeyPress = true;
					break;
				}

//				case Keys.OemMinus:
				case Keys.Subtract:
				{
					float z = CreatureVisualizerP.POS_OFF_Zd.Z;
					z -= grader(0.1F);
					tb_camera_height.Text = z.ToString("N2");

					e.Handled = e.SuppressKeyPress = true;
					break;
				}
			}
		}
		#endregion Handlers (camera)


		#region Handlers (model)
		void click_bu_model_zpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveModel(grader(CreatureVisualizerP.off_zpos));
		}

		void click_bu_model_zneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveModel(grader(CreatureVisualizerP.off_zneg));
		}

		void click_bu_model_ypos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveModel(grader(CreatureVisualizerP.off_ypos));
		}

		void click_bu_model_yneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveModel(grader(CreatureVisualizerP.off_yneg));
		}

		void click_bu_model_xpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveModel(grader(CreatureVisualizerP.off_xpos));
		}

		void click_bu_model_xneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveModel(grader(CreatureVisualizerP.off_xneg));
		}


		void click_bu_model_rotpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.RotateModel(grader(CreatureVisualizerP.rotpos));
		}

		void click_bu_model_rotneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.RotateModel(grader(CreatureVisualizerP.rotneg));
		}


		void click_bu_model_scale(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 unit;

				var bu = sender as Button;
				if      (bu == bu_model_xscalepos) unit = CreatureVisualizerP.off_xpos;
				else if (bu == bu_model_xscaleneg) unit = CreatureVisualizerP.off_xneg;
				else if (bu == bu_model_yscalepos) unit = CreatureVisualizerP.off_ypos;
				else if (bu == bu_model_yscaleneg) unit = CreatureVisualizerP.off_yneg;
				else if (bu == bu_model_zscalepos) unit = CreatureVisualizerP.off_zpos;
				else                               unit = CreatureVisualizerP.off_zneg; // (bu == bu_model_zscaleneg)

				_panel.ScaleModel(grader(unit));
			}
		}

		void click_bu_model_scaleall(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				int dir;

				var bu = sender as Button;
				if (bu == bu_model_scalepos) dir = +1;
				else                         dir = -1; // (bu == bu_model_scaleneg)

				_panel.ScaleModel(dir);
			}
		}


		void click_bu_model_reset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.ResetModel();
		}

		void click_bu_model_zreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.ResetModel(ResetType.RESET_z);
		}

		void click_bu_model_xyreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.ResetModel(ResetType.RESET_xy);
		}

		void click_bu_model_rotreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.ResetModel(ResetType.RESET_rot);
		}

		void click_bu_model_scalereset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.ResetModel(ResetType.RESET_scale);
		}
		#endregion Handlers (model)


		#region Handlers (light)
		void click_bu_light_zpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(CreatureVisualizerP.off_zpos));
		}

		void click_bu_light_zneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(CreatureVisualizerP.off_zneg));
		}

		void click_bu_light_ypos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(CreatureVisualizerP.off_ypos));
		}

		void click_bu_light_yneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(CreatureVisualizerP.off_yneg));
		}

		void click_bu_light_xpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(CreatureVisualizerP.off_xpos));
		}

		void click_bu_light_xneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(CreatureVisualizerP.off_xneg));
		}


		void click_bu_light_zreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				var pos = new Vector3(_panel.Light.Position.X,
									  _panel.Light.Position.Y,
									  CreatureVisualizerP.POS_START_LIGHT.Z);
				_panel.MoveLight(pos);
			}
		}

		void click_bu_light_xyreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				var pos = new Vector3(CreatureVisualizerP.POS_START_LIGHT.X,
									  CreatureVisualizerP.POS_START_LIGHT.Y,
									  _panel.Light.Position.Z);
				_panel.MoveLight(pos);
			}
		}

		void click_bu_light_reset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(CreatureVisualizerP.POS_START_LIGHT);
		}


		void textchanged_tb_light_intensity(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				float result;
				if (Single.TryParse(tb_light_intensity.Text, out result)
					&& result >= 0F && result < 100F)
				{
					CreatureVisualizerPreferences.that.LightIntensity =
					_panel.Light.Color.Intensity = result;
					PrintLightIntensity(result);
				}
				else if (result < 0F)
					tb_light_intensity.Text = 0F.ToString("N2");		// refire^
				else if (result >= 100F)
					tb_light_intensity.Text = 99.99F.ToString("N2");	// refire^
			}
		}

		void keydown_tb_light_intensity(object sender, KeyEventArgs e)
		{
			if (_panel.Model != null)
			{
				switch (e.KeyCode)
				{
					case Keys.Enter:
						_panel.Focus();
						break;

//					case Keys.Oemplus:
					case Keys.Add:
					{
						float i = _panel.Light.Color.Intensity;
						i += grader(0.1F);
						tb_light_intensity.Text = i.ToString("N2");

						e.Handled = e.SuppressKeyPress = true;
						break;
					}

//					case Keys.OemMinus:
					case Keys.Subtract:
					{
						float i = _panel.Light.Color.Intensity;
						i -= grader(0.1F);
						tb_light_intensity.Text = i.ToString("N2");

						e.Handled = e.SuppressKeyPress = true;
						break;
					}
				}
			}
		}



		internal static bool BypassCreate;

		ColorF _sano;

		void click_pa_light_diffuse(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				BypassCreate = true;

				_sano = new ColorF();
				_sano.ColorPanel.ColorChanged += colorchanged_diff;

				Color color;
				if (CreatureVisualizerP.ColorDiffuse != null)
				{
					color = _sano.ColorPanel.Color = (Color)CreatureVisualizerP.ColorDiffuse;
				}
				else
					color = _sano.ColorPanel.Color = _panel.Light.Color.DiffuseColor;

				_sano.ColorPanel.InitInactiveColorbox(color);


				if (_sano.ShowDialog(this) == DialogResult.OK)
				{
					CreatureVisualizerP.ColorCheckedDiffuse =
					cb_light_diffuse.Enabled =
					cb_light_diffuse.Checked = true;

					CreatureVisualizerP.ColorDiffuse =
					pa_light_diffuse.BackColor =
					_panel.Light.Color.DiffuseColor = _sano.ColorPanel.Color;
				}
				else
				{
					pa_light_diffuse.BackColor =
					_panel.Light.Color.DiffuseColor = color;
				}

				_sano.Dispose();
				_sano = null;

				BypassCreate = false;
			}
		}

		void colorchanged_diff()
		{
			pa_light_diffuse.BackColor =
			_panel.Light.Color.DiffuseColor = _sano.ColorPanel.Color;
		}

		void click_pa_light_specular(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				BypassCreate = true;

				_sano = new ColorF();
				_sano.ColorPanel.ColorChanged += colorchanged_spec;

				Color color;
				if (CreatureVisualizerP.ColorSpecular != null)
				{
					color = _sano.ColorPanel.Color = (Color)CreatureVisualizerP.ColorSpecular;
				}
				else
					color = _sano.ColorPanel.Color = _panel.Light.Color.SpecularColor;

				_sano.ColorPanel.InitInactiveColorbox(color);


				if (_sano.ShowDialog(this) == DialogResult.OK)
				{
					CreatureVisualizerP.ColorCheckedSpecular =
					cb_light_specular.Enabled =
					cb_light_specular.Checked = true;

					CreatureVisualizerP.ColorSpecular =
					pa_light_specular.BackColor =
					_panel.Light.Color.SpecularColor = _sano.ColorPanel.Color;
				}
				else
				{
					pa_light_specular.BackColor =
					_panel.Light.Color.SpecularColor = color;
				}

				_sano.Dispose();
				_sano = null;

				BypassCreate = false;
			}
		}

		void colorchanged_spec()
		{
			pa_light_specular.BackColor =
			_panel.Light.Color.SpecularColor = _sano.ColorPanel.Color;
		}


		void click_pa_light_ambient(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				BypassCreate = true;

				_sano = new ColorF();
				_sano.ColorPanel.ColorChanged += colorchanged_ambi;

				Color color;
				if (CreatureVisualizerP.ColorAmbient != null)
				{
					color = _sano.ColorPanel.Color = (Color)CreatureVisualizerP.ColorAmbient;
				}
				else
					color = _sano.ColorPanel.Color = _panel.Light.Color.AmbientColor;

				_sano.ColorPanel.InitInactiveColorbox(color);


				if (_sano.ShowDialog(this) == DialogResult.OK)
				{
					CreatureVisualizerP.ColorCheckedAmbient =
					cb_light_ambient.Enabled =
					cb_light_ambient.Checked = true;

					CreatureVisualizerP.ColorAmbient =
					pa_light_ambient.BackColor =
					_panel.Light.Color.AmbientColor = _sano.ColorPanel.Color;
				}
				else
				{
					pa_light_ambient.BackColor =
					_panel.Light.Color.AmbientColor = color;
				}

				_sano.Dispose();
				_sano = null;

				BypassCreate = false;
			}
		}

		void colorchanged_ambi()
		{
			pa_light_ambient.BackColor =
			_panel.Light.Color.AmbientColor = _sano.ColorPanel.Color;
		}


		void click_cb_light_diffuse(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				if (CreatureVisualizerP.ColorCheckedDiffuse = cb_light_diffuse.Checked)
					_panel.Light.Color.DiffuseColor = (Color)CreatureVisualizerP.ColorDiffuse;
				else
					_panel.Light.Color.DiffuseColor = Color.White;
			}
		}

		void click_cb_light_specular(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				if (CreatureVisualizerP.ColorCheckedSpecular = cb_light_specular.Checked)
					_panel.Light.Color.SpecularColor = (Color)CreatureVisualizerP.ColorSpecular;
				else
					_panel.Light.Color.SpecularColor = Color.White;
			}
		}

		void click_cb_light_ambient(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				if (CreatureVisualizerP.ColorCheckedAmbient = cb_light_ambient.Checked)
					_panel.Light.Color.AmbientColor = (Color)CreatureVisualizerP.ColorAmbient;
				else
					_panel.Light.Color.AmbientColor = Color.White;
			}
		}


		void mouseup_cb_light_diffuse(object sender, MouseEventArgs e)
		{
			if (_panel.Model != null && e.Button == MouseButtons.Right)
			{
				CreatureVisualizerP.ColorDiffuse = null;

				pa_light_diffuse.BackColor =
				_panel.Light.Color.DiffuseColor = Color.White;

				CreatureVisualizerP.ColorCheckedDiffuse =
				cb_light_diffuse.Checked =
				cb_light_diffuse.Enabled = false;

				_panel.Focus();
			}
		}

		void mouseup_cb_light_specular(object sender, MouseEventArgs e)
		{
			if (_panel.Model != null && e.Button == MouseButtons.Right)
			{
				CreatureVisualizerP.ColorSpecular = null;

				pa_light_specular.BackColor =
				_panel.Light.Color.SpecularColor = Color.White;

				CreatureVisualizerP.ColorCheckedSpecular =
				cb_light_specular.Checked =
				cb_light_specular.Enabled = false;

				_panel.Focus();
			}
		}

		void mouseup_cb_light_ambient(object sender, MouseEventArgs e)
		{
			if (_panel.Model != null && e.Button == MouseButtons.Right)
			{
				CreatureVisualizerP.ColorAmbient = null;

				pa_light_ambient.BackColor =
				_panel.Light.Color.AmbientColor = Color.White;

				CreatureVisualizerP.ColorCheckedAmbient =
				cb_light_ambient.Checked =
				cb_light_ambient.Enabled = false;

				_panel.Focus();
			}
		}
		#endregion Handlers (light)


		#region Handlers (character)
		void click_bu_char_apply(object sender, EventArgs e)
		{
			CreatureVisualizerPreferences.that.char_Female = cb_char_female.Checked;

			if (_panel.Model != null)
				_panel.CreateInstance();
		}
		#endregion Handlers (character)


		#region Methods
		/// <summary>
		/// Lays out the MiniPanel's buttons.
		/// </summary>
		void LayoutButtons()
		{
			if (WindowState != FormWindowState.Minimized)
			{
				int offx, offy;
				if (_itControlPanel != null && _itControlPanel.Checked)
				{
					offx = _pa_Con_w;
					offy = _pa_Con_h;
				}
				else
					offx = offy = 0;

				switch (_dir)
				{
					case CpDir.n:
						_i.Location = new Point(0, ClientRectangle.Bottom - BDI * 2);
						_o.Location = new Point(0, ClientRectangle.Bottom - BDI);
	
						_u.Location = new Point(ClientRectangle.Right - BDI, ClientRectangle.Bottom - BDI * 2);
						_d.Location = new Point(ClientRectangle.Right - BDI, ClientRectangle.Bottom - BDI);
	
						_l.Location = new Point(ClientRectangle.Right / 2 - BDI, ClientRectangle.Bottom - BDI);
						_r.Location = new Point(ClientRectangle.Right / 2,       ClientRectangle.Bottom - BDI);
						break;

					case CpDir.e:
						_i.Location = new Point(0, ClientRectangle.Bottom - BDI * 2);
						_o.Location = new Point(0, ClientRectangle.Bottom - BDI);
	
						_u.Location = new Point(ClientRectangle.Right - offx - BDI, ClientRectangle.Bottom - BDI * 2);
						_d.Location = new Point(ClientRectangle.Right - offx - BDI, ClientRectangle.Bottom - BDI);
	
						_l.Location = new Point((ClientRectangle.Right - offx) / 2 - BDI, ClientRectangle.Bottom - BDI);
						_r.Location = new Point((ClientRectangle.Right - offx) / 2,       ClientRectangle.Bottom - BDI);
						break;

					case CpDir.s:
						_i.Location = new Point(0, ClientRectangle.Bottom - offy - BDI * 2);
						_o.Location = new Point(0, ClientRectangle.Bottom - offy - BDI);

						_u.Location = new Point(ClientRectangle.Right - BDI, ClientRectangle.Bottom - offy - BDI * 2);
						_d.Location = new Point(ClientRectangle.Right - BDI, ClientRectangle.Bottom - offy - BDI);

						_l.Location = new Point(ClientRectangle.Right / 2 - BDI, ClientRectangle.Bottom - offy - BDI);
						_r.Location = new Point(ClientRectangle.Right / 2,       ClientRectangle.Bottom - offy - BDI);
						break;

					case CpDir.w:
						_i.Location = new Point(offx, ClientRectangle.Bottom - BDI * 2);
						_o.Location = new Point(offx, ClientRectangle.Bottom - BDI);

						_u.Location = new Point(ClientRectangle.Right - BDI, ClientRectangle.Bottom - BDI * 2);
						_d.Location = new Point(ClientRectangle.Right - BDI, ClientRectangle.Bottom - BDI);

						_l.Location = new Point((ClientRectangle.Right - offx) / 2 + offx - BDI, ClientRectangle.Bottom - BDI);
						_r.Location = new Point((ClientRectangle.Right - offx) / 2 + offx,       ClientRectangle.Bottom - BDI);
						break;
				}
			}
		}


		internal int getrot()
		{
			return Int32.Parse(tssl_camera_rot.Text);
		}

		internal void PrintCameraPosition()
		{
			// position ->
			Vector3 pos = _panel.CameraPosition;

			tssl_camera_xpos.Text = pos.X.ToString("N2");
			tssl_camera_ypos.Text = pos.Y.ToString("N2");
			tssl_camera_zpos.Text = pos.Z.ToString("N2");

			// rotation ->
			var axis = new Vector3();
			float angle = 0F;
			RHQuaternion.ToAxisAngle(_panel.CameraOrientation, ref axis, ref angle);

			if (axis.Z < 0F) angle = -angle;
			angle *= 180F / (float)Math.PI;
			if (angle < 0F) angle += 360F;

			tssl_camera_rot.Text = ((int)angle).ToString(); // 0 is north, goes clockwise


			var state = _panel.Receiver.CameraState as ModelViewerInputCameraReceiverState;
			la_camera_pitch.Text = ((int)(state.FocusPhi   * 180F / (float)Math.PI)).ToString(); // to degs
			la_camera_yaw  .Text = ((int)(state.FocusTheta * 180F / (float)Math.PI) % 360).ToString();

			tssl_camera_dist.Text = _panel.Receiver.Distance.ToString("N2");
		}

		/// <summary>
		/// quaternions ... because why not
		/// </summary>
		/// <param name="object"></param>
		internal void PrintModelPosition(NetDisplayObject @object)
		{
			// TODO: group per z,x/y,rot separately - too many prints here.

			// position ->
			Vector3 pos = @object.Position;

			tssl_model_xpos.Text = pos.X.ToString("N2");
			tssl_model_ypos.Text = pos.Y.ToString("N2");
			tssl_model_zpos.Text = pos.Z.ToString("N2");

			// rotation ->
			var axis = new Vector3();
			float angle = 0F;
			RHQuaternion.ToAxisAngle(@object.Orientation, ref axis, ref angle);

			if (axis.Z < 0F) angle = -angle;
			angle *= 180F / (float)Math.PI;
			if (angle < 0F) angle += 360F;

			tssl_model_rot.Text = ((int)angle).ToString(); // 0 is north, goes clockwise
		}
		//set: Orientation = RHQuaternion.RotationAxis(new Vector3(0f, 0f, 1f), (float)value * ((float)Math.PI / 180f));

		internal void PrintModelScale()
		{
			la_model_xscale.Text = _panel.Model.Scale.X.ToString("N2");
			la_model_yscale.Text = _panel.Model.Scale.Y.ToString("N2");
			la_model_zscale.Text = _panel.Model.Scale.Z.ToString("N2");
		}

		internal void PrintOriginalScale(string scale)
		{
			la_model_scaleorg.Text = scale;
		}


		internal void PrintLightPosition(Vector3 pos)
		{
			tssl_light_xpos.Text = pos.X.ToString("N2");
			tssl_light_ypos.Text = pos.Y.ToString("N2");
			tssl_light_zpos.Text = pos.Z.ToString("N2");
		}

		internal void PrintLightIntensity(float intensity)
		{
			tssl_light_intensity.Text = intensity.ToString("N2");
		}

		internal void PrintDiffuseColor()
		{
			if (CreatureVisualizerP.ColorDiffuse != null)
				pa_light_diffuse.BackColor = (Color)CreatureVisualizerP.ColorDiffuse;
			else
				pa_light_diffuse.BackColor = _panel.Light.Color.DiffuseColor;
		}

		internal void PrintSpecularColor()
		{
			if (CreatureVisualizerP.ColorSpecular != null)
				pa_light_specular.BackColor = (Color)CreatureVisualizerP.ColorSpecular;
			else
				pa_light_specular.BackColor = _panel.Light.Color.SpecularColor;
		}

		internal void PrintAmbientColor()
		{
			if (CreatureVisualizerP.ColorAmbient != null)
				pa_light_ambient.BackColor = (Color)CreatureVisualizerP.ColorAmbient;
			else
				pa_light_ambient.BackColor = _panel.Light.Color.AmbientColor;
		}


		/// <summary>
		/// Returns the current step used by inc/dec gradations.
		/// </summary>
		/// <param name="unit"></param>
		/// <returns></returns>
		internal Vector3 grader(Vector3 unit)
		{
			switch (Control.ModifierKeys)
			{
				case Keys.Control: return unit * 10.0F;
				case Keys.Shift:   return unit *  0.1F;
			}
			return unit;
		}

		/// <summary>
		/// Returns the current step used by inc/dec gradations.
		/// </summary>
		/// <param name="unit"></param>
		/// <returns></returns>
		internal float grader(float unit)
		{
			switch (Control.ModifierKeys)
			{
				case Keys.Control: return unit * 10.0F;
				case Keys.Shift:   return unit *  0.1F;
			}
			return unit;
		}


		internal void EnableCharacterPage(bool enabled)
		{
			tp_character.Enabled = enabled;

//			foreach (Control c in tp_character.Controls)
//				c.Enabled = enabled;
		}
		#endregion Methods
	}



	/// <summary>
	/// Lifted from StackOverflow.com:
	/// https://stackoverflow.com/questions/1600962/displaying-the-build-date#answer-1600990
	/// - what a fucking pain in the ass.
	/// </summary>
	static class DateTimeExtension
	{
		/// <summary>
		/// Gets the time/date of build timestamp.
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		internal static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
		{
			var filePath = assembly.Location;
			const int c_PeHeaderOffset = 60;
			const int c_LinkerTimestampOffset = 8;

			var buffer = new byte[2048];

			using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
				stream.Read(buffer, 0, 2048);

			var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
			var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			return epoch.AddSeconds(secondsSince1970);
		}
	}


	/// <summary>
	/// The direction of the controlpanel popout.
	/// </summary>
	enum CpDir
	{
		n,e,s,w
	}
}
