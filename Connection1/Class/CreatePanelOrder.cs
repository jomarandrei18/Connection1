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
        private Label _subtotal;
        private MenuValidation _validation;

        public CreatePanelOrder(
            IImageService imageService, 
            List<OrderList> orderList, 
            MenuValidation validation,
            Panel orderPanel, 
            Label subtotal)
        {
            _imageService = imageService;
            _orderList = orderList;
            _validation = validation;
            _orderPanel = orderPanel;
            _subtotal = subtotal;
            InitializeTimer();
        }

        public void CreatePanel()
        {
            var list = _orderList[_orderList.Count - 1];
            CreateOrderPanel(list);
        }

        public void CreateOrderOnHoldPanel(List<OrderList> orderList)
        {
            _orderList = orderList;
            foreach (var list in _orderList)
            {
                CreateOrderPanel(list);
            }
        }

        private void CreateOrderPanel(OrderList list)
        {
            Panel lastPanel = _orderPanel.Controls.OfType<Panel>().LastOrDefault();
            OrderPanelList = new Panel
            {
                Size = new Size(_orderPanel.Width - 3 * Margin, 70 - Margin),
                Location = lastPanel == null
                ? new Point(Margin - 5, Margin + 20)
                : new Point(Margin - 5, lastPanel.Bottom + Margin),
                Tag = list.Id

            };

            CreateOrderPanelControls _createOrderPanel = new CreateOrderPanelControls(list, OrderPanelList);

            _orderPanelControls = new OrderPanelControls()
            {
                TagId = list.Id,
                orderPanel = OrderPanelList,
                categName = _createOrderPanel.CreateCategLabel(),
                productName = _createOrderPanel.CreateProductLabel(),
                productQty = _createOrderPanel.CreateQuatityLabel(),
                productPrice = _createOrderPanel.CreatePriceLabel(),
                deletePanel = _createOrderPanel.CreateDeleteLabel(),
                lessQty = _createOrderPanel.CreateQtyButton(_imageService.GetImagePath("minus.png"), 150),
                addQty = _createOrderPanel.CreateQtyButton(_imageService.GetImagePath("plus.png"), 80),
            };


            OrderPanelList.Controls.AddRange(new Control[]
            {
                _orderPanelControls.productName,
                _orderPanelControls.productQty,
                _orderPanelControls.categName,
                _orderPanelControls.productPrice,
                _orderPanelControls.deletePanel,
                _orderPanelControls.lessQty,
                _orderPanelControls.addQty
            });

            _orderPanelControlsList.Add(_orderPanelControls); // Add to list all the Controls of the panel
            OrderPanelList.Click += PanelList_Click;
            _orderPanelControls.deletePanel.Click += LabelDelete_Click;
            _orderPanelControls.productQty.Click += ProductQty_Click;
            _orderPanelControls.productName.Click += ProductName_Click;
            _orderPanelControls.productPrice.Click += ProductPrice_Click;
            _orderPanelControls.lessQty.Click += PictureBoxMinus_Click;
            _orderPanelControls.addQty.Click += PictureBoxPlus_Click;

            _orderLabels[list.Id] = _orderPanelControls.productQty;
            _orderPanel.Controls.Add(OrderPanelList);
        }

        private Timer _animationTimer;
        private int _targetPositionX;
        private const int AnimationStep = 5;

        private void InitializeTimer()
        {
            _animationTimer = new Timer
            {
                Interval = 3
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

            var targetPanel = _orderPanelControlsList.FirstOrDefault(p => p.TagId == panelId);

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

            _subtotal.Text = GetTotalPrice();
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

            var list = _orderList.Find(p => p.Id ==  getOrderPanelControls.TagId);
            string currentText = getOrderPanelControls.productQty.Text;
            if (int.TryParse(currentText, out int currentQty))
            {
                currentQty = con? currentQty + 1 : _validation.CheckNoLessZero(currentQty);
                list.Quantity = currentQty;
                getOrderPanelControls.productQty.Text = currentQty.ToString();
                _subtotal.Text = GetTotalPrice();
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

            _subtotal.Text = GetTotalPrice();
        }
        private void UpdateLabelQuantity(int Id, int qty)
        {
            if (_orderLabels.TryGetValue(Id, out Label label))
            {
                label.Text = qty.ToString();
            }
        }
        public string GetTotalPrice()
        {
            decimal orderTotal = 0;
            foreach (var order in _orderList)
            {
                orderTotal += order.Price * order.Quantity;
            }
            return orderTotal.ToString("C", new CultureInfo("en-PH"));
        }
        public void ClearOrderListPanel(bool con = false)
        {
            //Put a confirmation message --------------------------------
            foreach (var orderPanel in _orderPanelControlsList)
            {
                _orderPanel.Controls.Remove(orderPanel.orderPanel);
                orderPanel.orderPanel.Dispose();
            }

            _orderPanelControlsList.Clear();
            _subtotal.Text = 0.ToString("C", new CultureInfo("en-PH"));

            if (con) { _orderList.Clear(); }
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
                getOrderPanelList.orderPanel.Width -= _orderPanelControls.deletePanel.Width; // Return to Original Position
                getOrderPanelList.deletePanel.Visible = false;
                clickedPanel = 0;
                return;
            }

            _targetPositionX = getOrderPanelList.orderPanel.Left - 50;
            getOrderPanelList.orderPanel.Width += _orderPanelControls.deletePanel.Width; // Move x location to -50
            getOrderPanelList.deletePanel.Visible = true;
            ClickOrderPanel = getOrderPanelList.orderPanel;
            _animationTimer.Start();
        }


    }
}
