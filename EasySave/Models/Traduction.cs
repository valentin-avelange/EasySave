using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasyTrad

{
    /// <summary>
    /// class de traduction
    /// </summary>
    public class Trad
    {
        private readonly Dictionary<string, string> dico;
        private readonly string lang;

        /// <summary>
        /// complaite le dictionaire de phrase en fonction de la langue
        /// </summary>
        /// <param name="lang">langue selectionée</param>
        public Trad(string lang)
        {
            var results = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(File.ReadAllText(Environment.CurrentDirectory + "/Properties/jsonlangage.json"));

            //EN Dictionary
            var enDict = results.SelectMany(x => x).ToDictionary(x => x.Key, x => JObject.Parse(x.Value.ToString())["en-EN"] == null ? "" : JObject.Parse(x.Value.ToString())["en-EN"].ToString());

            //FR Dictionary
            var frDict = results.SelectMany(x => x).ToDictionary(x => x.Key, x => JObject.Parse(x.Value.ToString())["fr-FR"] == null ? "" : JObject.Parse(x.Value.ToString())["fr-FR"].ToString());

            var langage = enDict;

            if (lang == "fr")
            {
                dico = frDict;
            }
            else if (lang == "en")
            {
                dico = enDict;
            }
            this.lang = lang;
        }
        /// <summary>
        /// dictionaire des phrase
        /// </summary>
        /// <returns>le dictionaire</returns>
        public Dictionary<string, string> GetDico()
        {
            return dico;
        }
        /// <summary>
        /// demande la langue
        /// </summary>
        /// <returns>la langue</returns>
        public string GetLangage()
        {
            return lang;
        }
    }
}



