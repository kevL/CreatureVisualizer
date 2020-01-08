using System;
using System.Drawing;
using System.Windows.Forms;


namespace creaturevisualizer
{
	/// <summary>
	/// Used by ToolStrips/StatusStrips to get rid of white borders and draw a
	/// 3d border.
	/// </summary>
	public class StripRenderer
		:
			ToolStripProfessionalRenderer
	{
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			e.Graphics.FillRectangle(Brushes.AliceBlue, e.AffectedBounds);
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			e.Graphics.DrawLine(Pens.Black, 0,0, e.ToolStrip.Width, 0);
		}
	}
}
