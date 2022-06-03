namespace EasySave.Views
{
    internal class Message
    {
        private readonly Dictionary<string, string> langue;
        /// <summary>
        /// View Constructor
        /// </summary>
        /// <param name="trad">Dictionary for view traduction</param>
        public Message(EasyTrad.Trad trad)
        {
            langue = trad.GetDico();
        }

        /// <summary>
        /// Display message in blue color
        /// </summary>
        /// <param name="message">Message ti display</param>
        /// <param name="trad">True if message is key of dictionary traduction</param>
        public void DisplayInfo(string message, bool trad)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            if (trad) { Console.WriteLine(langue[message]); }
            else { Console.WriteLine(message); }
            Console.ForegroundColor = ConsoleColor.Black;
        }
        /// <summary>
        /// Display message in Yellow color
        /// </summary>
        /// <param name="message">Message ti display</param>
        /// <param name="trad">True if message is key of dictionary traduction</param>
        public void DisplayWarning(string message, bool trad)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (trad) { Console.WriteLine(langue[message]); }
            else { Console.WriteLine(message); }
            Console.ForegroundColor = ConsoleColor.Black;
        }
        /// <summary>
        /// Display message in Red color
        /// </summary>
        /// <param name="message">Message ti display</param>
        /// <param name="trad">True if message is key of dictionary traduction</param>
        public void DisplayError(string message, bool trad)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (trad) { Console.WriteLine(langue[message]); }
            else { Console.WriteLine(message); }
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}
