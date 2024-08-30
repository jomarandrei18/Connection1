using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using Org.BouncyCastle.Tls;
namespace Connection1.Class
{
    public class OrderPanelControls
    {
        public int TagId { get; set; }
        public Panel orderPanel { get; set; }
        public System.Windows.Forms.Label productName { get; set; }
        public System.Windows.Forms.Label categName { get; set; }
        public System.Windows.Forms.Label productQty { get; set; }
        public System.Windows.Forms.Label productPrice { get; set; }
        public System.Windows.Forms.Label deletePanel { get; set; }
        public PictureBox lessQty { get; set; }
        public PictureBox addQty { get; set; }

    }

    public class CreateOrderPanelControls
    {
        private string _productname;
        private string _categname;
        private string _categAbb;
        private string _qty;
        private decimal _price;
        private int abbWidth;
        private Panel _orderPanel;

        public CreateOrderPanelControls(OrderList list, Panel orderPanel)
        {
            _productname = list.ProductName;
            _categname = list.CategName;
            _qty = list.Quantity.ToString();
            _price = list.Price;
            _orderPanel = orderPanel;
            _categAbb = list.Abbreviation;
        }

        public Label CreateProductLabel()
        {
            return new Label
            {
                Text = _productname,
                Location = new Point(42, 10),
                Font = new Font("Arial", 14),
                //Size = new Size(170, 20),
                AutoSize = true,
                Tag = _orderPanel.Tag
            };
        }

        public Label CreateCategLabel()
        {
            Label abb =  new Label
            {
                Text = _categAbb,
                Location = new Point(8, 10),
                ForeColor = Color.FromArgb(186, 1, 1),
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Tag = _orderPanel.Tag
            };

            return abb;
        }

        public Label CreateQuatityLabel()
        {
            return new Label
            {
                Text = _qty,
                Location = new Point(_orderPanel.Width - 120, 15),
                Font = new Font("Arial", 16),
                Size = new Size(40,30),
                TextAlign = ContentAlignment.MiddleCenter,
                Tag = _orderPanel.Tag
            };
        }

        public Label CreatePriceLabel()
        {
            string price = _price.ToString("C", new CultureInfo("en-PH"));
            return new Label
            {
                Text= price,
                Location = new Point(10, 34),
                Font = new Font("Arial",10),
                Size = new Size(200,20),
                Tag = _orderPanel.Tag
            };
        }

        public Label CreateDeleteLabel()
        {
            return new Label
            {
                Text = "X",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(_orderPanel.Right, 0),
                Size = new Size(50, _orderPanel.Height),
                BackColor = Color.FromArgb(186, 1, 1),
                ForeColor = Color.White,
                Visible = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Tag = _orderPanel.Tag
            };
        }

        public PictureBox CreateQtyButton(string _imagePath, int _pictureBoxX)
        {
            return new PictureBox
            {
                Location = new Point(_orderPanel.Width - _pictureBoxX, 15),
                Size = new Size(30, 30),
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackgroundImageLayout = ImageLayout.Stretch,
                Image = Image.FromFile(_imagePath),
                Tag = _orderPanel.Tag
            };
        }
    }
}
