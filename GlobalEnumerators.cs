using System;


namespace creaturevisualizer
{
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

	/// <summary>
	/// Reset types for the Model object.
	/// </summary>
	enum ResetType
	{
		RESET_non,	// 0
		RESET_z,	// 1
		RESET_xy,	// 2
		RESET_rot,	// 3
		RESET_scale	// 4
	}
	#endregion enums (global)
}
