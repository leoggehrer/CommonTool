//@BaseCode
//MdStart
using System.Text;

namespace CommonTool.Extensions
{
    /// <summary>
    /// Provides extension methods for formatting C# code.
    /// </summary>
    public static partial class FormatterExtensions
    {
        /// <summary>
        /// Formats a given C# code by replacing line breaks with the platform-specific line break and then formatting the code using a predefined set of rules.
        /// </summary>
        /// <param name="source">The C# code to be formatted.</param>
        /// <returns>The formatted C# code.</returns>
        public static string FormatCSharpSource(this string source)
        {
            var lines = source.Replace("\n", Environment.NewLine).ToLines();
            
            return lines.FormatCSharpSource().ToText();
        }

        /// <summary>
        /// Formats the given C# code by applying appropriate indentation.
        /// </summary>
        /// <param name="lines">An enumerable collection of C# code lines to be formatted.</param>
        /// <returns>An enumerable collection of formatted C# code lines.</returns>
        public static IEnumerable<string> FormatCSharpSource(this IEnumerable<string> lines)
        {
            var indent = 0;
            var isLastEmpty = false;
            var result = new List<string>();
            
            foreach (var line in lines)
            {
                var formatLine = line.Trim();
                var hasOpenBlock = formatLine.Contains('{', '"', '"');
                var hasCloseBlock = formatLine.Contains('}', '"', '"');
                
                if (formatLine.StartsWith("#pragma")
                || formatLine.StartsWith("#if")
                || formatLine.StartsWith("#else")
                || formatLine.StartsWith("#endif"))
                {
                    result.Add(formatLine);
                }
                else if (hasOpenBlock && hasCloseBlock)
                {
                    result.Add(formatLine.SetIndent(indent));
                }
                else if (isLastEmpty == false || formatLine.IsNullOrEmpty() == false)
                {
                    var offest = 0;
                    
                    indent = hasCloseBlock ? indent - 1 : indent;
                    if (formatLine.StartsWith('.')
                        && result.Count != 0
                        && (offest = result.Last().IndexOf('.')) > -1)
                    {
                        result.Add(formatLine.SetIndent(" ", offest));
                    }
                    else
                    {
                        result.Add(formatLine.SetIndent(indent));
                    }
                    indent = hasOpenBlock ? indent + 1 : indent;
                }
                isLastEmpty = formatLine.IsNullOrEmpty();
            }
            return result;
        }
        
        /// <summary>
        /// Checks if the given line is a comment line.
        /// </summary>
        /// <param name="line">The line to check.</param>
        /// <returns>Returns true if the line is a comment line; otherwise, false.</returns>
        public static bool IsCommentLine(this string line)
        {
            return IsXmlLineComment(line) || IsLineComment(line) || IsBlockCommentLine(line);
        }
        
        /// <summary>
        /// Determines whether the given string represents a line comment.
        /// </summary>
        /// <param name="line">The string to be checked.</param>
        /// <returns>
        /// <c>true</c> if the given string represents a line comment; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLineComment(this string line)
        {
            return line.Trim().StartsWith(@"//");
        } 
        
        /// <summary>
        /// Checks if the provided string line is a XML line comment.
        /// </summary>
        /// <param name="line">The string line to be checked.</param>
        /// <returns>True if the line is a XML line comment, otherwise false.</returns>
        public static bool IsXmlLineComment(this string line)
        {
            return line.Trim().StartsWith(@"///");
        }

        /// <summary>
        /// Checks if a line of code contains a block comment.
        /// </summary>
        /// <param name="line">The line of code to check.</param>
        /// <returns>True if the line contains a block comment, otherwise false.</returns>
        public static bool IsBlockCommentLine(this string line)
        {
            var result = line.Trim();
            
            return result.StartsWith(@"/*") || result.StartsWith(@"*/") || result.StartsWith('*');
        }

        /// <summary>
        /// Removes HTML comments from the specified string.
        /// </summary>
        /// <param name="source">The string from which to remove HTML comments.</param>
        /// <returns>A new string with HTML comments removed.</returns>
        public static string RemoveHtmlComments(this string source)
        {
            var result = new StringBuilder();
            var lastTagIndex = 0;
            var tags = source.GetAllTags("<!--", "-->");

            foreach (var tag in tags)
            {
                if (tag.StartTagIndex > lastTagIndex)
                {
                    var text = source[lastTagIndex..tag.StartTagIndex];

                    result.AppendLine(text);
                    lastTagIndex = tag.EndTagIndex + tag.EndTag.Length;
                }
                else
                {
                    lastTagIndex = tag.EndTagIndex + tag.EndTag.Length;
                }
            }
            if (lastTagIndex < source.Length)
            {
                result.AppendLine(source[lastTagIndex..]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Formats the HTML source code by indenting the tags based on their nesting level.
        /// </summary>
        /// <param name="source">The HTML source code to be formatted.</param>
        /// <returns>The formatted HTML source code.</returns>
        public static string FormatHtmlSource(this string source)
        {
            var result = new StringBuilder();
            var level = 0;
            var lastTagIndex = 0;
            var tags = source.GetAllTags("<", ">", '"', '\'');

            StringExtensions.IndentSpace = "  ";

            foreach (var tag in tags)
            {
                var tagItems = CreateHtmlTagItems(tag);
                var isCommentTag = tagItems.First().StartsWith("<!--") || tagItems.Last().StartsWith("-->");
                var isClosingTag = isCommentTag == false&& tagItems.First().StartsWith("</") && tagItems.First() == tagItems.Last();
                var isSingleTag = isCommentTag == false && tagItems.Last().Contains("/>");
                var isOpenTag = isCommentTag == false && isClosingTag == false && isSingleTag == false;
                string newTag;

                if (isCommentTag)
                {
                    newTag = tagItems.SetIndent(level).ToText();
                }
                else if (isOpenTag)
                {
                    newTag = tagItems.SetIndent(level).ToText();
                    level++;
                }
                else if (isSingleTag)
                {
                    newTag = tagItems.SetIndent(level).ToText();
                }
                else
                {
                    level--;
                    newTag = tagItems.SetIndent(level).ToText();
                }

                if (tag.StartTagIndex > lastTagIndex)
                {
                    var text = source[lastTagIndex..tag.StartTagIndex]
                                .Replace(Environment.NewLine, string.Empty)
                                .Replace("\n", string.Empty)
                                .Trim();

                    if (text != string.Empty)
                    {
                        result.AppendLine(text.SetIndent(level + (isClosingTag ? 1 : 0)));
                    }
                    result.AppendLine(newTag);
                    lastTagIndex = tag.EndTagIndex + tag.EndTag.Length;
                }
                else
                {
                    result.AppendLine(newTag);
                    lastTagIndex = tag.EndTagIndex + tag.EndTag.Length;
                }
            }
            if (lastTagIndex < source.Length)
            {
                var rest = source.Partialstring(lastTagIndex, source.Length - 1).Trim();

                if (string.IsNullOrEmpty(rest) == false)
                {
                    result.AppendLine(rest);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Creates HTML tag items based on the provided <paramref name="tagInfo"/>.
        /// </summary>
        /// <param name="tagInfo">The tag information.</param>
        /// <returns>An array of HTML tag items.</returns>
        private static string[] CreateHtmlTagItems(TagInfo tagInfo)
        {
            var result = new List<string>();
            var items = tagInfo.InnerText.SplitWithIgnores(' ', '\'', '"')
                               .Select(i => i.Replace(Environment.NewLine, string.Empty))
                               .Select(i => i.RemoveLeftAndRight(' '))
                               .Where(i => i != string.Empty);
            var first = items.FirstOrDefault() ?? string.Empty;
            var last = items.LastOrDefault() ?? string.Empty;
            var htmlId = items.FirstOrDefault(i => i != first && i.StartsWith("id=")) ?? string.Empty;
            var htmlFor = items.FirstOrDefault(i => i != first && i.StartsWith("for=")) ?? string.Empty;
            var htmlClass = items.FirstOrDefault(i => i != first && i.StartsWith("class=")) ?? string.Empty;
            var htmlStyle = items.FirstOrDefault(i => i != first && i.StartsWith("style=")) ?? string.Empty;
            var orderItems = new List<string>();

            orderItems.AddRange(items.Skip(1)
                                     .SkipLast(1)
                                     .Where(i => i != htmlId && i != htmlFor && i != htmlClass && i != htmlStyle)
                                     .OrderByDescending(e => e.Replace("#", string.Empty)
                                                              .Replace("*", string.Empty)));

            result.Add($"{tagInfo.StartTag}{first}{(htmlId.HasContent() ? $" {htmlId}" : string.Empty)}{(htmlFor.HasContent() ? $" {htmlFor}" : string.Empty)}{(htmlClass.HasContent() ? $" {htmlClass}" : string.Empty)}{(htmlStyle.HasContent() ? $" {htmlStyle}" : string.Empty)}");
            result.AddRange(orderItems.Select(i => i.SetIndent(1)));

            if (last.Contains('/'))
            {
                if (first == last)   // </tag
                {
                    result[^1] = ($"{result.Last()}{tagInfo.EndTag}");
                }
                else                 // /
                {
                    result[^1] = ($"{result.Last()} {last}{tagInfo.EndTag}");
                }
            }
            else
            {
                if (first == last 
                    || last == htmlId 
                    || last == htmlFor 
                    || last == htmlClass 
                    || last == htmlStyle)
                {
                    result[^1] = ($"{result.Last()}{tagInfo.EndTag}");
                }
                else
                {
                    result.Add($"{last}{tagInfo.EndTag}".SetIndent(1));
                }
            }
            return [.. result];
        }
    }
}
//MdEnd