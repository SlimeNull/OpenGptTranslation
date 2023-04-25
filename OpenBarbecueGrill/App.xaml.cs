using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenBarbecueGrill.Models;
using OpenBarbecueGrill.Services;
using OpenBarbecueGrill.Utilities;
using OpenBarbecueGrill.ViewModels;
using OpenBarbecueGrill.Views;

namespace OpenBarbecueGrill
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost Host { get; } = new HostBuilder()
            .ConfigureAppConfiguration(config =>
            {
                string path = Path.Combine(
                    FileSystemUtils.GetEntryPointFolder(),
                    GlobalValues.JsonConfigurationFilePath);

                // 支持使用 JSON 文件以及环境变量进行配置
                config
                    .AddJsonFile(path, true, true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                // common services
                services.AddSingleton<GlobalizationService>();
                services.AddSingleton<ConfigurationService>();

                // windows
                services.AddSingleton<MainWindow>();

                // view models
                services.AddSingleton<MainViewModel>();

                services.Configure<AppConfig>(o =>
                {
                    context.Configuration.Bind(o);
                });
            })
            .Build();

        protected override void OnStartup(StartupEventArgs e)
        {
            Host.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Host.StopAsync();
        }

        public static T GetService<T>()
        {
            return Host.Services.GetService(typeof(T)) is T service ? service : throw new InvalidOperationException("Cannot find service of specified type");
        }
    }
}
