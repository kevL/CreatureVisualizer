using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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
			e.Graphics.DrawLine(Pens.Black, e.ToolStrip.Width, 0, 0,0);
			e.Graphics.DrawLine(Pens.Black, 0,0, 0, e.ToolStrip.Height);


//			using (var path3d = new GraphicsPath())
//			{
//				path3d.AddLine(e.ToolStrip.Width, 0, 0,0);
//				path3d.AddLine(0,0, 0, e.ToolStrip.Height);
//
//				e.Graphics.DrawPath(Pens.Gray, path3d);
//			}
		}
	}
}
