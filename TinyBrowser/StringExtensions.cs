using System;
using System.Collections.Generic;
using System.Linq;

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
            
            if (titleIndex != -1) 
            {
                int titleEndIndex = end;
                if (titleEndIndex > titleIndex) title = original[titleIndex..titleEndIndex];
            }
            
            return title;
        }

        public static List<int> AllIndexesOf(this string str, string value) 
        {
            if (String.IsNullOrEmpty(value)) throw new ArgumentException("the string to find may not be empty", nameof(value));
            
            List<int> indexes = new List<int>();

            for (int index = 0;; index += value.Length) 
            {
                index = str.IndexOf(value, index, StringComparison.OrdinalIgnoreCase);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        public static List<UrlData> GetNameAndUrl(string website, string searchFor)
        {
            List<int> indexes = website.AllIndexesOf(searchFor);
            List<UrlData> result = new List<UrlData>();
            
            foreach (int index in indexes)
            {
                string url         = website.FindTextBetween(index + searchFor.Length, "\"");
                string displayName = website.FindTextBetween(index + $"{searchFor}{url}\">".Length, "</a>");
                
                result.Add(new UrlData(website.FindTextBetween(index + $"{searchFor}{url}\">".Length, "</a>"), null, website.FindTextBetween(index + searchFor.Length, "\"")));
            }
            return result;
        }

        public static void RemoveSpecifiedElement(this List<UrlData> list, string element)
        {
            foreach (UrlData hyperLink in list.ToList().Where(hyperLink => hyperLink.DisplayName.Contains(element)))
                list.Remove(hyperLink);
        }

        public static string Prettify(this string s)
        {
            return s == null ? null : $"{s.Substring(0, 6)}...{s.Substring(s.Length - 6)}";
        }
    }

    public struct UrlData
    {
        public UrlData(string displayName, string host)              { DisplayName = displayName; Host = host; Path = null; }
        public UrlData(string displayName, string host, string path) { DisplayName = displayName; Host = host; Path = path; }

        public string DisplayName { get; private set; }
        public string Host { get; private set; }
        
        public string Path { get; private set; }
    }
}