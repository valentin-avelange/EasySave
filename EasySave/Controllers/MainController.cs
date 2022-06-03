using EasySave.Models;
using EasySave.Views;

namespace EasySave.Controllers
{
    internal class MainController
    {
        private TraductionController Traduction { get; }
        private SaveWorkController SaveWork { get; }

        /// <summary>
        ///     MainController Constructor
        /// </summary>
        public MainController()
        {
            Traduction = new TraductionController();
            SaveWork = new SaveWorkController(Traduction.GetTraduction());
        }
        /// <summary>
        /// Start the loop main of application
        /// </summary>

        public void Start()
        {
            bool exit = false;

            while (!exit)
            {
                new Menu(Traduction.GetTraduction()).Display();

                // Verification de l'entré => non-nulle et bien un int
                var input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int action))
                {
                    switch (action)
                    {
                        case (int)MenuItems.Create: //add
                            SaveWork.AddSave(SaveWork.CreateSave());
                            break;

                        case (int)MenuItems.List://list saveWorks
                            new ListSaves(Traduction.GetTraduction()).Display(SaveWork.GetSaveWorks());
                            break;

                        case (int)MenuItems.Compare://Compare one saveWork (source/target)
                            new CompareSaves(Traduction.GetTraduction()).Display(SaveWork.GetSaveWorks());
                            break;

                        case (int)MenuItems.StartAll://Start all save
                            new SavingSaves(Traduction.GetTraduction()).Display(SaveWork.GetSaveWorks());
                            break;

                        case (int)MenuItems.StartOnce://Start once save
                            new SavingSave(Traduction.GetTraduction()).Display(SaveWork.GetSaveWorks());
                            break;

                        case (int)MenuItems.Stop://Stop
                            // Utilisation de cancellation token pour annuler le transfer en cours
                            exit = true;
                            break;

                        case (int)MenuItems.Switch://switch lang
                            Traduction.SwitchTrad();
                            break;

                        case (int)MenuItems.Remove:
                            int index = new RemoveSaves(Traduction.GetTraduction()).Display(SaveWork.GetSaveWorks());
                            if (index == -1) break;

                            if (new ConfirmRemoveSave(Traduction.GetTraduction()).Display())
                            {
                                SaveWork.RemoveSave(index);
                                break;
                            }
                            break;
                            

                        default:
                            // TODO : Place it in Trad doc
                            new Message(Traduction.GetTraduction()).DisplayWarning("ChoiceList", true);
                            break;
                    }
                    // TODO : Place it in Trad doc
                    new Message(Traduction.GetTraduction()).DisplayInfo("PressKey", true);
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    if (!new ConfirmContinue(Traduction.GetTraduction()).Display())
                    {
                        Thread.Sleep(5000);
                        exit = true;
                    }
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}