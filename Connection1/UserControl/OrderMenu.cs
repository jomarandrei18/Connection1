using Connection1.Class;
using Connection1.Entities;
using Connection1.Model;
using Connection1.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connection1
{
    public partial class OrderMenu : UserControl
    {
        private readonly IMenuCategoryService _menuCategoryService;
        private readonly ILabelService _labelService;
        private readonly IImageService _imageService;
        private CreatePanelOrder _createPanelOrder;
        private ButtonManagerCreation _buttonManagerCreation;
        private ButtonConfig _buttonConfig;
        private ButtonSize _buttonSize;
        private MenuValidation menuValidation;
        private CreatePricePanel _createPricePanel;
        private AddForOnHoldOrderList _addForOnHoldOrderList;

        private Label categName;
        private Label tagLine;
        private Label subtotal;
        private Label discSales;
        private Label totalSalesTax;
        private Label total;

        private Button btnOnHold;
        private Button btnProceed;

        private Panel priceBotPanel;
        private Panel priceTopPanel;
        private List<OrderList> _orderList;
        private List<ForOnHoldOrderList> _forOnHoldOrderList;

        private int _categY;
        private int mainSize;
        private string categoryName;
        private string productName;
        private string categAbb;
        private int productId;
        private decimal productPrice;

        public OrderMenu(IMenuCategoryService menuCategoryService,
            IButtonService buttonService,
            ButtonSize buttonSize,
            IImageService imageService,
             ILabelService labelService)
        {
            InitializeComponent();
            _menuCategoryService = menuCategoryService;
            _buttonManagerCreation = new ButtonManagerCreation(buttonService, buttonSize);
            _labelService = labelService;
            _buttonSize = buttonSize;
            _imageService = imageService;
            _createPricePanel = new CreatePricePanel();

            _forOnHoldOrderList = new List<ForOnHoldOrderList>();
            _orderList = new List<OrderList>();
            menuValidation = new MenuValidation(_orderList);
            _addForOnHoldOrderList = new AddForOnHoldOrderList();

            priceBotPanel = _createPricePanel.CreateRoundBotPanel();
            priceTopPanel = _createPricePanel.CreateRoundTopPanel();

            subtotal = _createPricePanel.CreatedRightAlignLabel(17, 12, 18);
            discSales = _createPricePanel.CreatedRightAlignLabel(44, 12, 18);
            totalSalesTax = _createPricePanel.CreatedRightAlignLabel(71, 12, 18);
            total = _createPricePanel.CreatedRightAlignLabel(17, 27, 35);

            _createPanelOrder = new CreatePanelOrder(_imageService, _orderList, menuValidation, OrderPanel, subtotal);

            AddOrderList.Click += AddOrderList_Click;
            PricePanel.Controls.Add(priceBotPanel);
            PricePanel.Controls.Add(priceTopPanel);

            priceBotPanel.Controls.Add(subtotal);
            priceBotPanel.Controls.Add(discSales);
            priceBotPanel.Controls.Add(totalSalesTax);
            priceBotPanel.Controls.Add(_createPricePanel.CreateLabelForBotPricePanel(15, "Subtotal"));
            priceBotPanel.Controls.Add(_createPricePanel.CreateLabelForBotPricePanel(42, "Discount Sales"));
            priceBotPanel.Controls.Add(_createPricePanel.CreateLabelForBotPricePanel(69, "Total Sales Tax"));

            priceTopPanel.Controls.Add(_createPricePanel.CreateLabelForTopPricePanel(27, "Total"));
            priceTopPanel.Controls.Add(total);
        }

        private void OrderMenu_Load(object sender, EventArgs e)
        {
            CreateUIElements();
        }

        private void CreateUIElements()
        {
            InitializeButtonConfig();
            CreatePricePanelButton();
            SetMainSize();
            Button btnClick = null;

            int counter = 1;
            foreach (var item in _menuCategoryService.GetPagedMenuCategories())
            {
                ConfigureButton(item);
                var button = _buttonManagerCreation.CreateButton(_buttonConfig);
                button.Click += Button_Click;
                MenuList.Controls.Add(button);
                btnClick = (counter == 1) ? button : btnClick;
                counter++;
            }

            _categY = _buttonSize.y;
            btnClick.PerformClick();
        }

        private void InitializeButtonConfig()
        {
            _buttonConfig = new ButtonConfig
            {
                Size = new Size(_buttonSize.sizeW, _buttonSize.sizeH)
            };
        }

        private void CreatePricePanelButton()
        {
            //add button in price panel
            //btnClear = _buttonManagerCreation.RedesignPricePanelButton(_buttonConfig, 90, 24, Color.White, Color.FromArgb(186, 1, 1), "X");
            btnOnHold = _buttonManagerCreation.RedesignPricePanelButton(_buttonConfig, 151, 130, Color.White, Color.FromArgb(250, 192, 27), "ON HOLD");
            btnProceed = _buttonManagerCreation.RedesignPricePanelButton(_buttonConfig, 151, 297, Color.White, Color.FromArgb(24, 139, 70), "PROCEED>>");

            btnClear.Click += BtnClear_Click;
            btnProceed.Click += btnProceed_Click;
            btnOnHold.Click += btnOnHold_Click;
            btnHold.Click += btnHold_Click;

            //PricePanel.Controls.Add(btnClear);
            PricePanel.Controls.Add(btnOnHold);
            PricePanel.Controls.Add(btnProceed);
        }

        private void SetMainSize()
        {
            mainSize = this.MenuList.Width;
            _buttonManagerCreation.Initialize(mainSize);
        }

        private void ConfigureButton(MenuCategory category)
        {
            _buttonConfig.Text = category.CategName.ToUpper();
            _buttonConfig.ImagePath = _imageService.GetImagePath(category.CategImagePath);
            _buttonConfig.TagLine = category.TagLine == null ? "" : category.TagLine.ToUpper();
            _buttonConfig.Id = category.CategId;
            _buttonConfig.abbreviation = category.CategAbb;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            AddOrderList.Enabled = false;
            priceTag.Text = "Price : ";
            _buttonManagerCreation.ClearButtons(MenuList.Controls);
            MenuList.Controls.Remove(categName);
            MenuList.Controls.Remove(tagLine);

            Button button = (Button)sender;
            categoryName = button.Text;
            categAbb = _buttonManagerCreation.GetDetailsFromButton(button).abbreviation;
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

            MenuList.Controls.Add(categName);

            tagLine = _labelService.CreateLabel(
                 _buttonManagerCreation.GetDetailsFromButton(clickedButton).TagLine,
                12,
                FontStyle.Regular,
                new Point(_buttonSize.x, categName.Location.Y + categName.Height - 10),
                Color.Black);

            MenuList.Controls.Add(tagLine);

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
                MenuList.Controls.Add(button);
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
            var getPanelId = menuValidation.CheckIfExistInList(categoryName, productName, productPrice, productId, categAbb, out bool isExist);

            if (isExist)
                _createPanelOrder.CreatePanel();
            else
                _createPanelOrder.UpdateQuantity(getPanelId);

            subtotal.Text = _createPanelOrder.GetTotalPrice();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            _createPanelOrder.ClearOrderListPanel(true);
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            if (_orderList.Count == 0)
                return;

            _forOnHoldOrderList
                .Add(_addForOnHoldOrderList
                .Add(_forOnHoldOrderList.Count + 1, _orderList));
            _createPanelOrder.ClearOrderListPanel(true);
        }

        private void btnOnHold_Click(object sender, EventArgs e)
        {
            OnHold hold = new OnHold(10, _forOnHoldOrderList, _orderList);
            hold.ShowDialog();

            if (hold.isClicked)
            {
                _orderList = hold._orderlist;
                _forOnHoldOrderList.Remove(_forOnHoldOrderList.Find(p => p.OnHoldOrderList == _orderList));
                menuValidation = new MenuValidation(_orderList);
                _createPanelOrder.CreateOrderOnHoldPanel(_orderList);
                subtotal.Text = _createPanelOrder.GetTotalPrice();
            }
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            int count = _menuCategoryService.CountOrderToday();
            AddOrderList _addOrderlist = new AddOrderList();
            _menuCategoryService.AddMenuCategory(_addOrderlist.Add(count, _orderList));
            _createPanelOrder.ClearOrderListPanel();

            if (_orderList.Count == 0) { return; }
            CustomMessageBox.Show(_imageService, "Success");
        }


    }
}
