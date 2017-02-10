using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hackernews.Classes
{
    public static class Utility
    {
        internal static int intTryParse(string input)
        {
            int output = 0;
            int.TryParse(input, out output);
            return output;
        }

        internal static bool checkIfValidURI(string inputURI)
        {
            if (new Regex(@"^(http://|https://|item\?id=)").Match(inputURI).Success)
                return true;
            else
                return false;
        }
    }
}
