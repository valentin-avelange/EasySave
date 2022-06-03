using System;
using System.Windows;
using System.Windows.Controls;

namespace EasySave2.MVVM.Views
{
    /// <summary>
    /// Logique d'interaction pour AddSaveView.xaml
    /// </summary>
    public partial class AddSaveView : Page
    {
        private Window win;
        private string NameSave;
        private string SourceSave;
        private string TargetSave;
        private MVVM.Models.EnumTypeSaveWork TypeSave;
        public AddSaveView(Window window)
        {
            win = window;
            this.NameSave = "Ma nouvelle Sauvegarde";
            this.SourceSave = @"C:\";
            this.TargetSave = @"C:\";
            this.TypeSave = Models.EnumTypeSaveWork.Complet;
            InitializeComponent();
        }

        private void swapFR(object sender, RoutedEventArgs e)
        {
            SelectCulture("fr-FR");
        }
        private void swapEN(object sender, RoutedEventArgs e)
        {
            SelectCulture("en-US");
        }
        public static void SelectCulture(string culture = null)
        {
            ResourceDictionary dict = new ResourceDictionary
            {
                Source = new Uri(@"Resources\" + (string.IsNullOrWhiteSpace(culture) ? string.Empty : culture) + ".xaml", UriKind.Relative)
            };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }


        public void update()
        {
            bool verifName = this.NameSave != "";
            bool verifSource = this.SourceSave != "";
            bool veriftarget = this.TargetSave != "";

            if (BtnSend != null)
            {
                if (verifName && verifSource && veriftarget)
                {
                    BtnSend.IsEnabled = true;
                }
                else
                {
                    BtnSend.IsEnabled = false;
                }
            }
        }

        private void CreateSave(object sender, RoutedEventArgs e)
        {
            update();
            MVVM.ViewModels.Context.GetInstance().GetListSavesVM().AddSaveWork(new Models.SaveWork(this.NameSave, this.SourceSave, this.TargetSave, this.TypeSave));
            backHome();
        }

        private void btnBack(object sender, RoutedEventArgs e)
        {
            backHome();
        }

        private void backHome()
        {
            Views.ListSavesView newListView = new Views.ListSavesView(this.win);
            this.win.Content = newListView;
        }

        public void BtnParcourirTarget(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                string rep = openFileDlg.SelectedPath;
                NewTarget.Text = rep;
                this.TargetSave = rep;
            }
        }
        public void BtnParcourirSource(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                string rep = openFileDlg.SelectedPath;
                NewSource.Text = rep;
                this.SourceSave = rep;

            }
        }

        public void NameChanged(object sender, RoutedEventArgs e)
        {
            this.NameSave = (sender as TextBox).Text;
            update();
        }
        public void SourceChanged(object sender, RoutedEventArgs e)
        {
            this.SourceSave = (sender as TextBox).Text;
            update();
        }
        public void TargetChanged(object sender, RoutedEventArgs e)
        {
            this.TargetSave = (sender as TextBox).Text;
            update();
        }
        public void CompletChecked(object sender, RoutedEventArgs e)
        {
            this.TypeSave = Models.EnumTypeSaveWork.Complet;
            update();
        }
        public void DiffChecked(object sender, RoutedEventArgs e)
        {
            this.TypeSave = Models.EnumTypeSaveWork.Diff;
            update();
        }

        private void SourceChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
