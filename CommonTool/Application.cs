//@BaseCode
//MdStart
using CommonTool.Extensions;

namespace CommonTool
{
    /// <summary>
    /// Represents the base application class.
    /// </summary>
    /// <remarks>
    /// This class provides common properties and methods for the application.
    /// </remarks>
    public abstract partial class Application
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the <see cref="Application"/> class.
        /// </summary>
        /// <remarks>
        /// This static constructor sets up the necessary properties for the program.
        /// </remarks>
        static Application()
        {
            ClassConstructing();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            HomePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
            Environment.OSVersion.Platform == PlatformID.MacOSX)
            ? Environment.GetEnvironmentVariable("HOME")
            : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

            UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            SourcePath = Path.Combine(UserPath, "source");
            if (Directory.Exists(SourcePath) == false)
            {
                SourcePath = UserPath;
            }
            SolutionPath = GetCurrentSolutionPath();
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

        #region Instance-Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application()
        {
            Constructing();
            Constructed();
        }
        /// <summary>
        /// This method is called during the construction of the object.
        /// </summary>
        partial void Constructing();
        /// <summary>
        /// This method is called when the object is constructed.
        /// </summary>
        partial void Constructed();
        #endregion Instance-Constructors

        #region properties
        /// <summary>
        /// Gets or sets a value indicating whether the operation should be forced.
        /// </summary>
        public static bool Force { get; set; } = false;
        /// <summary>
        /// Gets or sets the home path.
        /// </summary>
        public static string? HomePath { get; set; }
        /// <summary>
        /// Gets or sets the user path.
        /// </summary>
        public static string UserPath { get; set; }
        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        public static string SourcePath { get; set; }
        /// <summary>
        /// Gets or sets the solution path.
        /// </summary>
        public static string SolutionPath { get; set; }
        #endregion properties

        #region methods for changing the properties
        /// <summary>
        /// Changes the value of the Force property.
        /// </summary>
        public static void ChangeForce() => Force = !Force;
        /// <summary>
        /// Gets the current solution path.
        /// </summary>
        /// <returns>The current solution path.</returns>
        public static string GetCurrentSolutionPath()
        {
            var result = string.Empty;
            var paths = AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar);
            var checkPath = paths.Length > 0 && paths[0].IsNullOrEmpty() ? $"{Path.DirectorySeparatorChar}" : string.Empty;
            var index = paths.Length > 0 && paths[0].IsNullOrEmpty() ? 1 : 0;

            while (result.IsNullOrEmpty() && index < paths.Length)
            {
                var folder = paths[index++];
                var fileName = $"{folder}.sln";

                checkPath = Path.Combine(checkPath, folder);

                var filePath = Path.Combine(checkPath, fileName);

                if (File.Exists(filePath))
                {
                    result = checkPath;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the parent directory of the specified path.
        /// </summary>
        /// <param name="path">The path to get the parent directory of.</param>
        /// <returns>The parent directory of the specified path.</returns>
        public static string GetParentFromPath(string path)
        {
            var result = Directory.GetParent(path);

            return result != null ? result.FullName : path;
        }

        /// <summary>
        /// Finds files from the specified path and its parent path that match the given search pattern.
        /// </summary>
        /// <param name="path">The path to search for files.</param>
        /// <param name="searchPattern">The search pattern to match against the names of files in the specified path.</param>
        /// <returns>A list of file paths that match the search pattern.</returns>
        public static List<string> FindFilesFromPathAndParentPath(string path, string searchPattern)
        {
            var result = new List<string>();

            if (Directory.Exists(path))
            {
                result.AddRange(Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly));
            }

            var parentPath = GetParentFromPath(path);

            if (Directory.Exists(parentPath))
            {
                result.AddRange(Directory.GetFiles(parentPath, searchPattern, SearchOption.TopDirectoryOnly));
            }
            return result;
        }

        /// <summary>
        /// Retrieves a collection of source code files in the specified directory and its subdirectories based on the given search patterns.
        /// </summary>
        /// <param name="path">The directory path to search for source code files.</param>
        /// <param name="searchPatterns">An array of search patterns to match against the names of files in the specified directory and its subdirectories.</param>
        /// <returns>A collection of source code file paths.</returns>
        public static List<string> GetSourceCodeFiles(string path, string[] searchPatterns)
        {
            var result = new List<string>();
            var ignoreFolders = new string[] { $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", $"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}" };

            foreach (var searchPattern in searchPatterns)
            {
                result.AddRange(Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories)
                      .Where(f => ignoreFolders.Any(e => f.Contains(e)) == false)
                      .OrderBy(i => i));
            }
            return result;
        }
        /// <summary>
        /// Retrieves the distinct directory paths of source code files that match the specified search patterns within the given path.
        /// </summary>
        /// <param name="path">The root path to search for source code files.</param>
        /// <param name="searchPatterns">An array of search patterns to match against the names of the source code files.</param>
        /// <returns>A list of distinct directory paths containing the source code files.</returns>
        public static List<string> GetSourceCodePaths(string path, string[] searchPatterns)
        {
            var result = GetSourceCodeFiles(path, searchPatterns);

            return [.. result.Select(f => Path.GetDirectoryName(f) ?? string.Empty).Distinct().Order()];
        }
        #endregion methods for changing the properties
    }
}
//MdEnd