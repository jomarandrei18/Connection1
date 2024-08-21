using Connection1.Class;
using Connection1.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connection1
{
    
    public partial class OnHold : Form
    {
        private List<Color> colorPanel = new List<Color>() { 
            Color.FromArgb(20, 96, 124, 60),
            Color.FromArgb(20, 128,156,19),
            Color.FromArgb(20, 171,195,47),
            Color.FromArgb(20, 181,229,80),
            Color.FromArgb(20, 236,236,163)};

        private const int PanelMargin = 10;
        private List<ForOnHoldOrderList> _forOnHoldOrderLists;
        public List<OrderList> _orderlist;
        public bool isClicked = false;

        public OnHold(int count, List<ForOnHoldOrderList> forOnHoldOrderLists, List<OrderList> orderlist)
        {
            InitializeComponent();
            _forOnHoldOrderLists = forOnHoldOrderLists;
            _orderlist = orderlist;
            DisplayOnHoldOrder();
        }

        public void DisplayOnHoldOrder()
        {
            int colorCount = 0;
            foreach (var list in _forOnHoldOrderLists.OrderByDescending(order => order.OrderDate).ToList())
            {
                Panel lastPanel = PanelHolder.Controls.OfType<Panel>().LastOrDefault();
                Panel addPanel = OnHoldPanel(colorPanel[colorCount], lastPanel, list.Id);
                PanelHolder.Controls.Add(addPanel);
                addPanel.Controls.Add(PanelLabel(list.CustomerName, 12, new Font("Constantia", 12, FontStyle.Bold)));
                addPanel.Controls.Add(PanelLabel(FormatDate(list.OrderDate), 36, new Font("Microsoft Sans Serif", 8)));

                
                if (colorCount == colorPanel.Count - 1)
                    colorCount = 0;

                colorCount++;
            }
        }

        private Panel OnHoldPanel(Color color, Panel lastPanel, int Id)
        {
            Panel panel = new TransparentPanel
            {
                Size = new Size(300, 59),
                Location = new Point(12, lastPanel == null ? 12 : lastPanel.Bottom + PanelMargin),
                BackColor = color,
                Tag = Id
            };

            panel.Click += Panel_Click;
            return panel;
        }

        private Label PanelLabel(string item, int pointY, Font font)
        {
            return new Label
            {
                Location = new Point(17, pointY),
                Text = item,
                Font = font,
                AutoSize = true
            };

        }

        private string FormatDate(DateTime date)
        {
            return date.ToString("hh:mm tt MMMM d");
        }


        private void Panel_Click(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            var selectedOnHold = _forOnHoldOrderLists.Find(p => p.Id == (int)panel.Tag);

            _orderlist = selectedOnHold.OnHoldOrderList;
            isClicked = true;
            this.Close();
        }
    }
}
