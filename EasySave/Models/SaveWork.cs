using EasySave.Controllers;
using System.Diagnostics;

namespace EasySave.Models
{
    /// <summary>
    /// classe de sauvgarde
    /// </summary>
    internal class SaveWork
    {
        private readonly LoggerController _logger = new();
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public string Name { get; set; }
        public bool TypeComplet { get; set; }

        /// <summary>
        /// constructeur de sauvgarde
        /// </summary>
        /// <param name="name">nom du travail</param>
        /// <param name="sourcePath">chemin source du travail</param>
        /// <param name="targetPath">chemin de destination du travail</param>
        /// <param name="typeComplet">type si complet ou diférensiel</param>
        public SaveWork(string name, string sourcePath, string targetPath, bool typeComplet)
        {
            Name = name;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            TypeComplet = typeComplet;
        }

        /// <summary>
        /// récupération du nom du trvail
        /// </summary>
        /// <returns>le nom</returns>
        public string GetName()
        {
            return Name;
        }

        /// <summary>
        /// création de la phrase de présentation du travail
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            return Name + " || " + SourcePath + " -> " + TargetPath + " || type : " + (TypeComplet ? "Comp" : "Diff");
        }

        /// <summary>
        /// comparéson des fichier
        /// </summary>
        /// <returns>liste des diférence</returns>
        public String[] Compare()
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

            return list.ToArray();
        }

        /// <summary>
        /// copie des fichier
        /// </summary>
        /// <param name="callback">pourcentage de copie</param>
        public void Copy(Action<long> callback)
        {
            //init info
            DirectoryInfo dirSource = new(SourcePath);
            DirectoryInfo dirTarget = new(TargetPath);
            long sizeCopie = TypeComplet ? 0 : DirSize(dirTarget);
            String[] list = TypeComplet ? null : Compare();
            long filesCopie = TypeComplet ? 0 : DirNumberFiles(dirTarget);
            long nbTtFile = DirNumberFiles(dirSource);

            //copy folders
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(SourcePath, TargetPath));
            }

            //copy files
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                if (list?.Contains(newPath) != false)
                {
                    try
                    {
                        File.Copy(newPath, newPath.Replace(SourcePath, TargetPath), true);
                        filesCopie++;

                        long ttFilesSize = DirSize(new DirectoryInfo(SourcePath));
                        long ttFileSizeLeft = ttFilesSize - sizeCopie;
                        long nbFilesLeft = nbTtFile - filesCopie;

                        ProgressLog progression = new(nbFilesLeft, ttFileSizeLeft, newPath, newPath.Replace(SourcePath, TargetPath));

                        long fileSize = new DirectoryInfo(Path.GetDirectoryName(newPath)).GetFiles().FirstOrDefault(f => f.FullName == newPath)?.Length ?? 0;

                        long durationFileTransfer = 0;

                        Stopwatch sw = Stopwatch.StartNew();
                        // TODO : replace by "CopyFileAsync"
                        //CopyFileAsync(newPath, TargetPath, cancellation)
                        File.Copy(newPath, newPath.Replace(SourcePath, TargetPath), true);
                        sw.Stop();
                        durationFileTransfer = sw.ElapsedMilliseconds;

                        FileInfo file = new(newPath);
                        sizeCopie += file.Length;
                        SaveState state = nbFilesLeft <= 0 ? SaveState.End : SaveState.Active;

                        _logger.LogState(this, state, sizeCopie, ttFilesSize, progression);

                        _logger.LogDay(this, SaveState.Active, (int)nbTtFile, ttFilesSize, progression, fileSize, durationFileTransfer);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(Name + ":" + ex.ToString());
                        _logger.LogException(ex.ToString());
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                }
                callback(PourcentProgress(sizeCopie));
            }
            callback(PourcentProgress(sizeCopie));

            Thread.Sleep(1000);
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
    }
}
