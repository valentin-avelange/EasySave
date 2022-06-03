using System.Diagnostics;
using System.Windows;

namespace EasySave2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (IsProcessOpen())
            {
                Window window = new MainWindow();
                window.Show();

                base.OnStartup(e);
            }
            else
            {
                MessageBox.Show("alredy exist", "Alert", MessageBoxButton.OK);
                Application.Current.Shutdown();
            }
        }
        private bool IsProcessOpen()
        {
            int i = 0;
            string name = "EasySave2";
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    i++;
                }
            }
            return i < 2;
        }
    }
}
