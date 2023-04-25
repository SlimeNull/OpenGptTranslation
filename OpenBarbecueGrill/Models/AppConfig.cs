using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenBarbecueGrill.Models
{
    [ObservableObject]
    public partial class AppConfig
    {
        [ObservableProperty]
        private string _apiKey = string.Empty;

        [ObservableProperty]
        private string _translationLanguage = "简体中文";
    }
}
