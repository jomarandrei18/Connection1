using Connection1.Entities;
using Connection1.Service;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Connection1.Class
{
    public class CreatePanelOrder
    {
        private readonly IImageService _imageService;
        private Dictionary<int, Label> _orderLabels = new Dictionary<int, Label>();

        private List<OrderPanelControls> _orderPanelControlsList = new List<OrderPanelControls>();
        private OrderPanelControls _orderPanelControls;
        private List<OrderList> _orderList;
        private const int Margin = 10;
        private Panel OrderPanelList;
        private Panel _orderPanel;
        private Panel ClickOrderPanel;
        private Label deleteLabel;
        private MenuValidation _validation;

        public CreatePanelOrder(IImageService imageService, List<OrderList> orderList, MenuValidation validation, Panel orderPanel)
        {
            _imageService = imageService;
            _orderList = orderList;
            _validation = validation;
            _orderPanel = orderPanel;
            InitializeTimer();
        }

        public void CreatePanel()
        {
            var list = _orderList[_orderList.Count - 1];
            Panel lastPanel = _orderPanel.Controls.OfType<Panel>().LastOrDefault();

            OrderPanelList = new Panel
            {
                Size = new Size(_orderPanel.Width - 3 * Margin, 70 - Margin),
                Location = lastPanel == null
                ? new Point(Margin - 5, Margin)
                : new Point(Margin - 5, lastPanel.Bottom + Margin),
                Tag = list.Id
            };

            string price = list.Price.ToString("C", new CultureInfo("en-PH"));

            _orderPanelControls = new OrderPanelControls()
            {
                TagId = list.Id,
                orderPanel = OrderPanelList,
                productName = CreateLabel(list.ProductName, new Point(8, 10), new Font("Arial", 14), new Size(200, 20)),
                productQty = CreateLabel(list.Quantity.ToString(), new Point(OrderPanelList.Width - 120, 15), new Font("Arial", 16), new Size(40, 30), ContentAlignment.MiddleCenter),
                productPrice = CreateLabel(price, new Point(10, 34), new Font("Arial", 10), new Size(200, 20)),
                deletePanel = CreateDeleteLabel(OrderPanelList),
                lessQty = CreatePictureBox(new Point(OrderPanelList.Width - 150, 15), _imageService.GetImagePath("minus.png"), false),
                addQty = CreatePictureBox(new Point(OrderPanelList.Width - 80, 15), _imageService.GetImagePath("plus.png"), true)
            };


            OrderPanelList.Controls.AddRange(new Control[]
            {
                _orderPanelControls.productName,
                _orderPanelControls.productQty,
                _orderPanelControls.productPrice,
                _orderPanelControls.deletePanel,
                _orderPanelControls.lessQty,
                _orderPanelControls.addQty
            });

            _orderPanelControlsList.Add(_orderPanelControls); // Add to list all the Controls of the panel
            OrderPanelList.Click += PanelList_Click;
            _orderPanelControls.productQty.Click += ProductQty_Click;
            _orderPanelControls.productName.Click += ProductName_Click;
            _orderPanelControls.productPrice.Click += ProductPrice_Click;

            _orderLabels[list.Id] = _orderPanelControls.productQty;
            _orderPanel.Controls.Add(OrderPanelList);
        }

        private Label CreateLabel(string data, Point point, Font font, Size size, ContentAlignment align = ContentAlignment.MiddleLeft)
        {
            return new Label
            {
                Text = data,
                Location = point,
                TextAlign = align,
                Size = size,
                Font = font,
                Tag = OrderPanelList.Tag
            };
        }
        private PictureBox CreatePictureBox(Point point, string ImagePath, bool identification)
        {
            PictureBox pictureBox =  new PictureBox
            {
                Location = point,
                Size = new Size(30,30),
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackgroundImageLayout = ImageLayout.Stretch,
                Image = Image.FromFile(ImagePath),
                Tag = OrderPanelList.Tag
            };

            if (identification)
                pictureBox.Click += PictureBoxPlus_Click;
            else
                pictureBox.Click += PictureBoxMinus_Click;

            return pictureBox;
        }
        private Label CreateDeleteLabel(Panel panel)
        {
            deleteLabel = new Label
            {
                Text = "X",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(panel.Right, 0),
                Size = new Size(50, panel.Height),
                BackColor = Color.FromArgb(186, 1, 1),
                ForeColor = Color.White,
                Visible = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Tag = OrderPanelList.Tag
            };

            deleteLabel.Click += LabelDelete_Click;
            return deleteLabel;
        }

        private Timer _animationTimer;
        private int _targetPositionX;
        private const int AnimationStep = 5;

        private void InitializeTimer()
        {
            _animationTimer = new Timer
            {
                Interval = 10 
            };
            _animationTimer.Tick += AnimationTimer_Tick;
        }
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (ClickOrderPanel != null)
            {
                ClickOrderPanel.Left -= AnimationStep;

                if (ClickOrderPanel.Left <= _targetPositionX)
                {
                    _animationTimer.Stop(); // Stop the animation
                }
            }
        }
        private void PanelList_Click(object sender, EventArgs e)
        {
            OrderPanelList = sender as Panel;
            ClickControlsFunction(GetPanelTag((int)OrderPanelList.Tag));
        }
        private void ProductQty_Click(object sender, EventArgs e)
        {
            Label qtyLabel = sender as Label;
            ClickControlsFunction(GetPanelTag((int)qtyLabel.Tag));
        }
        private void ProductName_Click(object sender, EventArgs e)
        {
            Label nameLabel = sender as Label;
            ClickControlsFunction(GetPanelTag((int)nameLabel.Tag));
        }
        private void ProductPrice_Click(object sender, EventArgs e)
        {
            Label priceLabel = sender as Label;
            ClickControlsFunction(GetPanelTag((int)priceLabel.Tag));
        }
        private void LabelDelete_Click(object sender, EventArgs e)
        {
            int panelId = (int)ClickOrderPanel.Tag;
            var list = _orderList.Find(r => r.Id == panelId);
            _orderList.Remove(list);

            var targetPanel = _orderPanelControlsList
                .FirstOrDefault(p => p.TagId == panelId);

            if (targetPanel == null)
                return;

            _orderPanel.Controls.Remove(targetPanel.orderPanel);
            _orderPanelControlsList.Remove(targetPanel);
            targetPanel.orderPanel.Dispose();

            
            int top = 10;
            foreach (var panelList in _orderPanelControlsList) // Adjust the panel location
            {
                panelList.orderPanel.Top = top;
                top += 70;
            }
        }
        private OrderPanelControls GetPanelTag(int TagId)
        {
            return _orderPanelControlsList.Find(p => p.TagId == TagId);
        }
        private void PictureBoxMinus_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            UpdateQuantity(false, pictureBox);
        }
        private void PictureBoxPlus_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            UpdateQuantity(true, pictureBox);
        }
        private void UpdateQuantity(bool con, PictureBox picBox)
        {
            var getOrderPanelControls = GetPanelTag((int)picBox.Tag);

            string currentText = getOrderPanelControls.productQty.Text;
            if (int.TryParse(currentText, out int currentQty))
            {
                currentQty = con? currentQty + 1 : _validation.CheckNoLessZero(currentQty);
                getOrderPanelControls.productQty.Text = currentQty.ToString();
                return;
            }
        }
        public void UpdateQuantity(int panelId)
        {
            foreach (var list in _orderList)
            {
                if (list.Id == panelId)
                {
                    list.Quantity++;
                    UpdateLabelQuantity(panelId, list.Quantity);
                    break;
                }
            }
        }
        public void UpdateLabelQuantity(int Id, int qty)
        {
            if (_orderLabels.TryGetValue(Id, out Label label))
            {
                label.Text = qty.ToString();
            }
        }

        private int clickedPanel = 0;
        private void ClickControlsFunction(OrderPanelControls panelControls)
        {
            clickedPanel = panelControls.TagId;

            // If click another panel it will return its oringal position
            foreach (var list in _orderPanelControlsList)
            {
                var panel = list.orderPanel;

                if ((int)panel.Tag != clickedPanel)
                {
                    if (panel.Left < 0)
                    {
                        panel.Left += 50;
                        panel.Width -= 50;

                        foreach (Control control in panel.Controls)
                        {
                            if (control is Label label)
                            {
                                if (label.Text == "X")
                                    label.Visible = false;
                            }
                        }
                    }
                }
            }
            // ---------

            var getOrderPanelList = _orderPanelControlsList.Find(d => d.TagId == panelControls.TagId); // Get the DeleteLabel to Show

            if (OrderPanelList == null)
                return;

            if (getOrderPanelList.orderPanel.Location.X != 5 && panelControls.TagId == clickedPanel)
            {
                getOrderPanelList.orderPanel.Location = new Point(getOrderPanelList.orderPanel.Location.X + 50, getOrderPanelList.orderPanel.Top);
                getOrderPanelList.orderPanel.Width -= deleteLabel.Width; // Return to Original Position
                getOrderPanelList.deletePanel.Visible = false;
                clickedPanel = 0;
                return;
            }

            _targetPositionX = getOrderPanelList.orderPanel.Left - 50;
            getOrderPanelList.orderPanel.Width += deleteLabel.Width; // Move x location to -50
            getOrderPanelList.deletePanel.Visible = true;
            ClickOrderPanel = getOrderPanelList.orderPanel;
            _animationTimer.Start();
        }


    }
}
