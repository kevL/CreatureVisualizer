using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.DirectX;

using NWN2Toolset;
using NWN2Toolset.NWN2.Data.Blueprints;
using NWN2Toolset.NWN2.Data.Campaign;
using NWN2Toolset.NWN2.Data.Instances;
using NWN2Toolset.NWN2.Data.Templates;
//using NWN2Toolset.NWN2.Data.TypedCollections;
//using NWN2Toolset.NWN2.NetDisplay;
//using NWN2Toolset.NWN2.UI;
using NWN2Toolset.NWN2.Views;

using OEIShared.IO;
//using OEIShared.IO.MDB;
using OEIShared.NetDisplay;
using OEIShared.OEIMath;
using OEIShared.UI.Input;
using OEIShared.Utils;


namespace creaturevisualizer
{
	sealed partial class CreVisF
		: Form
	{
/*		/// <summary>
		/// If a change to the current blueprint is Displayed in the visualizer
		/// it gets two asterisks. If that change is also Applied to the toolset
		/// it gets one asterisk.
		/// </summary>
		internal enum ChangedType
		{
			ct_nul,	// 0
			ct_non,	// 1 - Blueprint in visualizer is the same as the original that was loaded. (no asterisks)
			ct_Ts,	// 2 - Blueprint in visualizer is different than the one in the toolset but the same as the original. (single asterisk)
			ct_Vi	// 3 - Blueprint in visualizer is different than the one in the toolset and the original. (double asterisks)
		} */


		#region Fields (static)
		const string TITLE = "Creature Visualizer";

		const int BDI = 22; // minipanel Button DImensions x/y
		#endregion Fields (static)


		#region Fields
		/// <summary>
		/// The ElectronPanel panel.
		/// </summary>
		ElectronPanel_ _panel;

		MenuItem _itRefreshProtocol_non;
		MenuItem _itRefreshProtocol_foc;
		MenuItem _itRefreshProtocol_oac;

		MenuItem _itSaveToModule;
		MenuItem _itSaveToCampaign;
		MenuItem _itSaveToFile;

		MenuItem _itStayOnTop;

		MenuItem _itProcessItemsBody;
		MenuItem _itProcessItemsHeld;
		MenuItem _itProcessInventory;

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


		#region Properties
/*		ChangedType _changed;
		internal ChangedType Changed
		{
			get { return _changed; }
			set
			{
				if ((_changed = value) == ChangedType.ct_nul)
				{
					Text = TITLE;
				}
				else
				{
					string asterisks = String.Empty;
					switch (_changed)
					{
						case ChangedType.ct_non:                   goto case ChangedType.ct_nul;
						case ChangedType.ct_Ts: asterisks = " *";  goto case ChangedType.ct_nul;
						case ChangedType.ct_Vi: asterisks = " **"; goto case ChangedType.ct_nul;

						case ChangedType.ct_nul: // not nul, is just a label
							if (_panel.Blueprint != null) // is a blueprint NOT a placed instance
							{
								// _panel.Blueprint.TemplateResRef.Value	-> parent resref
								// _panel.Blueprint.ResourceName.Value		-> resref == '_panel.Blueprint.Resource.ResRef.Value'
								// _panel.Blueprint.Name					-> tag

								string resref, loc;

//								if (_panel.Blueprint.Resource != null && _panel.Blueprint.Resource.ResRef != null)
//								{
								resref = _panel.Blueprint.Resource.ResRef.Value;

								if ((_panel.Blueprint.Resource.Repository as DirectoryResourceRepository) != null)
								{
									loc = Enum.GetName(typeof(NWN2BlueprintLocationType), _panel.Blueprint.BlueprintLocation);
								}
								else
									loc = "data/zip";
//								}
//								else
//								{
//									resref = "invalid";
//									loc    = "stock";
//								}

								Text = TITLE + " - "
									 + resref + " [" + loc + "]"
									 + asterisks + " (" + _panel.Blueprint.Name + ")";
							}
							else if (_panel.Instance != null) // is a placed instance NOT a blueprint
							{
								Text = TITLE + " - [area]" + asterisks + " (" + _panel.Instance.Name + ")"; // tag
							}
							break;
					}
				}
			}
		} */


		int _refreshprotocol;
		int RefreshProtocol
		{
			get { return _refreshprotocol; }
			set
			{
				CreatureVisualizerPreferences.that.RefreshProtocol = (_refreshprotocol = value);

				_itRefreshProtocol_non.Checked = _refreshprotocol == (int)RefreshType.non;
				_itRefreshProtocol_foc.Checked = (_refreshprotocol & (int)RefreshType.foc) != 0;
				_itRefreshProtocol_oac.Checked = (_refreshprotocol & (int)RefreshType.oac) != 0;
			}
		}

		#endregion Properties

		internal void SetTitleText()
		{
			if (_panel.Blueprint != null) // is a blueprint NOT a placed instance
			{
				// _panel.Blueprint.TemplateResRef.Value	-> parent resref
				// _panel.Blueprint.ResourceName.Value		-> resref == '_panel.Blueprint.Resource.ResRef.Value'
				// _panel.Blueprint.Name					-> tag

				string loc;
				if ((_panel.Blueprint.Resource.Repository as DirectoryResourceRepository) != null)
				{
					loc = Enum.GetName(typeof(NWN2BlueprintLocationType), _panel.Blueprint.BlueprintLocation);
				}
				else
					loc = "data/zip";

				Text = TITLE + " - "
					 + _panel.Blueprint.Resource.ResRef.Value + " [" + loc + "] (" + _panel.Blueprint.Name + ")";
			}
			else if (_panel.Instance != null) // is a placed instance NOT a blueprint
			{
				Text = TITLE + " - [area] (" + _panel.Instance.Name + ")"; // tag
			}
			else
				Text = TITLE;
		}


		#region cTor
		/// <summary>
		/// cTor. Creates and adds menuitems and adds the NetDisplay panel. Also
		/// attempts to create and render a blueprint instance.
		/// </summary>
		internal CreVisF()
		{
//			string info = String.Empty;
//			info += StringDecryptor.Decrypt("ᓖᓈᓌᒻᓎᓙᓘᓜᓒᓝᓘᓛᓢᒷᓊᓖᓎ") + "\n";
//			System.IO.File.WriteAllText(@"C:\GIT\CreatureVisualizer\t\decrypt.txt", info);

			Owner = NWN2ToolsetMainForm.App;

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

			RefreshProtocol = CreatureVisualizerPreferences.that.RefreshProtocol;

			_dir = (CpDir)CreatureVisualizerPreferences.that.ControlPanelDirection;

			if (CreatureVisualizerPreferences.that.ShowControlPanel)
				_itControlPanel.PerformClick();

			if (!CreatureVisualizerPreferences.that.ShowMiniPanel)
				_itMiniPanel.PerformClick();

			if (!CreatureVisualizerPreferences.that.ProcessEquipped_body)
				_itProcessItemsBody.PerformClick();

			if (!CreatureVisualizerPreferences.that.ProcessEquipped_held)
				_itProcessItemsHeld.PerformClick();

			if (!CreatureVisualizerPreferences.that.ProcessInventory)
				_itProcessInventory.PerformClick();

			tc1.SelectedIndex = CreatureVisualizerPreferences.that.TabPageCurrent;


			// handle toolset events ->
			NWN2ToolsetMainForm.App.BlueprintView.SelectionChanged += OnBlueprintSelectionChanged;
			NWN2CampaignManager.Instance.ActiveCampaignChanged     += OnActiveCampaignChanged;

			NWN2CreatureTemplate.AppearanceChanged = (AppearanceChangedHandler)Delegate.Combine(NWN2CreatureTemplate.AppearanceChanged,
																								new AppearanceChangedHandler(OnAppearanceChanged));

//			NWN2NetDisplayManager.Instance.Objects.Inserted = // this is the cleverest thing I've ever seen ... he should be lynched.
//				(OEICollectionWithEvents.ChangeHandler)Delegate.Combine(NWN2NetDisplayManager.Instance.Objects.Inserted,
//																		new OEICollectionWithEvents.ChangeHandler(OnObjectsInserted));


			ActiveControl = _panel;

			_panel.CreateModel();
		}


//		/// <summary>
//		/// Prevents the infinite loop that would occur as the object is added
//		/// to the NetDisplayObjectCollection by the visualizer.
//		/// </summary>
//		internal bool _bypassInsert;

/*		void OnObjectsInserted(OEICollectionWithEvents cList, int index, object value)
		{
			// NOTE: The object could get inserted to 1+ collections causing
			// this to fire for every one. In practice I've seen 1..3 repeats.
			// Ironically this does *not* even fire when drag selecting objects.
			//
			// It seems that if, for example, a creature has equipment, this
			// fires for each item - as if another NetDisplayObject is being
			// created for each item and added to the Collection, one after
			// another. But it further seems that only certain equipment-slots
			// actually trigger this behavior - for example, a sword or a shield
			// will trigger this extra-object-added, but armor doesn't.
			//
			// so, you know by now, go figure. Not much I can do about it short
			// of rewriting and recompiling and redistributing their .DLLs
			//
			// - which isn't going to happen.

			// value= OEIShared.NetDisplay.NetDisplayModel
			if (!_bypassInsert)
//				&& (cList is NetDisplayObjectCollection)
//				&& (value is NetDisplayModel))
			{
				var areaviewer = NWN2ToolsetMainForm.App.GetActiveViewer() as NWN2AreaViewer;
				if (areaviewer != null)
				{
					NWN2InstanceCollection collection = areaviewer.SelectedInstances;
					if (collection != null && collection.Count == 1
						&& (   collection[0] is NWN2CreatureTemplate
							|| collection[0] is NWN2DoorTemplate
							|| collection[0] is NWN2PlaceableTemplate))
					{
						//MessageBox.Show(cList + "\nindex= " + index + " \nvalue= " + value);
						_panel.CreateModel();
					}
				}
			}
		} */

		/// <summary>
		/// This is not SelectionChanged; it's a straightforward click-event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnBlueprintSelectionChanged(object sender, BlueprintSelectionChangedEventArgs e)
		{
			_panel.CreateModel();
/*			if (!IsAreaInstanceSelected()) // doesn't appear to help at all - could even be worse
			{
				NWN2BlueprintView blueprintview = NWN2ToolsetMainForm.App.BlueprintView;
				object[] selection = blueprintview.Selection;
				if (selection != null && selection.Length == 1
					&& (selection[0] as INWN2Template) != null
					&& (   _panel.Blueprint_base == null
						|| _panel.Blueprint_base != selection[0] as INWN2Template))
				{
					switch ((e.Selection[0] as INWN2Template).ObjectType)
					{
						case NWN2ObjectType.Creature:
						case NWN2ObjectType.Door:
						case NWN2ObjectType.Placeable:
						case NWN2ObjectType.PlacedEffect:
						case NWN2ObjectType.Item:
							_panel.CreateModel();
							break;
					}
				}
			} */
		}

/*		bool IsAreaInstanceSelected()
		{
			var areaviewer = NWN2ToolsetMainForm.App.GetActiveViewer() as NWN2AreaViewer;
			if (areaviewer != null)
			{
				NWN2InstanceCollection collection = areaviewer.SelectedInstances;
				if (collection != null && collection.Count == 1
					&& (   collection[0] is NWN2CreatureTemplate
						|| collection[0] is NWN2DoorTemplate
						|| collection[0] is NWN2PlaceableTemplate))
				{
					return true;
				}
			}
			return false;
		} */

		void OnActiveCampaignChanged(NWN2Campaign cOldCampaign, NWN2Campaign cNewCampaign)
		{
			_itSaveToCampaign.Enabled = NWN2CampaignManager.Instance.ActiveCampaign != null
									 && _panel.Instance != null;
		}

		/// <summary>
		/// Prevents the infinite loop that would occur as the object is
		/// recreated in the visualizer.
		/// </summary>
		internal bool _bypassAppearanceChanged;

		// NWN2Toolset.NWN2.UI.PropertyTabs.NWN2ArmorSetAppearanceTab.HandleAppearanceChange()
		void OnAppearanceChanged(INWN2Template cTemplate, AppearanceChangeType eType)
		{
			if (!_bypassAppearanceChanged
			    && (RefreshProtocol & (int)RefreshType.oac) != 0
				&& _panel.Model != null
				&& WindowState != FormWindowState.Minimized)
			{
				_panel.CreateModel();
			}
		}

		internal void EnableSaveToModule(bool valid)
		{
			_itSaveToModule.Enabled = valid;
		}

		internal void EnableSaveToCampaign(bool valid)
		{
			_itSaveToCampaign.Enabled = valid
									 && NWN2CampaignManager.Instance.ActiveCampaign != null;
		}

		internal void EnableSaveToFile(bool valid)
		{
			_itSaveToFile.Enabled = valid;
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

			Menu.MenuItems.Add("&Instance");	// 0
			Menu.MenuItems.Add("&Options");		// 1
			Menu.MenuItems.Add("&View");		// 2
			Menu.MenuItems.Add("&Help");		// 3

// Instance ->
			Menu.MenuItems[0].MenuItems.Add("&refresh", instanceclick_Refresh);
			Menu.MenuItems[0].MenuItems[0].Shortcut = Shortcut.F5;

			Menu.MenuItems[0].MenuItems.Add("-");

			MenuItem it = Menu.MenuItems[0].MenuItems.Add("re&fresh protocol");
			_itRefreshProtocol_non = it.MenuItems.Add("&user-invoked",          instanceclick_RefreshProtocol);
			it.MenuItems.Add("-");
			_itRefreshProtocol_foc = it.MenuItems.Add("on f&ocus",              instanceclick_RefreshProtocol);
			_itRefreshProtocol_oac = it.MenuItems.Add("on appeara&nce changed", instanceclick_RefreshProtocol);
			_itRefreshProtocol_non.Checked = true;

			Menu.MenuItems[0].MenuItems.Add("-");

			_itSaveToModule = Menu.MenuItems[0].MenuItems.Add("save to &Module ...", instanceclick_SaveToModule);
			_itSaveToModule.Shortcut = Shortcut.CtrlM;
			_itSaveToModule.Enabled = _panel.Instance != null;

			_itSaveToCampaign = Menu.MenuItems[0].MenuItems.Add("save to Campai&gn ...", instanceclick_SaveToCampaign);
			_itSaveToCampaign.Shortcut = Shortcut.CtrlG;
			_itSaveToCampaign.Enabled = NWN2CampaignManager.Instance.ActiveCampaign != null
									 && _panel.Instance != null;

			_itSaveToFile = Menu.MenuItems[0].MenuItems.Add("sav&e to file ...", instanceclick_SaveToFile); // ie. to Override or whereva ya like.
			_itSaveToFile.Shortcut = Shortcut.CtrlE;
			_itSaveToFile.Enabled = _panel.Instance != null;

// Options ->
			_itProcessItemsBody = Menu.MenuItems[1].MenuItems.Add("process equipped &body-items", optionsclick_ProcessItemsBody);
//			_itProcessItemsBody.Shortcut = Shortcut.CtrlB;
			_itProcessItemsBody.Checked = true;

			_itProcessItemsHeld = Menu.MenuItems[1].MenuItems.Add("process equipped &held-items", optionsclick_ProcessItemsHeld);
//			_itProcessItemsHeld.Shortcut = Shortcut.CtrlH;
			_itProcessItemsHeld.Checked = true;

			_itProcessInventory = Menu.MenuItems[1].MenuItems.Add("process &inventory", optionsclick_ProcessInventory);
//			_itProcessInventory.Shortcut = Shortcut.CtrlI;
			_itProcessInventory.Checked = true;

// View ->
			_itControlPanel = Menu.MenuItems[2].MenuItems.Add("control &panel", viewclick_ControlPanel);
			_itControlPanel.Shortcut = Shortcut.CtrlP;

			_itMiniPanel = Menu.MenuItems[2].MenuItems.Add("&mini panel", viewclick_MiniPanel);
			_itMiniPanel.Shortcut = Shortcut.F7;
			_itMiniPanel.Checked = true;

			Menu.MenuItems[2].MenuItems.Add("-");

			_itCyclePanel = Menu.MenuItems[2].MenuItems.Add("&cycle panel", viewclick_CyclePanel);
			_itCyclePanel.Shortcut = Shortcut.F8;
			_itCyclePanel.Enabled = false;

			Menu.MenuItems[2].MenuItems.Add("-");

			_itStayOnTop = Menu.MenuItems[2].MenuItems.Add("stay on &top", viewclick_StayOnTop);
			_itStayOnTop.Shortcut = Shortcut.CtrlT;
			_itStayOnTop.Checked = true;

// Help ->
			Menu.MenuItems[3].MenuItems.Add("&help", helpclick_Help);
			Menu.MenuItems[3].MenuItems[0].Shortcut = Shortcut.F1;

			Menu.MenuItems[3].MenuItems.Add("&about", helpclick_About);
			Menu.MenuItems[3].MenuItems[1].Shortcut = Shortcut.F2;
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
			if ((RefreshProtocol & (int)RefreshType.foc) != 0
				&& WindowState != FormWindowState.Minimized)
			{
				_panel.CreateModel();
			}
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
			switch (e.CloseReason)
			{
				case CloseReason.None:
				case CloseReason.TaskManagerClosing:
				case CloseReason.WindowsShutDown:
					// let windows or the toolset do its thing ...
					break;

				case CloseReason.ApplicationExitCall:
				case CloseReason.UserClosing:
//				case CloseReason.MdiFormClosing:
//					if (!ConfirmClose(true))
//					{
//						e.Cancel = true;
//						return;
//					}
//					goto case CloseReason.FormOwnerClosing;

				case CloseReason.FormOwnerClosing:
					CreatureVisualizerPreferences.that.x = DesktopLocation.X;
					CreatureVisualizerPreferences.that.y = DesktopLocation.Y;

					// store Width and Height as if the controlpanel is closed ->
					CreatureVisualizerPreferences.that.w = pa_gui.Width;
					CreatureVisualizerPreferences.that.h = pa_gui.Height;

					if (_t1 != null)
					{
						_t1.Dispose(); _t1 = null;
					}
					break;
			}

			NWN2ToolsetMainForm.App.BlueprintView.SelectionChanged -= OnBlueprintSelectionChanged;
			NWN2CampaignManager.Instance.ActiveCampaignChanged     -= OnActiveCampaignChanged;

			NWN2CreatureTemplate.AppearanceChanged = (AppearanceChangedHandler)Delegate.Remove(NWN2CreatureTemplate.AppearanceChanged,
																							   new AppearanceChangedHandler(OnAppearanceChanged));

//			NWN2NetDisplayManager.Instance.Objects.Inserted =
//				(OEICollectionWithEvents.ChangeHandler)Delegate.Remove(NWN2NetDisplayManager.Instance.Objects.Inserted,
//																	   new OEICollectionWithEvents.ChangeHandler(OnObjectsInserted));
		}

/*		internal bool ConfirmClose(bool cancancel)
		{
			bool ret = false;

			switch (Changed)
			{
				case ChangedType.ct_nul:	// no creature loaded
				case ChangedType.ct_non:	// no changes
				case ChangedType.ct_Ts:		// blueprint/instance has changed (needs to be saved by the toolset)
					ret = true;
					break;

				case ChangedType.ct_Vi:		// blueprint/instance is different than visualizer creature
				{
					BypassCreate = true;	// do not refresh creature on return to the visualizer (if RefreshOnFocus happens to be active)

					CloseF.InstanceType type; bool hasresdir;
					if (_panel.Blueprint != null)
					{
						type = CloseF.InstanceType.blueprint;
						hasresdir =  _panel.Blueprint != null
								 && (_panel.Blueprint.Resource.Repository as DirectoryResourceRepository) != null;
					}
					else
					{
						type = CloseF.InstanceType.instance;
						hasresdir = false;
					}

					using (var f = new CloseF(type, cancancel, hasresdir))
					{
						switch (f.ShowDialog(this))
						{
							case DialogResult.Cancel:	// close dialog but don't do anything else
								ret = false;
								break;

							case DialogResult.Ignore:	// lose changes and proceed
								ret = true;
								break;

							case DialogResult.OK:		// apply changes to blueprint/instance and proceed
//								click_bu_creature_apply(null, EventArgs.Empty);
								ret = true;
								break;

							case DialogResult.Yes:		// save changes to a utc-file and proceed
								instanceclick_SaveTo(null, EventArgs.Empty);
								ret = true;
								break;
						}
					}
					BypassCreate = false;
					break;
				}
			}
			return ret;
		} */


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

		void instanceclick_RefreshProtocol(object sender, EventArgs e)
		{
			var it = sender as MenuItem;
			if (it == _itRefreshProtocol_oac)
			{
				if (!_itRefreshProtocol_oac.Checked)
				{
					RefreshProtocol |= (int)RefreshType.oac;
				}
				else
				{
					RefreshProtocol &= ~(int)RefreshType.oac;
				}
			}
			else if (it == _itRefreshProtocol_foc)
			{
				if (!_itRefreshProtocol_foc.Checked)
				{
					RefreshProtocol |= (int)RefreshType.foc;
				}
				else
				{
					RefreshProtocol &= ~(int)RefreshType.foc;
				}
			}
			else if (!_itRefreshProtocol_non.Checked)
			{
				RefreshProtocol = (int)RefreshType.non;
			}
		}

		void instanceclick_SaveToModule(object sender, EventArgs e)
		{
			if (_panel.Blueprint != null)
			{
				Io.SaveBlueprintToModule(_panel.Blueprint);
				// TODO: update blueprint tree
			}
			else if (_panel.Instance != null)
			{
				Io.SaveInstanceToModule(_panel.Instance);
				// TODO: update blueprint tree
			}
		}

		void instanceclick_SaveToCampaign(object sender, EventArgs e)
		{
			if (NWN2CampaignManager.Instance.ActiveCampaign != null)
			{
				if (_panel.Blueprint != null)
				{
					Io.SaveBlueprintToCampaign(_panel.Blueprint);
					// TODO: update blueprint tree
				}
				else if (_panel.Instance != null)
				{
					Io.SaveInstanceToCampaign(_panel.Instance);
					// TODO: update blueprint tree
				}
			}
		}

		void instanceclick_SaveToFile(object sender, EventArgs e)
		{
//			NWN2Toolset.NWN2.IO.NWN2ResourceManager.Instance.UserOverrideDirectory;
//			NWN2Toolset.NWN2.IO.NWN2ResourceManager.Instance.OverrideDirectory;
//			NWN2Toolset.NWN2.IO.NWN2ResourceManager.Instance.BaseDirectory;

			if (_panel.Blueprint != null)
			{
				Io.SaveBlueprintToFile(_panel.Blueprint);
//				Changed = ChangedType.ct_non;
				// TODO: update blueprint tree (if applicable)
			}
			else if (_panel.Instance != null)
			{
				Io.SaveInstanceToFile(_panel.Instance, NWN2BlueprintLocationType.Global);
//				Changed = ChangedType.ct_non;
				// TODO: update blueprint tree (if applicable)
			}

			// TODO: add resource to the toolset's Blueprint tree (if applicable)
		}


		void optionsclick_ProcessItemsBody(object sender, EventArgs e)
		{
			CreatureVisualizerPreferences.that.ProcessEquipped_body =
			(_itProcessItemsBody.Checked = !_itProcessItemsBody.Checked);

			_panel.CreateModel();
		}

		void optionsclick_ProcessItemsHeld(object sender, EventArgs e)
		{
			CreatureVisualizerPreferences.that.ProcessEquipped_held =
			(_itProcessItemsHeld.Checked = !_itProcessItemsHeld.Checked);

			_panel.CreateModel();
		}

		void optionsclick_ProcessInventory(object sender, EventArgs e)
		{
			CreatureVisualizerPreferences.that.ProcessInventory =
			(_itProcessInventory.Checked = !_itProcessInventory.Checked);

			_panel.CreateModel();
		}


		bool _toggle;

		void viewclick_ControlPanel(object sender, EventArgs e)
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

						CreatureVisualizerPreferences.that.ShowControlPanel = true;
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

						CreatureVisualizerPreferences.that.ShowControlPanel = false;
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

		void viewclick_MiniPanel(object sender, EventArgs e)
		{
			_i.Visible = _o.Visible = _u.Visible =
			_d.Visible = _l.Visible = _r.Visible =
			CreatureVisualizerPreferences.that.ShowMiniPanel = (_itMiniPanel.Checked = !_itMiniPanel.Checked);
		}

		void viewclick_CyclePanel(object sender, EventArgs e)
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

		void viewclick_StayOnTop(object sender, EventArgs e)
		{
			if (CreatureVisualizerPreferences.that.StayOnTop = (_itStayOnTop.Checked = !_itStayOnTop.Checked))
			{
				Owner = NWN2ToolsetMainForm.App;
			}
			else
				Owner = null;
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
			if (_t1 != null)
			{
				_t1.Stop();
				_repeater = null;
			}
		}

		void tick(object sender, EventArgs e)
		{
			if (_repeater != null && _repeater.Focused)
			{
				_repeater.PerformClick();

				if (_firstrepeat)
				{
					_firstrepeat = false;
					_t1.Interval = 89;
				}
			}
			else
				mouseup_DisableRepeater(null,null);
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
				_panel.CameraPosition += ElectronPanel_.CAM_BASEHEIGHT + Offset;
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


		#region Handlers (creature)
		internal void ClearResourceInfo()
		{
			la_itype            .Text =
			la_object           .Text =
			la_tag              .Text =

			la_resref           .Text =
			la_template         .Text =
			la_repotype         .Text =

			la_resource_file    .Text =
			la_resource_resref  .Text =
			la_resource_type    .Text =
			la_resource_repo    .Text =

			la_resource_file_t  .Text =
			la_resource_resref_t.Text =
			la_resource_type_t  .Text =
			la_resource_repo_t  .Text =

			la_areatag          .Text = String.Empty;

			la_head_resource_t  .Text = "TEMPLATE RESOURCE";

			toolTip1.Active = false;
			toolTip1.SetToolTip(la_resource_repo,   String.Empty);
			toolTip1.SetToolTip(la_resource_repo_t, String.Empty);
		}

		internal void PrintResourceInfo(INWN2Template itemplate)
		{
			// TEMPLATE
			// Name					string
			// ObjectType			NWN2ObjectType
			la_object.Text = Enum.GetName(typeof(NWN2ObjectType), itemplate.ObjectType);
			la_tag   .Text = itemplate.Name;


			// BLUEPRINT (inherits TEMPLATE)
			// BlueprintLocation	NWN2BlueprintLocationType
			// Comment				string
			// TemplateResRef		OEIResRef
			// Resource				IResourceEntry
			// ResourceName			OEIResRef
			if ((itemplate as INWN2Blueprint) != null)
			{
				la_itype.Text = "INWN2Blueprint";

				var iblueprint = itemplate as INWN2Blueprint;

				if (   iblueprint.Resource != null
					&& iblueprint.Resource.ResRef != null
					&& !String.IsNullOrEmpty(iblueprint.Resource.ResRef.Value))
				{
					la_resref.Text = iblueprint.ResourceName.Value; // 'ResourceName' IS 'Resource.Resref'
				}
				else
					la_resref.Text = "invalid";

				if (iblueprint.TemplateResRef != null
					&& !String.IsNullOrEmpty(iblueprint.TemplateResRef.Value))
				{
					// TODO: search repositories for TemplateResRef and print "(not found)" if not found
					la_template.Text = iblueprint.TemplateResRef.Value;
				}
				else
					la_template.Text = "invalid";

				if (    iblueprint.Resource != null
					&& (iblueprint.Resource.Repository as DirectoryResourceRepository) != null)
				{
					la_repotype.Text = Enum.GetName(typeof(NWN2BlueprintLocationType), iblueprint.BlueprintLocation);
				}
				else
					la_repotype.Text = "stock resource";

				la_areatag.Text = "-";

				if (iblueprint.Resource != null)
				{
					la_resource_file  .Text = iblueprint.Resource.FullName;
					la_resource_resref.Text = iblueprint.Resource.ResRef.Value;										// <- redundant
					la_resource_type  .Text = BWResourceTypes.GetFileExtension(iblueprint.Resource.ResourceType);	// <- redundant

					if (iblueprint.Resource.Repository != null && !String.IsNullOrEmpty(iblueprint.Resource.Repository.Name))
					{
						toolTip1.Active = true;
						toolTip1.SetToolTip(la_resource_repo, iblueprint.Resource.Repository.Name);
						la_resource_repo.Text = SplitRepoText(iblueprint.Resource.Repository.Name);
					}
					else
						la_resource_repo.Text = "invalid";
				}

				INWN2Blueprint blueprint_tpl;
				OEIResRef base_resref = iblueprint.TemplateResRef;
				if (base_resref != null
					&& (blueprint_tpl = NWN2GlobalBlueprintManager.FindBlueprint(NWN2ObjectType.Creature, base_resref)) != null
					&& blueprint_tpl.Resource != null)
				{
					la_resource_file_t  .Text = blueprint_tpl.Resource.FullName;
					la_resource_resref_t.Text = blueprint_tpl.Resource.ResRef.Value;									// <- redundant
					la_resource_type_t  .Text = BWResourceTypes.GetFileExtension(blueprint_tpl.Resource.ResourceType);	// <- redundant

					if (blueprint_tpl.Resource.Repository != null && !String.IsNullOrEmpty(blueprint_tpl.Resource.Repository.Name))
					{
						toolTip1.Active = true;
						toolTip1.SetToolTip(la_resource_repo_t, blueprint_tpl.Resource.Repository.Name);
						la_resource_repo_t.Text = SplitRepoText(blueprint_tpl.Resource.Repository.Name);

						if ((blueprint_tpl.Resource.Repository as DirectoryResourceRepository) != null)
						{
							la_head_resource_t.Text += " (" + Enum.GetName(typeof(NWN2BlueprintLocationType), blueprint_tpl.BlueprintLocation) + ")";
						}
						else
							la_head_resource_t.Text += " (stock)";
					}
					else
						la_resource_repo_t.Text = "invalid";
				}
				else
				{
					la_resource_file_t  .Text =
					la_resource_resref_t.Text =
					la_resource_type_t  .Text =
					la_resource_repo_t  .Text = "invalid";
				}
			}
			// INSTANCE (inherits TEMPLATE)
			// Template				IResourceEntry
			// Comment				string
			// DebugStruct			GFFStruct
			// Area					NWN2GameArea
			// ObjectID				Guid
			else if ((itemplate as INWN2Instance) != null)
			{
				la_itype.Text = "INWN2Instance";

				var iinstance = itemplate as INWN2Instance;

				la_resref  .Text =
				la_template.Text =
				la_repotype.Text = "-";

				if (iinstance.Area != null)
					la_areatag.Text = iinstance.Area.Tag;
				else
					la_areatag.Text = "invalid";

				la_resource_file  .Text =
				la_resource_resref.Text =
				la_resource_type  .Text =
				la_resource_repo  .Text = "-";

				if (iinstance.Template != null) // ie. template-resource (IResourceEntry)
				{
					la_resource_file_t  .Text = iinstance.Template.FullName;
					la_resource_resref_t.Text = iinstance.Template.ResRef.Value;									// <- redundant
					la_resource_type_t  .Text = BWResourceTypes.GetFileExtension(iinstance.Template.ResourceType);	// <- redundant

					if (iinstance.Template.Repository != null)
					{
						toolTip1.Active = true;
						toolTip1.SetToolTip(la_resource_repo_t, iinstance.Template.Repository.Name);
						la_resource_repo_t.Text = SplitRepoText(iinstance.Template.Repository.Name);

						if ((iinstance.Template.Repository as DirectoryResourceRepository) != null)
						{
							var iblueprint = NWN2GlobalBlueprintManager.FindBlueprint(NWN2ObjectType.Creature, iinstance.Template.ResRef);
							if (iblueprint != null)
								la_head_resource_t.Text += " (" + Enum.GetName(typeof(NWN2BlueprintLocationType), iblueprint.BlueprintLocation) + ")";
						}
						else
							la_head_resource_t.Text += " (stock)";
					}
					else
						la_resource_repo_t.Text = "invalid";
				}
				else
				{
					la_resource_file_t  .Text =
					la_resource_resref_t.Text =
					la_resource_type_t  .Text =
					la_resource_repo_t  .Text = "invalid";
				}
			}
		}

		string SplitRepoText(string text)
		{
			var lines = new List<string>();

			while (text.Length != 0)
			{
				int length = Math.Min(text.Length, 32);
				lines.Add(text.Substring(0, length));
				text = text.Substring(length, text.Length - length);
			}

			foreach (var line in lines)
			{
				if (!String.IsNullOrEmpty(text)) text += Environment.NewLine;
				text += line;
			}
			return text;
		}

/*		void click_bu_creature_display(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				if (_panel.Blueprint != null)
				{
					(_panel.Blueprint as NWN2CreatureTemplate).Gender = (CreatureGender)cbo_creature_gendertype.SelectedIndex;


					_panel.UpdateModel();

					Changed = ChangedType.ct_Vi;
				}
				else if (_panel.Instance != null)
				{
					(_panel.Instance as NWN2CreatureTemplate).Gender = (CreatureGender)cbo_creature_gendertype.SelectedIndex;


					_panel.UpdateModel();

					Changed = ChangedType.ct_Vi;
				}
			}
		}

		void click_bu_creature_apply(object sender, EventArgs e)
		{
			if (_panel.Model != null)
			{
				if (_panel.Blueprint != null)
				{
					if (Changed != ChangedType.ct_Vi)
						click_bu_creature_display(null, EventArgs.Empty);

					(_panel.Blueprint_base as NWN2CreatureTemplate).Gender = (CreatureGender)cbo_creature_gendertype.SelectedIndex;


					Changed = ChangedType.ct_Ts;
				}
				else if (_panel.Instance != null)
				{
					if (Changed != ChangedType.ct_Vi)
						click_bu_creature_display(null, EventArgs.Empty);

					(_panel.Instance_base as NWN2CreatureTemplate).Gender = (CreatureGender)cbo_creature_gendertype.SelectedIndex;


					Changed = ChangedType.ct_Ts;
				}

				// TODO: update Property in the toolset panel co-temporaneously.
			}
		} */
		#endregion Handlers (creature)


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
		#endregion Methods


		#region Methods (creature)
//		internal void EnableCreaturePages(bool enabled)
//		{
//			tp_creature1.Enabled =
//			tp_creature2.Enabled = enabled;
//		}

/*		internal void InitializeCreaturePages(NWN2CreatureTemplate template)
		{
// Creature1 page ->

			cbo_creature_gendertype.SelectedIndex = (int)template.Gender;

			cb_creature_facialhair.Checked = template.AppearanceFacialHair;


			int id;

			var iprefixer = template as INWN2ModelPartPrefixer;
			object[] attributes;

			id = -1;
			cbo_creature_headtype.Items.Clear();
			attributes = iprefixer.GetType()
								  .GetProperty("AppearanceHead")
								  .GetCustomAttributes(typeof(PartSelectorAttribute), true);
			if (attributes.Length != 0)
			{
				var attribute = attributes[0] as PartSelectorAttribute;
				if (attribute != null)
				{
					List<int> variations = MDBUtils.GetNumPartVariations(iprefixer.GetModelPartPrefix(attribute.StringToPassToPartPrefixer),
																		 attribute.PartType,
																		 attribute.FileFormatString,
																		 attribute.PacketFormatString,
																		 attribute.SeparateFiles);
					for (int i = 0; i != variations.Count; ++i)
					{
						cbo_creature_headtype.Items.Add(variations[i]);
						if (variations[i] == template.AppearanceHead)
							id = i;
					}
				}
			}

			if (cbo_creature_headtype.Enabled = id != -1)
				cbo_creature_headtype.SelectedIndex = id;


			id = -1;
			cbo_creature_hairtype.Items.Clear();
			attributes = iprefixer.GetType()
								  .GetProperty("AppearanceHair")
								  .GetCustomAttributes(typeof(PartSelectorAttribute), true);
			if (attributes.Length != 0)
			{
				var attribute = attributes[0] as PartSelectorAttribute;
				if (attribute != null)
				{
					List<int> variations = MDBUtils.GetNumPartVariations(iprefixer.GetModelPartPrefix(attribute.StringToPassToPartPrefixer),
																		 attribute.PartType,
																		 attribute.FileFormatString,
																		 attribute.PacketFormatString,
																		 attribute.SeparateFiles);
					for (int i = 0; i != variations.Count; ++i)
					{
						cbo_creature_hairtype.Items.Add(variations[i]);
						if (variations[i] == template.AppearanceHair)
							id = i;
					}
				}
			}

			if (cbo_creature_hairtype.Enabled = id != -1)
				cbo_creature_hairtype.SelectedIndex = id;


// Creature2 page ->

			cb_creature_helm  .Checked = template.HasHelm;
			cb_creature_boots .Checked = template.HasBoots;
			cb_creature_gloves.Checked = template.HasGloves;
			cb_creature_belt  .Checked = template.HasBelt;
			cb_creature_cloak .Checked = template.HasCloak;

			cb_creature_nevershowarmor .Checked = template.NeverShowArmor;
			cb_creature_neverdrawhelmet.Checked = template.NeverDrawHelmet;


			//cbo_creature_maintype
			//cbo_creature_mainvariation
		} */
		#endregion Methods (creature)


		#region Handlers (apparel)
/*		void checkchanged_Apparel(object sender, EventArgs e)
		{
			var cb = sender as CheckBox;
			if (cb == cb_creature_helm)
			{
			}
			else if (cb == cb_creature_boots)
			{
			}
			else if (cb == cb_creature_gloves)
			{
			}
			else if (cb == cb_creature_belt)
			{
			}
			else if (cb == cb_creature_cloak)
			{
			}
		} */
		#endregion Handlers (apparel)
	}



	#region enums (global)
	/// <summary>
	/// Bitwise refresh types.
	/// </summary>
	[Flags]
	enum RefreshType
	{
		non = 0x0,	// no auto refresh (ie. user shall invoke Refresh [F5] to update the Model w/ any latent changes)
		foc = 0x1,	// auto refresh on focus
		oac = 0x2	// auto refresh OnAppearanceChanged
	}

	/// <summary>
	/// The direction of the controlpanel popout.
	/// </summary>
	enum CpDir
	{
		n,e,s,w
	}
	#endregion enums (global)
}
