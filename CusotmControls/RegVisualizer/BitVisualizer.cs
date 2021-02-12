using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace CusotmControls.RegVisualizer
{
    public class BitVisualizer : Control
    {
        public Color OnColor { get; set; }
        public Color OffColor { get; set; }
        public int BitNumber { get; set; } = 0;
        public int BitMask { get => 0x01 << BitNumber; }

        public bool Checked { get; set; }

        public event EventHandler<EventArgs> CheckedChanged;

        private const int _margin = 2; 
        
        private Rectangle DrawRect { get
            {
                return new Rectangle(ClientRectangle.X + _margin, ClientRectangle.Y + _margin, ClientRectangle.Width - 2 * _margin, ClientRectangle.Height - 2 * _margin);
            } 
        }

        private void drawLamp(Graphics g, Color color)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, this.DrawRect);
            }

            ExtensionMethods.ColorToHSV(color, out double h, out double s, out double v);
            v = v * .1; // Take 10% of the v value to have a darker border

            using (Pen pen = new Pen(ExtensionMethods.ColorFromHSV(h, s, v)))
            {
                g.DrawEllipse(pen, this.DrawRect);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (Checked)
                drawLamp(pevent.Graphics, OnColor);
            else
                drawLamp(pevent.Graphics, OffColor);

            SizeF textSize = pevent.Graphics.MeasureString(Text, Font);
            PointF textHomePoint = new PointF((ClientRectangle.Width/2) - (textSize.Width/2),
                (ClientRectangle.Height / 2) - (textSize.Height/ 2)
                );
            using (SolidBrush brush = new SolidBrush(ForeColor))
                pevent.Graphics.DrawString(Text, Font, brush, textHomePoint);
        }

        protected override void OnClick(EventArgs e)
        {
            Checked = !Checked;
            CheckedChanged?.Invoke(this, new EventArgs());
        }
    }
}
