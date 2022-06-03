namespace EasySave.Views
{
    internal class ListSaves
    {
        private readonly Dictionary<string, string> langue;

        /// <summary>
        /// View constructor
        /// </summary>
        /// <param name="trad">Dictionary for view traduction</param>
        public ListSaves(EasyTrad.Trad trad)
        {
            langue = trad.GetDico();
        }

        /// <summary>
        /// Liste tous les travaux existant
        /// </summary>
        /// <param name="saveWorks">Liste des travaux</param>
        public void Display(List<Models.SaveWork> saveWorks)
        {
            Console.WriteLine("=== " + langue["List"] + " ===");
            if (saveWorks.Count > 0)
            {
                foreach (Models.SaveWork saveWork in saveWorks)
                {
                    Console.WriteLine(saveWork.ToString());
                }
            }
            else if (saveWorks.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(langue["NothingList"]);
                Console.ForegroundColor = ConsoleColor.Black;
            }
        }
    }
}
