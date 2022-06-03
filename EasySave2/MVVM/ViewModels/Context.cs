using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasySave2.MVVM.ViewModels
{
    internal sealed class Context
    {

        private MVVM.ViewModels.ListSavesVM SaveWorks;
        private MVVM.ViewModels.ExtentionVM Extentions;
        private MVVM.ViewModels.MetierVM Metiers;
        private MVVM.ViewModels.LoggerVM Log;
        private string cryptoPath;
        private long octetMaxFile;

        private static Context _instance = new Context();

        private Context()
        {
            this.octetMaxFile = 2000;
            this.SaveWorks = new ViewModels.ListSavesVM();
            this.Extentions = new ViewModels.ExtentionVM();
            this.Metiers = new ViewModels.MetierVM();
            this.Log = new ViewModels.LoggerVM(@"C:\Users\Public");
            this.cryptoPath = @"C:\Users\Admin\source\repos\EasySave\cryptsoft\bin\Debug\netcoreapp3.1\cryptsoft.exe";
            
        }

        /// <summary>
        ///     MainController Constructor
        /// </summary>
        public static Context GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Context();
            }
            return _instance;
        }

        public long GetSizeMaxFile()
        {
            return octetMaxFile;
        }

        public void SetSizeMaxFile(long newSize)
        {
            this.octetMaxFile = newSize;
        }

        public MVVM.ViewModels.ListSavesVM GetListSavesVM()
        {
            return this.SaveWorks;
        }

        public string GetCryptoPath()
        {
            return this.cryptoPath;
        }

        public void SetCryptoPath(string newPath)
        {
            this.cryptoPath = newPath;
        }

        public MVVM.ViewModels.ExtentionVM GetExtensionVM()
        {
            return this.Extentions;
        }
        public MVVM.ViewModels.MetierVM GetMetierVM()
        {
            return this.Metiers;
        }
        public MVVM.ViewModels.LoggerVM GetLoggerVM()
        {
            return this.Log;
        }
        public bool Metier()
        {
            //création de la site des logiciel métier
            List<string> metier = new List<string>();
            //implémentation de la site
            metier.Add("CalculatorApp");

            //verifi que le logiciel n'est pas run
            foreach (string metierItem in metier)
            {
                if (IsProcessOpen(metierItem))
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
