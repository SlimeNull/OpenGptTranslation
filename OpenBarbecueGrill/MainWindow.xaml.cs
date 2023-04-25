using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Data;
using Microsoft.Win32;
using OpenAI;
using OpenBarbecueGrill.Models;
using OpenBarbecueGrill.Services;
using OpenBarbecueGrill.Utilities;
using OpenBarbecueGrill.ViewModels;
using OpenBarbecueGrill.Views;
using OpenGptTranslation;
using HCC = HandyControl.Controls;

namespace OpenBarbecueGrill
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(
            MainViewModel viewModel,
            ConfigurationService configurationService)
        {
            ViewModel = viewModel;
            ConfigurationService = configurationService;


            DataContext = this;
            InitializeComponent();

        }

        private readonly Dictionary<string, string> nouns =
            new Dictionary<string, string>();

        //TranlationEngine engine = new TranlationEngine();

        public MainViewModel ViewModel { get; }
        public ConfigurationService ConfigurationService { get; }

        [RelayCommand]
        public async Task TranslateCurrent()
        {
            string apikey =
                ConfigurationService.Configuration.ApiKey;

            if (string.IsNullOrWhiteSpace(apikey))
            {
                HCC.Growl.Error(
                    new GrowlInfo()
                    {
                        Message = "Empty API Key"
                    });

                return;
            }


            string sourceText =
                ViewModel.SourceText.NormalizeLineEnding();
            string destText=
                ViewModel.DestText.NormalizeLineEnding();

            int before = destText.Count("\n\n");
            int index = sourceText.IndexOfSkip("\n\n", before);
            if (index < 0)
                goto End;

            int end = sourceText.IndexOf("\n\n", index);

            if (end < 0)
                end = sourceText.Length;

            string language =
                ConfigurationService.Configuration.TranslationLanguage;

            string sourceParagraph = ViewModel.SourceText.Substring(index, end);

            if (string.IsNullOrWhiteSpace(sourceParagraph))
            {
                HCC.Growl.Error(
                    new GrowlInfo()
                    {
                        Message = "No more paragraph"
                    });

                return;
            }

            try
            {
                ViewModel.TextLoading = true;

                OpenAIClient client = new OpenAIClient(
                new OpenAIAuthentication(apikey),
                new OpenAIClientSettings("openaiapi.elecho.org"));

                TranslationEngine engine = new TranslationEngine(client);
                engine.Nouns.Populate(nouns);
                await engine.StreamNext(language, sourceParagraph, translation =>
                {
                    ViewModel.CurrentParagraphText = translation;
                });

                nouns.Populate(engine.Nouns);
            }
            catch (Exception ex)
            {
                HCC.Growl.Error(
                    new GrowlInfo()
                    {
                        Message = ex.Message,
                    });
            }

            End:
            ViewModel.TextLoading = false;
        }


        [RelayCommand]
        public async Task SubmitAndGetNextTranslation()
        {
            ViewModel.SubmitParagraph();
            await TranslateCurrent();
        }

        [RelayCommand]
        public void ShowConfigurationDialog()
            => ViewModel.ShowConfigurationDialog = true;

        [RelayCommand]
        public void CloseConfigurationDialog() =>
            ViewModel.ShowConfigurationDialog = false;

        [RelayCommand]
        public void ShowNounsMappingDialog()
        {
            LoadNounsMapping();
            ViewModel.ShowNounsMappingDialog = true;
        }

        [RelayCommand]
        public void CloseNounsMappingDialog()
        {
            AcceptNounsMapping();
            ViewModel.ShowNounsMappingDialog = false;
        }

        [RelayCommand]
        public void CloseNounsMappingDialogWithCancellation()
        {
            ViewModel.ShowNounsMappingDialog = false;
        }

        [RelayCommand]
        public void LoadNounsMapping()
        {
            ViewModel.NounsMapping.Clear();
            foreach (var kv in nouns)
                ViewModel.NounsMapping.Add(new NounMap(kv.Key, kv.Value));
        }

        [RelayCommand]
        public void AcceptNounsMapping()
        {
            nouns.Clear();
            foreach (var map in ViewModel.NounsMapping)
                nouns[map.Origin] = map.Dest;
        }




        private readonly OpenFileDialog openFileDialogForNounsMapping =
            new OpenFileDialog()
            {
                Title = "打开文件以导入名词",
                Filter = "文本文件|*.txt|任意|*.*",
                CheckFileExists = true,
            };

        private readonly SaveFileDialog saveFileDialogForNounsMapping =
            new SaveFileDialog()
            {
                Title = "打开文件以导入名词",
                Filter = "文本文件|*.txt|任意|*.*",
            };


        [RelayCommand]
        public void ImportNounsMapping()
        {
            if (openFileDialogForNounsMapping.ShowDialog() ?? false)
            {
                using TextReader reader = 
                    File.OpenText(openFileDialogForNounsMapping.FileName);

                while (true)
                {
                    string? line =
                        reader.ReadLine();

                    if (line == null)
                        break;

                    int index = line.IndexOf(':');
                    string key = line.Substring(0, index).Trim();
                    string value = line.Substring(index + 1).Trim();

                    nouns[key] = value;
                }
            }
        }

        [RelayCommand]
        public void ExportNounsMapping()
        {
            if (saveFileDialogForNounsMapping.ShowDialog() ?? false)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var kv in nouns)
                    sb.AppendLine($"{kv.Key}:{kv.Value}");

                File.WriteAllText(saveFileDialogForNounsMapping.FileName, sb.ToString());
            }
        }

        private void RemoveParapraphIfCurrentEmpty(object sender, KeyEventArgs e)
        {
            if (sender is not HCC.TextBox tb)
                return;

            if (e.Key != Key.Back)
                return;

            if (string.IsNullOrEmpty(tb.Text))
            {
                ViewModel.RemoveParagraph();
                tb.CaretIndex = tb.Text.Length;
                e.Handled = true;
            }
        }
    }
}
