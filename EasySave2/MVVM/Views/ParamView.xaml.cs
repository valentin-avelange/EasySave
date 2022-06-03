using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EasySave2.MVVM.Views
{
    /// <summary>
    /// Logique d'interaction pour ParamView.xaml
    /// </summary>
    public partial class ParamView : Page
    {
        private Window win;

        public ParamView(Window win)
        {
            InitializeComponent();
            this.win = win;
            DisplayContent();
        }

        private void SwapFR(object sender, RoutedEventArgs e)
        {
            SelectCulture("fr-FR");
            Update();
        }
        private void SwapEN(object sender, RoutedEventArgs e)
        {
            SelectCulture("en-US");
            Update();
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

        public void DisplayExtention()
        {
            var bc = new BrushConverter();
        }
        public void DisplayMetier()
        {
            var bci = new BrushConverter();
        }

        private void BackHome(object sender, RoutedEventArgs e)
        {
            Views.ListSavesView newListView = new Views.ListSavesView(this.win);
            this.win.Content = newListView;
        }

        public int GetIndexOfButton(string name)
        {
            string[] subs = name.Split('_');
            return int.Parse(subs[1]);
        }

        public void RemoveClick(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Name.ToString();
            MVVM.ViewModels.Context.GetInstance().GetExtensionVM().RemoveExtention(GetIndexOfButton(name));
            Update();
        }

        public void RemoveClickM(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Name.ToString();
            MVVM.ViewModels.Context.GetInstance().GetMetierVM().RemoveMetier(GetIndexOfButton(name));
            Update();
        }

        public void Update()
        {
            Views.ParamView newParamView = new Views.ParamView(this.win);
            this.win.Content = newParamView;
        }

        public void NewSizeFile(object sender, RoutedEventArgs e)
        {
            long octet = Convert.ToInt64(tailleFileMax.Text);
            MVVM.ViewModels.Context.GetInstance().SetSizeMaxFile(octet);
        }

        public void DisplayContent()
        {
            LogZone.Text = MVVM.ViewModels.Context.GetInstance().GetLoggerVM().GetLogPath();
            tailleFileMax.Text = MVVM.ViewModels.Context.GetInstance().GetSizeMaxFile().ToString();
            cryptoPath.Text = MVVM.ViewModels.Context.GetInstance().GetCryptoPath();

            var bc = new BrushConverter();
            List<string> listExtension = MVVM.ViewModels.Context.GetInstance().GetExtensionVM().GetExtentions();

            for (int i = 0; i < listExtension.Count; i++)
            {
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(45);
                TheGridExtension.RowDefinitions.Add(gridRow1);

                TextBlock NameText = new TextBlock();
                NameText.Text = listExtension[i].ToString();
                NameText.FontSize = 12;
                NameText.FontWeight = FontWeights.Bold;
                Grid.SetRow(NameText, i + 1);
                Grid.SetColumn(NameText, 0);

                Button BoutonRemove = new Button();
                BoutonRemove.Content = FindResource("Remove");
                BoutonRemove.Click += RemoveClick;
                BoutonRemove.Name = "btnRemove_" + i.ToString();
                BoutonRemove.Background = (Brush)bc.ConvertFrom("#fa5252");
                Grid.SetRow(BoutonRemove, i + 1);
                Grid.SetColumn(BoutonRemove, 1);


                // Add first row to Grid
                TheGridExtension.Children.Add(NameText);
                TheGridExtension.Children.Add(BoutonRemove);
            }

            var bci = new BrushConverter();
            List<string> metierList = MVVM.ViewModels.Context.GetInstance().GetMetierVM().GetMetier();

            for (int i = 0; i < metierList.Count; i++)
            {
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(45);
                TheGridMetier.RowDefinitions.Add(gridRow1);

                TextBlock NameText = new TextBlock();
                NameText.Text = metierList[i].ToString();
                NameText.FontSize = 12;
                NameText.FontWeight = FontWeights.Bold;
                Grid.SetRow(NameText, i + 1);
                Grid.SetColumn(NameText, 0);

                Button BoutonRemove = new Button();
                BoutonRemove.Content = FindResource("Remove");
                BoutonRemove.Click += RemoveClickM;
                BoutonRemove.Name = "btnRemoveM_" + i.ToString();
                BoutonRemove.Background = (Brush)bci.ConvertFrom("#fa5252");
                Grid.SetRow(BoutonRemove, i + 1);
                Grid.SetColumn(BoutonRemove, 1);


                // Add first row to Grid
                TheGridMetier.Children.Add(NameText);
                TheGridMetier.Children.Add(BoutonRemove);
            }
        }

        public void BrowseCryptosoft(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            openFileDlg.FilterIndex = 1;

            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {

                string filename = openFileDlg.FileName;
                cryptoPath.Text = filename;
            }
        }

        public void btnAdd(object sender, RoutedEventArgs e)
        {
            MVVM.ViewModels.Context.GetInstance().GetExtensionVM().AddExtention(rechreche.Text);
            Update();
        }
        public void BtnAddM(object sender, RoutedEventArgs e)
        {
            MVVM.ViewModels.Context.GetInstance().GetMetierVM().AddMetier(addTexetBox.Text);
            Update();
        }

        public void BtnParcourirLogs(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                string rep = openFileDlg.SelectedPath;
                LogZone.Text = rep;

            }
        }

        public void CryptoPath(object sender, RoutedEventArgs e)
        {
            MVVM.ViewModels.Context.GetInstance().SetCryptoPath(cryptoPath.Text);
        }

        public void BtnNewPath(object send, RoutedEventArgs e)
        {
            MVVM.ViewModels.Context.GetInstance().GetLoggerVM().SetLogPath(LogZone.Text);
            Views.ListSavesView newListView = new Views.ListSavesView(this.win);
            this.win.Content = newListView;
        }


    }
}
