//@BaseCode
//MdStart
using CommonTool.Extensions;

namespace CommonTool
{
    /// <summary>
    /// Represents an abstract console application.
    /// </summary>
    public abstract partial class ConsoleApplication : Application
    {
        #region Class-Constructors
        /// <summary>
        /// Initializes the <see cref="ConsoleApplication"/> class.
        /// </summary>
        /// <remarks>
        /// This static constructor sets up the necessary properties for the program.
        /// </remarks>
        static ConsoleApplication()
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

        #region Instance-Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleApplication"/> class.
        /// </summary>
        public ConsoleApplication()
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

        #region menuitem
        /// <summary>
        /// Represents a menu item in a console application.
        /// </summary>
        /// Represents a menu item.
        /// <remarks
        /// Gets or sets the unique key of the menu item.
        /// Gets or sets the non-unique optional key of the menu item.
        /// Gets or sets the displayed text of the menu item.
        /// Gets or sets the action to be performed when the menu item is selected.
        /// </remarks>
        protected partial class MenuItem
        {
            /// <summary>
            /// Gets or sets a value indicating whether the item is displayed.
            /// </summary>
            public bool IsDisplayed { get; set; } = true;
            /// <summary>
            /// Gets or sets the key.
            /// </summary>
            public required string Key { get; set; }

            /// <summary>
            /// Gets or sets the optional key.
            /// </summary>
            public string OptionalKey { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            public required string Text { get; set; }

            /// <summary>
            /// Gets or sets the action associated with the property.
            /// </summary>
            public required Action<MenuItem> Action { get; set; }

            /// <summary>
            /// Gets or sets the parameters for the console application.
            /// </summary>
            public Dictionary<string, object> Params { get; set; } = [];

            /// <summary>
            /// Gets or sets the foreground color of the console.
            /// </summary>
            public ConsoleColor ForegroundColor { get; set; } = ConsoleApplication.ForegroundColor;
            /// <summary>
            /// Gets or sets the tag for the console application.
            /// </summary>
            public string Tag { get; set; } = string.Empty;
        }
        #endregion menuitem

        #region properties
        /// <summary>
        /// The width of the menu key.
        /// </summary>
        protected const int MENU_KEY_WIDTH = 5;
        /// <summary>
        /// The width of the menu text.
        /// </summary>
        protected const int MENU_TEXT_WIDTH = 65;
        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        public static ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }
        /// <summary>
        /// Gets or sets the current page index.
        /// </summary>
        protected int PageIndex { get; set; } = 0;
        /// <summary>
        /// Gets or sets the page size for pagination.
        /// </summary>
        protected int PageSize { get; set; } = 10;
        #endregion properties

        #region console-methods
        /// <summary>
        /// Clears the console screen.
        /// </summary>
        /// <summary>
        /// Clears the console screen.
        /// </summary>
        public static void Clear()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Console.Clear();
            }
            else
            {
                var width = Console.BufferWidth;

                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < Console.BufferHeight; i++)
                {
                    Console.Write(new string(' ', width));
                }
                Console.SetCursorPosition(0, 0);
            }
        }
        /// <summary>
        /// Prints the specified message to the console.
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        /// <returns>The length of the printed message.</returns>
        public static int Print(string message)
        {
            Console.Write(message);
            return message.Length;
        }

        /// <summary>
        /// Prints a message with a specified character and length.
        /// </summary>
        /// <param name="chr">The character to print.</param>
        /// <param name="message">The message to print.</param>
        /// <param name="length">The desired length of the output.</param>
        public static void Print(char chr, string message, int length)
        {
            for (int i = message.Length; i < length; i++)
            {
                Console.Write(chr);
            }
            Console.Write(message);
        }
        /// <summary>
        /// Prints a new line to the console.
        /// </summary>
        public static void PrintLine()
        {
            Console.WriteLine();
        }
        /// <summary>
        /// Prints a message to the console and returns the length of the message.
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        /// <returns>The length of the message.</returns>
        public static int PrintLine(string message)
        {
            Console.WriteLine(message);
            return message.Length;
        }
        /// <summary>
        /// Prints a line consisting of a specified character repeated a specified number of times.
        /// </summary>
        /// <param name="chr">The character to be repeated.</param>
        /// <param name="count">The number of times the character should be repeated.</param>
        /// <returns>The length of the printed line.</returns>
        public static int PrintLine(char chr, int count)
        {
            string message = new(chr, count);

            Console.WriteLine(message);
            return message.Length;
        }

        /// <summary>
        /// Reads a line of input from the console.
        /// </summary>
        /// <returns>The line of input read from the console, or an empty string if no input is available.</returns>
        public static string ReadLine()
        {
            return Console.ReadLine() ?? string.Empty;
        }
        /// <summary>
        /// Reads a line of input from the console with the specified message.
        /// </summary>
        /// <param name="message">The message to display before reading the input.</param>
        /// <returns>The line of input read from the console.</returns>
        public static string ReadLine(string message)
        {
            Print(message);
            return ReadLine();
        }
        /// <summary>
        /// Gets the current cursor position in the console.
        /// </summary>
        /// <returns>A tuple containing the left and top position of the cursor.</returns>
        public static (int Left, int Top) GetCursorPosition()
        {
            return (Console.CursorLeft, Console.CursorTop);
        }
        /// <summary>
        /// Sets the position of the cursor in the console window.
        /// </summary>
        /// <param name="left">The column position of the cursor.</param>
        /// <param name="top">The row position of the cursor.</param>
        public static void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Converts the given label and text into a formatted label text.
        /// </summary>
        /// <param name="label">The label to be displayed.</param>
        /// <param name="text">The text to be displayed.</param>
        /// <returns>The formatted label text.</returns>
        public static string ToLabelText(string label, string text)
        {
            return ToLabelText(label, text, 20, '.');
        }
        /// <summary>
        /// Formats a label and text into a single string with a specified width and padding character.
        /// </summary>
        /// <param name="label">The label to be displayed.</param>
        /// <param name="text">The text to be displayed.</param>
        /// <param name="width">The total width of the resulting string.</param>
        /// <param name="chr">The character used for padding.</param>
        /// <returns>A formatted string with the label and text.</returns>
        public static string ToLabelText(string label, string text, int width, char chr)
        {
            var diff = width - label.Length;
            var space = new string(chr, Math.Max(0, diff));

            return $"{label}{space}{text}";
        }
        #endregion console-methods

        #region progressbar-properties
        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        /// <value>
        /// The foreground color of the console.
        /// </value>
        public static ConsoleColor ProgressBarForegroundColor { get; set; } = ForegroundColor;
        /// <summary>
        /// Gets or sets a value indicating whether the RunBusyProgress is active or not.
        /// </summary>
        private static bool RunProgressBar { get; set; }
        /// <summary>
        /// Indicates whether printing is allowed when the application is busy.
        /// </summary>
        public static bool CanProgressBarPrint { get; set; } = true;
        #endregion progressbar-properties

        #region app-properties
        /// <summary>
        /// Gets or sets the menu items.
        /// </summary>
        protected MenuItem[] MenuItems { get; set; } = [];
        /// <summary>
        /// Gets or sets a value indicating whether the application should continue running.
        /// </summary>
        protected bool RunApp { get; set; } = false;
        /// <summary>
        /// Gets or sets the maximum subpath depth.
        /// </summary>
        protected int MaxSubPathDepth { get; set; } = 3;
        #endregion app-properties

        #region progressbar-methods
        /// <summary>
        /// Prints a busy progress indicator in the console.
        /// </summary>
        public static void StartProgressBar()
        {
            static void Write(int cursorLeft, int cursorTop, string output)
            {
                var saveCursorTop = Console.CursorTop;
                var saveCursorLeft = Console.CursorLeft;
                var saveForeColor = Console.ForegroundColor;

                ForegroundColor = ConsoleColor.Green;
                SetCursorPosition(cursorLeft, cursorTop);
                Print(output);
                SetCursorPosition(saveCursorLeft, saveCursorTop);
                ForegroundColor = saveForeColor;
            }
            if (RunProgressBar == false)
            {
                var head = '>';
                var runSign = '=';
                var counter = 0;

                RunProgressBar = true;
                PrintLine();

                var (Left, Top) = GetCursorPosition();

                PrintLine();
                PrintLine();
                Task.Factory.StartNew(async () =>
                {
                    while (RunProgressBar)
                    {
                        if (CanProgressBarPrint)
                        {
                            if (Left > 65)
                            {
                                var timeInSec = counter / 5;

                                for (int i = 0; i <= Left; i++)
                                {
                                    Write(i, Top, " ");
                                }
                                Left = 0;
                            }
                            else
                            {
                                Write(Left++, Top, $"{runSign}{head}");
                            }

                            if (counter % 5 == 0)
                            {
                                Write(65, Top, $"{counter / 5,5} [sec]");
                            }
                        }
                        counter++;
                        await Task.Delay(200);
                    }
                });
            }
        }
        /// <summary>
        /// Stops the execution of the busy progress.
        /// </summary>
        public static void StopProgressBar()
        {
            RunProgressBar = false;
        }
        #endregion progressbar-methods

        #region abstract and virtual methods
        /// <summary>
        /// Prints the header information for the console application.
        /// </summary>
        protected virtual void PrintHeader()
        {
            var solutionPath = GetCurrentSolutionPath();
            var solutionName = TemplatePath.GetSolutionNameFromPath(solutionPath);

            PrintHeader(solutionName, new("Force flag:", Force),
                                      new("Page index:", PageIndex),
                                      new("Page size:", PageSize));
        }
        /// <summary>
        /// Prints the header information for the console application.
        /// </summary>
        /// <param name="title">The header text.</param>
        /// <param name="keyValuePairs">The header text.</param>
        protected virtual void PrintHeader(string title, params KeyValuePair<string, object>[] keyValuePairs)
        {
            var saveForeColor = ForegroundColor;

            Clear();
            ForegroundColor = ConsoleColor.Green;
            PrintLine('=', title.Length);
            PrintLine(title);
            PrintLine('=', title.Length);
            PrintLine();
            ForegroundColor = ConsoleColor.DarkYellow;
            if (keyValuePairs.Length > 0)
            {
                int maxLabel = keyValuePairs.Select(kv => kv.Key.Length).Max();

                foreach (var (key, value) in keyValuePairs)
                {
                    PrintLine($"{key.PadRight(maxLabel)} {value}");
                }
                PrintLine();
            }
            ForegroundColor = saveForeColor;
        }

        /// <summary>
        /// Creates an array of menu items.
        /// </summary>
        /// <returns>An array of <see cref="MenuItem"/> objects.</returns>
        protected abstract MenuItem[] CreateMenuItems();
        /// <summary>
        /// Represents a menu item in a console application.
        /// </summary>
        protected virtual MenuItem CreateMenuSeparator()
        {
            return new()
            {
                Key = new string('-', MENU_KEY_WIDTH),
                Text = new string('-', MENU_TEXT_WIDTH),
                Action = (self) => { },
                ForegroundColor = ConsoleColor.DarkGreen,
            };
        }
        /// <summary>
        /// Creates the exit menu items for the application.
        /// </summary>
        /// <returns>An array of MenuItem objects representing the exit menu items.</returns>
        protected virtual MenuItem[] CreateExitMenuItems()
        {
            return [ CreateMenuSeparator(),
                     new()
                     {
                        Key = "x|X",
                        Text = ToLabelText("Exit", "Exits the application"),
                        Action = (self) => { RunApp = false; },
                     },
            ];
        }
        /// <summary>
        /// Creates an array of menu items for a given list of items, taking into account pagination.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="mnuIdx">A reference to the menu index.</param>
        /// <param name="items">The array of items.</param>
        /// <param name="newMenuItemHandler">An optional handler for creating new menu items.</param>
        /// <returns>An array of menu items.</returns>
        protected virtual MenuItem[] CreatePageMenuItems<T>(ref int mnuIdx, T[] items, Action<T, MenuItem> newMenuItemHandler)
        {
            List<MenuItem> result = [];

            if (items.Length > PageSize)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    var menuItem = new MenuItem
                    {
                        IsDisplayed = i >= PageIndex * PageSize && i < (PageIndex + 1) * PageSize,
                        Key = (++mnuIdx).ToString(),
                        OptionalKey = "a", // it's for choose option all
                        Text = string.Empty,
                        Action = (self) =>
                        {
                        },
                        ForegroundColor = i % 2 == 0 ? ForegroundColor : ConsoleColor.DarkGreen,
                    };
                    newMenuItemHandler?.Invoke(item, menuItem);
                    result.Add(menuItem);
                }
                var pageLabel = $"{(PageIndex * PageSize) + 1}..{Math.Min((PageIndex + 1) * PageSize, items.Length)}/{items.Length}";

                result.Add(CreateMenuSeparator());
                result.Add(new()
                {
                    Key = "",
                    Text = ToLabelText(pageLabel, string.Empty, 20, ' '),
                    Action = (self) => { },
                    ForegroundColor = ConsoleColor.DarkGreen,
                });
                result.Add(CreateMenuSeparator());
                result.Add(new()
                {
                    Key = "+",
                    Text = ToLabelText("Next", "Load next page"),
                    Action = (self) =>
                    {
                        PageIndex = (PageIndex + 1) * PageSize < items.Length ? PageIndex + 1 : PageIndex;
                    },
                    ForegroundColor = ConsoleColor.DarkGreen,
                });

                result.Add(new()
                {
                    Key = "-",
                    Text = ToLabelText("Previous", "Load previous page"),
                    Action = (self) =>
                    {
                        PageIndex = Math.Max(0, PageIndex - 1);
                    },
                    ForegroundColor = ConsoleColor.DarkGreen,
                });
            }
            else
            {
                PageIndex = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    var item = items[i];
                    var menuItem = new MenuItem
                    {
                        Key = (++mnuIdx).ToString(),
                        OptionalKey = "a", // it's for choose option all
                        Text = string.Empty,
                        Action = (self) =>
                        {
                        },
                        ForegroundColor = i % 2 == 0 ? ForegroundColor : ConsoleColor.DarkGreen,
                    };
                    newMenuItemHandler?.Invoke(item, menuItem);
                    result.Add(menuItem);
                }
            }
            return [.. result];
        }
        /// <summary>
        /// Prints the footer of the console application.
        /// </summary>
        protected virtual void PrintFooter()
        {
            PrintLine();
            Print("Choose [n|n,n|a...all|x|X]: ");
        }
        /// <summary>
        /// Prints the screen with the menu items.
        /// </summary>
        protected virtual void PrintScreen()
        {
            var saveForegrondColor = ForegroundColor;

            MenuItems = CreateMenuItems();
            ForegroundColor = saveForegrondColor;
            PrintHeader();
            MenuItems.Where(mi => mi.IsDisplayed).ToList().ForEach(m =>
            {
                var menuKey = m.Key;

                ForegroundColor = m.ForegroundColor;
                PrintLine($"[{menuKey,MENU_KEY_WIDTH}] {m.Text}");
            });
            ForegroundColor = saveForegrondColor;
            PrintFooter();
        }
        #endregion abstract and virtual methods

        #region main-method
        /// <summary>
        /// This method is called before the execution of the program's main logic.
        /// </summary>
        /// <param name="args">The command-line arguments passed to the program.</param>
        protected virtual void BeforeRun(string[] args) { }
        /// <summary>
        /// This method is called before the execution of the main logic.
        /// </summary>
        protected virtual void BeforeExecution() { }
        /// <summary>
        /// Runs the console application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public virtual void Run(string[] args)
        {
            var choose = default(string[]);
            var saveForegrondColor = ForegroundColor;

            BeforeRun(args);
            RunApp = true;
            do
            {
                PrintScreen();

                choose = ReadLine().ToLower().Split(',', StringSplitOptions.RemoveEmptyEntries);
                var chooseIterator = choose.GetEnumerator();

                BeforeExecution();
                ForegroundColor = saveForegrondColor;
                while (RunApp && chooseIterator.MoveNext())
                {
                    var actions = MenuItems.Where(m => m.IsDisplayed && (m.Key.Equals(chooseIterator.Current) || m.OptionalKey.Equals(chooseIterator.Current)));
                    var actionIterator = actions.GetEnumerator();

                    while (RunApp && actionIterator.MoveNext())
                    {
                        actionIterator.Current?.Action(actionIterator.Current);
                    }
                    RunApp = RunApp && chooseIterator.Current.Equals("x") == false;
                    StopProgressBar();
                }
                AfterExecution();
            } while (RunApp);
            AfterRun();
        }
        /// <summary>
        /// This method is called after the execution of the main logic in the derived class.
        /// </summary>
        protected virtual void AfterExecution() { }
        /// <summary>
        /// This method is called after the execution of the Run method.
        /// </summary>
        protected virtual void AfterRun() { }
        #endregion main-method

        #region file and path methods
        /// <summary>
        /// Changes the page index.
        /// </summary>
        public void ChangePageIndex()
        {
            PrintLine();
            Print("Enter page index >= 0: ");
            if (int.TryParse(ReadLine(), out var result) && result >= 0)
            {
                PageIndex = result;
            }
        }
        /// <summary>
        /// Changes the page size.
        /// </summary>
        public void ChangePageSize()
        {
            PrintLine();
            Print("Enter page size > 0: ");
            if (int.TryParse(ReadLine(), out var result) && result > 0)
            {
                PageIndex = 0;
                PageSize = result;
            }
        }
        /// <summary>
        /// Changes the maximum subpath depth.
        /// </summary>
        public void ChangeMaxSubPathDepth()
        {
            var saveForegroundColor = ForegroundColor;

            ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Print("Enter the maximum subpath depth [>= 0] : ");
            ForegroundColor = saveForegroundColor;

            var text = ReadLine();

            if (int.TryParse(text, out int depth))
            {
                MaxSubPathDepth = depth;
            }
        }
        /// <summary>
        /// Changes the source path for the solution.
        /// </summary>
        public static void ChangeSolutionPath()
        {
            var saveForegroundColor = ForegroundColor;

            ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Print("Enter solution path: ");
            ForegroundColor = saveForegroundColor;

            var text = ReadLine();

            if (Directory.Exists(text))
            {
                SolutionPath = text;
            }
        }
        /// <summary>
        /// Changes the source path for the application.
        /// </summary>
        public static void ChangeSourcePath()
        {
            var saveForegroundColor = ForegroundColor;

            ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Print("Enter source path: ");
            ForegroundColor = saveForegroundColor;

            var text = ReadLine();

            if (Directory.Exists(text))
            {
                SourcePath = text;
            }
        }

        /// <summary>
        /// Changes the path for the application.
        /// </summary>
        public static string ChangePath(string path)
        {
            return ChangePath("Enter path: ", path);
        }
        /// <summary>
        /// Changes the path for the application.
        /// </summary>
        public static string ChangePath(string title, string path)
        {
            var saveForegroundColor = ForegroundColor;

            ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Print(title);
            ForegroundColor = saveForegroundColor;

            var text = ReadLine();

            if (Directory.Exists(text))
            {
                path = text;
            }
            return path;
        }

        /// <summary>
        /// Selects or changes the current path to a path based on the provided query paths.
        /// </summary>
        /// <param name="currentPath">The current path.</param>
        /// <param name="maxDepth">The maximum depth to search for subpaths.</param>
        /// <param name="queryPaths">The query paths to search for subpaths.</param>
        /// <returns>The selected or changed subpath.</returns>
        public static string SelectOrChangeToPath(string currentPath, int maxDepth, params string[] queryPaths)
        {
            var result = currentPath;
            var subPaths = new List<string>();
            var saveForegroundColor = ForegroundColor;

            TemplatePath.GetSubPaths(currentPath, maxDepth).Where(p => p != currentPath).ForEach(p => subPaths.Add(p));
            queryPaths.ForEach(qp => TemplatePath.GetSubPaths(qp, maxDepth).ToList().ForEach(p => subPaths.Add(p)));

            var paths = subPaths.Distinct().OrderBy(p => p).ToArray();

            var title = $"Change path: {currentPath}";

            ForegroundColor = ConsoleColor.DarkYellow;
            Clear();
            PrintLine('-', title.Length);
            PrintLine(title);
            PrintLine('-', title.Length);

            for (int i = 0; i < paths.Length; i++)
            {
                if (i == 0)
                {
                    PrintLine();
                }

                ForegroundColor = i % 2 == 0 ? ConsoleColor.DarkGreen : saveForegroundColor;
                PrintLine($"[{i + 1,3}] Change path to: {paths[i]}");
            }

            ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Print("Select or enter target path: ");
            ForegroundColor = saveForegroundColor;

            var text = ReadLine();

            if (int.TryParse(text, out var result2))
            {
                if (result2 - 1 >= 0 && result2 - 1 < paths.Length)
                {
                    result = paths[result2 - 1];
                }
            }
            else if (!string.IsNullOrEmpty(text) && text.Equals(".."))
            {
                var parentPath = TemplatePath.GetParentDirectory(currentPath);

                if (Directory.Exists(parentPath))
                {
                    result = parentPath;
                }
            }
            else if (!string.IsNullOrEmpty(text) && text.Contains(Path.DirectorySeparatorChar) == false)
            {
                var subPath = Path.Combine(currentPath, text);

                if (Directory.Exists(subPath))
                {
                    result = subPath;
                }
            }
            else if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
            {
                result = text;
            }
            return result;
        }
        /// <summary>
        /// Selects or changes the current path to a path based on the provided query paths.
        /// </summary>
        /// <param name="currentPath">The current path.</param>
        /// <param name="maxDepth">The maximum depth to search for subpaths.</param>
        /// <param name="searchPattern">The search pattern used to filter the files.</param>
        /// <param name="queryPaths">The query paths to search for subpaths.</param>
        /// <returns>The selected or changed subpath.</returns>
        public static string SelectOrChangeToPath(string currentPath, int maxDepth, string searchPattern, params string[] queryPaths)
        {
            var result = currentPath;
            var subPaths = new List<string>();
            var saveForegroundColor = ForegroundColor;

            TemplatePath.GetSubPaths(currentPath, maxDepth).Where(p => p != currentPath).ForEach(p => subPaths.Add(p));
            queryPaths.ForEach(qp => TemplatePath.GetSubPaths(qp, maxDepth).ToList().ForEach(p => subPaths.Add(p)));

            var paths = subPaths.Distinct()
                                .Where(p => Directory.GetFiles(p, searchPattern).Length > 0)
                                .OrderBy(p => p)
                                .ToArray();

            var title = $"Change path: {currentPath}";

            ForegroundColor = ConsoleColor.DarkYellow;
            Clear();
            PrintLine('-', title.Length);
            PrintLine(title);
            PrintLine('-', title.Length);

            for (int i = 0; i < paths.Length; i++)
            {
                if (i == 0)
                {
                    PrintLine();
                }

                ForegroundColor = i % 2 == 0 ? ConsoleColor.DarkGreen : saveForegroundColor;
                PrintLine($"[{i + 1,3}] Change path to: {paths[i]}");
            }

            ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Print("Select or enter target path: ");
            ForegroundColor = saveForegroundColor;

            var text = ReadLine();

            if (int.TryParse(text, out var result2))
            {
                if (result2 - 1 >= 0 && result2 - 1 < paths.Length)
                {
                    result = paths[result2 - 1];
                }
            }
            else if (!string.IsNullOrEmpty(text) && text.Equals(".."))
            {
                var parentPath = TemplatePath.GetParentDirectory(currentPath);

                if (Directory.Exists(parentPath))
                {
                    result = parentPath;
                }
            }
            else if (!string.IsNullOrEmpty(text) && text.Contains(Path.DirectorySeparatorChar) == false)
            {
                var subPath = Path.Combine(currentPath, text);

                if (Directory.Exists(subPath))
                {
                    result = subPath;
                }
            }
            else if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
            {
                result = text;
            }
            return result;
        }

        /// <summary>
        /// Changes the template solution path based on the provided parameters.
        /// </summary>
        /// <param name="currentPath">The current path.</param>
        /// <param name="maxDepth">The maximum depth to search for template solutions.</param>
        /// <param name="queryPaths">The query paths to search for template solutions.</param>
        /// <returns>The updated template solution path.</returns>
        public static string ChangeTemplateSolutionPath(string currentPath, int maxDepth, params string[] queryPaths)
        {
            var result = currentPath;
            var solutionPath = GetCurrentSolutionPath();
            var subPaths = new List<string>() { solutionPath };
            var saveForegroundColor = ForegroundColor;

            TemplatePath.GetTemplateSolutions(currentPath, maxDepth).Where(p => p != currentPath).ForEach(p => subPaths.Add(p));
            queryPaths.ForEach(qp => TemplatePath.GetTemplateParentPaths(qp, maxDepth).ToList().ForEach(p => subPaths.Add(p)));

            var paths = subPaths.Distinct().OrderBy(e => e).ToArray();

            var title = $"Change path: {currentPath}";

            ForegroundColor = ConsoleColor.DarkYellow;
            Clear();
            PrintLine('-', title.Length);
            PrintLine(title);
            PrintLine('-', title.Length);

            for (int i = 0; i < paths.Length; i++)
            {
                if (i == 0)
                    PrintLine();

                ForegroundColor = i % 2 == 0 ? ConsoleColor.DarkGreen : saveForegroundColor;
                PrintLine($"[{i + 1,3}] Change path to: {paths[i]}");
            }

            ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Print("Select or enter target path: ");
            ForegroundColor = saveForegroundColor;

            var text = ReadLine();

            if (int.TryParse(text, out int number))
            {
                if ((number - 1) >= 0 && (number - 1) < paths.Length)
                {
                    result = paths[number - 1];
                }
            }
            else if (string.IsNullOrEmpty(text) == false && Directory.Exists(text))
            {
                result = text;
            }
            return result;
        }
        /// <summary>
        /// Selects or changes the current path to a subpath based on the provided query paths.
        /// </summary>
        /// <param name="currentPath">The current path.</param>
        /// <param name="maxDepth">The maximum depth to search for subpaths.</param>
        /// <param name="queryPaths">The query paths to search for subpaths.</param>
        /// <returns>The selected or changed subpath.</returns>
        public static string SelectOrChangeToSubPath(string currentPath, int maxDepth, params string[] queryPaths)
        {
            var result = currentPath;
            var subPaths = new List<string>();
            var saveForegroundColor = ForegroundColor;

            TemplatePath.GetSubPaths(currentPath, maxDepth).Where(p => p != currentPath).ForEach(p => subPaths.Add(p));
            queryPaths.ForEach(qp => TemplatePath.GetSubPaths(qp, maxDepth).ToList().ForEach(p => subPaths.Add(p)));

            var paths = subPaths.Distinct().OrderBy(p => p).ToArray();

            var title = $"Change path: {currentPath}";

            ForegroundColor = ConsoleColor.DarkYellow;
            Clear();
            PrintLine('-', title.Length);
            PrintLine(title);
            PrintLine('-', title.Length);

            for (int i = 0; i < paths.Length; i++)
            {
                if (i == 0)
                {
                    PrintLine();
                }

                ForegroundColor = i % 2 == 0 ? ConsoleColor.DarkGreen : saveForegroundColor;
                PrintLine($"[{i + 1,3}] Change path to: {paths[i]}");
            }

            ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Print("Select or enter target path: ");
            ForegroundColor = saveForegroundColor;

            var text = ReadLine();

            if (int.TryParse(text, out var result2))
            {
                if (result2 - 1 >= 0 && result2 - 1 < paths.Length)
                {
                    result = paths[result2 - 1];
                }
            }
            else if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
            {
                result = text;
            }
            return result;
        }
        #endregion file and path methods
    }
}
//MdEnd
