using EasyTrad;
using System.Text.RegularExpressions;

namespace EasySave.Views
{
    internal class ConfirmContinue
    {
        private readonly Dictionary<string, string> langue;
        /// <summary>
        /// View constructor
        /// </summary>
        /// <param name="trad">Dictionary for view traduction</param>
        public ConfirmContinue(Trad trad)
        {
            langue = trad.GetDico();
        }
        /// <summary>
        /// Display form for confirm
        /// </summary>
        /// <returns></returns>
        public bool Display()
        {
            Console.WriteLine(langue["ChoiceListContinue"]);
            var inputErr = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(inputErr) || (!string.IsNullOrWhiteSpace(inputErr) && !string.Equals(Regex.Replace(inputErr, @"\s", ""), "y", StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine(langue["Closing"]);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
