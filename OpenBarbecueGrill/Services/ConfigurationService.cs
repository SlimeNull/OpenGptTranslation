using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using OpenBarbecueGrill.Models;
using OpenBarbecueGrill.Utilities;

namespace OpenBarbecueGrill.Services
{
    [ObservableObject]
    public partial class ConfigurationService
    {

        public ConfigurationService(IOptions<AppConfig> configOptions)
        {
            ConfigOptions = configOptions;
        }

        private IOptions<AppConfig> ConfigOptions { get; }
        public AppConfig Configuration => ConfigOptions.Value;


        [RelayCommand]
        public void Save()
        {
            using FileStream fs = File.Create(GlobalValues.JsonConfigurationFilePath);
            JsonSerializer.Serialize(fs, ConfigOptions.Value, JsonHelper.ConfigurationOptions);
        }
    }
}
