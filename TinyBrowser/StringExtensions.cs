using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TinyBrowser
{
    public static class StringExtensions {
        public static string FindTextBetween(this string original, string start, string end) { return original.FindTextBetweenIndex(original.IndexOf(start, StringComparison.OrdinalIgnoreCase), original.IndexOf(end, StringComparison.OrdinalIgnoreCase)); }
        
        public static string FindTextBetween(this string original, int start, int end)       { return original.FindTextBetweenIndex(start, end); }
        
        public static string FindTextBetween(this string original, int start, string end)    { return original.FindTextBetweenIndex(start, original.IndexOf(end, start, StringComparison.OrdinalIgnoreCase)); }
        
        //public static string FindTextBetween(this string original, string start, int end)    { return original.FindTextBetweenIndex(original.IndexOf(start, StringComparison.OrdinalIgnoreCase), end); }
        
        private static string FindTextBetweenIndex(this string original, int start, int end) {
            int titleIndex = start;
            string title = string.Empty;
            
            if (titleIndex != -1) {
                int titleEndIndex = end;
                if (titleEndIndex > titleIndex) title = original[titleIndex..titleEndIndex];
            }
            
            return title;
        }

        public static List<int> AllIndexesOf(this string str, string value) {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", nameof(value));
            List<int> indexes = new List<int>();

            for (int index = 0;; index += value.Length) {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}