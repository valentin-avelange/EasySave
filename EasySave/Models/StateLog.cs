namespace EasySave.Models
{
    /// <summary>
    /// Log de changement d'état
    /// </summary>
    public class StateLog : Logs
    {
        /// <summary>
        /// Contructeur de loggeur d'état sans argument
        /// </summary>
        public StateLog()
        { }

        /// <summary>
        /// Contructeur de loggeur d'état
        /// </summary>
        /// <param name="name">Appellation du travail de sauvegarde</param>
        /// <param name="state">Etat du travail de Sauvegarde</param>
        /// <param name="timestampSave">Etat du travail de Sauvegarde</param>
        /// <param name="totalFilesToCopy"></param>
        /// <param name="totalFilesSize"></param>
        /// <param name="progression">Progression</param>
        public StateLog(string name, SaveState state, DateTime timestampSave, long totalFilesToCopy, long totalFilesSize, long pnbFileLeft, long pttFileSizeLeft, string pcurrentSourceFilePath, string pcurrentDestinationFilePath)
        {
            Name = name;
            State = state;
            TimestampSave = timestampSave;
            TotalFilesToCopy = totalFilesToCopy;
            TotalFilesSize = totalFilesSize;
            PnbFileLeft = pnbFileLeft;
            PttFileSizeLeft = pttFileSizeLeft;
            PcurrentSourceFilePath = pcurrentSourceFilePath;
            PcurrentDestinationFilePath = pcurrentDestinationFilePath;
        }
        /// <summary>
        /// Le nombre total de fichiers éligibles
        /// </summary>
        public long TotalFilesToCopy { get; set; }
        /// <summary>
        /// La taille des fichiers à transférer
        /// </summary>
        public long TotalFilesSize { get; set; }
        /// <summary>
        /// Progression si null => Travail non actif
        /// nombre de Fichier restant
        /// </summary>
        public long PnbFileLeft { get; set; }
        /// <summary>
        /// Progression si null => Travail non actif
        /// taille des fichier restan
        /// </summary>
        public long PttFileSizeLeft { get; set; }
        /// <summary>
        /// Progression si null => Travail non actif
        /// lien de fichier source en cour de copie
        /// </summary>
        public string PcurrentSourceFilePath { get; set; }
        /// <summary>
        /// Progression si null => Travail non actif
        /// lien de fichier de destination en cour de copie
        /// </summary>
        public string PcurrentDestinationFilePath { get; set; }
    }
}
