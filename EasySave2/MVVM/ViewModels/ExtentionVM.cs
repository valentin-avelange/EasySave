using System.Collections.Generic;

namespace EasySave2.MVVM.ViewModels
{
    internal class ExtentionVM
    {
        /// <summary>
        /// liste des extention de fichier
        /// </summary>
        private List<string> extentionList;
        /// <summary>
        /// création des elément de la liste des extention de fichier
        /// </summary>
        public ExtentionVM()
        {
            this.extentionList = new List<string>();
            this.extentionList.Add(".txt");
            this.extentionList.Add(".pdf");
        }
        /// <summary>
        /// ajout d'une extention a la liste
        /// </summary>
        /// <param name="extention"></param>
        public void AddExtention(string extention)
        {
            extentionList.Add("." + extention);
        }
        /// <summary>
        /// renvoie la liste des extention
        /// </summary>
        /// <returns>extentionList</returns>
        public List<string> GetExtentions()
        {
            return extentionList;
        }
        /// <summary>
        /// retire en element de la liste a l'index indiqué 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveExtention(int index)
        {
            this.extentionList.RemoveAt(index);
        }
    }
}
