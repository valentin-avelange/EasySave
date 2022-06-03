using EasySave2.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EasySave2.MVVM.Views
{
    /// <summary>
    /// Logique d'interaction pour ListSavesView.xaml
    /// </summary>
    public partial class ListSavesView : Page
    {
        private Window win;

        private string search;
        delegate void DELG(object o);

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

        public ListSavesView(Window window)
        {
            this.search = "";
            this.win = window;
            InitializeComponent();
            DisplayContent();
        }

        public ListSavesView(Window window, string filter)
        {
            this.search = filter;
            this.win = window;
            InitializeComponent();
            DisplayContent();
        }

        public void SearchButton(object sender, RoutedEventArgs e)
        {
            Views.ListSavesView newAddView = new Views.ListSavesView(this.win, zonerecherche.Text);
            this.win.Content = newAddView;

        }

        public int getIndexOfButton(string name)
        {
            string[] subs = name.Split('_');
            return int.Parse(subs[1]);
        }

        public void EditClick(object sender, RoutedEventArgs e)
        {
            //TODO: add redirection
        }

        public void Update()
        {
            Views.ListSavesView newAddView = new Views.ListSavesView(this.win, this.search);
            this.win.Content = newAddView;
        }

        public void DisplayContent()
        {
            var bc = new BrushConverter();
            List<Models.SaveWork> availableSaves = new List<Models.SaveWork>();
            Dictionary<Models.SaveWork, bool> listSaves = MVVM.ViewModels.Context.GetInstance().GetListSavesVM().GetSaveWorks();
            foreach (var s in listSaves)
            {
                if (s.Key != null)
                {
                    if (this.search == "" || s.Key.GetName().Contains(this.search) || s.Key.GetSourcePath().Contains(this.search) || s.Key.GetTargetPath().Contains(this.search))
                    {
                        availableSaves.Add(s.Key);
                    }
                }
            }

            for (int i = 0; i < availableSaves.Count; i++)
            {
                SaveWork sw = availableSaves[i];

                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(45);
                TheGrid.RowDefinitions.Add(gridRow1);

                TextBlock NumText = new TextBlock();
                NumText.Text = i.ToString();
                NumText.FontSize = 12;
                NumText.FontWeight = FontWeights.Bold;
                Grid.SetRow(NumText, i + 1);
                Grid.SetColumn(NumText, 0);

                TextBlock NameText = new TextBlock();
                NameText.Text = sw.GetName();
                NameText.FontSize = 12;
                NameText.FontWeight = FontWeights.Bold;
                Grid.SetRow(NameText, i + 1);
                Grid.SetColumn(NameText, 1);

                TextBlock SourcePathText = new TextBlock();
                SourcePathText.Text = sw.GetSourcePath();
                SourcePathText.FontSize = 12;
                Grid.SetRow(SourcePathText, i + 1);
                Grid.SetColumn(SourcePathText, 2);

                TextBlock TargetPathText = new TextBlock();
                TargetPathText.Text = sw.GetTargetPath();
                TargetPathText.FontSize = 12;
                Grid.SetRow(TargetPathText, i + 1);
                Grid.SetColumn(TargetPathText, 3);

                TextBlock TypeText = new TextBlock();
                TypeText.Text = sw.GetTypePath().ToString();
                TypeText.FontSize = 12;
                Grid.SetRow(TypeText, i + 1);
                Grid.SetColumn(TypeText, 4);

                Button BoutonRemove = new Button();
                BoutonRemove.Content = "Remove";
                BoutonRemove.Click += (s, e) =>
                {
                    MVVM.ViewModels.Context.GetInstance().GetListSavesVM().RemoveSaveWork(sw);
                    Update();
                };
                BoutonRemove.Name = "btnRemove_" + i.ToString();
                BoutonRemove.Background = (Brush)bc.ConvertFrom("#fa5252");
                Grid.SetRow(BoutonRemove, i + 1);
                Grid.SetColumn(BoutonRemove, 5);

                Button BoutonEdit = new Button();
                BoutonEdit.Content = FindResource("Edit");
                BoutonEdit.Name = "btnEdit_" + i.ToString();
                BoutonEdit.Click += (s, e) =>
                {
                    Views.EditSave newEditView = new Views.EditSave(this.win, sw);
                    this.win.Content = newEditView;
                };
                BoutonEdit.Background = (Brush)bc.ConvertFrom("#228be6");
                Grid.SetRow(BoutonEdit, i + 1);
                Grid.SetColumn(BoutonEdit, 6);

                Button BoutonStart = new Button();
                BoutonStart.Content = "Start";
                BoutonStart.Click += (_, _) =>
                {

                    if (MVVM.ViewModels.Context.GetInstance().GetListSavesVM().GetSaveWorks()[sw] == false)
                    {
                        if (MVVM.ViewModels.Context.GetInstance().Metier())
                        {
                            MVVM.ViewModels.Context.GetInstance().GetListSavesVM().GetSaveWorks()[sw] = true;
                            MVVM.ViewModels.Context.GetInstance().GetListSavesVM().StartSaveWork(sw);
                            BoutonRemove.IsEnabled = false;
                            BoutonEdit.IsEnabled = false;
                            BoutonStart.Content = "Pause";
                        }
                        else
                        {
                            MessageBox.Show("Error: business software is open", "Alert", MessageBoxButton.OK);
                            MVVM.ViewModels.Context.GetInstance().GetListSavesVM().GetSaveWorks()[sw] = false;
                        }

                    }
                    else
                    {
                        MVVM.ViewModels.Context.GetInstance().GetListSavesVM().PauseSave(sw);
                        BoutonStart.Content = "Start";
                    }
                };
                BoutonStart.Name = "btnStart_" + i.ToString();
                BoutonStart.Background = (Brush)bc.ConvertFrom("#82c91e");
                Grid.SetRow(BoutonStart, i + 1);
                Grid.SetColumn(BoutonStart, 7);

                ProgressBar Progress = new ProgressBar();
                //Trace.WriteLine(i.ToString());
                Progress.Name = "progress_" + i.ToString();
                Progress.Value = 0;
                Grid.SetRow(Progress, i + 1);
                Grid.SetColumn(Progress, 8);
                sw.ProgressChanged += (progress) =>
                {

                    Progress.Dispatcher.Invoke(() => { Progress.Value = progress; });
                    Progress.Dispatcher.Invoke(() =>
                    {
                        if (progress == 100)
                        {
                            BoutonRemove.IsEnabled = true;
                            BoutonEdit.IsEnabled = true;
                            BoutonStart.Content = "Start";
                            MVVM.ViewModels.Context.GetInstance().GetListSavesVM().GetSaveWorks()[sw] = false;
                        }
                    });
                    if (!MVVM.ViewModels.Context.GetInstance().Metier())
                    {
                        MessageBox.Show("Error: business software is open", "Alert", MessageBoxButton.OK);
                        MVVM.ViewModels.Context.GetInstance().GetListSavesVM().GetSaveWorks()[sw] = false;
                    }
                };


                // Add first row to Grid
                TheGrid.Children.Add(NumText);
                TheGrid.Children.Add(NameText);
                TheGrid.Children.Add(SourcePathText);
                TheGrid.Children.Add(TargetPathText);
                TheGrid.Children.Add(TypeText);
                TheGrid.Children.Add(BoutonRemove);
                TheGrid.Children.Add(BoutonEdit);
                TheGrid.Children.Add(BoutonStart);
                TheGrid.Children.Add(Progress);
            }
        }

        private void BtnClickParam(object sender, RoutedEventArgs e)
        {
            Views.ParamView newParamView = new Views.ParamView(this.win);
            this.win.Content = newParamView;
        }

        private void BtnClickRemoveAll(object sender, RoutedEventArgs e)
        {
            MVVM.ViewModels.Context.GetInstance().GetListSavesVM().RemoveAllSaves();
            Update();
        }

        private void BtnClickStartAll(object sender, RoutedEventArgs e)
        {
            foreach (var tb in FindVisualChildren<Button>(this.win))
            {
                if (tb.Name.Contains("btnStart_"))
                {
                    tb.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
            //StartAll.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            //MVVM.ViewModels.Context.GetInstance().GetListSavesVM().StartAllSaveWork();
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                if (child != null && child is T)
                    yield return (T)child;

                foreach (T childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }

        public static class ResourceDictionaryExtensions
        {
            private static readonly Dictionary<ResourceDictionary, string> _mapping = new Dictionary<ResourceDictionary, string>();
            public static void SetName(ResourceDictionary element, string value)
            {
                _mapping[element] = value;
            }

            public static string GetName(ResourceDictionary element)
            {
                if (!_mapping.ContainsKey(element))
                    return null;
                return _mapping[element];
            }
        }

        private void BtnClickAdd(object sender, RoutedEventArgs e)
        {
            Views.AddSaveView newAddView = new Views.AddSaveView(this.win);
            this.win.Content = newAddView;
        }
    }
}
