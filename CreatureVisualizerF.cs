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
		internal static CreatureVisualizerF that;
		#endregion Fields (static)


		#region Fields
		CreatureVisualizerP _panel = new CreatureVisualizerP();

		MenuItem _itStayOnTop;
		MenuItem _itRefreshOnFocus;
		MenuItem _itFeline;
		MenuItem _itControlPanel;

		Timer _t1 = new Timer();
		Button _repeater;
		bool _firstrepeat;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor. Creates and adds menuitems and adds the NetDisplay panel. Also
		/// attempts to create and render a blueprint instance.
		/// </summary>
		internal CreatureVisualizerF()
		{
			InitializeComponent();

			that = this;

			_panel.Dock = DockStyle.Fill;
			_panel.BorderStyle = BorderStyle.FixedSingle;
			Controls.Add(_panel);

			_panel.BringToFront();

			ClientSize = new Size(ClientSize.Width - pa_controls.Width,	// the ControlPanel starts non-visible
								  ClientSize.Height);					// but let it show in the designer

			_t1.Tick += tick;

			CreateMainMenu();

			_panel.CreateInstance();
			_panel.Select();


			_itControlPanel  .PerformClick(); // TEST
			_itRefreshOnFocus.PerformClick(); // TEST
		}


		/// <summary>
		/// Instantiates the Menu.
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

			Menu.MenuItems[0].MenuItems.Add("-");

			_itFeline = Menu.MenuItems[0].MenuItems.Add("fema&le", instanceclick_Female);
			_itFeline.Shortcut = Shortcut.CtrlL;

			// Options ->
			_itControlPanel = Menu.MenuItems[1].MenuItems.Add("control &panel", optionsclick_ControlPanel);
			_itControlPanel.Shortcut = Shortcut.CtrlP;

			Menu.MenuItems[1].MenuItems.Add("-");

			_itStayOnTop = Menu.MenuItems[1].MenuItems.Add("stay on &top", optionsclick_StayOnTop);
			_itStayOnTop.Shortcut = Shortcut.CtrlT;
			_itStayOnTop.Checked = TopMost = true;

			// Help ->
			Menu.MenuItems[2].MenuItems.Add("&about", helpclick_About);
			Menu.MenuItems[2].MenuItems[0].Shortcut = Shortcut.F2;
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_t1.Dispose();
			_t1 = null;
		}
		#endregion Handlers (override)


		#region Handlers
		void activated_Refresh(object sender, EventArgs e)
		{
			if (_itRefreshOnFocus.Checked && WindowState != FormWindowState.Minimized)
				_panel.CreateInstance();
		}

		void instanceclick_RefreshOnFocus(object sender, EventArgs e)
		{
			_itRefreshOnFocus.Checked = !_itRefreshOnFocus.Checked;
		}

		void instanceclick_Refresh(object sender, EventArgs e)
		{
			_panel.CreateInstance();
		}

		void instanceclick_Female(object sender, EventArgs e)
		{
			_itFeline.Checked = !_itFeline.Checked;
			_panel.CreateInstance();
		}


		void optionsclick_ControlPanel(object sender, EventArgs e)
		{
			if (_itControlPanel.Checked = !_itControlPanel.Checked)
			{
				ClientSize = new Size(ClientSize.Width + pa_controls.Width,
									  ClientSize.Height);

				pa_controls.Visible = true;
			}
			else
			{
				ClientSize = new Size(ClientSize.Width - pa_controls.Width,
									  ClientSize.Height);

				pa_controls.Visible = false;
			}
		}

		void optionsclick_StayOnTop(object sender, EventArgs e)
		{
			TopMost = (_itStayOnTop.Checked = !_itStayOnTop.Checked);
		}

		void helpclick_About(object sender, EventArgs e)
		{
			string text = "Creature Visualizer"
						+ Environment.NewLine
						+ "- plugin for Neverwinter Nights 2 toolset"
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
			text += " release";
#endif

			text += Environment.NewLine
				  + String.Format(CultureInfo.CurrentCulture,
								  "{0:yyyy MMM d} {0:HH}:{0:mm}:{0:ss} UTC",
								  ass.GetLinkerTime());

			MessageBox.Show(this, text, "About");
		}
		#endregion Handlers


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
			if (_panel.Object != null)
			{
				_panel.CameraPosition += CreatureVisualizerP.off_zpos;
				Offset += CreatureVisualizerP.off_zpos;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_zneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.CameraPosition += CreatureVisualizerP.off_zneg;
				Offset += CreatureVisualizerP.off_zneg;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_ypos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.CameraPosition += CreatureVisualizerP.off_ypos;
				Offset += CreatureVisualizerP.off_ypos;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_yneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.CameraPosition += CreatureVisualizerP.off_yneg;
				Offset += CreatureVisualizerP.off_yneg;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_xpos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.CameraPosition += CreatureVisualizerP.off_xpos;
				Offset += CreatureVisualizerP.off_xpos;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_xneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.CameraPosition += CreatureVisualizerP.off_xneg;
				Offset += CreatureVisualizerP.off_xneg;
				PrintCameraPosition();
			}
		}


		void click_bu_camera_distpos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				((ModelViewerInputCameraReceiverState)_panel.Receiver.CameraState).Distance += 0.1F;
				_panel.UpdateCamera();
				PrintCameraPosition();
			}
		}

		void click_bu_camera_distneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				((ModelViewerInputCameraReceiverState)_panel.Receiver.CameraState).Distance -= 0.1F;
				_panel.UpdateCamera();
				PrintCameraPosition();
			}
		}


		void click_bu_camera_vertpos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.RaiseCameraPolar();
				PrintCameraPosition();
			}
		}

		void click_bu_camera_vertneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.LowerCameraPolar();
				PrintCameraPosition();
			}
		}

		void click_bu_camera_horipos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.Receiver.CameraAngleXY += (float)Math.PI / 64F;
				_panel.CameraPosition += CreatureVisualizerP.POS_OFF_Zd + Offset;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_horineg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.Receiver.CameraAngleXY -= (float)Math.PI / 64F;
				_panel.CameraPosition += CreatureVisualizerP.POS_OFF_Zd + Offset;
				PrintCameraPosition();
			}
		}


		void click_bu_camera_zreset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.CameraPosition = new Vector3(_panel.CameraPosition.X,
													_panel.CameraPosition.Y,
													CreatureVisualizerP.POS_START_CAMERA.Z);
				Offset.Z = 0F;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_xyreset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.CameraPosition = new Vector3(CreatureVisualizerP.POS_START_CAMERA.X,
													CreatureVisualizerP.POS_START_CAMERA.Y,
													_panel.CameraPosition.Z);
				Offset.X = Offset.Y = 0F;
				PrintCameraPosition();
			}
		}

		void click_bu_camera_resetdist(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				((ModelViewerInputCameraReceiverState)_panel.Receiver.CameraState).Distance = CreatureVisualizerP.DIST_START;
				_panel.UpdateCamera();
				PrintCameraPosition();
			}
		}

		void click_bu_camera_resetpolar(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.Receiver.CameraAngleXY = (float)Math.PI /  2F;
				_panel.Receiver.CameraAngleYZ = (float)Math.PI / 32F;
				_panel.CameraPosition += CreatureVisualizerP.POS_OFF_Zd;
				PrintCameraPosition();
			}
		}


		void click_bu_camera_focusobject(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				((ModelViewerInputCameraReceiverState)_panel.Receiver.CameraState).FocusPoint = _panel.Object.Position;
				Offset = new Vector3();
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
		#endregion Handlers (camera)


		#region Handlers (model)
		void click_bu_model_zpos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.off_zpos);
		}

		void click_bu_model_zneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.off_zneg);
		}

		void click_bu_model_ypos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.off_ypos);
		}

		void click_bu_model_yneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.off_yneg);
		}

		void click_bu_model_xpos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.off_xpos);
		}

		void click_bu_model_xneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.off_xneg);
		}


		void click_bu_model_rotpos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.RotateModel(CreatureVisualizerP.rotpos);
		}

		void click_bu_model_rotneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.RotateModel(CreatureVisualizerP.rotneg);
		}


		void click_bu_model_scale(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				Vector3 unit;

				var bu = sender as Button;
				if      (bu == bu_model_xscalepos) unit = CreatureVisualizerP.off_xpos;
				else if (bu == bu_model_xscaleneg) unit = CreatureVisualizerP.off_xneg;
				else if (bu == bu_model_yscalepos) unit = CreatureVisualizerP.off_ypos;
				else if (bu == bu_model_yscaleneg) unit = CreatureVisualizerP.off_yneg;
				else if (bu == bu_model_zscalepos) unit = CreatureVisualizerP.off_zpos;
				else                               unit = CreatureVisualizerP.off_zneg; // (bu == bu_model_zscaleneg)

				_panel.ScaleModel(unit);
			}
		}

		void click_bu_model_scaleall(object sender, EventArgs e)
		{
			if (_panel.Object != null)
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
			if (_panel.Object != null)
				_panel.ResetModel();
		}

		void click_bu_model_zreset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.ResetModel(ResetType.RESET_z);
		}

		void click_bu_model_xyreset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.ResetModel(ResetType.RESET_xy);
		}

		void click_bu_model_rotreset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.ResetModel(ResetType.RESET_rot);
		}

		void click_bu_model_scalereset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.ResetModel(ResetType.RESET_scale);
		}
		#endregion Handlers (model)


		#region Handlers (light)
		void click_bu_light_zpos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.RecreateLight(_panel.Light.Position + CreatureVisualizerP.off_zpos);
				PrintLightPosition(_panel.Light.Position, _panel.Light.Color.Intensity);
			}
		}

		void click_bu_light_zneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.RecreateLight(_panel.Light.Position + CreatureVisualizerP.off_zneg);
				PrintLightPosition(_panel.Light.Position, _panel.Light.Color.Intensity);
			}
		}

		void click_bu_light_ypos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.RecreateLight(_panel.Light.Position + CreatureVisualizerP.off_ypos);
				PrintLightPosition(_panel.Light.Position, _panel.Light.Color.Intensity);
			}
		}

		void click_bu_light_yneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.RecreateLight(_panel.Light.Position + CreatureVisualizerP.off_yneg);
				PrintLightPosition(_panel.Light.Position, _panel.Light.Color.Intensity);
			}
		}

		void click_bu_light_xpos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.RecreateLight(_panel.Light.Position + CreatureVisualizerP.off_xpos);
				PrintLightPosition(_panel.Light.Position, _panel.Light.Color.Intensity);
			}
		}

		void click_bu_light_xneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.RecreateLight(_panel.Light.Position + CreatureVisualizerP.off_xneg);
				PrintLightPosition(_panel.Light.Position, _panel.Light.Color.Intensity);
			}
		}


		void click_bu_light_zreset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				var pos = new Vector3(_panel.Light.Position.X,
									  _panel.Light.Position.Y,
									  CreatureVisualizerP.POS_START_LIGHT.Z);
				_panel.RecreateLight(pos);
				PrintLightPosition(_panel.Light.Position, _panel.Light.Color.Intensity);
			}
		}

		void click_bu_light_xyreset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				var pos = new Vector3(CreatureVisualizerP.POS_START_LIGHT.X,
									  CreatureVisualizerP.POS_START_LIGHT.Y,
									  _panel.Light.Position.Z);
				_panel.RecreateLight(pos);
				PrintLightPosition(_panel.Light.Position, _panel.Light.Color.Intensity);
			}
		}

		void click_bu_light_reset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				_panel.RecreateLight(CreatureVisualizerP.POS_START_LIGHT);
				PrintLightPosition(_panel.Light.Position, _panel.Light.Color.Intensity);
			}
		}
		#endregion Handlers (light)


		#region Methods
		internal bool feline()
		{
			return _itFeline.Checked;
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
			la_model_xscale.Text = _panel.Object.Scale.X.ToString("N2");
			la_model_yscale.Text = _panel.Object.Scale.Y.ToString("N2");
			la_model_zscale.Text = _panel.Object.Scale.Z.ToString("N2");
		}

		internal void PrintOriginalScale(string scale)
		{
			la_model_scaleorg.Text = scale;
		}


		internal void PrintLightPosition(Vector3 pos, float intensity)
		{
			tssl_light_xpos.Text = pos.X.ToString("N2");
			tssl_light_ypos.Text = pos.Y.ToString("N2");
			tssl_light_zpos.Text = pos.Z.ToString("N2");

			tssl_light_intensity.Text = intensity.ToString("N2");
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
}
