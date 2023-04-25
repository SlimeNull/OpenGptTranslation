using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenBarbecueGrill.Utilities;

namespace OpenBarbecueGrill.Services
{
    class GlobalizationService
    {
        public Dictionary<CultureInfo, ResourceDictionary> LanguageResources { get; } =
            new Dictionary<CultureInfo, ResourceDictionary>()
            {
                { new CultureInfo("zh-CN"), new ResourceDictionary() { Source = CommonUtils.GetAssemblyResourceUri("/Strings/zh-CN.xaml") } }
            };


        public void SwitchTo(CultureInfo culture)
        {

        }
    }
}
