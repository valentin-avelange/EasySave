namespace EasySave.Views
{
    internal class SavingSave
    {
        private readonly Dictionary<string, string> langue;

        /// <summary>
        /// View constructor
        /// </summary>
        /// <param name="trad">Dictionary for view traduction</param>
        public SavingSave(EasyTrad.Trad trad)
        {
            langue = trad.GetDico();
        }
        /// <summary>
        /// Display save progress
        /// </summary>
        /// <param name="saveWorks"></param>
        public void Display(List<Models.SaveWork> saveWorks)
        {
            Console.WriteLine("=== " + langue["StartOnce"] + " ===");
            if (saveWorks.Count > 0)
            {
                foreach (var save in saveWorks.Select((value, index) => new { value, index }))
                {
                    Console.WriteLine(save.index + " -> " + save.value.ToString());
                }
                Console.WriteLine(saveWorks.Count + " -> " + langue["Cancel"]);

                int saveChoiced = Convert.ToInt32(Console.ReadLine());

                if (saveChoiced < 0 || saveChoiced > saveWorks.Count + 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(langue["Invalid"]);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadKey();
                    Display(saveWorks);
                }
                else if (saveChoiced == saveWorks.Count)
                {
                    Console.WriteLine(langue["SaveCanceled"]);
                    //break
                }
                else
                {
                    try
                    {
                        Console.WriteLine(saveWorks[saveChoiced].GetName());
                        using var progress = new ProgressBar();
                        saveWorks[saveChoiced].Copy((pourcent) => progress.Report((double)pourcent / 100));
                        Console.WriteLine(langue["SaveFinish"]);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                }

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(langue["NothingList"]);
                Console.ForegroundColor = ConsoleColor.Black;
            }
        }
    }
}
