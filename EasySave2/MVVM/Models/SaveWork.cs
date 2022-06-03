using EasySave2.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace EasySave2.MVVM.Models
{
    /// <summary>
    /// classe de sauvgarde
    /// </summary>
    public class SaveWork
    {
        private string SourcePath { get; set; }
        private string TargetPath { get; set; }
        private string Name { get; set; }
        private Models.EnumTypeSaveWork TypeComplet { get; set; }

        private List<string> prioritFiles;
        private long ProgressPourcent = 0;
        public event Action<long> ProgressChanged;

        public static Mutex m = new Mutex();



        /// <summary>
        /// constructeur de sauvgarde
        /// </summary>
        /// <param name="name">nom du travail</param>
        /// <param name="sourcePath">chemin source du travail</param>
        /// <param name="targetPath">chemin de destination du travail</param>
        /// <param name="typeComplet">type si complet ou diférensiel</param>
        public SaveWork(string name, string sourcePath, string targetPath, Models.EnumTypeSaveWork typeComplet)
        {
            Name = name;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            TypeComplet = typeComplet;
            prioritFiles = new List<string>();
        }

        public string GetSourcePath()
        {
            return SourcePath;
        }
        public string GetTargetPath()
        {
            return TargetPath;
        }
        public Models.EnumTypeSaveWork GetTypePath()
        {
            return TypeComplet;
        }

        public long GetProgressPourcent()
        {
            return ProgressPourcent;
        }

        public List<string> GetPrioritFiles()
        {
            return prioritFiles;
        }

        public void AddPrioritFile(string pathFile)
        {
            this.prioritFiles.Add(pathFile);
        }

        public void RemovePrioritFile(string pathFile)
        {
            this.prioritFiles.Remove(pathFile);
        }

        /// <summary>
        /// récupération du nom du trvail
        /// </summary>
        /// <returns>le nom</returns>
        public string GetName()
        {
            return Name;
        }

        public List<string> GetAllFiles()
        {
            List<string> list = new();
            foreach (var v in Directory.GetFiles(this.SourcePath, "", SearchOption.AllDirectories))
            {
                list.Add(v);
            }
            return list;
        }

        /// <summary>
        /// comparéson des fichier
        /// </summary>
        /// <returns>liste des diférence</returns>
        public List<string> Compare()
        {
            //new list for my answers
            List<string> list = new();
            //file list
            IEnumerable<FileInfo> list1 = new DirectoryInfo(SourcePath).GetFiles("*.*", SearchOption.AllDirectories);
            IEnumerable<FileInfo> list2 = new DirectoryInfo(TargetPath).GetFiles("*.*", SearchOption.AllDirectories);

            FileCompare myFileCompare = new();
            //files not present or modified in the target path
            var queryList1Only = (from file in list1 select file).Except(list2, myFileCompare);
            //push in list
            foreach (var v in queryList1Only) { list.Add(v.FullName); }

            return list;
        }

        /// <summary>
        /// copie des fichier
        /// </summary>
        public void Copy()
        {
            //init info
            DirectoryInfo dirSource = new(SourcePath);
            DirectoryInfo dirTarget = new(TargetPath);
            long sizeCopie = TypeComplet == Models.EnumTypeSaveWork.Complet ? 0 : DirSize(dirTarget);
            List<string> list = TypeComplet == Models.EnumTypeSaveWork.Complet ? GetAllFiles() : Compare();
            long filesCopie = TypeComplet == Models.EnumTypeSaveWork.Complet ? 0 : DirNumberFiles(dirTarget);
            long nbTtFile = DirNumberFiles(dirSource);

            //copy folders
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(SourcePath, TargetPath));
            }

            //sort prio
            List<string> listPrio = GetPrioritFiles();
            foreach (string path in list)
            {
                if (!listPrio.Contains(path))
                {
                    listPrio.Add(path);
                }
            }




            //copy files
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                while (!MVVM.ViewModels.Context.GetInstance().GetListSavesVM().GetSaveWorks()[this])
                {
                    Thread.Sleep(1000);
                }
                if (list?.Contains(newPath) != false)
                {
                    try
                    {

                        filesCopie++;

                        long ttFilesSize = DirSize(new DirectoryInfo(SourcePath));
                        long ttFileSizeLeft = ttFilesSize - sizeCopie;
                        long nbFilesLeft = nbTtFile - filesCopie;

                        ProgressLog progression = new(nbFilesLeft, ttFileSizeLeft, newPath, newPath.Replace(SourcePath, TargetPath));

                        long fileSize = new DirectoryInfo(Path.GetDirectoryName(newPath)).GetFiles().FirstOrDefault(f => f.FullName == newPath)?.Length ?? 0;

                        long durationFileTransfer = 0;
                        long durationEncryptFile = 0;

                        Stopwatch sw = Stopwatch.StartNew();
                        Stopwatch swe = Stopwatch.StartNew();
                        //Trace.WriteLine(newPath);
                        //Trace.WriteLine(RemoveLastDirectoryPartOf(newPath.Replace(SourcePath, TargetPath)));
                        FileInfo file = new(newPath);
                        if (file.Length < Context.GetInstance().GetSizeMaxFile())
                        {

                            if (Context.GetInstance().GetExtensionVM().GetExtentions().Contains(file.Extension))
                            {
                                UseCryptoSoft(newPath, RemoveLastDirectoryPartOf(newPath.Replace(SourcePath, TargetPath)), "Key");
                                list.Remove(newPath);
                                swe.Stop();
                            }
                            else
                            {
                                File.Copy(newPath, newPath.Replace(SourcePath, TargetPath), true);
                                list.Remove(newPath);
                                swe.Stop();
                                swe.Reset();
                            }
                        }
                        sw.Stop();
                        durationFileTransfer = sw.ElapsedMilliseconds;
                        durationEncryptFile = swe.ElapsedMilliseconds;


                        sizeCopie += file.Length;
                        SaveState state = nbFilesLeft <= 0 ? SaveState.End : SaveState.Active;
                        m.WaitOne();

                        MVVM.ViewModels.Context.GetInstance().GetLoggerVM().LogState(this, state, sizeCopie, ttFilesSize, progression);

                        MVVM.ViewModels.Context.GetInstance().GetLoggerVM().LogDay(this, SaveState.Active, (int)nbTtFile, ttFilesSize, progression, fileSize, durationFileTransfer, durationEncryptFile);
                        m.ReleaseMutex();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Name + ":" + ex.ToString());
                        MVVM.ViewModels.Context.GetInstance().GetLoggerVM().LogException(ex.ToString());
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                }
                ProgressPourcent = PourcentProgress(sizeCopie);
                ProgressChanged(ProgressPourcent);
            }
            ProgressPourcent = PourcentProgress(0);
            ProgressChanged(0);
        }

        /// <summary>
        /// donne la taille d'un dosier
        /// </summary>
        /// <param name="d">chemin du dossier</param>
        /// <returns>taille du fichier</returns>
        public long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        /// <summary>
        /// donne le nombre de fichier
        /// </summary>
        /// <param name="d">chemin du dossier</param>
        /// <returns>nombre de fichier</returns>
        public long DirNumberFiles(DirectoryInfo d)
        {
            long files = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                files++;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                files += DirNumberFiles(di);
            }
            return files;
        }
        /// <summary>
        /// donne le pourcentage d'avencement
        /// </summary>
        /// <param name="size">taille copier</param>
        /// <returns>taille copier/taille total en poucent</returns>
        public long PourcentProgress(long size)
        {
            DirectoryInfo dirSource = new(SourcePath);
            return (size * 100) / DirSize(dirSource);
        }


        private void UseCryptoSoft(string file, string path, string key)
        {
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.FileName = Context.GetInstance().GetCryptoPath();
            string newFile = file.Replace(" ", "?");
            string newPath = path.Replace(" ", "?");
            Trace.WriteLine(newFile + " " + newPath + " " + key);
            proc.StartInfo.Arguments = newFile + " " + newPath + " " + key;
            proc.Start();
            proc.WaitForExit();
        }

        private string RemoveLastDirectoryPartOf(string myString)
        {
            string[] segments = myString.Split('\\');

            var noLastSegment = "";

            for (int i = 0; i < segments.Length - 1; i++)
            {
                noLastSegment += segments[i] + "\\";
            }
            //Trace.WriteLine(noLastSegment);
            return noLastSegment;
        }

    }
}
