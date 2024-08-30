using Connection1.Class;
using Connection1.Connection;
using Connection1.Entities;
using Connection1.Model;
using Connection1.Service;
using Microsoft.Win32;
using Mysqlx.Crud;
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
using Order = Connection1.Entities.Order;

namespace Connection1
{
    public partial class Menu : Form
    {
        private readonly IButtonService _buttonService;
        private readonly ILabelService _labelService;
        private readonly IImageService _imageService;
        private readonly IMenuCategoryService _menuCategoryService;
        private ButtonSize _buttonSize;
        private MenuButtons _menuButtons;
        private List<Panel> _panelMenuButtonList;
        private Button btnClick = null;
        public Menu(ButtonSize buttonSize,
            IButtonService buttonService,
            ILabelService labelService,
            IImageService imageService,
            IMenuCategoryService menuCategoryService)
        {
            InitializeComponent();
            _buttonSize = buttonSize;
            _buttonService = buttonService; // remove here
            _labelService = labelService;
            _imageService = imageService;
            _menuCategoryService = menuCategoryService;

            _panelMenuButtonList = new List<Panel>();
            _menuButtons = new MenuButtons();

            for (int i = 0; i < _menuButtons.menuButtonImage.Length; i++)
            {
                Button button = _menuButtons.CreateMenuButton(i);
                Panel panel = _menuButtons.CreateMenuPanel(i);
                button.Click += Button_Click;
                MenuPanel.Controls.Add(button);
                MenuPanel.Controls.Add(panel);
                _panelMenuButtonList.Add(panel);
                btnClick = i == 0 ? button : btnClick;
            }
            
        }


        private void Button_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            _menuButtons.ReturnPanelColor(_panelMenuButtonList, (int)button.Tag);
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            btnClick.PerformClick();
            OrderMenu orderMenu = new OrderMenu(
                _menuCategoryService,
                _buttonService,
                _buttonSize,
                _imageService,
                _labelService);

            orderMenu.Dock = DockStyle.Fill;
            userControl.Controls.Add(orderMenu);


        }
        //private void CreateUIElements()
        //{
        //    InitializeButtonConfig();
        //    CreatePricePanelButton();
        //    SetMainSize();
        //    Button btnClick = null;

        //    int counter = 1;
        //    foreach (var item in _menuCategoryService.GetPagedMenuCategories())
        //    {
        //        ConfigureButton(item);
        //        var button = _buttonManagerCreation.CreateButton(_buttonConfig);
        //        button.Click += Button_Click;
        //        this.Controls.Add(button);
        //        btnClick = (counter == 1) ? button : btnClick;
        //        counter++;
        //    }

        //    _categY = _buttonSize.y;
        //    btnClick.PerformClick();
        //}
        //private void SetMainSize()
        //{
        //    mainSize = this.Width - this.PricePanel.Width - this.MenuPanel.Width - 45;
        //    _buttonManagerCreation.Initialize(mainSize);
        //}
        //private void InitializeButtonConfig()
        //{
        //    _buttonConfig = new ButtonConfig
        //    {
        //        Size = new Size(_buttonSize.sizeW, _buttonSize.sizeH)
        //    };
        //}

        //private void CreatePricePanelButton()
        //{
        //    //add button in price panel
        //    //btnClear = _buttonManagerCreation.RedesignPricePanelButton(_buttonConfig, 90, 24, Color.White, Color.FromArgb(186, 1, 1), "X");
        //    btnOnHold = _buttonManagerCreation.RedesignPricePanelButton(_buttonConfig, 151, 130, Color.White, Color.FromArgb(250, 192, 27), "ON HOLD");
        //    btnProceed = _buttonManagerCreation.RedesignPricePanelButton(_buttonConfig, 151, 297, Color.White, Color.FromArgb(24, 139, 70), "PROCEED>>");

        //    btnClear.Click += BtnClear_Click;
        //    btnProceed.Click += btnProceed_Click;
        //    btnOnHold.Click += btnOnHold_Click;
        //    btnHold.Click += btnHold_Click;

        //    //PricePanel.Controls.Add(btnClear);
        //    PricePanel.Controls.Add(btnOnHold);
        //    PricePanel.Controls.Add(btnProceed);
        //}

        //private void ConfigureButton(MenuCategory category)
        //{
        //    _buttonConfig.Text = category.CategName.ToUpper();
        //    _buttonConfig.ImagePath = _imageService.GetImagePath(category.CategImagePath);
        //    _buttonConfig.TagLine = category.TagLine == null ? "" : category.TagLine.ToUpper();
        //    _buttonConfig.Id = category.CategId;

        //}
        //private void Button_Click(object sender, EventArgs e)
        //{
        //    AddOrderList.Enabled = false;
        //    priceTag.Text = "Price : ";
        //    _buttonManagerCreation.ClearButtons(this.Controls);
        //    this.Controls.Remove(categName);
        //    this.Controls.Remove(tagLine);

        //    Button button = (Button)sender;
        //    categoryName = button.Text;
        //    DisplayCategoryDetails(button);
        //    LoadProductButtons(_buttonManagerCreation.GetDetailsFromButton(button).Id);
        //}
        //private void DisplayCategoryDetails(Button clickedButton)
        //{
        //    _buttonManagerCreation.ChangeBackColorButton(Color.FromArgb(186, 1, 1), clickedButton);

        //    categName = _labelService.CreateLabel(
        //        clickedButton.Text,
        //        36,
        //        FontStyle.Bold,
        //        new Point(_buttonSize.x - 5, _categY + _buttonSize.sizeH + 25),
        //        Color.FromArgb(251, 189, 13));

        //    this.Controls.Add(categName);

        //    tagLine = _labelService.CreateLabel(
        //        _buttonManagerCreation.GetDetailsFromButton(clickedButton).TagLine,
        //        12,
        //        FontStyle.Regular,
        //        new Point(_buttonSize.x, categName.Location.Y + categName.Height - 10),
        //        Color.Black);

        //    this.Controls.Add(tagLine);

        //    tagLine.BringToFront();
        //}
        //private void LoadProductButtons(int categoryDetails)
        //{
        //    int x = categName.Location.Y + categName.Height + 25;
        //    _buttonManagerCreation.ResetPoitionForNewRow(x);
        //    SetMainSize();

        //    foreach (var product in _menuCategoryService.GetProductList(categoryDetails))
        //    {
        //        _buttonConfig.Text = product.ProductName.ToUpper();
        //        _buttonConfig.Id = product.ProductId;
        //        _buttonConfig.price = product.Price;
        //        var button = _buttonManagerCreation.CreateButton(_buttonConfig, true);
        //        button.Click += ProductButton_Click;
        //        this.Controls.Add(button);
        //    }
        //}

        //private void ProductButton_Click(object sender, EventArgs e)
        //{
        //    AddOrderList.Enabled = true;
        //    Button button = sender as Button;

        //    decimal price = _buttonManagerCreation.GetDetailsFromButton(button).Price;
        //    productId = _buttonManagerCreation.GetDetailsFromButton(button).Id;
        //    productName = button.Text;
        //    productPrice = price;

        //    _buttonManagerCreation.ClearButtons2();
        //    _buttonManagerCreation.ChangeBackColorButton(Color.FromArgb(251, 189, 13), button);
        //    priceTag.Text = $"Price : {price:F2}";

        //}

        //private void AddOrderList_Click(object sender, EventArgs e)
        //{
        //    var getPanelId = validation.CheckIfExistInList(categoryName, productName, productPrice, productId, out bool isExist);

        //    if (isExist)
        //    {
        //        _createPanelOrder.CreatePanel();
        //    }
        //    else
        //    {
        //        _createPanelOrder.UpdateQuantity(getPanelId);
        //    }

        //    subtotal.Text = _createPanelOrder.GetTotalPrice();
        //}

        //private void BtnClear_Click(object sender, EventArgs e)
        //{
        //    _createPanelOrder.ClearOrderListPanel(true);
        //}
        //private void btnOnHold_Click(object sender, EventArgs e)
        //{
        //    OnHold hold = new OnHold(10, _forOnHoldOrderList, _orderList);
        //    hold.ShowDialog();

        //    if (hold.isClicked)
        //    {
        //        _orderList = hold._orderlist;
        //        _forOnHoldOrderList.Remove(_forOnHoldOrderList.Find(p => p.OnHoldOrderList == _orderList));
        //        validation = new MenuValidation(_orderList);
        //        _createPanelOrder.CreateOrderOnHoldPanel(_orderList);
        //    }

        //}

        //private void btnHold_Click(object sender, EventArgs e)
        //{
        //    if (_orderList.Count == 0)
        //    {
        //        return;
        //    }

        //    _forOnHoldOrderList
        //        .Add(_addForOnHoldOrderList
        //        .Add(_forOnHoldOrderList.Count + 1, _orderList));
        //    _createPanelOrder.ClearOrderListPanel(true);
        //}
        //private void btnProceed_Click(object sender, EventArgs e)
        //{
        //    int count = _menuCategoryService.CountOrderToday();
        //    AddOrderList _addOrderlist = new AddOrderList();
        //    _menuCategoryService.AddMenuCategory(_addOrderlist.Add(count, _orderList));
        //    _createPanelOrder.ClearOrderListPanel();

        //    if (_orderList.Count == 0) { return; }
        //    CustomMessageBox.Show(_imageService, "Success");
        //}

        //private void BtnSales_Clcick(object sender, EventArgs e)
        //{
        //    Sales sales = new Sales(_menuCategoryService);
        //    sales.ShowDialog();
        //}


    }
}

