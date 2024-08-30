using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection1.Global
{
    public static class HostCommon
    {
        public static string MenuImagesPath = ConfigurationManager.AppSettings["MenuImagesPath"];
    }
}
