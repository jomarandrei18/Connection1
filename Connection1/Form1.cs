using Connection1.Class;
using Connection1.Connection;
using Connection1.Entities;
using Connection1.Model;
using Connection1.Service;
using Microsoft.Win32;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connection1
{
    public partial class Menu : Form
    {
        private readonly IButtonService _buttonService;
        private readonly ILabelService _labelService;
        private readonly IImageService _imageService;
        private readonly IMenuCategoryService _menuCategoryService;
        private CreatePanelOrder _createPanelOrder;
        private ButtonManagerCreation _buttonManagerCreation;
        private ButtonSize _buttonSize;
        private ButtonConfig _buttonConfig;
        private MenuValidation validation;
        private CreatePricePanel _createPricePanel;
        
        private Label categName;
        private Label tagLine;
        private Label subtotal;
        private Label discSales;
        private Label totalSalesTax;
        private Label total;

        private Panel priceBotPanel;
        private Panel priceTopPanel;
        private OrderList _order;
        private List<OrderList> _orderList;
        private int _categY;
        private int mainSize;
        private string productName;
        private int productId;
        private decimal productPrice;


        public Menu(ButtonSize buttonSize, 
            IButtonService buttonService, 
            ILabelService labelService, 
            IImageService imageService, 
            IMenuCategoryService menuCategoryService)
        {
            InitializeComponent();
            _buttonSize = buttonSize;
            _buttonService = buttonService;
            _labelService = labelService;
            _imageService = imageService;
            _menuCategoryService = menuCategoryService;

            _createPricePanel = new CreatePricePanel();
            _order = new OrderList();
            _orderList = new List<OrderList>();
            validation = new MenuValidation(_orderList);

            priceBotPanel = _createPricePanel.CreateRoundBotPanel();
            priceTopPanel = _createPricePanel.CreateRoundTopPanel();

            //Creating Label
            subtotal = _createPricePanel.CreatedRightAlignLabel(17, 12, 18);
            discSales = _createPricePanel.CreatedRightAlignLabel(44, 12, 18);
            totalSalesTax = _createPricePanel.CreatedRightAlignLabel(71, 12, 18);
            total = _createPricePanel.CreatedRightAlignLabel(17, 27, 35);

            _buttonManagerCreation = new ButtonManagerCreation(buttonService, buttonSize);
            _createPanelOrder = new CreatePanelOrder(_imageService, _orderList, validation, OrderPanel);
            
            AddOrderList.Click += AddOrderList_Click;

            //add the price panel
            PricePanel.Controls.Add(priceBotPanel);
            PricePanel.Controls.Add(priceTopPanel);

            //For bottom price panel
            priceBotPanel.Controls.Add(subtotal);
            priceBotPanel.Controls.Add(discSales);
            priceBotPanel.Controls.Add(totalSalesTax);
            priceBotPanel.Controls.Add(_createPricePanel.CreateLabelForBotPricePanel(15,"Subtotal"));
            priceBotPanel.Controls.Add(_createPricePanel.CreateLabelForBotPricePanel(42, "Discount Sales"));
            priceBotPanel.Controls.Add(_createPricePanel.CreateLabelForBotPricePanel(69, "Total Sales Tax"));

            //For top price panel
            priceTopPanel.Controls.Add(_createPricePanel.CreateLabelForTopPricePanel(27, "Total"));
            priceTopPanel.Controls.Add(total);
        }

        
        private void Menu_Load(object sender, EventArgs e)
        {
            CreateUIElements();
        }
        private void CreateUIElements()
        {
            InitializeButtonConfig();
            SetMainSize();

            foreach (var item in _menuCategoryService.GetPagedMenuCategories())
            {
                ConfigureButton(item);
                var button = _buttonManagerCreation.CreateButton(_buttonConfig);
                button.Click += Button_Click;
                this.Controls.Add(button);
            }

            _categY = _buttonSize.y;
        }
        private void SetMainSize()
        {
            mainSize = this.Width - this.PricePanel.Width - this.MenuPanel.Width - 45;
            _buttonManagerCreation.Initialize(mainSize);
        }
        private void InitializeButtonConfig()
        {
            _buttonConfig = new ButtonConfig
            {
                Size = new Size(_buttonSize.sizeW, _buttonSize.sizeH)
            };
        }
        private void ConfigureButton(MenuCategory category)
        {
            _buttonConfig.Text = category.CategName.ToUpper();
            _buttonConfig.ImagePath = _imageService.GetImagePath(category.CategImagePath);
            _buttonConfig.TagLine = category.TagLine == null ? "" : category.TagLine.ToUpper();
            _buttonConfig.Id = category.CategId;

        }
        private void Button_Click(object sender, EventArgs e)
        {
            AddOrderList.Enabled = false;
            priceTag.Text = "Price : ";
            _buttonManagerCreation.ClearButtons(this.Controls);
            this.Controls.Remove(categName);
            this.Controls.Remove(tagLine);

            Button button = (Button)sender;
            _order.CategName = button.Text;
            DisplayCategoryDetails(button);
            LoadProductButtons(_buttonManagerCreation.GetDetailsFromButton(button).Id);
        }
        private void DisplayCategoryDetails(Button clickedButton)
        {
            _buttonManagerCreation.ChangeBackColorButton(Color.FromArgb(186, 1, 1), clickedButton);

            categName = _labelService.CreateLabel(
                clickedButton.Text,
                36,
                FontStyle.Bold,
                new Point(_buttonSize.x - 5, _categY + _buttonSize.sizeH + 25),
                Color.FromArgb(251, 189, 13));

            this.Controls.Add(categName);

            tagLine = _labelService.CreateLabel(
                _buttonManagerCreation.GetDetailsFromButton(clickedButton).TagLine,
                12,
                FontStyle.Regular,
                new Point(_buttonSize.x, categName.Location.Y + categName.Height - 10),
                Color.Black);

            this.Controls.Add(tagLine);

            tagLine.BringToFront();
        }
        private void LoadProductButtons(int categoryDetails)
        {
            int x = categName.Location.Y + categName.Height + 25;
            _buttonManagerCreation.ResetPoitionForNewRow(x);
            SetMainSize();

            foreach (var product in _menuCategoryService.GetProductList(categoryDetails))
            {
                _buttonConfig.Text = product.ProductName.ToUpper();
                _buttonConfig.Id = product.ProductId;
                _buttonConfig.price = product.Price;
                var button = _buttonManagerCreation.CreateButton(_buttonConfig, true);
                button.Click += ProductButton_Click;
                this.Controls.Add(button);
            }

        }

        private void ProductButton_Click(object sender, EventArgs e)
        {
            AddOrderList.Enabled = true;
            Button button = sender as Button;

            decimal price = _buttonManagerCreation.GetDetailsFromButton(button).Price;
            productId = _buttonManagerCreation.GetDetailsFromButton(button).Id;
            productName = button.Text;
            productPrice = price;

            _buttonManagerCreation.ClearButtons2();
            _buttonManagerCreation.ChangeBackColorButton(Color.FromArgb(251, 189, 13), button);
            priceTag.Text = $"Price : {price:F2}";

        }
        
        private void AddOrderList_Click(object sender, EventArgs e)
        {
            //Panel lastPanel = OrderPanel.Controls.OfType<Panel>().LastOrDefault();

            var getPanelId = validation.CheckIfExistInList(productName, productPrice, productId, out bool isExist);
            
            if (isExist)
            {
                _createPanelOrder.CreatePanel();
            }
            else
            {
                _createPanelOrder.UpdateQuantity(getPanelId);
            }
            GetPrice();
        }

        private void GetPrice()
        {
            decimal orderTotal = 0;
            foreach (var order in _orderList)
            {
                orderTotal += order.Price * order.Quantity;
            }

            subtotal.Text = orderTotal.ToString("C", new CultureInfo("en-PH"));
            total.Text = orderTotal.ToString("C", new CultureInfo("en-PH"));
            
        }

    }
}

