using NLog.Config;
using NLog.Targets;
using NLog;
using System.IO.Compression;
using Base.Abstractions.Diagnostic;
using Base.Abstractions.Platform;

namespace Base.Impl.Common
{
    internal class NLogFileLoger : IFileLoger
    {
        private Logger Logger;
        private string _logDir;
        private string _logFileName;
        private string currentLogPath;
        private readonly Lazy<IDirectoryService> directory;

        public NLogFileLoger(Lazy<IDirectoryService> directory)
        {
            this.directory = directory;
        }

        public void Init()
        {
            //Logger creates folder for every day with yyyy-MM-dd as name 
            //Each folder(yyyy-MM-dd) contains log files for each session/run 
            _logDir = GetLogsFolder();
            //create day folder
            var dayStamp = DateTime.Now.ToString("yyyy-MM-dd");
            var sessionFolder = Path.Combine(_logDir, dayStamp);
            // Create unique log file name for each session using timestamp
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            _logFileName = $"session_{timestamp}.log";
            currentLogPath = Path.Combine(sessionFolder, _logFileName);

            //set configuration
            var config = new LoggingConfiguration();
            //save log to file
            var fileTarget = new FileTarget("logfile")
            {
                FileName = currentLogPath,
                // Log just message as it is, because we have own message format
                Layout = "${message}",
                // Performance: keep file open
                KeepFileOpen = true,
                //ConcurrentWrites = true,
                // Disable automatic archiving, we will archive manually
                ArchiveEvery = FileArchivePeriod.None
            };
            //add this file target to config
            config.AddTarget(fileTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;
            Logger = LogManager.GetCurrentClassLogger();

            CleanupOldLogs();
        }

        public void Info(string message)
        {
            Logger.Info(message);
        }



        public void Warn(string message)
        {
            Logger.Warn(message);
        }

        public void Warn(string message, Exception ex)
        {
            Logger.Warn($"{message} {ex}");
        }

        /// <summary>
        /// Keep only logs from the last 7 days(7 folders with logs)
        /// </summary>
        public void CleanupOldLogs()
        {
            try
            {
                // Get all directories matching the yyyy-MM-dd pattern
                var folders = Directory.GetDirectories(_logDir)
                    .Select(dir => new DirectoryInfo(dir))
                    .Where(di => DateTime.TryParseExact(
                        di.Name,
                        "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out _))
                    .OrderByDescending(di => DateTime.ParseExact(
                        di.Name,
                        "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture))
                    .ToList();

                if (folders.Count > 7)
                {
                    // Keep the newest 7, remove the rest
                    var oldFolders = folders.Skip(7);

                    foreach (var folder in oldFolders)
                    {
                        try
                        {
                            Console.WriteLine($"Deleting: {folder.FullName}");
                            Directory.Delete(folder.FullName, true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to delete {folder.FullName}: {ex}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Skip log clean up because there is less than 7 logs");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public Stream GetCompressedLogsSync(bool getOnlyLastSession)
        {
            try
            {
                //flush to make sure that buffer is saved in the log file
                LogManager.Flush();
                //pause logger
                LogManager.SuspendLogging();

                if (getOnlyLastSession)
                {
                    return ZipLastSessionToStream();
                }
                else
                {
                    return ZipMainFolderToStream(_logDir);
                }
            }
            finally
            {
                LogManager.ResumeLogging();
            }
        }


        public async Task<List<string>> GetLogListAsync()
        {
            // Flush current log buffer to ensure file has latest entries
            LogManager.Flush();

            if (!File.Exists(currentLogPath))
                return new List<string>();

            var list = await Task.Run(() =>
            {
                // Read all lines (or stream from end for large files)
                var lines = File.ReadAllLines(currentLogPath);
                return lines.ToList();
            });

            //returns last 300 lines
            var lineCount = 100;
            if (list.Count <= lineCount)
                return list;

            var lines = list.Skip(Math.Max(0, list.Count - lineCount)).ToList();
            return lines;
        }



        public string GetLogsFolder()
        {
            var appDataDir = directory.Value.GetAppDataDir();
            var path = Path.Combine(appDataDir, "NLog");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }


        /// <summary>
        /// Creates a ZIP archive from the the main log folder that includes all logs(for all sessions).
        /// The caller is responsible for disposing the stream.
        /// </summary>
        private Stream ZipMainFolderToStream(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Folder not found: {folderPath}");

            var memoryStream = new MemoryStream();

            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
                foreach (var filePath in files)
                {
                    string entryName = Path.GetRelativePath(folderPath, filePath);
                    var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);

                    using (var entryStream = entry.Open())
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        fileStream.CopyTo(entryStream);
                    }
                }
            }

            // Reset stream position for reading
            memoryStream.Position = 0;

            return memoryStream;
        }

        /// <summary>
        /// Zips current last log 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        private Stream ZipLastSessionToStream()
        {
            if (!File.Exists(currentLogPath))
                throw new FileNotFoundException($"File not found: {currentLogPath}");

            string zipPath = Path.ChangeExtension(currentLogPath, ".zip");

            // Create the zip and ensure it's fully written
            using (var zipStream = new FileStream(zipPath, FileMode.Create))
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(currentLogPath, Path.GetFileName(currentLogPath));
            }

            // Now read the finalized zip into memory
            var memoryStream = new MemoryStream(File.ReadAllBytes(zipPath));
            memoryStream.Position = 0;
            return memoryStream;
        }



        private string GetUserAppDataPath()
        {
            var docsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (!Directory.Exists(docsFolder))
            {
                Directory.CreateDirectory(docsFolder);
            }
            return docsFolder;
        }        
    }
}
