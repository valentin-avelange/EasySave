using EasySave.Views;
using EasyTrad;

namespace EasySave.Controllers
{
    internal class TraductionController
    {
        private Trad Traduction { get; set; }

        /// <summary>
        /// TraductionController constructor
        /// </summary>
        public TraductionController()
        {
            Traduction = new Trad("fr");
        }

        /// <summary>
        /// Getter Traduction attribute
        /// </summary>
        /// <returns></returns>
        public Trad GetTraduction()
        {
            return Traduction;
        }
        /// <summary>
        /// Switch language between FR and EN languages
        /// </summary>
        public void SwitchTrad()
        {
            if (Traduction.GetLangage() == "fr")
            {
                Traduction = new Trad("en");
                new Message(Traduction).DisplayInfo("EasySave is now in English", false);
            }
            else
            {
                Traduction = new Trad("fr");
                new Message(Traduction).DisplayInfo("EasySave est maintenant en Francais", false);
            }
        }
    }
}
