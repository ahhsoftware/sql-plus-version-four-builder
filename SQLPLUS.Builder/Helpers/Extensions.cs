using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLPLUS.Builder.Helpers
{
    public static class Extensions
    {
        public static void TryAddItem(this List<string> value, string item)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!string.IsNullOrEmpty(item))
            {
                if(!value.Contains(item))
                {
                    value.Add(item);
                }
            }
        }
        public static string AsNullable(this string value)
        {
            return string.Concat(value, "?");
        }
    }
}
