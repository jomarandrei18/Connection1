using Connection1.Model;
using Connection1.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connection1.Class
{
    public class ButtonManagerCreation
    {
        private int _currentWindowSize;
        private int _constantWindowSize;
        private int _constantX;
        private int _column;
        private IButtonService _buttonService;
        private ButtonSize _buttonSize;
        private List<Button> _generatedButtons;

        public ButtonManagerCreation(IButtonService buttonService, ButtonSize buttonSize) 
        {
            _buttonService = buttonService;
            _buttonSize = buttonSize;
            _generatedButtons = new List<Button>();
        }

        public void Initialize(int initialWindowSize)
        {
            _constantWindowSize = initialWindowSize;
            _constantX = _buttonSize.x;
            _currentWindowSize = _constantWindowSize;
            _column = 1;
        }

        public Button CreateButton(ButtonConfig buttonConfig, bool addToGenerated = false)
        {
            var button = _buttonService.CreateButton(buttonConfig, _buttonSize.x, _buttonSize.y);
            if (addToGenerated)
            {
                _generatedButtons.Add(button);
            }

            PositionButton(button);
            _currentWindowSize -= _buttonSize.x;

            return button;
        }      
        
        private void PositionButton(Button button)
        {
            if(_currentWindowSize < _buttonSize.x)
            {
                ResetPoitionForNewRow();
            }

            button.Location = new System.Drawing.Point(_buttonSize.x + (_buttonSize.sizeW * (_column - 1)), _buttonSize.y);
            _buttonSize.x += 5;
            _column++;
        }

        public void ChangeBackColorButton(Color color, Button button)
        {
            button.ForeColor = Color.White;
            button.BackColor = color;
        }

        public void ClearButtons(Control.ControlCollection controls)
        {
            foreach(var buttons in _generatedButtons)
            {
                controls.Remove(buttons);
            }

            foreach (Control control in controls)
            {
                if (control is Button button)
                {
                    button.BackColor = Color.White;
                    button.ForeColor = Color.Black; 
                }
            }

            _generatedButtons.Clear();
        }

        public (int Id, string TagLine, decimal Price) GetDetailsFromButton(Button button)
        {
            var details = ((int Id, string TagLine, decimal Price))button.Tag;
            return details;
        }

        public void ClearButtons2()
        {
            foreach (var button in _generatedButtons)
            {
                button.BackColor = Color.White;
                button.ForeColor = Color.Black;
            }
        }

        public void ResetPoitionForNewRow()
        {
            _currentWindowSize = _constantWindowSize;
            _buttonSize.y += _buttonSize.sizeH +5;
            _buttonSize.x = _constantX;
            _column = 1;
        }

        public void ResetPoitionForNewRow(int newValueY)
        {
            _currentWindowSize = _constantWindowSize;
            _buttonSize.y = newValueY;
            _buttonSize.x = _constantX;
            _column = 1;
        }

    }
}
