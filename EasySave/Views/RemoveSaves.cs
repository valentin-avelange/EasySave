using EasySave.Models;

namespace EasySave.Views
{
    internal class RemoveSaves
    {
        private readonly Dictionary<string, string> langue;

        public RemoveSaves(EasyTrad.Trad trad)
        {
            langue = trad.GetDico();
        }

        /// <summary>
        /// supprimer un travail de sauvegarde de la liste
        /// </summary>
        /// <param name="saveWorks">Liste des travaux</param>
        public int Display(List<SaveWork> saveWorks)
        {
            foreach (var save in saveWorks.Select((value, index) => new { value, index }))
            {
                Console.WriteLine(save.index + " -> " + save.value.ToString());
            }
            Console.WriteLine(saveWorks.Count + " -> " + langue["Cancel"]);

            try
            {
                int saveChoiced = Convert.ToInt32(Console.ReadLine());
                if (saveWorks.Count == saveChoiced)
                {
                    Console.WriteLine(langue["Canceled"]);
                    return -1;
                }
                else if (saveChoiced < 0 || saveChoiced > saveWorks.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(langue["Invalid"]);
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.ReadKey();
                    return Display(saveWorks);
                }
               
                return saveChoiced;
            }
            catch (Exception ex)
            {
                return Display(saveWorks);
            }
        }
    }
}


