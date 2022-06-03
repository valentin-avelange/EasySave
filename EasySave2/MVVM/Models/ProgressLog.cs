namespace EasySave2.MVVM.Models
{
    /// <summary>
    /// Progression du travail de sauvegarde
    /// </summary>
    public class ProgressLog
    {
        /// <summary>
        /// Contructeur des information de progrsseion de travail de sauvegardes
        /// </summary>
        /// <param name="nbFileLeft">Nombre de fichiers restants</param>
        /// <param name="ttFileSizeLeft">Taille des fichiers restants</param>
        /// <param name="currentSourceFilePath">Adresse complète du fichier Source en cours de sauvegarde</param>
        /// <param name="currentDestinationFilePath">Adresse complète du fichier de destination</param>
        public ProgressLog(long nbFileLeft, long ttFileSizeLeft, string currentSourceFilePath, string currentDestinationFilePath)
        {
            NbFileLeft = nbFileLeft;
            TtFilesSizeLeft = ttFileSizeLeft;
            CurrentSourceFilePath = currentSourceFilePath;
            CurrentDestinationFilePath = currentDestinationFilePath;
        }
        /// <summary>
        /// Nombre de fichiers restants
        /// </summary>
        public long NbFileLeft { get; set; }
        /// <summary>
        /// Taille des fichiers restants
        /// </summary>
        public long TtFilesSizeLeft { get; set; }
        /// <summary>
        /// Adresse complète du fichier Source en cours de sauvegarde
        /// </summary>
        public string CurrentSourceFilePath { get; set; }
        /// <summary>
        /// Adresse complète du fichier de destination
        /// </summary>
        public string CurrentDestinationFilePath { get; set; }
    }
}
