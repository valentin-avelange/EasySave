using EasyTrad;
using EasySave.Models;
using System.Runtime.InteropServices;

namespace EasySave.Views
{
    internal class Menu
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        private readonly Dictionary<string, string> langue;

        /// <summary>
        /// View constructor
        /// </summary>
        /// <param name="trad">Dictionary for view traduction</param>
        public Menu(Trad trad)
        {
            langue = trad.GetDico();
        }
        /// <summary>
        /// Display Title and menu
        /// </summary>
        public void Display()
        {
            const string title = @"
  ██████████   ██████████    ▄████████ ▄██   ███    ▄████████    ▄████████ ███    ███   ██████████ 
  ███    ███   ███    ███   ███    ███ ███   ███   ███    ███   ███    ███ ███    ███   ███    ███ 
  ███    ███   ███    ███   ███    █▀  ███▄▄▄███   ███    █▀    ███    ███ ███    ███   ███    ███ 
  ███▄▄▄       ███    ███   ███        ▀▀▀▀▀▀███   ███          ███    ███ ███    ███   ███▄▄▄     
  ███▀▀▀       ██████████   ██████████ ▄██   ███   ██████████   ██████████ ███    ███   ███▀▀▀     
  ███    ███   ███    ███          ███ ███   ███          ███   ███    ███ ███    ███   ███    ███ 
  ███    ███   ███    ███          ███ ███   ███          ███   ███    ███ ███    ███   ███    ███ 
  ██████████   ███    ███  ▄████████▀   ▀█████▀   ▄████████▀    ███    ███  ▀██████▀    ██████████

  _             ____                       __ _   
 | |__  _   _  |  _ \ _ __ ___  ___  ___  / _| |_ 
 | '_ \| | | | | |_) | '__/ _ \/ __|/ _ \| |_| __|
 | |_) | |_| | |  __/| | | (_) \__ | (_) |  _| |_ 
 |_.__/ \__, | |_|   |_|  \___/|___/\___/|_|  \__|
        |___/ ";
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, 3);
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(title);
            Console.WriteLine("========================================\\");
            foreach (int action in Enum.GetValues(typeof(MenuItems)))
            {
                Console.WriteLine(action + " -> " + langue[$"{Enum.GetName(typeof(MenuItems), action)}"]);
            }
            Console.WriteLine("========================================/");
        }
    }
}
