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
    public partial class CustomMessageBox : Form
    {
        private Timer _closeTimer;
        private IImageService _imageService;
        private CustomerCoverPanel _customerCoverPanel;
        public CustomMessageBox(IImageService imageService)
        {
            InitializeComponent();
            _customerCoverPanel = new CustomerCoverPanel();
            _imageService = imageService;

            _closeTimer = new Timer();
            _closeTimer.Interval = 3000; // 3 seconds
            _closeTimer.Tick += CloseTimer_Tick;
            _closeTimer.Start();
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            _closeTimer.Stop();
            _customerCoverPanel.Close();
            this.Close();
        }

        public static DialogResult Show(IImageService imageService, string messageType)
        {
            using (var customMessageBox = new CustomMessageBox(imageService))
            {
                customMessageBox._customerCoverPanel.Show();
                switch (messageType)
                {
                    case "Success":
                        customMessageBox.Success();
                        break;
                    case "Error":
                        customMessageBox.Error();
                        break;
                    case "Question":
                        customMessageBox.Question();
                        break;
                }

                return customMessageBox.ShowDialog();
            }
        }

        public void Success()
        {
            this.msgPanel.BackColor = Color.FromArgb(75, 181, 67); // Green color
            this.lblMessage.Text = "The action completed successfully!";
            infoImage.Image = Image.FromFile(_imageService.GetGIFPath("check.gif"));
        }

        public void Error()
        {
            this.msgPanel.BackColor = Color.FromArgb(220, 53, 69); // Red color
            this.lblMessage.Text = "The action cannot be completed!";
            infoImage.Image = Image.FromFile(_imageService.GetGIFPath("error.gif"));
        }

        public void Question()
        {
            this.msgPanel.BackColor = Color.FromArgb(23, 162, 184); // Blue color
            this.lblMessage.Text = "Are you sure you want to proceed?";
            infoImage.Image = Image.FromFile(_imageService.GetGIFPath("question.gif"));
        }
    }
}
