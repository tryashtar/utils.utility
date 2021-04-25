using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryashtarUtils.Utility
{
    public static class StringUtils
    {
        // helper for inlining pluralization in message boxes
        public static string Pluralize(int amount, string singular, string plural)
        {
            if (amount == 1)
                return $"1 {singular}";
            return $"{amount} {plural}";
        }
        public static string Pluralize(int amount, string singular) => Pluralize(amount, singular, singular + "s");

        // replaces the last space with a non-break space, preventing orhaning of the last word
        public static string DeOrphan(string input)
        {
            if (input == null)
                return "";
            int place = input.LastIndexOf(' ');
            if (place == -1)
                return input;
            return input.Remove(place, 1).Insert(place, "\u00A0"); // non-break space
        }
    }
}
