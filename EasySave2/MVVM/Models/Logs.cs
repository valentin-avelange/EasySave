using System;

namespace EasySave2.MVVM.Models
{
    /// <summary>
    /// class abstraite des logs
    /// </summary>
    public abstract class Logs
    {
        /// <summary>
        /// Appellation du travail de sauvegarde
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Etat du travail de Sauvegarde (ex : Actif, Non Actif...)
        /// </summary>
        public SaveState State { get; set; }
        /// <summary>
        /// Horodatage
        /// </summary>
        public DateTime TimestampSave { get; set; }
    }
}