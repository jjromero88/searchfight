using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestProject1
{
    public static class UtilFactory
    {
        public static string clearLetters(string s)
        {
            if (!string.IsNullOrEmpty(s))
                return Regex.Replace(s, "[^0-9.]", "");

            return null;
        }

        public static string clearHtml(string s)
        {
            if (!string.IsNullOrEmpty(s))
                return Regex.Replace(s, "<.*?>", String.Empty);

            return null;
        }

        public static List<LANGUAGE> formatInputs(string s)
        {
            List<LANGUAGE> Lista = new List<LANGUAGE>();

            MatchCollection matchCollection = Regex.Matches(s, @"(?<match>[^""\s]+)|\""(?<match>[^""]*)""");
            foreach (Match match in matchCollection)
                Lista.Add(new LANGUAGE() { DESCRIPTION = match.Groups["match"].Value });

            if (Lista.Count > 0)
                return Lista;

            return null;
        }

    }
}
