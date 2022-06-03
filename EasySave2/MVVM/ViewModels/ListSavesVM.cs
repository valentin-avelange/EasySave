using EasySave2.MVVM.Models;
using System.Collections.Generic;
using System.Threading;

namespace EasySave2.MVVM.ViewModels
{
    internal class ListSavesVM
    {
        //private List<Models.SaveWork> SaveWorksList;

        private Dictionary<Models.SaveWork, bool> SaveWorksList;
        /// <summary>
        /// incrémente les travaux de sauvgarde
        /// </summary>
        public ListSavesVM()
        {
            this.SaveWorksList = new Dictionary<Models.SaveWork, bool>();
            this.SaveWorksList.Add(new Models.SaveWork("save", @"C:\source", @"D:\cible", Models.EnumTypeSaveWork.Complet), false);

        }
        /// <summary>
        /// Envoie la liste des travaux de sauvgarde
        /// </summary>
        /// <returns>SaveWorksList</returns>
        public Dictionary<Models.SaveWork, bool> GetSaveWorks()
        {
            return this.SaveWorksList;
        }
        /// <summary>
        /// ajoute un travail de sauvgarde a la liste
        /// </summary>
        /// <param name="newSaveWork"></param>
        public void AddSaveWork(Models.SaveWork newSaveWork)
        {
            this.SaveWorksList.Add(newSaveWork, false);
        }
        /// <summary>
        /// retire un travail de sauvgade de la liste
        /// </summary>
        /// <param name="sw"></param>
        public void RemoveSaveWork(Models.SaveWork sw)
        {
            this.SaveWorksList.Remove(sw);
        }
        /// <summary>
        /// lance un travail de sauvgarde
        /// </summary>
        /// <param name="sw"></param>
        public void StartSaveWork(SaveWork sw)
        {
            Thread Thread = new Thread(delegate () { sw.Copy(); });
            Thread.Start();
        }
        /// <summary>
        /// lance tout les travaux de sauvgarde
        /// </summary>
        public void StartAllSaveWork()
        {
            foreach (var save in this.SaveWorksList)
            {
                StartSaveWork(save.Key);
            }

        }
        /// <summary>
        /// retire tout les travail de sauvgarde
        /// </summary>
        public void RemoveAllSaves()
        {
            this.SaveWorksList = new Dictionary<Models.SaveWork, bool>();
        }
        /// <summary>
        /// met en pause un element de sauvgarde selectioné
        /// </summary>
        /// <param name="sw"></param>
        public void PauseSave(SaveWork sw)
        {
            this.SaveWorksList[sw] = false;
        }
    }
}
