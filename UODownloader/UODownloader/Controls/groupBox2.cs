using System;
using System.Drawing;
using System.Windows.Forms;

public partial class groupBox2 : GroupBox
{
	Color borderColor = Color.White; // a fancy default color
	public groupBox2() 
	{
		// to avoid flickering use double buffer
		SetStyle(ControlStyles.DoubleBuffer, true);
		// to force control to use OnPaint
		SetStyle(ControlStyles.UserPaint, true);
	}

	// assign the border color
	public Color BorderColor
	{
		get
		{
			return borderColor;
		}
		set
		{
			borderColor = value;
			this.Refresh();
		}
	}
	// simple but incomplete OnPaint method, needs to draw the group box label
	// and other stuff as well
	protected override void OnPaint(PaintEventArgs e) 
	{
		Graphics gr = e.Graphics;
		Rectangle r = new Rectangle(0, 7, (Width - 1), (Height - 8));
		using(Brush brush = new SolidBrush(BackColor))
		{
			using(Pen borderPen = new Pen(BorderColor))
			{
				gr.FillRectangle(brush, r);
				gr.DrawRectangle(borderPen, r);
			}
		}

		Rectangle r2 = new Rectangle(6, 5, (int)Math.Ceiling(gr.MeasureString(this.Text, this.Font).Width)-3, Height - 6);
		using (Brush brush = new SolidBrush(BackColor))
		{
			using (Pen borderPen = new Pen(BorderColor))
			{
				gr.FillRectangle(brush, r2);
			}
		}
		gr.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), (float)5.0, (float)-0.5);
	}
}