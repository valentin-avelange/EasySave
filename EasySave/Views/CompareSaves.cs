using EasySave.Models;

namespace EasySave.Views
{
    internal class CompareSaves
    {
        private readonly Dictionary<string, string> langue;

        /// <summary>
        /// View constructor
        /// </summary>
        /// <param name="trad">Dictionary for view traduction</param>
        public CompareSaves(EasyTrad.Trad trad)
        {
            langue = trad.GetDico();
        }

        /// <summary>
        /// Compare des travaux entre-eux
        /// </summary>
        /// <param name="saveWorks">Liste des travaux</param>
        public void Display(List<SaveWork> saveWorks)
        {
            Console.WriteLine("=== " + langue["Compare"] + " ===");
            foreach (var save in saveWorks.Select((value, index) => new { value, index }))
            {
                Console.WriteLine(save.index + " -> " + save.value.ToString());
            }
            int saveChoiced = Convert.ToInt32(Console.ReadLine());
            foreach (string line in saveWorks[saveChoiced].Compare())
            {
                Console.WriteLine("- " + line);
            }
        }
    }
}
