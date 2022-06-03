using EasyTrad;

namespace EasySave.Views
{
    internal class SavingSaves
    {
        private readonly Dictionary<string, string> langue;
        /// <summary>
        /// View constructor
        /// </summary>
        /// <param name="trad">Dictionary for view traduction</param>
        public SavingSaves(Trad trad)
        {
            langue = trad.GetDico();
        }
        /// <summary>
        /// Display saves progress
        /// </summary>
        /// <param name="saveWorks"></param>
        public void Display(List<Models.SaveWork> saveWorks)
        {
            Console.WriteLine("=== " + langue["StartAll"] + " ===");
            foreach (var save in saveWorks.Select((value, index) => new { value, index }))
            {
                Console.WriteLine(save.value.GetName());
                using var progress = new ProgressBar();
                save.value.Copy((pourcent) => progress.Report((double)pourcent / 100));
                Console.WriteLine(langue["SaveFinish"]);
            }
            Console.WriteLine(langue["PluralSaveFinish"]);
        }
    }
}
