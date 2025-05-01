//@BaseCode
//MdStart
using CommonTool.Extensions;

namespace CommonTool
{
    /// <summary>
    /// Represents an abstract console application.
    /// </summary>
    /// <remarks>
    /// This class provides a framework for building console applications with features like menu handling,
    /// pagination, progress bars, and customizable paths.
    /// </remarks>
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
        /// <remarks>
        /// A menu item includes properties for display, keys, actions, and additional parameters.
        /// </remarks>
        protected partial class MenuItem
        {
            /// <summary>
            /// Gets or sets a value indicating whether the item is displayed.
            /// </summary>
            public bool IsDisplayed { get; set; } = true;
            /// <summary>
            /// Gets or sets the unique key of the menu item.
            /// </summary>
            public required string Key { get; set; }
            /// <summary>
            /// Gets or sets the optional key of the menu item.
            /// </summary>
            public string OptionalKey { get; set; } = string.Empty;
            /// <summary>
            /// Gets or sets the displayed text of the menu item.
            /// </summary>
            public required string Text { get; set; }
            /// <summary>
            /// Gets or sets the action to be performed when the menu item is selected.
            /// </summary>
            public required Action<MenuItem> Action { get; set; }
            /// <summary>
            /// Gets or sets the parameters for the menu item.
            /// </summary>
            public Dictionary<string, object> Params { get; set; } = [];
            /// <summary>
            /// Gets or sets the foreground color of the menu item.
            /// </summary>
            public ConsoleColor ForegroundColor { get; set; } = ConsoleApplication.ForegroundColor;
            /// <summary>
            /// Gets or sets the tag associated with the menu item.
            /// </summary>
            public string Tag { get; set; } = string.Empty;
        }
        #endregion menuitem

        #region properties
        /// <summary>
        /// The width of the menu key column.
        /// </summary>
        protected const int MENU_KEY_WIDTH = 5;
        /// <summary>
        /// The width of the menu text column.
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
        /// Gets or sets the current page index for pagination.
        /// </summary>
        protected int PageIndex { get; set; } = 0;
        /// <summary>
        /// Gets or sets the page size for pagination.
        /// </summary>
        protected int PageSize { get; set; } = 10;
        /// <summary>
        /// A queue to store commands entered by the user.
        /// </summary>
        protected static Queue<string> CommandQueue { get; } = new();
        #endregion properties

        #region console-methods
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
    }
}
//MdEnd
