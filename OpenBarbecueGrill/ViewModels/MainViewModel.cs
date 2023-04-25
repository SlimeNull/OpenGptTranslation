using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenBarbecueGrill.Models;

namespace OpenBarbecueGrill.ViewModels
{
    [ObservableObject]
    public partial class MainViewModel
    {
        [ObservableProperty]
        private string _sourceText = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(CurrentParagraphText))]
        private string _destText = string.Empty;

        public string CurrentParagraphText
        {
            get 
            {
                string destText = DestText;
                int startIndex =
                    GetIndexOfLastParagraphText(destText);

                return destText
                    .Substring(startIndex)
                    .Trim('\n', '\r');
            }
            set
            {
                value = Regex.Replace(value, "\n+", "\n");

                string destText = DestText;
                int startIndex =
                    GetIndexOfLastParagraphText(destText);

                string newDestText = $"{destText.Substring(0, startIndex)}{value}";

                DestText = newDestText;
            }
        }



        [ObservableProperty]
        private bool _textLoading = false;

        [ObservableProperty]
        private bool _showConfigurationDialog = false;

        [ObservableProperty]
        private bool _showNounsMappingDialog = false;

        [ObservableProperty]
        private bool _enableAutoNounsExtract = false;


        [ObservableProperty]
        private ObservableCollection<NounMap> _nounsMapping
            = new ObservableCollection<NounMap>();


        [RelayCommand]
        public void AddNounMap()
        {
            NounsMapping.Add(
                new NounMap("名前", "名称"));
        }

        [RelayCommand]
        public void RemoveNounLastMap()
        {
            if (NounsMapping.Count > 0)
                NounsMapping.RemoveAt(0);
        }

        [RelayCommand]
        public void RemoveNounMap(NounMap map)
        {
            NounsMapping.Remove(map);
        }

        [RelayCommand]
        public void SubmitParagraph()
        {
            DestText += "\n\n";
        }

        [RelayCommand]
        public void RemoveParagraph()
        {
            string destText = DestText;
            int startIndex =
                    GetIndexOfLastParagraphText(destText);

            string newDestText = destText
                .Substring(0, startIndex)
                .Trim('\n', '\r');

            DestText = newDestText;
        }



        public int GetIndexOfLastParagraphText(string text)
        {
            int index = -1;

            index = text.LastIndexOf("\n\n");
            if (index >= 0)
                return index + 2;

            index = text.LastIndexOf("\r\n\r\n");
            if (index >= 0)
                return index + 4;

            return 0;
        }
    }
}

