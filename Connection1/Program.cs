using Connection1.Connection;
using Connection1.Model;
using Connection1.Repository;
using Connection1.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connection1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ConnDbContext context = new ConnDbContext();
            IMenuCategoryRepository repository = new MenuCategoryRepository(context);
            IMenuCategoryService service = new MenuCategoryService(repository);
            IButtonService buttonService = new ButtonService();
            ILabelService labelService = new LabelService();
            var imageService = new ImageService();
            ButtonSize  buttonSize = new ButtonSize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu(buttonSize, buttonService, labelService, imageService, service));
        }
    }
}
