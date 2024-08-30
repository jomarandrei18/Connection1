using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Connection1.Service;
using Connection1.Global;
using System.IO;

namespace Connection1.Class
{
    public class MenuButtons
    {
        public string[] menuButtonImage = new string[] { "home.png", "app.png", "pie-chart.png", "star.png", "logout.png" };
        private int pointY = 78;
        public Button CreateMenuButton(int i)
        {
            pointY += 75;
            Button button = new Button
            {
                Size = new Size(45,45),
                Location = new Point(32, pointY),
                Image = Image.FromFile(Path.Combine(HostCommon.MenuImagesPath, menuButtonImage[i])),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Tag = i + 1
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        public Panel CreateMenuPanel(int i)
        {
            return new Panel
            {
                Size = new Size(13, 45),
                Location = new Point(103, pointY),
                Tag = i + 1
            };
        }

        public void ReturnPanelColor(List<Panel> panel, int tag)
        {
            foreach (var item in panel)
            {
                item.BackColor = Color.Transparent;
            }

            var selectedPanel = panel.Find(p => (int)p.Tag == tag);
            selectedPanel.BackColor = Color.FromArgb(186, 1, 1);
        }
    }

}
