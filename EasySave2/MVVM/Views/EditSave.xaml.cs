using EasySave2.MVVM.Models;
using System.Windows;
using System.Windows.Controls;

namespace EasySave2.MVVM.Views
{
    /// <summary>
    /// Logique d'interaction pour EditSave.xaml
    /// </summary>
    public partial class EditSave : Page
    {

        private Window win;
        private Models.SaveWork SaveWork;
        public EditSave(Window window, SaveWork saveWork)
        {
            this.win = window;
            this.SaveWork = saveWork;
            InitializeComponent();
            update();
        }

        public void update()
        {
            ListFiles.Items.Clear();
            foreach (var val in this.SaveWork.GetAllFiles())
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Content = val;
                if (this.SaveWork.GetPrioritFiles().Contains(val))
                {
                    checkBox.IsChecked = true;
                }
                checkBox.Click += (_, _) =>
                {
                    if ((bool)checkBox.IsChecked)
                    {
                        SaveWork.AddPrioritFile(val);
                        update();
                    }
                    else
                    {
                        SaveWork.RemovePrioritFile(val);
                        update();
                    }
                };
                ListFiles.Items.Add(checkBox);
            }

        }

        private void backHome(object sender, RoutedEventArgs e)
        {
            Views.ListSavesView newListView = new Views.ListSavesView(this.win);
            this.win.Content = newListView;
        }
    }
}
