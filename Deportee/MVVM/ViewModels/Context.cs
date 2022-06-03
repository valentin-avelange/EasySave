using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MVVM.ViewModels
{
    internal sealed class Context
    {

        private string cryptoPath;

        private static Context _instance = new Context();

        private Context()
        {
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

        public string GetCryptoPath()
        {
            return this.cryptoPath;
        }

        public void SetCryptoPath(string newPath)
        {
            this.cryptoPath = newPath;
        }

    }
}
