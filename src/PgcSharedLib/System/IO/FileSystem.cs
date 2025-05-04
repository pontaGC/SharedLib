using System.Diagnostics;
using System.Text;
using SharedLib.Helpers;

namespace System.IO
{
    /// <summary>
    /// The file system operations with retry.
    /// </summary>
    public class FileSystem : IFileSystem
    {
        #region Singleton instance

        public static FileSystem Instance { [DebuggerStepThrough] get; }

        private FileSystem() { }

        static FileSystem()
        {
            // Do not remove static constructor for lazy initialization.
            Instance = new FileSystem();
        }

        #endregion

        #region Check

        /// <inhertidoc />
        public bool ExistsFile(string path)
        {
            return File.Exists(path);
        }

        /// <inhertidoc />
        public bool ExistsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        /// <inhertidoc />
        public bool HasExtension(string filename)
        {
            return Path.HasExtension(filename);
        }

        /// <inhertidoc />
        public bool HasExtension(string filename, string expectedFileExtension)
        {
            return Path.HasExtension(filename) && Path.GetExtension(filename) == expectedFileExtension;
        }

        /// <inhertidoc />
        public bool IsPathFullyQualified(string path)
        {
            return Path.IsPathFullyQualified(path);
        }

        /// <inhertidoc />
        public bool IsDirectorySeparator(char value)
        {
            return value == Path.DirectorySeparatorChar || value == Path.AltDirectorySeparatorChar;
        }

        #endregion

        #region Create/Open

        /// <inhertidoc />
        public FileStream CreateFile(string filePath)
        {
            return RetryHelper.InvokeWithRetry(() => File.Create(filePath));
        }

        /// <inhertidoc />
        public DirectoryInfo CreateDirectory(string dirPath)
        {
            return RetryHelper.InvokeWithRetry(() => Directory.CreateDirectory(dirPath));
        }

        /// <inhertidoc />
        public FileStream OpenFile(string filePath, FileAccess access, FileShare share)
        {
            return RetryHelper.InvokeWithRetry(() => File.Open(filePath, FileMode.Open, access, share));
        }

        /// <inhertidoc />
        public FileStream OpenFile(string filePath, FileAccess access)
        {
            return RetryHelper.InvokeWithRetry(() => File.Open(filePath, FileMode.Open, access));
        }

        /// <inhertidoc />
        public FileStream OpenFile(string filePath)
        {
            return RetryHelper.InvokeWithRetry(() => File.Open(filePath, FileMode.Open));
        }

        /// <inhertidoc />
        public FileStream OpenReadFile(string filePath)
        {
            return RetryHelper.InvokeWithRetry(() => File.OpenRead(filePath));
        }

        /// <inhertidoc />
        public FileStream OpenOrCreateFile(string filePath)
        {
            return RetryHelper.InvokeWithRetry(() => File.Open(filePath, FileMode.OpenOrCreate));
        }

        /// <inhertidoc />
        public FileStream OpenOrCreateFile(string filePath, FileAccess access)
        {
            return RetryHelper.InvokeWithRetry(() => File.Open(filePath, FileMode.OpenOrCreate, access));
        }

        /// <inhertidoc />
        public FileStream OpenOrCreateFile(string filePath, FileAccess access, FileShare share)
        {
            return RetryHelper.InvokeWithRetry(() => File.Open(filePath, FileMode.OpenOrCreate, access, share));
        }

        /// <inhertidoc />
        public string ReadAllText(string filePath)
        {
            return RetryHelper.InvokeWithRetry(() => File.ReadAllText(filePath));
        }

        /// <inhertidoc />
        public IReadOnlyCollection<string> ReadAllLine(string filePath, Encoding encording)
        {
            var textEncording = encording ?? Encoding.UTF8;
            return RetryHelper.InvokeWithRetry(() => File.ReadAllLines(filePath, textEncording));
        }

        #endregion

        #region Delete

        /// <inhertidoc />
        public FileInfo DeleteFile(string path)
        {
            return RetryHelper.InvokeWithRetry(() =>
            {
                var fileInfo = new FileInfo(path)
                {
                    IsReadOnly = false
                };
                fileInfo.Delete();
                return fileInfo;
            });
        }

        /// <inhertidoc />
        public DirectoryInfo DeleteDirectory(string path)
        {
            return RetryHelper.InvokeWithRetry(() =>
            {
                var dirInfo = new DirectoryInfo(path);
                dirInfo.Delete();
                return dirInfo;
            });
        }

        /// <inhertidoc />
        public void CleanDirectory(string directoryPath, bool recursive)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                return;
            }

            foreach (var filePath in EnumerateFiles(directoryPath, false))
            {
                DeleteFile(filePath);
            }

            if (recursive)
            {
                foreach (var subDirPath in EnumerateDirectories(directoryPath, false))
                {
                    DeleteDirectory(subDirPath);
                }
            }
        }

        #endregion

        #region Copy

        /// <inhertidoc />
        public void CopyFile(string sourceFilePath, string destFilePath, bool overwrite)
        {
            RetryHelper.InvokeWithRetry(() => File.Copy(sourceFilePath, destFilePath, overwrite));
        }

        /// <inhertidoc />
        public IEnumerable<FileInfo> CopyFiles(string sourceDirPath, string destDirPath)
        {
            if (!ExistsDirectory(destDirPath))
            {
                CreateDirectory(destDirPath);
            }

            var copiedFileInfos = new List<FileInfo>();
            foreach (var filePath in EnumerateDirectories(sourceDirPath, true))
            {
                var copyingFilePath = Path.Combine(destDirPath, GetFileName(filePath));
                CopyFile(filePath, copyingFilePath, true);
                copiedFileInfos.Add(new FileInfo(copyingFilePath));
            }

            return copiedFileInfos;
        }

        /// <inhertidoc />
        public (bool IsFailed, IEnumerable<FileInfo> Copied) TryCopyFiles(string sourceDirPath, string destDirPath)
        {
            var copiedFileInfos = new List<FileInfo>();

            try
            {
                if (!ExistsDirectory(destDirPath))
                {
                    CreateDirectory(destDirPath);
                }

                foreach (var filePath in EnumerateDirectories(sourceDirPath, true))
                {
                    var copyingFilePath = Path.Combine(destDirPath, GetFileName(filePath));
                    CopyFile(filePath, copyingFilePath, true);
                    copiedFileInfos.Add(new FileInfo(copyingFilePath));
                }

                return (false, copiedFileInfos);
            }
            catch
            {
                return (true, copiedFileInfos);
            }
        }

        #endregion

        #region Modify

        /// <inhertidoc />
        public string AddExtensionIfNotHave(string filename, string extension)
        {
            if (HasExtension(filename))
            {
                return filename;
            }

            return filename + extension;
        }

        /// <inhertidoc />
        public string ChangeExtension(string path, string extension)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            return Path.ChangeExtension(path, extension);
        }

        /// <inhertidoc />
        public string ChangeFilename(string path, string filename)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(filename))
            {
                return string.Empty;
            }

            var directoryName = GetDirectoryName(path);
            if (string.IsNullOrEmpty(directoryName))
            {
                return string.Empty;
            }

            return Path.Combine(directoryName, filename);
        }

        /// <inhertidoc />
        public void WriteAllText(string filePath, string fileContent)
        {
            RetryHelper.InvokeWithRetry(() => File.WriteAllText(filePath, fileContent));
        }

        #endregion

        #region Query

        /// <inheritdoc />
        public FileInfo? GetFileInfo(string filename)
        {
            try
            {
                return new FileInfo(filename);
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc />
        public string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <inheritdoc />
        public DirectoryInfo? GetDirectoryInfo(string path)
        {
            try
            {
                return new DirectoryInfo(path);
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc />
        public DirectoryInfo GetParent(string path)
        {
            try
            {
                return RetryHelper.InvokeWithRetry(() => Directory.GetParent(path));
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc />
        public string GetExtension(string filename, bool removesDot = false)
        {
            if (Path.HasExtension(filename) == false)
            {
                return string.Empty;
            }

            var extension = Path.GetExtension(filename);
            if (removesDot)
            {
                return extension.Replace(".", string.Empty);
            }

            return extension;
        }

        /// <inheritdoc />
        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <inheritdoc />
        public string GetDirectoryName(string path, string defaultDirName = "")
        {
            try
            {
                return Path.GetDirectoryName(path) ?? defaultDirName;
            }
            catch
            {
                return defaultDirName;
            }
        }

        /// <inheritdoc />
        public IEnumerable<string> EnumerateFiles(string directoryPath, bool recursive)
        {
            return EnumerateFiles(directoryPath, "*.*", recursive);
        }

        /// <inheritdoc />
        public IEnumerable<string> EnumerateFiles(string directoryPath, string searchPattern, bool recursive)
        {
            return RetryHelper.InvokeWithRetry(() => Directory.EnumerateFiles(directoryPath, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
        }

        /// <inheritdoc />
        public IEnumerable<FileInfo> EnumerateFileInfos(string directoryPath, bool recursive)
        {
            return EnumerateFileInfos(directoryPath, "*", recursive);
        }

        /// <inheritdoc />
        public IEnumerable<FileInfo> EnumerateFileInfos(string directoryPath, string searchPattern, bool recursive)
        {
            return EnumerateFiles(directoryPath, searchPattern, recursive).Select(path => new FileInfo(path));
        }

        /// <inheritdoc />
        public IEnumerable<string> EnumerateDirectories(string directoryPath, bool recursive)
        {
            return RetryHelper.InvokeWithRetry(() => Directory.EnumerateDirectories(directoryPath, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
        }

        /// <inheritdoc />
        public IEnumerable<DirectoryInfo> EnumerateDirectoryInfos(string directoryPath, bool recursive)
        {
            return EnumerateDirectories(directoryPath, recursive).Select(path => new DirectoryInfo(path));
        }

        #endregion
    }
}
