using System.Diagnostics;
using System.IO;

namespace OpenBarbecueGrill.Utilities
{
    public class GlobalValues
    {
        public static string AppName => nameof(OpenBarbecueGrill);

        public static string JsonConfigurationFilePath { get; } =
            Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) ?? "./", "AppConfig.json");
    }
}
