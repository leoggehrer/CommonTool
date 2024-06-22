﻿//@BaseCode
//MdStart
using System.Text;

namespace CommonTool.Extensions
{
    /// <summary>
    /// Provides extension methods for strings with taginfo.
    /// </summary>
    static partial class StringExtensions
    {
        /// <summary>
        /// Retrieves all the tag information for the specified tags in the given text.
        /// </summary>
        /// <param name="text">The text to search in.</param>
        /// <param name="tags">An array of tags to search for. The tags must be specified in pairs,
        /// where the first element in each pair is the start tag and the second element is the end tag.</param>
        /// <returns>An enumerable collection of TagInfo objects representing the found tags in the text.</returns>
        public static IEnumerable<TagInfo> GetAllTags(this string text, string[] tags)
        {
            int parseIndex = 0;
            List<TagInfo> result = [];

            for (int i = 0; i + 1 < tags.Length; i += 2)
            {
                var tagInfos = text.GetAllTags(tags[i], tags[i + 1], parseIndex);

                if (tagInfos.Any())
                {
                    result.AddRange(tagInfos);
                    parseIndex = tagInfos.Last().EndTagIndex;
                }
            }
            return result;
        }
        /// <summary>
        /// Retrieves all tags from the specified text that are located between the given start tag and end tag.
        /// </summary>
        /// <typeparam name="TagInfo">The type of tag information to be retrieved.</typeparam>
        /// <param name="text">The text from which the tags are to be extracted.</param>
        /// <param name="startTag">The start tag of the desired tags.</param>
        /// <param name="endTag">The end tag of the desired tags.</param>
        /// <returns>An IEnumerable collection of tag information that meets the specified criteria.</returns>
        /// <remarks>
        /// This method is an extension method that can be invoked on a string object.
        /// </remarks>
        public static IEnumerable<TagInfo> GetAllTags(this string text, string startTag, string endTag)
        {
            return text.GetAllTags<TagInfo>(startTag, endTag, 0);
        }
        /// <summary>
        /// Retrieves all tags within a specified string starting from a given index.
        /// </summary>
        /// <typeparam name="TagInfo">The type of information to retrieve from the tags.</typeparam>
        /// <param name="text">The string to search for the tags.</param>
        /// <param name="startTag">The starting tag to look for.</param>
        /// <param name="endTag">The ending tag to look for.</param>
        /// <param name="parseIndex">The index to start parsing from within the string.</param>
        /// <returns>An enumerable of type TagInfo containing all the retrieved tag information.</returns>
        public static IEnumerable<TagInfo> GetAllTags(this string text, string startTag, string endTag, int parseIndex)
        {
            return text.GetAllTags<TagInfo>(startTag, endTag, parseIndex);
        }
        /// <summary>
        /// Retrieves all occurrences of a specific tag within a given string.
        /// </summary>
        /// <typeparam name="TagInfo">The type of tag information objects to be returned.</typeparam>
        /// <param name="text">The string to search for tags in.</param>
        /// <param name="startTag">The starting tag to search for.</param>
        /// <param name="endTag">The ending tag to search for.</param>
        /// <param name="parseIndex">The parse index indicating the starting position in the string to begin search from.</param>
        /// <param name="excludeBlocks">Optional characters that define excluded block sections, preventing tag extraction within those sections.</param>
        /// <returns>An IEnumerable collection of tag information objects representing each occurrence of the specified tag within the string.</returns>
        public static IEnumerable<TagInfo> GetAllTags(this string text, string startTag, string endTag, int parseIndex, params char[] excludeBlocks)
        {
            return text.GetAllTags<TagInfo>(startTag, endTag, parseIndex, excludeBlocks);
        }
        /// <summary>
        /// Retrieves all tags from the given text that match the specified start and end tags.
        /// </summary>
        /// <typeparam name="T">The type of the tags to retrieve. Must inherit from TagInfo and have a default constructor.</typeparam>
        /// <param name="text">The text to search for tags in.</param>
        /// <param name="startTag">The start tag to search for.</param>
        /// <param name="endTag">The end tag to search for.</param>
        /// <param name="parseIndex">The starting index for parsing the text for tags.</param>
        /// <param name="excludeBlocks">Optional characters to exclude when determining the end of a tag.</param>
        /// <returns>An IEnumerable of tags matching the specified start and end tags.</returns>
        public static IEnumerable<T> GetAllTags<T>(this string text, string startTag, string endTag, int parseIndex, params char[] excludeBlocks)
        where T : TagInfo, new()
        {
            int startTagIndex;
            int endTagIndex;
            var result = new List<T>();
            var tagHeader = new TagInfo.TagHeader(text);

            do
            {
                startTagIndex = text.IndexOf(startTag, parseIndex, StringComparison.CurrentCultureIgnoreCase);
                var startTagEndIndex = startTagIndex > -1 ? startTagIndex + startTag.Length : parseIndex;
                endTagIndex = startTagEndIndex >= 0 ? text.IndexOf(endTag, startTagEndIndex, StringComparison.CurrentCultureIgnoreCase) : -1;

                if (startTagIndex > -1 && endTagIndex > startTagIndex)
                {
                    int idx = startTagEndIndex;
                    int endTagSearchPosAt = startTagEndIndex;
                    var blockCounter = new int[excludeBlocks.Length];

                    while (idx < endTagIndex)
                    {
                        for (int j = 0; j < blockCounter.Length; j++)
                        {
                            if (text[idx] == excludeBlocks[j])
                            {
                                endTagSearchPosAt = idx;
                                blockCounter[j] += j % 2 == 0 ? 1 : -1;
                            }
                        }
                        idx++;
                    }
                    while (idx < text.Length && blockCounter.Sum() != 0)
                    {
                        for (int j = 0; j < blockCounter.Length; j++)
                        {
                            if (text[idx] == excludeBlocks[j])
                            {
                                endTagSearchPosAt = idx;
                                blockCounter[j] += j % 2 == 0 ? 1 : -1;
                            }
                        }
                        idx++;
                    }
                    if (endTagSearchPosAt > endTagIndex && blockCounter.Sum() == 0)
                    {
                        endTagIndex = text.IndexOf(endTag, endTagSearchPosAt, StringComparison.CurrentCultureIgnoreCase);
                    }
                }

                if (startTagIndex > -1 && endTagIndex > startTagIndex)
                {
                    result.Add(new T
                    {
                        Header = tagHeader,
                        StartTag = startTag,
                        StartTagIndex = startTagIndex,
                        EndTag = endTag,
                        EndTagIndex = endTagIndex,
                    });
                    parseIndex = startTagEndIndex;
                }
            } while (startTagIndex > -1 && endTagIndex > -1);
            return result;
        }

        /// <summary>
        /// Replaces all occurrences of a specified tag within a string with a replacement string generated by a specified function.
        /// </summary>
        /// <param name="text">The original string.</param>
        /// <param name="tagInfo">The tag information, including the start and end tags to be replaced.</param>
        /// <param name="replace">A function that takes a tag substring and returns a replacement string.</param>
        /// <returns>A new string with all occurrences of the specified tag replaced with the generated replacement strings.</returns>
        public static string ReplaceAll(this string text, TagInfo tagInfo, Func<string, string> replace)
        {
            StringBuilder result = new();
            int parseIndex = 0;
            int startTagIndex;
            int endTagIndex;

            do
            {
                startTagIndex = text.IndexOf(tagInfo.StartTag, parseIndex, StringComparison.CurrentCultureIgnoreCase);
                int startTagEndIndex = startTagIndex > -1 ? startTagIndex + tagInfo.StartTag.Length : parseIndex;
                endTagIndex = startTagEndIndex >= 0 ? text.IndexOf(tagInfo.EndTag, startTagEndIndex, StringComparison.CurrentCultureIgnoreCase) : -1;
                int endTagEndIndex = endTagIndex > -1 ? endTagIndex + tagInfo.EndTag.Length : parseIndex;

                if (startTagIndex > -1 && endTagIndex > startTagIndex)
                {
                    string substr = text.Substring(startTagIndex, endTagIndex - startTagIndex + tagInfo.EndTag.Length);

                    result.Append(text[parseIndex..startTagIndex]);
                    if (replace != null)
                    {
                        result.Append(replace(substr));
                    }
                    parseIndex = endTagEndIndex;
                }
            } while (startTagIndex > -1 && endTagIndex > -1);

            if (parseIndex < text.Length)
            {
                result.Append(text[parseIndex..]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Divides a string into multiple substrings based on specified tags.
        /// </summary>
        /// <param name="text">The input string to be divided.</param>
        /// <param name="tags">An array of tags to look for in the string.</param>
        /// <returns>An enumerable collection of DivideInfo objects representing the divided substrings.</returns>
        public static IEnumerable<DivideInfo> Divide(this string text, string[] tags)
        {
            List<DivideInfo> result = [];
            int startIdx = 0;
            var tagInfos = text.GetAllTags(tags);

            foreach (var tagInfo in tagInfos)
            {
                if (startIdx < tagInfo.StartTagIndex)
                {
                    result.Add(new DivideInfo(startIdx, tagInfo.StartTagIndex - 1)
                    {
                        Text = text.Partialstring(startIdx, tagInfo.StartTagIndex - 1),
                    });
                    result.Add(new DivideInfo(tagInfo)
                    {
                        Text = text.Partialstring(tagInfo.StartTagIndex, tagInfo.EndTagIndex),
                    });
                    startIdx = tagInfo.EndTagIndex + 1;
                }
                else if (startIdx == tagInfo.StartTagIndex)
                {
                    result.Add(new DivideInfo(tagInfo)
                    {
                        Text = text.Partialstring(tagInfo.StartTagIndex, tagInfo.EndTagIndex),
                    });
                    startIdx = tagInfo.EndTagIndex + 1;
                }
            }
            if (startIdx < text.Length - 1)
            {
                result.Add(new DivideInfo(startIdx, text.Length)
                {
                    Text = text.Partialstring(startIdx, text.Length - 1),
                });
            }
            return result;
        }
    }
}
//MdEnd