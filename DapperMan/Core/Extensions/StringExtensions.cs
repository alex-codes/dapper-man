using System.Text.RegularExpressions;

namespace DapperMan.Core
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Recursively replaces a string of empty spaces with a single space.
        /// </summary>
        /// <param name="str">The string to sanitize.</param>
        /// <returns>
        /// A sanitized string.
        /// </returns>
        internal static string TrimEmptySpace(this string str)
        {
            var regex = new Regex(@"[\s]{2,}");
            return regex.Replace(str, " ")
                // the above can add a " ;" to the end of the query, so let's remove
                // the space for good measure
                .Replace(" ;", ";")
                .Trim();
        }
    }
}
