using EasySave2.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasySave2.MVVM.ViewModels
{
    internal class LoggerVM
    {
        private readonly string globalPathLog;
        private readonly string dailyFileLog;
        private readonly string stateFileLog;
        private readonly string exceptionFileLog;
        private readonly string extensionFile;
        private readonly string extensionFileXml;

        private string stateLogPath = string.Empty;
        private string dailyLogPath = string.Empty;

        /// <summary>
        /// inisialisation des variables
        /// </summary>
        public LoggerVM(string newPath)
        {
            globalPathLog = newPath;
            dailyFileLog = "dailyLog";
            stateFileLog = "stateLog";
            exceptionFileLog = "exceptionLog";
            extensionFile = ".json";
            extensionFileXml = ".xml";

            stateLogPath = string.Empty;
            dailyLogPath = string.Empty;
        }

        public string GetLogPath()
        {
            return stateLogPath;
        }
        public void SetLogPath(string newPath)
        {
            stateLogPath = newPath;
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
            StateLog stateLog = new(saveWork.GetName(), state, logTimestamp, nbTtFileToCopy, TtFilesSize, progression.NbFileLeft, progression.TtFilesSizeLeft, progression.CurrentSourceFilePath, progression.CurrentDestinationFilePath);
            stateLogPath = Path.Combine(globalPathLog, string.Concat(stateFileLog, extensionFile));

            string stateLogJson = string.Concat(System.Text.Json.JsonSerializer.Serialize(stateLog), ",");
            IEnumerable<StateLog> stateLogs = new List<StateLog>();

            //Génération Json statelog
            WriteLogs(stateLogPath, stateLogJson);

            string xmlPath = string.Concat(globalPathLog, "\\", stateFileLog, extensionFileXml);


            if (!File.Exists(xmlPath))
            {
                XmlTextWriter writer = new XmlTextWriter(xmlPath, null);
                writer.WriteStartElement("Clients");
                writer.WriteEndElement();
                writer.Close();
            }
            // Load existing clients and add new 
            XElement xml = XElement.Load(xmlPath);
            xml.Add(new XElement("StateLog",
            new XAttribute("TotalFilesToCopy", nbTtFileToCopy),
            new XElement("TotalFilesSize", TtFilesSize),
            new XElement("PnbFileLeft", progression.NbFileLeft),
            new XElement("PttFileSizeLeft", progression.TtFilesSizeLeft),
            new XElement("PcurrentSourceFilePath", progression.CurrentSourceFilePath),
            new XElement("PcurrentDestinationFilePath", progression.CurrentDestinationFilePath),
            new XElement("Name", saveWork.GetName()),
            new XElement("State", state),
            new XElement("TimestampSave", logTimestamp)));
            xml.Save(xmlPath);
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
        /// <param name="durationEncryptFile"></param>
        public void LogDay(SaveWork saveWork, SaveState state, int nbTtFileToCopy, long TtFilesSize, ProgressLog progression, long currentFileSize, long durationFileTranfer, long durationEncryptFile)
        {
            DateTime logTimestamp = DateTime.Now;

            //TODO : Ready to create json 
            DayLog dayLog = new(saveWork.GetName(), state, logTimestamp, Convert.ToBoolean(saveWork.GetTypePath()), saveWork.GetSourcePath(), saveWork.GetTargetPath(), currentFileSize, durationFileTranfer, durationEncryptFile);

            // Ecriture du fichier par jour au chemein suivant : D:\AAATestLog\dailyLog_21/11/2021 20:10:30.json
            dailyLogPath = Path.Combine(globalPathLog, string.Concat(dailyFileLog, "_", DateTime.Now.ToString("yyyy_MM_dd"), extensionFile));

            string dailyLogJson = string.Concat(System.Text.Json.JsonSerializer.Serialize(dayLog), ",");

            //Console.WriteLine(dailyLogJson);
            WriteLogs(dailyLogPath, dailyLogJson);
            
            string xmlPath = string.Concat(globalPathLog, "\\", dailyFileLog, "_", DateTime.Now.ToString("yyyy_MM_dd"), extensionFileXml);

            if (!File.Exists(xmlPath))
            {
                XmlTextWriter writer = new XmlTextWriter(xmlPath, null);
                writer.WriteStartElement("Clients");
                writer.WriteEndElement();
                writer.Close();
            }
            // Load existing clients and add new 
            XElement xml = XElement.Load(xmlPath);
            xml.Add(new XElement("StateLog",
            new XAttribute("SaveType", Convert.ToBoolean(saveWork.GetTypePath())),
            new XElement("SourcePathFile", saveWork.GetSourcePath()),
            new XElement("TargetPathFile", saveWork.GetTargetPath()),
            new XElement("CurrentFileSize", currentFileSize),
            new XElement("DurationFileTransfer", durationFileTranfer),
            new XElement("DurationEncryptFile", durationEncryptFile),
            new XElement("Name", saveWork.GetName()),
            new XElement("State", state),
            new XElement("TimestampSave", logTimestamp)));
            xml.Save(xmlPath);
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
