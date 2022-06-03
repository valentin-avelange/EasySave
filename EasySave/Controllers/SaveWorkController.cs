using EasySave.Models;
using EasySave.Views;
using EasyTrad;

namespace EasySave.Controllers
{
    internal class SaveWorkController
    {
        private List<SaveWork> SaveWorks { get; }
        private Trad Traduction { get; }

        /// <summary>
        /// SaveWorkController constructor
        /// </summary>
        /// <param name="Traduction">Traduction object for display view in choiced language</param>
        public SaveWorkController(Trad Traduction)
        {
            SaveWorks = new List<SaveWork>();
            this.Traduction = Traduction;
        }
        /// <summary>
        /// Methode for create SaveWork object
        /// </summary>
        /// <returns>The new SaveWork object</returns>
        public SaveWork CreateSave()
        {
            return new CreateSave(Traduction).Display();
        }
        /// <summary>
        /// Give list of all saveWorks saved in SaveWorks attribute
        /// </summary>
        /// <returns>the list of saveworks objects</returns>
        public List<SaveWork> GetSaveWorks()
        {
            return SaveWorks;
        }
        /// <summary>
        /// Remove saveWork in SaveWorks attribute with index
        /// </summary>
        /// <param name="index">Index of the futur saveworks removed </param>
        public void RemoveSave(int index)
        {
            SaveWorks.RemoveAt(index);
        }
        /// <summary>
        /// Add Savework object in the SaveWorks attribute
        /// </summary>
        /// <param name="saveWork">SaveWork object to push in the list SaveWorks attribute</param>
        public void AddSave(SaveWork saveWork)
        {
            if (SaveWorks.Count < 5)
            {
                SaveWorks.Add(saveWork);
                new Message(Traduction).DisplayInfo("AddSuccess", true);
            }
            else
            {
                new Message(Traduction).DisplayError("AddFail", true);
            }
        }
    }
}
