using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace creaturevisualizer
{
	sealed class CreatureVisualizerF
		: Form
	{
		#region Fields
		CreatureVisualizerP _panel = new CreatureVisualizerP();

		MenuItem _itStayOnTop;
		MenuItem _itRefreshOnFocus;
		#endregion Fields


		#region cTor
		/// <summary>
		/// cTor.
		/// </summary>
		internal CreatureVisualizerF()
		{
			InitializeComponent();

			_panel.Dock = DockStyle.Fill;
			_panel.BorderStyle = BorderStyle.FixedSingle;
			Controls.Add(_panel);

			Menu = new MainMenu();
			Menu.MenuItems.Add("&Instance");
			Menu.MenuItems.Add("&Options");

			Menu.MenuItems[0].MenuItems.Add("&refresh", click_Refresh);
			Menu.MenuItems[0].MenuItems[0].Shortcut = Shortcut.F5;

			Menu.MenuItems[0].MenuItems.Add("-");

			_itRefreshOnFocus = Menu.MenuItems[0].MenuItems.Add("refresh on &focus", click_RefreshOnFocus);
			_itRefreshOnFocus.Shortcut = Shortcut.F6;
			_itRefreshOnFocus.Checked = true;

			_itStayOnTop = Menu.MenuItems[1].MenuItems.Add("stay on &top", click_StayOnTop);
			_itStayOnTop.Shortcut = Shortcut.CtrlT;
			_itStayOnTop.Checked = TopMost = true;

			_panel.CreateInstance();
		}
		#endregion cTor


		#region Handlers
		void click_RefreshOnFocus(object sender, EventArgs e)
		{
			_itRefreshOnFocus.Checked = !_itRefreshOnFocus.Checked;
		}

		void activated_Refresh(object sender, EventArgs e)
		{
			if (_itRefreshOnFocus.Checked)
				_panel.CreateInstance();
		}

		void click_Refresh(object sender, EventArgs e)
		{
			_panel.CreateInstance();
		}


		void click_StayOnTop(object sender, EventArgs e)
		{
			TopMost = (_itStayOnTop.Checked = !_itStayOnTop.Checked);
		}
		#endregion Handlers



		#region Designer
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		IContainer components = null;


		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// CreatureVisualizerF
			// 
			this.ClientSize = new System.Drawing.Size(292, 474);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "CreatureVisualizerF";
			this.Text = "Creature Visualizer";
			this.Activated += new System.EventHandler(this.activated_Refresh);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
