using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Hosting;

namespace OpenBarbecueGrill.Services
{
    class ApplicationHostService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (Application.Current.Windows.OfType<MainWindow>().FirstOrDefault() is not MainWindow mainWindow)
                mainWindow = App.GetService<MainWindow>();

            mainWindow.Show();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
