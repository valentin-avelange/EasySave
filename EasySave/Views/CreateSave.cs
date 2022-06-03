using EasySave.Models;
using EasyTrad;

namespace EasySave.Views
{
    internal class CreateSave
    {
        private readonly Dictionary<string, string> langue;
        /// <summary>
        /// View Constructor
        /// </summary>
        /// <param name="trad">Dictionary for view traduction</param>
        public CreateSave(Trad trad)
        {
            langue = trad.GetDico();
        }
        /// <summary>
        /// Display form for create new savework
        /// </summary>
        /// <returns>new SaveWork</returns>
        public SaveWork Display()
        {
            Console.WriteLine("=== " + langue["Create"] + " ===");

            Console.Write(langue["Name"] + " : ");
            string name = Console.ReadLine();

            Console.Write(langue["Source"]+ " : ");
            string sourcePath = Console.ReadLine();
            if (!Directory.Exists(sourcePath)) { 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(langue["PathExist"]);
                Console.ForegroundColor = ConsoleColor.Black;
                return Display();
            }

            Console.Write(langue["Target"] + " : ");
            string targetPath = Console.ReadLine();
            if (!Directory.Exists(targetPath)) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(langue["PathExist"]);
                Console.ForegroundColor = ConsoleColor.Black;
                return Display();
            }

            Console.Write(langue["TypeDiffSave"] + " : ");

            bool typeComplet = true;

            if (Console.ReadLine() == "yes")
            {
                typeComplet = false;
            }

            return new SaveWork(name, sourcePath, targetPath, typeComplet);
        }
    }
}
