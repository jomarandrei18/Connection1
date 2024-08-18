using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connection1.Class
{
    public class RoundBottomPanel : Panel
    {
        public int CornerRadius { get; set; } = 20;

        protected override void OnPaint(PaintEventArgs e)
        {
            GraphicsPath path = new GraphicsPath();
            int arcWidth = CornerRadius * 2;

            //path.AddLine(0, 0, this.Width, 0);
            path.AddLine(this.Width, 0, this.Width, this.Height - CornerRadius);

            path.AddArc(this.Width - arcWidth, this.Height - arcWidth, arcWidth, arcWidth, 0, 90);
            path.AddArc(0, this.Height - arcWidth, arcWidth, arcWidth, 90, 90);
            path.AddLine(0, this.Height - CornerRadius, 0, 0);

            this.Region = new Region(path);

            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillPath(brush, path);
            }

            using (Pen pen = new Pen(Color.Gray, 2)) // Thin line
            {
                pen.DashStyle = DashStyle.Custom; // Set the dash style to custom
                pen.DashPattern = new float[] { 15, 20 }; // 10 units dash, 20 units gap
                e.Graphics.DrawLine(pen, CornerRadius, Height - 1, Width - CornerRadius, Height - 1);
            }
        }
    }

    public class RoundTopPanel : Panel
    {
        public int CornerRadius { get; set; } = 20;

        protected override void OnPaint(PaintEventArgs e)
        {
            GraphicsPath path = new GraphicsPath();
            int arcWidth = CornerRadius * 2;

            path.AddArc(0, 0, arcWidth, arcWidth, 180, 90);
            path.AddArc(this.Width - arcWidth, 0, arcWidth, arcWidth, 270, 90);
            path.AddLine(this.Width, arcWidth, this.Width, this.Height);
            path.AddLine(this.Width, this.Height, 0, this.Height);
            path.AddLine(0, this.Height, 0, arcWidth);

            this.Region = new Region(path);

            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillPath(brush, path);
            }
        }
    }

    public class RightAlignedLabel : Label
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Far, // Align text to the right
                LineAlignment = StringAlignment.Center // Center text vertically
            };

            e.Graphics.Clear(this.BackColor);
            // Draw the text
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, stringFormat);
        }
    }

    public class CreatePricePanel
    {
        public RoundBottomPanel CreateRoundBotPanel()
        {
            return new RoundBottomPanel
            {
                Size = new Size(425, 112),
                Location = new Point(24, 689),
                BackColor = Color.FromArgb(242, 242, 242)
            };
        }

        public RoundTopPanel CreateRoundTopPanel()
        {
            return new RoundTopPanel
            {
                Size = new Size(425, 77),
                Location = new Point(24, 801),
                BackColor = Color.FromArgb(242, 242, 242)
            };
        }

        public RightAlignedLabel CreatedRightAlignLabel(int pointY, int fontSize, int sizeH)
        {
            return new RightAlignedLabel
            {
                Text = "₱0.00",
                Font = new Font("Verdana", fontSize, FontStyle.Bold),
                ForeColor = Color.FromArgb(47, 46, 46),
                Size = new Size(240, sizeH),
                Location = new Point(150, pointY)
            };
        }

        public Label CreateLabelForBotPricePanel(int pointY, string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Constantia", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(126, 121, 121),
                Size = new Size(150, 18), // Set the size of the label
                Location = new Point(28, pointY)
            };
        }

        public Label CreateLabelForTopPricePanel(int pointY, string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Constantia", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(47, 46, 46),
                Size = new Size(150, 30), // Set the size of the label
                Location = new Point(28, pointY)
            };
        }
    }
}
