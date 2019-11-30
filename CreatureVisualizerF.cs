using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.DirectX;

using OEIShared.OEIMath;


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


//			_itRefreshOnFocus.PerformClick(); // TEST
//			_itControlPanel  .PerformClick(); // TEST
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
		// TODO: [Enter] repeat

		void mousedown_EnableRepeater(object sender, MouseEventArgs e)
		{
			_repeater = sender as Button;
			_firstrepeat = true;

			if (_t1 != null)
			{
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


		#region Handlers (model)
		void bu_zpos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.vec_zpos);
		}

		void bu_zneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.vec_zneg);
		}

		void bu_ypos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.vec_ypos);
		}

		void bu_yneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.vec_yneg);
		}

		void bu_xpos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.vec_xpos);
		}

		void bu_xneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.MoveModel(CreatureVisualizerP.vec_xneg);
		}


		void bu_rotpos(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.RotateModel(CreatureVisualizerP.rotpos);
		}

		void bu_rotneg(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.RotateModel(CreatureVisualizerP.rotneg);
		}


		void bu_scale(object sender, EventArgs e)
		{
			if (_panel.Object != null)
			{
				Vector3 vec;

				var bu = sender as Button;
				if      (bu == bu_model_xscalepos) vec = CreatureVisualizerP.vec_xpos;
				else if (bu == bu_model_xscaleneg) vec = CreatureVisualizerP.vec_xneg;
				else if (bu == bu_model_yscalepos) vec = CreatureVisualizerP.vec_ypos;
				else if (bu == bu_model_yscaleneg) vec = CreatureVisualizerP.vec_yneg;
				else if (bu == bu_model_zscalepos) vec = CreatureVisualizerP.vec_zpos;
				else                               vec = CreatureVisualizerP.vec_zneg; // (bu == bu_model_zscaleneg)

				_panel.ScaleModel(vec);
			}
		}

		void bu_scaleall(object sender, EventArgs e)
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


		void bu_modelreset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.ResetModel();
		}

		void bu_modelzreset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.ResetModel(ResetType.RESET_z);
		}

		void bu_modelxyreset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.ResetModel(ResetType.RESET_xy);
		}

		void bu_modelrotreset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.ResetModel(ResetType.RESET_rot);
		}

		void bu_modelscalereset(object sender, EventArgs e)
		{
			if (_panel.Object != null)
				_panel.ResetModel(ResetType.RESET_scale);
		}
		#endregion Handlers (model)


		#region Methods
		internal bool feline()
		{
			return _itFeline.Checked;
		}

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

		internal void PrintModelPosition(Vector3 vec, RHQuaternion quaternion)
		{
			tssl_xpos.Text = vec.X.ToString("N2");
			tssl_ypos.Text = vec.Y.ToString("N2");
			tssl_zpos.Text = vec.Z.ToString("N2");

			// quaternions ... because why not ->
			var axis = new Vector3();
			float angle = 0F;
			RHQuaternion.ToAxisAngle(quaternion, ref axis, ref angle);

			if (axis.Z < 0F) angle = -angle;
			angle *= 180F / (float)Math.PI;
			if (angle < 0F) angle += 360F;

			tssl_rot.Text = ((int)angle).ToString();
		}
		//set: Orientation = RHQuaternion.RotationAxis(new Vector3(0f, 0f, 1f), (float)value * ((float)Math.PI / 180f));
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
