//@BaseCode
//MdStart
using System.Diagnostics;
using System.Reflection;
using CommonTool.Extensions;

namespace CommonTool
{
    public partial class TemplatePath
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the <see cref="Program"/> class.
        /// </summary>
        /// <remarks>
        /// This static constructor sets up the necessary properties for the program.
        /// </remarks>
        static TemplatePath()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called during the construction of the class.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// Represents a method that is called when a class is constructed.
        /// </summary>
        static partial void ClassConstructed();
        #endregion Class-Constructors

        #region methods
        /// <summary>
        /// Retrieves an array of sub-paths within the specified start path, up to the specified maximum depth.
        /// </summary>
        /// <param name="startPath">The starting path from which to retrieve sub-paths.</param>
        /// <param name="maxDepth">The maximum depth of sub-paths to retrieve.</param>
        /// <returns>An array of sub-paths within the specified start path.</returns>
        public static string[] GetSubPaths(string startPath, int maxDepth)
        {
            return QueryDirectoryStructure(startPath, n => n.StartsWith($"{Path.DirectorySeparatorChar}.") == false, maxDepth, "bin", "obj", "node_modules");
        }
        /// <summary>
        /// Retrieves an array of template paths from the specified start path.
        /// </summary>
        /// <param name="startPath">The starting path to search for templates.</param>
        /// <param name="maxDepth">The maximum depth to search for templates.</param>
        /// <returns>An array of template paths.</returns>
        public static string[] GetTemplatePaths(string startPath, int maxDepth)
        {
            return QueryDirectoryStructure(startPath, n => n.StartsWith("QT") || n.Equals("QuickTemplate"), maxDepth, "bin", "obj", "node_modules");
        }
        /// <summary>
        /// Retrieves an array of template solutions from the specified start path up to the specified maximum depth.
        /// </summary>
        /// <param name="startPath">The starting path to search for template solutions.</param>
        /// <param name="maxDepth">The maximum depth to search for template solutions.</param>
        /// <returns>An array of template solutions found within the specified start path and maximum depth.</returns>
        public static string[] GetTemplateSolutions(string startPath, int maxDepth)
        {
            var result = new List<string>();
            var qtPaths = GetTemplatePaths(startPath, maxDepth);

            foreach (var qtPath in qtPaths)
            {
                var di = new DirectoryInfo(qtPath);

                if (di.GetFiles().Any(f => Path.GetExtension(f.Name).Equals(".sln", StringComparison.CurrentCultureIgnoreCase)))
                {
                    result.Add(qtPath);
                }
            }
            return [.. result];
        }
        /// <summary>
        /// Queries the directory structure starting from the specified path and returns an array of directory paths that match the specified criteria.
        /// </summary>
        /// <param name="path">The root path from which to start querying the directory structure.</param>
        /// <param name="filter">An optional filter function to apply to each directory name. Only directories that satisfy the filter will be included in the result.</param>
        /// <param name="maxDepth">The maximum depth of the directory structure to query.</param>
        /// <param name="excludeFolders">An array of folder names to exclude from the result.</param>
        /// <returns>An array of directory paths that match the specified criteria.</returns>
        public static string[] QueryDirectoryStructure(string path, Func<string, bool>? filter, int maxDepth, params string[] excludeFolders)
        {
            static void GetDirectoriesWithoutHidden(Func<string, bool>? filter, DirectoryInfo directoryInfo, List<string> list, int maxDeep, int deep, params string[] excludeFolders)
            {
                try
                {
                    if (directoryInfo.Attributes.HasFlag(FileAttributes.Hidden) == false)
                    {
                        if (filter == null || filter(directoryInfo.Name))
                        {
                            list.Add(directoryInfo.FullName);
                        }
                        if (maxDeep < 0 || deep < maxDeep)
                        {
                            foreach (var di in directoryInfo.GetDirectories())
                            {
                                if (excludeFolders.Any(e => e.Equals(di.Name, StringComparison.CurrentCultureIgnoreCase)) == false)
                                {
                                    GetDirectoriesWithoutHidden(filter, di, list, maxDeep, deep + 1, excludeFolders);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in {MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                }
            }
            var result = new List<string>();
            var directoryInfo = new DirectoryInfo(path);

            GetDirectoriesWithoutHidden(filter, directoryInfo, result, maxDepth, 0, excludeFolders);
            return [.. result];
        }

        /// <summary>
        /// Retrieves the parent paths of the templates based on the start path and maximum depth.
        /// </summary>
        /// <param name="startPath">The starting path to search for templates.</param>
        /// <param name="maxDepth">The maximum depth to search for templates.</param>
        /// <param name="includePaths">Additional paths to include in the result.</param>
        /// <returns>An array of parent paths of the templates.</returns>
        public static string[] GetTemplateParentPaths(string startPath, int maxDepth, params string[] includePaths)
        {
            var result = new List<string>();
            var qtProjects = GetTemplatePaths(startPath, maxDepth).Union(includePaths).ToArray();
            var qtPaths = qtProjects.Select(p => GetParentDirectory(p))
                                    .Distinct()
                                    .OrderBy(p => p);

            foreach (var qtPath in qtPaths)
            {
                if (result.Any(x => qtPath.Length > x.Length && qtPath.Contains(x)) == false)
                {
                    result.Add(qtPath);
                }
            }
            return [.. result];
        }
        /// <summary>
        /// Returns the parent directory of the specified path.
        /// </summary>
        /// <param name="path">The path for which to retrieve the parent directory.</param>
        /// <returns>
        /// The full path of the parent directory of the specified path if it exists; otherwise, the original path is returned.
        /// </returns>
        /// <remarks>
        /// This method uses the <see cref="Directory.GetParent(string)"/> method to retrieve the parent directory of the specified path.
        /// It returns the full path of the parent directory if it exists; otherwise, it returns the original path.
        /// </remarks>
        public static string GetParentDirectory(string path)
        {
            var result = Directory.GetParent(path);

            return result != null ? result.FullName : path;
        }

        /// <summary>
        /// Determines whether the specified path is a solution path.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the specified path is a solution path; otherwise, <c>false</c>.</returns>
        public static bool IsSolutionPath(string path)
        {
            return path.HasContent() && GetSolutionItemDataFromPath(path, ".sln").Name.HasContent();
        }
        /// <summary>
        /// Determines whether the specified file path is a solution file path.
        /// </summary>
        /// <param name="filePath">The file path to check.</param>
        /// <returns><c>true</c> if the file path is a solution file path; otherwise, <c>false</c>.</returns>
        public static bool IsSolutionFilePath(string filePath)
        {
            var path = Path.GetDirectoryName(filePath);

            return IsProjectPath(path ?? string.Empty);
        }
        /// <summary>
        /// Retrieves the solution name from the given file path.
        /// </summary>
        /// <param name="path">The path of the solution file.</param>
        /// <returns>The name of the solution file.</returns>
        public static string GetSolutionNameFromPath(string path)
        {
            return GetSolutionItemDataFromPath(path, ".sln").Name;
        }
        /// <summary>
        /// Determines whether the specified path is a project path.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>true</c> if the specified path is a project path; otherwise, <c>false</c>.</returns>
        public static bool IsProjectPath(string path)
        {
            return path.HasContent() && GetSolutionItemDataFromPath(path, ".csproj").Name.HasContent();
        }
        /// <summary>
        /// Determines whether the given file path is a project file path.
        /// </summary>
        /// <param name="filePath">The file path to check.</param>
        /// <returns><c>true</c> if the file path is a project file path; otherwise, <c>false</c>.</returns>
        public static bool IsProjectFilePath(string filePath)
        {
            var path = Path.GetDirectoryName(filePath);

            return IsProjectPath(path ?? string.Empty);
        }

        /// <summary>
        /// Gets the project name from the given path.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>The project name.</returns>
        public static string GetProjectNameFromPath(string path)
        {
            return GetSolutionItemDataFromPath(path, ".csproj").Name;
        }

        /// <summary>
        /// Gets the sub file path by extracting the file path relative to the .csproj file within the solution.
        /// </summary>
        /// <param name="filePath">The full file path.</param>
        /// <returns>The sub file path relative to the .csproj file within the solution.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the filePath is null.</exception>
        public static string GetProjectSubFilePath(string filePath)
        {
            var subPath = GetSolutionItemDataFromPath(filePath, ".csproj").SubPath;
            var result = filePath!.Replace(subPath, string.Empty);

            if (result.StartsWith(Path.DirectorySeparatorChar))
            {
                result = result[1..];
            }
            return result;
        }

        /// <summary>
        /// Gets the sub file path of a solution file.
        /// </summary>
        /// <param name="filePath">The full file path.</param>
        /// <returns>The sub file path.</returns>
        public static string GetSolutionSubFilePath(string filePath)
        {
            var subPath = GetSolutionItemDataFromPath(filePath, ".sln").SubPath;
            var result = filePath!.Replace(subPath, string.Empty);

            if (result.StartsWith(Path.DirectorySeparatorChar))
            {
                result = result[1..];
            }
            return result;
        }

        /// <summary>
        /// Retrieves the solution item data from the given path and item extension.
        /// </summary>
        /// <param name="path">The path to search for the solution item.</param>
        /// <param name="itemExtension">The extension of the solution item.</param>
        /// <returns>A tuple containing the name and subpath of the solution item.</returns>
        public static (string Name, string SubPath) GetSolutionItemDataFromPath(string path, string itemExtension)
        {
            var itemName = string.Empty;
            var subPath = path.StartsWith(Path.DirectorySeparatorChar) ? Path.DirectorySeparatorChar.ToString() : string.Empty;
            var itemsEnumerator = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries)
                                      .GetEnumerator();

            while (itemName.IsNullOrEmpty() && itemsEnumerator.MoveNext())
            {
                subPath = Path.Combine(subPath, itemsEnumerator.Current.ToString()!);

                var filePath = Path.Combine(subPath, $"{itemsEnumerator.Current}{itemExtension}");

                if (File.Exists(filePath))
                {
                    itemName = itemsEnumerator.Current.ToString() ?? string.Empty;
                }
            }
            return (itemName, subPath);
        }

        /// <summary>
        /// Retrieves the path from the given path by checking for the presence of a file with the specified extension.
        /// </summary>
        /// <param name="path">The original path.</param>
        /// <param name="checkFileExtension">The file extension to check for.</param>
        /// <returns>
        /// The path up to the directory where the first file with the specified extension is found,
        /// or an empty string if no such file is found.
        /// </returns>
        public static string GetPathFromPath(string path, string checkFileExtension)
        {
            var result = string.Empty;
            var checkPath = path.StartsWith(Path.DirectorySeparatorChar) ? Path.DirectorySeparatorChar.ToString() : string.Empty;
            var data = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < data.Length && result == string.Empty; i++)
            {
                checkPath = Path.Combine(checkPath, data[i]);

                var projectFilePath = Path.Combine(checkPath, $"{data[i]}{checkFileExtension}");

                if (File.Exists(projectFilePath))
                {
                    result = checkPath;
                }
            }
            return result;
        }
        /// <summary>
        /// Retrieves the directory name from a given path by checking for a specific file extension.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <param name="checkFileExtension">The file extension to check for.</param>
        /// <returns>
        /// The directory name that contains the file with the specified extension, or an empty string if no such file is found.
        /// </returns>
        public static string GetDirectoryNameFromPath(string path, string checkFileExtension)
        {
            var result = string.Empty;
            var checkPath = path.StartsWith(Path.DirectorySeparatorChar) ? Path.DirectorySeparatorChar.ToString() : string.Empty;
            var data = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < data.Length && result == string.Empty; i++)
            {
                checkPath = Path.Combine(checkPath, data[i]);

                var projectFilePath = Path.Combine(checkPath, $"{data[i]}{checkFileExtension}");

                if (File.Exists(projectFilePath))
                {
                    result = data[i];
                }
            }
            return result;
        }

        /// <summary>
        /// Recursively deletes files and empty directories from the specified path,
        /// excluding directories specified in the dropFolders parameter.
        /// </summary>
        /// <param name="path">The root path from where the cleaning operation begins.</param>
        /// <param name="dropFolders">The array of folder names to exclude from deletion.</param>
        public static void CleanDirectories(string path, params string[] dropFolders)
        {
            static int CleanDirectories(DirectoryInfo dirInfo, params string[] dropFolders)
            {
                int result = 0;

                try
                {
                    result = dirInfo.GetFiles().Length;
                    foreach (var item in dirInfo.GetDirectories())
                    {
                        int fileCount = CleanDirectories(item, dropFolders);

                        try
                        {
                            if (fileCount == 0)
                            {
                                item.Delete();
                            }
                            else if ((dropFolders.FirstOrDefault(df => item.FullName.EndsWith(df))) != null)
                            {
                                fileCount = 0;
                                item.Delete(true);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                        }
                        result += fileCount;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod()!.Name}: {ex.Message}");
                }
                return result;
            }
            CleanDirectories(new DirectoryInfo(path), dropFolders);
        }
        #endregion methods

        #region CLI argument methods
        /// <summary>
        /// Opens the solution folder in Windows Explorer.
        /// </summary>
        /// <param name="solutionPath">The path of the solution folder.</param>
        public static void OpenSolutionFolder(string solutionPath)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Process.Start(new ProcessStartInfo()
                {
                    WorkingDirectory = solutionPath,
                    FileName = "explorer",
                    Arguments = solutionPath,
                    CreateNoWindow = true,
                });
            }
        }
        #endregion CLI argument methods
    }
}
//MdEnd