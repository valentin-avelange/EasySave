using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MVVM.ViewModels.Context MainController;
        public MainWindow()
        {
            InitializeComponent();
            this.MainController = MVVM.ViewModels.Context.GetInstance();
            this.StartLoading();
            Thread t = new Thread(new ThreadStart(() => MVVM.ViewModels.AsynchronousSocketListener.StartListening()));
            t.Start();
        }

        private async void StartLoading()
        {
            for (int i = 0; i < 100; i++)
            {
                PB_Loading.Value++;
            }
            await Task.Delay(1000);
            MVVM.Views.ListSavesView newListSaves = new MVVM.Views.ListSavesView(this);
            this.Content = newListSaves;
        }
    }
}
