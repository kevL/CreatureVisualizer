using System;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.DirectX;

using OEIShared.NetDisplay;
using OEIShared.OEIMath;
using OEIShared.UI.Input;


namespace creaturevisualizer
{
	sealed partial class CreVisF
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
		ElectronPanel_ _panel;

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
		internal CreVisF()
		{
			InitializeComponent();
			Text = TITLE;

			ss_camera.Renderer =
			ss_light .Renderer =
			ss_model .Renderer = new StripRenderer();

			tb_camera_baseheight.MouseWheel += mousewheel_textbox;
			tb_light_intensity  .MouseWheel += mousewheel_textbox;

			_panel = new ElectronPanel_(this);
			_panel.Dock = DockStyle.Fill;
			_panel.BorderStyle = BorderStyle.FixedSingle;
			Controls.Add(_panel);

			_panel.BringToFront();


			int x = CreatureVisualizerPreferences.that.x;
			if (x > -1)
			{
				int y = CreatureVisualizerPreferences.that.y;
				if (y > -1 && checklocation(x,y))
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

			tb_camera_baseheight.Text = ElectronPanel_.CAM_BASEHEIGHT.Z                  .ToString("N2");
			tb_light_intensity  .Text = CreatureVisualizerPreferences.that.LightIntensity.ToString("N2");


			if (ElectronPanel_.ColorDiffuse != null)
			{
				cb_light_diffuse.Enabled = true;
				cb_light_diffuse.Checked = ElectronPanel_.ColorCheckedDiffuse;
			}

			if (ElectronPanel_.ColorSpecular != null)
			{
				cb_light_specular.Enabled = true;
				cb_light_specular.Checked = ElectronPanel_.ColorCheckedSpecular;
			}

			if (ElectronPanel_.ColorAmbient != null)
			{
				cb_light_ambient.Enabled = true;
				cb_light_ambient.Checked = ElectronPanel_.ColorCheckedAmbient;
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
		internal static bool checklocation(int x, int y)
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
			_l.Click += click_bu_camera_rotneg;
			_r = ButtonFactory(_r, "r");
			_r.Click += click_bu_camera_rotpos;

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
				_panel.CreateModel();
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

		void mousewheel_textbox(object sender, MouseEventArgs e)
		{
			Keys                  keydata = Keys.None;
			if      (e.Delta > 0) keydata = Keys.Add;
			else if (e.Delta < 0) keydata = Keys.Subtract;

			if (keydata != Keys.None)
			{
				if (sender == tb_camera_baseheight)
				{
					keydown_tb_camera_baseheight(null, new KeyEventArgs(keydata));
				}
				else // sender == tb_light_intensity
				{
					keydown_tb_light_intensity(null, new KeyEventArgs(keydata));
				}
			}
		}
		#endregion Handlers


		#region Handlers (menu)
		void instanceclick_Refresh(object sender, EventArgs e)
		{
			_panel.CreateModel();
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
			var f = new HelpF();
			f.Show(this);
		}

		void helpclick_About(object sender, EventArgs e)
		{
			using (var f = new AboutF())
				f.ShowDialog(this);
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
				Vector3 delta = grader(ElectronPanel_.off_zpos);
				_panel.CameraPosition += delta;
				Offset                += delta;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_zneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 delta = grader(ElectronPanel_.off_zneg);
				_panel.CameraPosition += delta;
				Offset                += delta;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_ypos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 delta = grader(ElectronPanel_.off_ypos);
				_panel.CameraPosition += delta;
				Offset                += delta;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_yneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 delta = grader(ElectronPanel_.off_yneg);
				_panel.CameraPosition += delta;
				Offset                += delta;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_xpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 delta = grader(ElectronPanel_.off_xpos);
				_panel.CameraPosition += delta;
				Offset                += delta;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_xneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 delta = grader(ElectronPanel_.off_xneg);
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

		void click_bu_camera_rotpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.Receiver.CameraAngleXY += grader((float)Math.PI / 64F); // FocusTheta

				_panel.CameraPosition += ElectronPanel_.CAM_BASEHEIGHT + Offset;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_rotneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.Receiver.CameraAngleXY -= grader((float)Math.PI / 64F); // FocusTheta
				_panel.CameraPosition += ElectronPanel_.CAM_BASEHEIGHT + Offset;
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
													ElectronPanel_.CAM_START_POS.Z + ElectronPanel_.CAM_BASEHEIGHT.Z);
				PrintCameraPosition();
				_panel.Focus();
			}
		}

		void click_bu_camera_xyreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Offset.X = Offset.Y = 0F;
				_panel.CameraPosition = new Vector3(ElectronPanel_.CAM_START_POS.X,
													ElectronPanel_.CAM_START_POS.Y,
													_panel.CameraPosition.Z);
				PrintCameraPosition();
				_panel.Focus();
			}
		}

		void click_bu_camera_resetdist(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				((ModelViewerInputCameraReceiverState)_panel.Receiver.CameraState).Distance = ElectronPanel_.CAM_START_DIST;
				_panel.UpdateCamera();
				PrintCameraPosition();
				_panel.Focus();
			}
		}

		void click_bu_camera_resetpolar(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.Receiver.CameraAngleXY = ElectronPanel_.CAM_START_TET;
				_panel.Receiver.CameraAngleYZ = ElectronPanel_.CAM_START_PHI;
				_panel.CameraPosition += ElectronPanel_.CAM_BASEHEIGHT;
				PrintCameraPosition();
				_panel.Focus();
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
				_panel.Focus();

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
			if (Single.TryParse(tb_camera_baseheight.Text, out result)
				&& result > -100F && result < 100F)
			{
				ElectronPanel_.CAM_BASEHEIGHT = new Vector3(0F,0F, result);
				_panel.UpdateCamera();
			}
			else if (result <= -100F)
				tb_camera_baseheight.Text = (-99.99F).ToString("N2"); // recurse^
			else if (result >=  100F)
				tb_camera_baseheight.Text =   99.99F .ToString("N2"); // recurse^
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
					float z = ElectronPanel_.CAM_BASEHEIGHT.Z;
					z += grader(0.1F);
					tb_camera_baseheight.Text = z.ToString("N2");

					e.Handled = e.SuppressKeyPress = true;
					break;
				}

//				case Keys.OemMinus:
				case Keys.Subtract:
				{
					float z = ElectronPanel_.CAM_BASEHEIGHT.Z;
					z -= grader(0.1F);
					tb_camera_baseheight.Text = z.ToString("N2");

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
				_panel.MoveModel(grader(ElectronPanel_.off_zpos));
		}

		void click_bu_model_zneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveModel(grader(ElectronPanel_.off_zneg));
		}

		void click_bu_model_ypos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveModel(grader(ElectronPanel_.off_ypos));
		}

		void click_bu_model_yneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveModel(grader(ElectronPanel_.off_yneg));
		}

		void click_bu_model_xpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveModel(grader(ElectronPanel_.off_xpos));
		}

		void click_bu_model_xneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveModel(grader(ElectronPanel_.off_xneg));
		}


		void click_bu_model_rotpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.RotateModel(grader(ElectronPanel_.rotpos));
		}

		void click_bu_model_rotneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.RotateModel(grader(ElectronPanel_.rotneg));
		}


		void click_bu_model_scale(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				Vector3 unit;

				var bu = sender as Button;
				if      (bu == bu_model_xscalepos) unit = ElectronPanel_.off_xpos;
				else if (bu == bu_model_xscaleneg) unit = ElectronPanel_.off_xneg;
				else if (bu == bu_model_yscalepos) unit = ElectronPanel_.off_ypos;
				else if (bu == bu_model_yscaleneg) unit = ElectronPanel_.off_yneg;
				else if (bu == bu_model_zscalepos) unit = ElectronPanel_.off_zpos;
				else                               unit = ElectronPanel_.off_zneg; // (bu == bu_model_zscaleneg)

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
			{
				_panel.ResetModel();
				_panel.Focus();
			}
		}

		void click_bu_model_zreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.ResetModel(ResetType.RESET_z);
				_panel.Focus();
			}
		}

		void click_bu_model_xyreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.ResetModel(ResetType.RESET_xy);
				_panel.Focus();
			}
		}

		void click_bu_model_rotreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.ResetModel(ResetType.RESET_rot);
				_panel.Focus();
			}
		}

		void click_bu_model_scalereset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.ResetModel(ResetType.RESET_scale);
				_panel.Focus();
			}
		}
		#endregion Handlers (model)


		#region Handlers (light)
		void click_bu_light_zpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(ElectronPanel_.off_zpos));
		}

		void click_bu_light_zneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(ElectronPanel_.off_zneg));
		}

		void click_bu_light_ypos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(ElectronPanel_.off_ypos));
		}

		void click_bu_light_yneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(ElectronPanel_.off_yneg));
		}

		void click_bu_light_xpos(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(ElectronPanel_.off_xpos));
		}

		void click_bu_light_xneg(object sender, EventArgs e)
		{
			if (_panel.Model != null)
				_panel.MoveLight(_panel.Light.Position + grader(ElectronPanel_.off_xneg));
		}


		void click_bu_light_zreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				var pos = new Vector3(_panel.Light.Position.X,
									  _panel.Light.Position.Y,
									  ElectronPanel_.LIGHT_START_POS.Z);
				_panel.MoveLight(pos);
				_panel.Focus();
			}
		}

		void click_bu_light_xyreset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				var pos = new Vector3(ElectronPanel_.LIGHT_START_POS.X,
									  ElectronPanel_.LIGHT_START_POS.Y,
									  _panel.Light.Position.Z);
				_panel.MoveLight(pos);
				_panel.Focus();
			}
		}

		void click_bu_light_reset(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				_panel.MoveLight(ElectronPanel_.LIGHT_START_POS);
				_panel.Focus();
			}
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
					tb_light_intensity.Text =  0.00F.ToString("N2"); // recurse^
				else if (result >= 100F)
					tb_light_intensity.Text = 99.99F.ToString("N2"); // recurse^
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

		void mouseup_pa_light_diffuse(object sender, MouseEventArgs e)
		{
			if (_panel.Model != null)
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						BypassCreate = true;

						_sano = new ColorF();
						_sano.ColorControl.ColorChanged += colorchanged_diff;

						Color color;
						if (ElectronPanel_.ColorDiffuse != null)
						{
							color = (Color)ElectronPanel_.ColorDiffuse;
						}
						else
							color = _panel.Light.Color.DiffuseColor;

						_sano.ColorControl.InitialColor(color);


						if (_sano.ShowDialog(this) == DialogResult.OK)
						{
							ElectronPanel_.ColorCheckedDiffuse =
							cb_light_diffuse.Enabled =
							cb_light_diffuse.Checked = true;

							ElectronPanel_.ColorDiffuse =
							pa_light_diffuse.BackColor =
							_panel.Light.Color.DiffuseColor = _sano.ColorControl.GetColor();
						}
						else
						{
							pa_light_diffuse.BackColor =
							_panel.Light.Color.DiffuseColor = color;
						}

						_sano.Dispose();
						_sano = null;
		
						BypassCreate = false;
						break;

					case MouseButtons.Right:
						if (cb_light_diffuse.Enabled)
							mouseup_cb_light_diffuse(null, e);
						break;
				}
			}
		}

		void colorchanged_diff()
		{
			pa_light_diffuse.BackColor =
			_panel.Light.Color.DiffuseColor = _sano.ColorControl.GetColor();
		}


		void mouseup_pa_light_specular(object sender, MouseEventArgs e)
		{
			if (_panel.Model != null)
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						BypassCreate = true;

						_sano = new ColorF();
						_sano.ColorControl.ColorChanged += colorchanged_spec;

						Color color;
						if (ElectronPanel_.ColorSpecular != null)
						{
							color = (Color)ElectronPanel_.ColorSpecular;
						}
						else
							color = _panel.Light.Color.SpecularColor;

						_sano.ColorControl.InitialColor(color);


						if (_sano.ShowDialog(this) == DialogResult.OK)
						{
							ElectronPanel_.ColorCheckedSpecular =
							cb_light_specular.Enabled =
							cb_light_specular.Checked = true;

							ElectronPanel_.ColorSpecular =
							pa_light_specular.BackColor =
							_panel.Light.Color.SpecularColor = _sano.ColorControl.GetColor();
						}
						else
						{
							pa_light_specular.BackColor =
							_panel.Light.Color.SpecularColor = color;
						}

						_sano.Dispose();
						_sano = null;
		
						BypassCreate = false;
						break;

					case MouseButtons.Right:
						if (cb_light_specular.Enabled)
							mouseup_cb_light_specular(null, e);
						break;
				}
			}
		}

		void colorchanged_spec()
		{
			pa_light_specular.BackColor =
			_panel.Light.Color.SpecularColor = _sano.ColorControl.GetColor();
		}


		void mouseup_pa_light_ambient(object sender, MouseEventArgs e)
		{
			if (_panel.Model != null)
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						BypassCreate = true;

						_sano = new ColorF();
						_sano.ColorControl.ColorChanged += colorchanged_ambi;

						Color color;
						if (ElectronPanel_.ColorAmbient != null)
						{
							color = (Color)ElectronPanel_.ColorAmbient;
						}
						else
							color = _panel.Light.Color.AmbientColor;

						_sano.ColorControl.InitialColor(color);


						if (_sano.ShowDialog(this) == DialogResult.OK)
						{
							ElectronPanel_.ColorCheckedAmbient =
							cb_light_ambient.Enabled =
							cb_light_ambient.Checked = true;

							ElectronPanel_.ColorAmbient =
							pa_light_ambient.BackColor =
							_panel.Light.Color.AmbientColor = _sano.ColorControl.GetColor();
						}
						else
						{
							pa_light_ambient.BackColor =
							_panel.Light.Color.AmbientColor = color;
						}

						_sano.Dispose();
						_sano = null;
		
						BypassCreate = false;
						break;

					case MouseButtons.Right:
						if (cb_light_ambient.Enabled)
							mouseup_cb_light_ambient(null, e);
						break;
				}
			}
		}

		void colorchanged_ambi()
		{
			pa_light_ambient.BackColor =
			_panel.Light.Color.AmbientColor = _sano.ColorControl.GetColor();
		}


		void mouseup_cb_light_diffuse(object sender, MouseEventArgs e)
		{
			if (_panel.Model != null)
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						if (ElectronPanel_.ColorCheckedDiffuse = cb_light_diffuse.Checked)
							_panel.Light.Color.DiffuseColor = (Color)ElectronPanel_.ColorDiffuse;
						else
							_panel.Light.Color.DiffuseColor = Color.White;
						break;

					case MouseButtons.Right:
						ElectronPanel_.ColorDiffuse = null;

						pa_light_diffuse.BackColor =
						_panel.Light.Color.DiffuseColor = Color.White;

						ElectronPanel_.ColorCheckedDiffuse =
						cb_light_diffuse.Checked =
						cb_light_diffuse.Enabled = false;

						_panel.Focus();
						break;
				}
			}
		}

		void mouseup_cb_light_specular(object sender, MouseEventArgs e)
		{
			if (_panel.Model != null && e.Button == MouseButtons.Right)
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						if (ElectronPanel_.ColorCheckedSpecular = cb_light_specular.Checked)
							_panel.Light.Color.SpecularColor = (Color)ElectronPanel_.ColorSpecular;
						else
							_panel.Light.Color.SpecularColor = Color.White;
						break;

					case MouseButtons.Right:
						ElectronPanel_.ColorSpecular = null;

						pa_light_specular.BackColor =
						_panel.Light.Color.SpecularColor = Color.White;

						ElectronPanel_.ColorCheckedSpecular =
						cb_light_specular.Checked =
						cb_light_specular.Enabled = false;

						_panel.Focus();
						break;
				}
			}
		}

		void mouseup_cb_light_ambient(object sender, MouseEventArgs e)
		{
			if (_panel.Model != null && e.Button == MouseButtons.Right)
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						if (ElectronPanel_.ColorCheckedAmbient = cb_light_ambient.Checked)
							_panel.Light.Color.AmbientColor = (Color)ElectronPanel_.ColorAmbient;
						else
							_panel.Light.Color.AmbientColor = Color.White;
						break;

					case MouseButtons.Right:
						ElectronPanel_.ColorAmbient = null;

						pa_light_ambient.BackColor =
						_panel.Light.Color.AmbientColor = Color.White;

						ElectronPanel_.ColorCheckedAmbient =
						cb_light_ambient.Checked =
						cb_light_ambient.Enabled = false;

						_panel.Focus();
						break;
				}
			}
		}
		#endregion Handlers (light)


		#region Handlers (character)
		void click_bu_char_apply(object sender, EventArgs e)
		{
			CreatureVisualizerPreferences.that.char_Female = cb_char_female.Checked;

			if (_panel.Model != null)
			{
				_panel.CreateModel();
				_panel.Focus();
			}
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

			// rotation/pitch ->
			int rot, pitch;
			ConvertQuaternion(_panel.CameraOrientation, out rot, out pitch);
			tssl_camera_rot.Text = la_camera_yaw.Text = rot.ToString();
			la_camera_pitch.Text = pitch.ToString();

			// distance ->
			tssl_camera_dist.Text = _panel.Receiver.Distance.ToString("N2");
		}

		/// <summary>
		/// quaternions ... because why not
		/// </summary>
		/// <param name="object"></param>
		internal void PrintModelPosition(NetDisplayObject @object)
		{
			// position ->
			Vector3 pos = @object.Position;

			tssl_model_xpos.Text = pos.X.ToString("N2");
			tssl_model_ypos.Text = pos.Y.ToString("N2");
			tssl_model_zpos.Text = pos.Z.ToString("N2");

			// rotation ->
			int rot, pitch;
			ConvertQuaternion(@object.Orientation, out rot, out pitch);
			tssl_model_rot.Text = rot.ToString();
		}

		void ConvertQuaternion(RHQuaternion quaternion, out int rot, out int pitch)
		{
			float fYaw, fPitch, fRoll;
			quaternion.GetYawPitchRoll(out fYaw, out fPitch, out fRoll);

			// rotation ->
			if (fYaw >= 0F)
			{
				rot = (int)Math.Round(fYaw * 180F / (float)Math.PI, MidpointRounding.AwayFromZero);
				rot = 360 - rot;
			}
			else
				rot = (int)Math.Round(-fYaw * 180F / (float)Math.PI, MidpointRounding.AwayFromZero);

			if (rot == 360) rot = 0;

			// pitch ->
			pitch = (int)Math.Round(-fPitch * 90F / ((float)Math.PI / 2F), MidpointRounding.AwayFromZero);
		}

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
			if (ElectronPanel_.ColorDiffuse != null)
				pa_light_diffuse.BackColor = (Color)ElectronPanel_.ColorDiffuse;
			else
				pa_light_diffuse.BackColor = _panel.Light.Color.DiffuseColor;
		}

		internal void PrintSpecularColor()
		{
			if (ElectronPanel_.ColorSpecular != null)
				pa_light_specular.BackColor = (Color)ElectronPanel_.ColorSpecular;
			else
				pa_light_specular.BackColor = _panel.Light.Color.SpecularColor;
		}

		internal void PrintAmbientColor()
		{
			if (ElectronPanel_.ColorAmbient != null)
				pa_light_ambient.BackColor = (Color)ElectronPanel_.ColorAmbient;
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
	/// The direction of the controlpanel popout.
	/// </summary>
	enum CpDir
	{
		n,e,s,w
	}
}
