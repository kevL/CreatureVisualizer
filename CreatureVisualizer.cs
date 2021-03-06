﻿using System;

using NWN2Toolset.Plugins;

using TD.SandBar;


namespace creaturevisualizer
{
	/// <summary>
	/// NwN2 Electron toolset plugin.
	/// </summary>
	public class CreatureVisualizer
		: INWN2Plugin
	{
		#region INWN2Plugin (interface)
		public MenuButtonItem PluginMenuItem
		{ get; set; }

		public object Preferences
		{
			get { return CreatureVisualizerPreferences.that; }
			set { CreatureVisualizerPreferences.that = (CreatureVisualizerPreferences)value; }
		}

		/// <summary>
		/// Preferences will be stored in an XML-file w/ this label in
		/// C:\Users\User\AppData\Local\NWN2 Toolset\Plugins
		/// (or similar).
		/// </summary>
		public string Name
		{
			get { return "CreatureVisualizer"; }
		}

		/// <summary>
		/// The label of the operation on the toolset's "Plugins" menu.
		/// </summary>
		public string MenuName
		{
			get { return "Creature Visualizer"; }
		}

		/// <summary>
		/// The caption on the titlebar of the plugin window.
		/// </summary>
		public string DisplayName
		{
			get { return "Creature Visualizer"; }
		}


		public void Load(INWN2PluginHost host)
		{}

		public void Startup(INWN2PluginHost host)
		{
			PluginMenuItem = host.GetMenuForPlugin(this);
			PluginMenuItem.Activate += activate;
		}

		public void Shutdown(INWN2PluginHost host)
		{}

		public void Unload(INWN2PluginHost host)
		{}
		#endregion INWN2Plugin (interface)


		/// <summary>
		/// Handler that launches CreatureVisualizer from the toolset's Plugins.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void activate(object sender, EventArgs e)
		{
			var f = new CreVisF();
			f.Show();
		}
	}
}
