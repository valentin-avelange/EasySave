using System;

namespace EasySave2.MVVM.Models
{
    /// <summary>
    /// Log journalie
    /// </summary>
    public class DayLog : Logs
    {
        /// <summary>
        /// Contructeur de loggeur jouraliers sans argument
        /// </summary>
        public DayLog()
        { }

        /// <summary>
        /// Constructeur des logs jouraliers
        /// </summary>
        /// <param name="name">horodatage</param>
        /// <param name="state">Etat du travail de Sauvegarde</param>
        /// <param name="timestampSave">horodatage</param>
        /// <param name="saveType">Type de savegarde : Complete ou differentielle</param>
        /// <param name="sourcePathFile">Adresse complète du fichier Source</param>
        /// <param name="targetPathFile">Adresse complète du fichier de destination</param>
        /// <param name="currentFileSize">Taille du fichier</param>
        /// <param name="durationFileTransfer">Temps de transfert du fichier en ms</param>
        public DayLog(string name, SaveState state, DateTime timestampSave, bool saveType, string sourcePathFile, string targetPathFile, long currentFileSize, long durationFileTransfer, long durationEncryptFile)
        {
            Name = name;
            State = state;
            TimestampSave = timestampSave;
            SaveType = saveType;
            SourcePathFile = sourcePathFile;
            TargetPathFile = targetPathFile;
            CurrentFileSize = currentFileSize;
            DurationFileTransfer = durationFileTransfer;
            DurationEncryptFile = durationEncryptFile;
        }

        /// <summary>
        /// Type de savegarde : Complete ou differentielle
        /// </summary>
        public bool SaveType { get; set; }

        /// <summary>
        /// Adresse complète du fichier Source
        /// </summary>
        public string SourcePathFile { get; set; }

        /// <summary>
        /// Adresse complète du fichier de destination
        /// </summary>
        public string TargetPathFile { get; set; }

        /// <summary>
        /// Taille du fichier
        /// </summary>
        public long CurrentFileSize { get; set; }

        /// <summary>
        /// Temps de transfert du fichier en ms
        /// </summary>
        public long DurationFileTransfer { get; set; }

        /// <summary>
        /// Temps de transfert du fichier en ms
        /// </summary>
        public long DurationEncryptFile { get; set; }
    }
}
