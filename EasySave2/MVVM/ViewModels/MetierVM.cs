using System.Collections.Generic;

namespace EasySave2.MVVM.ViewModels
{
    internal class MetierVM
    {
        private List<string> metierList;
        /// <summary>
        /// création de la liste métier
        /// </summary>
        public MetierVM()
        {
            this.metierList = new List<string>();
            this.metierList.Add("CalculatorApp");
        }
        /// <summary>
        /// ajout d'un metier a la siste
        /// </summary>
        /// <param name="metier"></param>
        public void AddMetier(string metier)
        {
            metierList.Add(metier);
        }
        /// <summary>
        /// renvoie la liste des metier
        /// </summary>
        /// <returns>metierList</returns>
        public List<string> GetMetier()
        {
            return metierList;
        }
        /// <summary>
        /// retire lélement de la siste a l'index indiqué
        /// </summary>
        /// <param name="index">index de l'element de la liste</param>
        public void RemoveMetier(int index)
        {
            this.metierList.RemoveAt(index);
        }
    }
}