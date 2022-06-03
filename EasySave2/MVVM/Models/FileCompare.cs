using System.Collections.Generic;
using System.IO;

namespace EasySave2.MVVM.Models
{
    /// <summary>
    /// comparaison des fichier
    /// </summary>
    internal class FileCompare : IEqualityComparer<FileInfo>
    {
        /// <summary>
        /// vérification d'égalité des fichier
        /// </summary>
        /// <param name="f1">information du premier fichier</param>
        /// <param name="f2">information du deuxième fichier</param>
        /// <returns>bool if name et taile</returns>
        public bool Equals(FileInfo f1, FileInfo f2)
        {
            return f1.Name == f2.Name &&
                    f1.Length == f2.Length;
        }
        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="fi">The System.Object for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        public int GetHashCode(FileInfo fi)
        {
            string s = $"{fi.Name}{fi.Length}";
            return s.GetHashCode();
        }
    }
}

