﻿using System;
using System.Windows.Forms;


namespace start
{
	/// <summary>
	/// Entry point.
	/// </summary>
	sealed class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new creaturevisualizer.ColorF());
		}
	}
}
