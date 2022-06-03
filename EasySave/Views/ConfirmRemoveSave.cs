using EasyTrad;

namespace EasySave.Views
{
    internal class ConfirmRemoveSave
    {
        private readonly Dictionary<string, string> langue;
        /// <summary>
        /// View constructor
        /// </summary>
        /// <param name="trad">Dictionary for view traduction</param>
        public ConfirmRemoveSave(Trad trad)
        {
            langue = trad.GetDico();
        }
        /// <summary>
        /// Display confirm message for remove
        /// </summary>
        /// <returns>True for remove</returns>
        public bool Display()
        {
            Console.WriteLine(langue["WantSupp"]);
            if (Console.ReadLine() == "yes")
            {
                Console.WriteLine(langue["Removed"]);
                return true;
            }
            else
            {
                Console.WriteLine(langue["Canceled"]);
                return false;
            }
        }
    }
}
