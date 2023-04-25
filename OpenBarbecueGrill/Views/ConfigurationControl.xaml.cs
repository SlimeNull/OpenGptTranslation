using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenBarbecueGrill.Services;

namespace OpenBarbecueGrill.Views
{
    /// <summary>
    /// Interaction logic for ConfigurationControl.xaml
    /// </summary>
    public partial class ConfigurationControl : UserControl
    {
        public ConfigurationControl()
        {

            DataContext = this;
            InitializeComponent();
        }

        public ConfigurationService ConfigurationService { get; } =
            App.GetService<ConfigurationService>();
    }
}
