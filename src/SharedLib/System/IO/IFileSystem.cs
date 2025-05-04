using System.Text;
using System.Security;

namespace System.IO
{
    /// <summary>
    /// The file system operations with retry.
    /// </summary>
    public interface IFileSystem
    {
        #region Check

        /// <summary>
        /// Checks whether a given file exists.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns><c>true</c>, if the file exists, otherwise <c>false</c>.</returns>
        bool ExistsFile(string path);

        /// <summary>
        /// Checks whether a given directory exists.
        /// </summary>
        /// <param name="path">The directory path.</param>
        /// <returns><c>true</c>, if the directory exists, otherwise <c>false</c>.</returns>
        bool ExistsDirectory(string path);

        /// <summary>
        /// Checks whether or not the file name has a file extension.
        /// </summary>
        /// <param name="filename">The file name to check.</param>
        /// <returns><c>true</c>, if <c>filename</c> includes a file extension. Otherwise; <c>false</c>.</returns>
        bool HasExtension(string filename);

        /// <summary>
        /// Checks whether or not the file has the specified file extension.
        /// </summary>
        /// <param name="filename">The target file name (or path) to check.</param>
        /// <param name="expectedFileExtension">The expected file extension.</param>
        /// <returns><c>true</c>, if the <c>targetFileName</c>'s extension equals <c>expectedFileExtension</c>. Otherwise; <c>false</c>.</returns>
        bool HasExtension(string filename, string expectedFileExtension);

        /// <summary>
        /// Returns a value that indicates whether a file path is fully qualified.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>
        /// <c>true</c> if <c>path</c> is fixed to a specific drive or UNC path.
        /// Otherwise; <c>false</c> if <c>path</c> is relative to the current drive or working directory.
        /// </returns>
        bool IsPathFullyQualified(string path);

        /// <summary>
        /// Checks whether the character is the directory separator.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><c>true</c>, if <c>value</c> is directory separator. Otherwise; <c>false</c>.</returns>
        bool IsDirectorySeparator(char value);

        #endregion

        #region Create/Open

        /// <summary>
        /// Creates, or truncates and overwrites, a file in the specified path.
        /// </summary>
        /// <param name="filePath">The path of the file to create.</param>
        /// <returns>The stream of the created file.</returns>
        /// <exception cref="Exception">See <see href="https://learn.microsoft.com/en-us/dotnet/api/system.io.file.open?view=net-8.0"/>.</exception>
        FileStream CreateFile(string filePath);

        /// <summary>
        /// Creates all directories and subdirectories in the specified path unless they already exist.
        /// </summary>
        /// <param name="path">The path of the directory to create.</param>
        /// <exception cref="ArgumentNullException"><c>path</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><para>path is prefixed with, or contains, only a colon character (:).</para> or <para><c>path</c> is a zero-length string, contains only white space, or contains one or more invalid characters.</para></exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException"><para>The directory specified by path is a file.</para> or <para>The network name is not known.</para></exception>
        /// <exception cref="NotSupportedException"><c>path</c> contains a colon character (:) that is not part of a drive label ("C:\").</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="UnauthorizedAccessException"><para>The caller does not have the required permission.</para> or <para><c>path</c> specified a file that is read-only.</para> or <para>path specified a file that is hidden.</para></exception>
        /// <return>An object that represents the directory at the specified path. This object is returned regardless of whether a directory at the specified path already exists.</return>
        DirectoryInfo CreateDirectory(string path);

        /// <summary>
        /// Opens a file on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.
        /// <see cref="FileMode"/> is <c>Open</c>.
        /// </summary>
        /// <param name="filePath">The file path to create a stream.</param>
        /// <param name="access">
        /// A bitwise combination of the enumeration values that determines how the file can be accessed by the <see cref="FileStream"/> object.
        /// This also determines the values returned by the <c>CanRead</c> and <c>CanWrite</c> properties of the <see cref="FileStream"/> object.
        /// <c>CanSeek</c> is true if path specifies a disk file.
        /// </param>
        /// <param name="share">A bitwise combination of the enumeration values that determines how the file will be shared by processes.</param>
        /// <returns>A file stream contained by the specified <c>filePath</c>.</returns>
        /// <exception>See <see href="https://learn.microsoft.com/en-us/dotnet/api/system.io.file.open?view=net-8.0"/>.</exception>
        FileStream OpenFile(string filePath, FileAccess access, FileShare share);

        /// <summary>
        /// Opens a file on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.
        /// <see cref="FileMode"/> is <c>Open</c>.
        /// </summary>
        /// <param name="filePath">The file path to create a stream.</param>
        /// <param name="access">
        /// A bitwise combination of the enumeration values that determines how the file can be accessed by the <see cref="FileStream"/> object.
        /// This also determines the values returned by the <c>CanRead</c> and <c>CanWrite</c> properties of the <see cref="FileStream"/> object.
        /// <c>CanSeek</c> is true if path specifies a disk file.
        /// </param>
        /// <returns>A file stream contained by the specified <c>filePath</c>.</returns>
        /// <exception>See <see href="https://learn.microsoft.com/en-us/dotnet/api/system.io.file.open?view=net-8.0"/>.</exception>
        FileStream OpenFile(string filePath, FileAccess access);

        /// <summary>
        /// Opens a file on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.
        /// <see cref="FileMode"/> is <c>Open</c>.
        /// </summary>
        /// <param name="filePath">The file path to create a stream.</param>
        /// <returns>A file stream contained by the specified <c>filePath</c>.</returns>
        /// <exception>See <see href="https://learn.microsoft.com/en-us/dotnet/api/system.io.file.open?view=net-8.0"/>.</exception>
        FileStream OpenFile(string filePath);

        /// <summary>
        /// Opens a file on the specified path as read-only.
        /// </summary>
        /// <param name="filePath">The file path to create a stream.</param>
        /// <returns>A file stream contained by the specified <c>filePath</c>.</returns>
        /// <exception>See <see href="https://learn.microsoft.com/en-us/dotnet/api/system.io.file.open?view=net-8.0"/>.</exception>
        FileStream OpenReadFile(string filePath);

        /// <summary>
        /// Opens or create a file on the specified path as read-only.
        /// </summary>
        /// <param name="filePath">The file path to create a stream.</param>
        /// <returns>A file stream contained by the specified <c>filePath</c>.</returns>
        /// <exception>See <see href="https://learn.microsoft.com/en-us/dotnet/api/system.io.file.open?view=net-8.0"/>.</exception>
        FileStream OpenOrCreateFile(string filePath);

        /// <summary>
        /// Opens or create a file on the specified path.
        /// </summary>
        /// <param name="filePath">The file path to create a stream.</param>
        /// <param name="access">
        /// A bitwise combination of the enumeration values that determines how the file can be accessed by the <see cref="FileStream"/> object.
        /// This also determines the values returned by the <c>CanRead</c> and <c>CanWrite</c> properties of the <see cref="FileStream"/> object.
        /// <c>CanSeek</c> is true if path specifies a disk file.
        /// </param>
        /// <returns>A file stream contained by the specified <c>filePath</c>.</returns>
        /// <exception>See <see href="https://learn.microsoft.com/en-us/dotnet/api/system.io.file.open?view=net-8.0"/>.</exception>
        FileStream OpenOrCreateFile(string filePath, FileAccess access);

        /// <summary>
        /// Opens or create a file on the specified path.
        /// </summary>
        /// <param name="filePath">The file path to create a stream.</param>
        /// <param name="access">
        /// A bitwise combination of the enumeration values that determines how the file can be accessed by the <see cref="FileStream"/> object.
        /// This also determines the values returned by the <c>CanRead</c> and <c>CanWrite</c> properties of the <see cref="FileStream"/> object.
        /// <c>CanSeek</c> is true if path specifies a disk file.
        /// </param>
        /// <param name="share">A bitwise combination of the enumeration values that determines how the file will be shared by processes.</param>
        /// <returns>A file stream contained by the specified <c>filePath</c>.</returns>
        /// <exception>See <see href="https://learn.microsoft.com/en-us/dotnet/api/system.io.file.open?view=net-8.0"/>.</exception>
        FileStream OpenOrCreateFile(string filePath, FileAccess access, FileShare share);

        /// <summary>
        /// Opens a text file, reads all the text in the file, and then closes the file.
        /// </summary>
        /// <param name="filePath">The path of file to read.</param>
        /// <returns>A string containing all the text in the file.</returns>
        /// <exception>See <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.io.file.readalltext?view=net-8.0#system-io-file-readalltext(system-string)"/>.</exception>
        string ReadAllText(string filePath);

        /// <summary>
        /// Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="filePath">The path of file to read.</param>
        /// <param name="encoding">The encoding applied to the contents of the file. The deafult encording is UTF-8.</param>
        /// <returns>A string array containing all lines of the file.</returns>
        /// <exception cref="Exception">See <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.io.file.readalllines"/>.</exception>
        IReadOnlyCollection<string> ReadAllLine(string filePath, Encoding encoding = null);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="path">The path of the file to delete.</param>
        /// <returns>The deleted file information.</returns>
        /// <exception cref="ArgumentNullException"><c>path</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><c>path</c> is a zero-length string, contains only white space, or contains one or more invalid characters.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="IOException">
        /// The specified file is in use.
        /// -or-
        /// There is an open handle on the file, and the operating system is Windows XP or earlier.This open handle can result from enumerating directories and files. </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="NotSupportedException"><c>path</c> is in an invalid format</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// -or-n
        /// The file is an executable file that is in use.
        /// -or-
        /// <c>path</c> is a directory.
        /// </exception>
        FileInfo DeleteFile(string path);

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="path">The directory path to delete.</param>
        /// <returns>The deleted directory information.</returns>
        /// <exception cref="ArgumentNullException"><c>path</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><c>path</c> is a zero-length string, contains only white space, or contains one or more invalid characters.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="IOException">
        /// A file with the same name and location specified by <c>path</c> exists.
        /// -or-
        /// The directory is the application's current working directory.
        /// -or-
        /// The directory specified by <c>path</c> is not empty.
        /// -or-
        /// The directory is read-only or contains a read-only file
        /// -or-
        /// The directory is being used by another process.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// <c>path</c> does not exist or could not be found.
        /// -or-
        /// The specified path is invalid(for example, it is on an unmapped drive).
        /// </exception>
        /// <exception cref="UnauthorizedAccessException" />
        /// <remarks>This methods throws exceptions. See the details on <see href="https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.delete?view=netframework-4.8"/>.</remarks>
        DirectoryInfo DeleteDirectory(string path);

        /// <summary>
        /// Deletes the files or sub-directories in the specified directory.
        /// </summary>
        /// <param name="directoryPath">The path to clean.</param>
        /// <param name="recursive">The value indicating deletes also the sub directories.</param>
        /// <exception cref="ArgumentNullException"><c>directoryPath</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><c>path</c> is a zero-length string, contains only white space, or contains one or more invalid characters.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        void CleanDirectory(string directoryPath, bool recursive);

        #endregion

        #region Copy

        /// <summary>
        /// Copies a file from one directory to another.
        /// </summary>
        /// <param name="sourceFilePath">Path of a source file.</param>
        /// <param name="destFilePath">Path of a destination file.</param>
        /// <param name="overwrite">Overwrite option.</param>
        /// <exception cref="ArgumentNullException"><c>sourceFilePath</c> or <c>destFilePath</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><para><c>sourceFilePath</c> or <c>destFilePath</c> specifies a directory.</para> or <para><c>sourceFileName</c> or <c>destFileName</c> is a zero-length string, contains only white space, or contains one or more invalid characters.</para></exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="IOException"><c>destFilePath</c> exists and <c>overwrite</c> is <c>false</c>.-or-An I/O error has occurred.</exception>
        /// <exception cref="FileNotFoundException"><c>sourceFilePath</c> was not found.</exception>
        /// <exception cref="DirectoryNotFoundException">The path specified in <c>sourceFilePath</c> or <c>destFilePath</c> is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="NotSupportedException"><c>sourceFilePath</c> or <c>destinationFilePath</c> is in an invalid format.</exception>
        /// <exception cref="UnauthorizedAccessException">https://learn.microsoft.com/en-us/dotnet/api/system.io.file.copy?view=net-8.0#system-io-file-copy(system-string-system-string-system-boolean):~:text=The%20caller%20does,is%20not%20hidden.</exception>
        void CopyFile(string sourceFilePath, string destFilePath, bool overwrite);

        /// <summary>
        /// Flat copy all the files from one directory to another.
        /// </summary>
        /// <param name="sourceDirPath">Path of a source directory.</param>
        /// <param name="destDirPath">Path of a destination directory.</param>
        /// <exception cref="ArgumentNullException"><c>sourceDirPath</c> or <c>destDirPath</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><para><c>sourceDirPath</c> or <c>destDirPath</c> specifies a directory.</para> or <para><c>sourceFileName</c> or <c>destFileName</c> is a zero-length string, contains only white space, or contains one or more invalid characters.</para></exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="IOException">An I/O error has occurred.</exception>
        /// <exception cref="DirectoryNotFoundException">The path specified in <c>sourceDirPath</c> or <c>destDirPath</c> is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="NotSupportedException"><c>sourceDirPath</c> or <c>destDirPath</c> is in an invalid format.</exception>
        /// <exception cref="UnauthorizedAccessException">https://learn.microsoft.com/en-us/dotnet/api/system.io.file.copy?view=net-8.0#system-io-file-copy(system-string-system-string-system-boolean):~:text=The%20caller%20does,is%20not%20hidden.</exception>
        /// <returns>The collection of the file which was copied sucessfully.</returns>
        IEnumerable<FileInfo> CopyFiles(string sourceDirPath, string destDirPath);

        /// <summary>
        /// Tries to flat copy all the files from one directory to another.
        /// </summary>
        /// <param name="sourceDirPath">Path of a source directory.</param>
        /// <param name="destDirPath">Path of a destination directory.</param>
        /// <returns>The tuple of the value indicating copying all files fails and the collection of the copied file.</returns>
        (bool IsFailed, IEnumerable<FileInfo> Copied) TryCopyFiles(string sourceDirPath, string destDirPath);

        #endregion

        #region Modify

        /// <summary>
        /// Adds a file extension if the target filename including it.
        /// </summary>
        /// <param name="filename">The file name adding a file extension to.</param>
        /// <param name="extension">The file extension.</param>
        /// <returns>A file name with <c>extension</c> if <c>filename</c> has no extension. Otherwise; <c>filename</c>.</returns>
        string AddExtensionIfNotHave(string filename, string extension);

        /// <summary>
        /// Changes the extension of a path string.
        /// If <paramref name="extension"/> is set to <c>null</c>, removes an extension from <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path information to modify.</param>
        /// <param name="extension">
        /// The new extension (with or without a leading period). Specify <c>null</c> to remove an existing extension from <c>path</c>.
        /// </param>
        /// <returns>The modified path. If the change fails, returns an empty string.</returns>
        string ChangeExtension(string path, string extension);

        /// <summary>
        /// Changes file name.
        /// If <paramref name="filename"/> is set to <c>null</c>, removes an existing file name.
        /// </summary>
        /// <param name="path">The path information to modify.</param>
        /// <param name="filename">The new file name.</param>
        /// <returns>The modified path. If the change fails, returns an empty string.</returns>
        string ChangeFilename(string path, string filename);

        /// <summary>
        /// Creates a new file, writes the specified string to the file,
        /// and then closes the file. If the target file already exists, it is truncated and overwritten.
        /// </summary>
        /// <param name="filePath">The path of file to read.</param>
        /// <param name="fileContent">The file content.</param>
        /// <exception>See <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.io.file.writealltext?view=net-8.0#system-io-file-writealltext(system-string-system-string)"/>.</exception>
        void WriteAllText(string filePath, string fileContent);

        #endregion

        #region Query

        /// <summary>
        /// Gets a file information.
        /// </summary>
        /// <param name="filename">The fully qualified name of the new file, or the relative file name. Do not end the path with the directory separator character..</param>
        /// <returns>The file information if the <c>fileName</c> is valid and the caller has the required permission, otherwise, returns <c>null</c>.</returns>
        FileInfo? GetFileInfo(string filename);

        /// <summary>
        /// Gets a directory information.
        /// </summary>
        /// <param name="path">A string specifying the path on which to create the <c>DirectoryInfo</c>.</param>
        /// <returns>The directory information if the <c>path</c> is valid and the caller has the required permission, otherwise, returns <c>null</c>.</returns>
        DirectoryInfo? GetDirectoryInfo(string path);

        /// <summary>
        /// Retrieves the parent directory of the specified path, including both absolute and relative paths.
        /// </summary>
        /// <param name="path">The path for which to retrieve the parent directory.</param>
        /// <returns>The parent directory path, or <c>null</c> if <c>path</c> is the root directory, including the root of a UNC server or share name.</returns>
        /// <exception cref="ArgumentNullException"><c>path</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><c>path</c> is a zero-length string, contains only white space, or contains one or more invalid characters.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">The directory specified by <c>path</c> is read-only.</exception>
        /// <exception cref="NotSupportedException"><c>path</c> is in an invalid format.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="UnauthorizedAccessException"><para>The caller does not have the required permission.</para> or <para><c>path</c> specified a file that is read-only.</para> or <para>path specified a file that is hidden.</para></exception>
        DirectoryInfo GetParent(string path);

        /// <summary>
        /// Gets a file extension. The default file extension has dot like '.txt'.
        /// To get a file extension without dot like 'txt', sets <paramref name="removesDot"/> to <c>true</c>.
        /// </summary>
        /// <param name="filename">The file name.</param>
        /// <param name="removesDot">The value indicating removing the dot character from the file extension.</param>
        /// <returns>A file extension, if the file name has a file extension. Otherwise; an empty string.</returns>
        string GetExtension(string filename, bool removesDot = false);

        /// <summary>
        /// Gets the filename from the file path.
        /// </summary>
        /// <param name="path">Path of a file.</param>
        /// <returns>A filename from path</returns>
        string GetFileName(string path);

        /// <summary>
        /// Gets the directory name from the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A directory name which has the file.
        /// Returns <c>null</c> if path denotes a root directory or is <c>null</c>.
        /// Returns <paramref name="defaultDirName"/> if path does not contain directory information.
        /// </returns>
        string GetDirectoryName(string path, string defaultDirName = "");

        /// <summary>
        /// Gets the filename without extension from the file path.
        /// </summary>
        /// <param name="path">Path of a file.</param>
        /// <returns>A filename without the file extension.</returns>
        string GetFileNameWithoutExtension(string path);

        /// <summary>
        /// Enumerates all files in the specified directory.
        /// </summary>
        /// <param name="directoryPath">The path of the directory to be searched.</param>
        /// <param name="recursive">Whether to also perform the action on files in all subdirectories.</param>
        /// <exception cref="ArgumentNullException"><c>directoryPath</c> or <c>searchPattern</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><para><c>directoryPath</c> is a zero-length string, contains only white space, or contains one or more invalid characters</para>.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="IOException"><c>directoryPath</c> is a file name..</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permissions.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permissions.</exception>
        /// <returns>An enumerable collection of the file paths found in the specified directory.</returns>
        IEnumerable<string> EnumerateFiles(string directoryPath, bool recursive);

        /// <summary>
        /// Enumerates files in the specified directory.
        /// </summary>
        /// <param name="directoryPath">The path of the directory to be searched.</param>
        /// <param name="searchPattern">The search string to match against the file names.</param>
        /// <param name="recursive">Whether to also perform the action on files in all subdirectories.</param>
        /// <exception cref="ArgumentNullException"><c>directoryPath</c> or <c>searchPattern</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><para><c>directoryPath</c> is a zero-length string, contains only white space, or contains one or more invalid characters.</para> or <para><c>searchPattern</c> does not contain a valid pattern</para></exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="IOException"><c>directoryPath</c> is a file name..</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permissions.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permissions.</exception>
        /// <returns>An enumerable collection of the file paths found in the specified directory.</returns>
        IEnumerable<string> EnumerateFiles(string directoryPath, string searchPattern, bool recursive);

        /// <summary>
        /// Enumerates all files in the specified directory.
        /// </summary>
        /// <param name="directoryPath">The path of the directory to be searched.</param>
        /// <param name="recursive">Whether to also perform the action on files in all subdirectories.</param>
        /// <exception cref="ArgumentNullException"><c>directoryPath</c> or <c>searchPattern</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><para><c>directoryPath</c> is a zero-length string, contains only white space, or contains one or more invalid characters</para>.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="IOException"><c>directoryPath</c> is a file name..</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permissions.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permissions.</exception>
        /// <returns>An enumerable collection of the file paths found in the specified directory.</returns>
        IEnumerable<FileInfo> EnumerateFileInfos(string directoryPath, bool recursive);

        /// <summary>
        /// Enumerates files in the specified directory.
        /// </summary>
        /// <param name="directoryPath">The path of the directory to be searched.</param>
        /// <param name="searchPattern">The search string to match against the file names.</param>
        /// <param name="recursive">Whether to also perform the action on files in all subdirectories.</param>
        /// <exception cref="ArgumentNullException"><c>directoryPath</c> or <c>searchPattern</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><para><c>directoryPath</c> is a zero-length string, contains only white space, or contains one or more invalid characters.</para> or <para><c>searchPattern</c> does not contain a valid pattern</para></exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="IOException"><c>directoryPath</c> is a file name..</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permissions.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permissions.</exception>
        /// <returns>An enumerable collection of the file paths found in the specified directory.</returns>
        IEnumerable<FileInfo> EnumerateFileInfos(string directoryPath, string searchPattern, bool recursive);

        /// <summary>
        /// Enumerates subdirectories in the specified directory
        /// </summary>
        /// <param name="directoryPath">The path of the directory to be searched.</param>
        /// <param name="recursive">Whether to also perform the action on all subdirectories.</param>
        /// <returns>An enumerable collection of the directory paths found in the specified directory.</returns>
        /// <exception cref="ArgumentNullException"><c>directoryPath</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><c>directoryPath</c> is a zero-length string, contains only white space, or contains one or more invalid characters.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="IOException"><c>directoryPath</c> is a file name..</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permissions.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permissions.</exception>
        IEnumerable<string> EnumerateDirectories(string directoryPath, bool recursive);

        /// <summary>
        /// Enumerates subdirectories in the specified directory
        /// </summary>
        /// <param name="directoryPath">The path of the directory to be searched.</param>
        /// <param name="recursive">Whether to also perform the action on all subdirectories.</param>
        /// <returns>An enumerable collection of the directory paths found in the specified directory.</returns>
        /// <exception cref="ArgumentNullException"><c>directoryPath</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><c>directoryPath</c> is a zero-length string, contains only white space, or contains one or more invalid characters.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="IOException"><c>directoryPath</c> is a file name..</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permissions.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permissions.</exception>
        IEnumerable<DirectoryInfo> EnumerateDirectoryInfos(string directoryPath, bool recursive);

        #endregion
    }
}
