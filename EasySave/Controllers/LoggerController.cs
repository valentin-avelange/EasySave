using EasySave.Models;
using System.Text.Json;

namespace EasySave.Controllers
{
    /// <summary>
    /// création des variables
    /// </summary>
    internal class LoggerController
    {
        private readonly string globalPathLog;
        private readonly string dailyFileLog;
        private readonly string stateFileLog;
        private readonly string exceptionFileLog;
        private readonly string extensionFile;

        private string stateLogPath = string.Empty;
        private string dailyLogPath = string.Empty;

        /// <summary>
        /// inisialisation des variables
        /// </summary>
        public LoggerController()
        {
            var logPath = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Environment.CurrentDirectory + "/Properties/logparameter.json"));
            globalPathLog = logPath["globalPathLog"];
            dailyFileLog = logPath["dailyFileLog"];
            stateFileLog = logPath["stateFileLog"];
            exceptionFileLog = logPath["exceptionFileLog"];
            extensionFile = logPath["extensionFile"];

            stateLogPath = string.Empty;
            dailyLogPath = string.Empty;
        }

        /// <summary>
        /// Log des opération en cour
        /// </summary>
        /// <param name="saveWork"></param>
        /// <param name="state"></param>
        /// <param name="nbTtFileToCopy"></param>
        /// <param name="TtFilesSize"></param>
        /// <param name="progression"></param>
        public void LogState(SaveWork saveWork, SaveState state, long nbTtFileToCopy, long TtFilesSize, ProgressLog progression)
        {
            DateTime logTimestamp = DateTime.Now;
            //Ready to create json
            StateLog stateLog = new(saveWork.Name, state, logTimestamp, nbTtFileToCopy, TtFilesSize, progression.NbFileLeft, progression.TtFilesSizeLeft, progression.CurrentSourceFilePath, progression.CurrentDestinationFilePath);
            stateLogPath = Path.Combine(globalPathLog, string.Concat(stateFileLog, extensionFile));

            string stateLogJson = string.Concat(JsonSerializer.Serialize(stateLog), ",");
            IEnumerable<StateLog> stateLogs = new List<StateLog>();

            // Déserialiser stateFileLog => stateLogs 
            //Compare : stateLog.Name avec tout les stateLogs   => Exist        ==> met à jour
            //                                                  => not Exists   ==> Créer/Ajoute à la liste
            //Génération Json statelog
            WriteLogs(stateLogPath, stateLogJson);
        }

        /// <summary>
        /// Log les opération de la journée
        /// </summary>
        /// <param name="saveWork"></param>
        /// <param name="state"></param>
        /// <param name="nbTtFileToCopy"></param>
        /// <param name="TtFilesSize"></param>
        /// <param name="progression"></param>
        /// <param name="currentFileSize"></param>
        /// <param name="durationFileTranfer"></param>
        public void LogDay(SaveWork saveWork, SaveState state, int nbTtFileToCopy, long TtFilesSize, ProgressLog progression, long currentFileSize, long durationFileTranfer)
        {
            DateTime logTimestamp = DateTime.Now;

            //TODO : Ready to create json 
            DayLog dayLog = new(saveWork.Name, state, logTimestamp, saveWork.TypeComplet, saveWork.SourcePath, saveWork.TargetPath, currentFileSize, durationFileTranfer);

            // Ecriture du fichier par jour au chemein suivant : D:\AAATestLog\dailyLog_21/11/2021 20:10:30.json
            dailyLogPath = Path.Combine(globalPathLog, string.Concat(dailyFileLog, "_", DateTime.Now.ToString("yyyy_MM_dd"), extensionFile));

            string dailyLogJson = string.Concat(JsonSerializer.Serialize(dayLog), ",");
            //Console.WriteLine(dailyLogJson);
            WriteLogs(dailyLogPath, dailyLogJson);
        }

        /// <summary>
        /// Ecriture général sur disque des logs.
        /// </summary>
        /// <param name="path">Chemin vers le fichier de log voulu</param>
        /// <param name="content">json à ajouté au fichier</param>
        private void WriteLogs(string path, string content)
        {
            try
            {
                if (!File.Exists(path))
                {
                    var myfile = System.IO.File.Create(path);
                    myfile.Close();
                    File.AppendAllText(path, "[\n]");
                }
                long length = new System.IO.FileInfo(path).Length;
                if (length == 0)
                {
                    File.AppendAllText(path, "[\n]");
                }
                FileStream fs = new(@path, FileMode.Open, FileAccess.ReadWrite);
                fs.SetLength(fs.Length - 1); // Remove the last symbol ']'
                fs.Close();
                System.IO.File.AppendAllText(@path, content + "\n]");
            }
            catch (Exception ex)
            {
                Console.WriteLine("L'application rencontre des soucis de sauvegarde, veuillez contacter l'administrateur local." + ex.ToString());
                LogException(string.Concat(ex.ToString(), "\n Contenu lors de l'essaie d'écriture : ", content));
            }
        }
        /// <summary>
        /// log de gestion de problem
        /// </summary>
        /// <param name="exception"></param>
        public void LogException(string exception)
        {
            WriteLogs(Path.Combine(globalPathLog, string.Concat(exceptionFileLog, "_", DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss"), extensionFile)), exception);
        }
    }
}
