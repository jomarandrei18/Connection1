using Connection1.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connection1.Service
{
    public interface IButtonService
    {
        Button CreateButton(ButtonConfig config, int x, int y);
    }

    public interface ILabelService
    {
        Label CreateLabel(string text, float fontSize, FontStyle fontStyle, Point location, Color foreColor);
    }

    public interface IImageService
    {
        string GetImagePath(string fileName);
    }

    public class ButtonService : IButtonService
    {
        public Button CreateButton(ButtonConfig config, int x, int y)
        {
            Button button = ButtonFactory.CreateButton(config);
            button.Location = new Point(x, y);
            return button;
        }
    }

    public class LabelService : ILabelService
    {
        public Label CreateLabel(string text, float fontSize, FontStyle fontStyle, Point location, Color foreColor)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Book Antiqua", fontSize, fontStyle),
                Location = location,
                ForeColor = foreColor,
                AutoSize = true
            };
        }
    }

    public class ImageService : IImageService
    {
        public string GetImagePath(string fileName)
        {
            if (fileName == null) { return "The ImagePath is null";  }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Images", fileName);
        }
    }
}
