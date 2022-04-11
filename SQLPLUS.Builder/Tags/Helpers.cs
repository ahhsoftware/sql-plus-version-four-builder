using System;
using System.Text.RegularExpressions;

namespace SQLPLUS.Builder.Tags
{

    public class Helpers
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>bool1 value indicates if the value parsed, bool2 value of the parse ignore if bool 1 is false</returns>
        public static Tuple<bool, bool> BoolValueAfterEqualsSign(string input)
        {
            if (input == null)
            {
                return new Tuple<bool, bool>(false, false);
            }

            if (input.Contains("="))
            {
                int idxAfterEquals = input.IndexOf("=") + 1;
                if (input.Length > idxAfterEquals)
                {
                    string rawValue = input.Substring(idxAfterEquals);
                    string testValue = rawValue.Trim().ToLower();
                    if (bool.TryParse(testValue, out bool result))
                    {
                        return new Tuple<bool, bool>(true, result);
                    }
                }

            }
            return new Tuple<bool, bool>(false, false);
        }

        public static string TagNamePart(string tagLine)
        {
            string trimmed = tagLine.Trim();
            if(trimmed.Contains("="))
            {
                return trimmed.Substring(0, trimmed.IndexOf("=")).Trim();
            }
            return trimmed;
        }

        public static Tuple<bool, int> Int1ValueAfterEqualsSign(string input)
        {
            if (input == null)
            {
                return new Tuple<bool, int>(false, 0);
            }

            if (input.Contains("="))
            {
                int idxAfterEquals = input.IndexOf("=") + 1;
                if (input.Length > idxAfterEquals)
                {
                    string rawValue = input.Substring(idxAfterEquals);
                    string testValue = Regex.Replace(rawValue, @"[^\d]","");
                    if (int.TryParse(testValue, out int result))
                    {
                        return new Tuple<bool, int>(true, result);
                    }
                }

            }
            return new Tuple<bool, int>(false, 0);
        }

        public static Tuple<bool, int> IntValueAfterReturn(string input)
        {
            if (input == null)
            {
                return new Tuple<bool, int>(false, 0);
            }

            if (input.StartsWith("RETURN", StringComparison.OrdinalIgnoreCase))
            {
                int idxAfterSpace = input.IndexOf(" ") + 1;
                if (input.Length > idxAfterSpace)
                {
                    string rawValue = input.Substring(idxAfterSpace);
                    string testValue = rawValue.Trim();
                    testValue = Regex.Replace(testValue, @"[^0-9]", "");
                    if (int.TryParse(testValue, out int result))
                    {
                        return new Tuple<bool, int>(true, result);
                    }
                }
            }
            return new Tuple<bool, int>(false, 0);
        }

        public static Tuple<bool, int, int> Int2ValueAfterEqualsSign(string input)
        {
            if (input == null)
            {
                return new Tuple<bool, int, int>(false, 0, 0);
            }

            if (input.Contains("="))
            {
                int idxAfterEquals = input.IndexOf("=") + 1;
                if (input.Length > idxAfterEquals)
                {
                    string rawValue = input.Substring(idxAfterEquals);
                    string testValue = rawValue.Trim().ToLower();
                    if (testValue.Contains(","))
                    {
                        string[] values = testValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length == 2)
                        {
                            string value1 = values[0].Trim();
                            string value2 = values[1].Trim();
                            if (!string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2))
                            {
                                if (int.TryParse(value1, out int int1) && int.TryParse(value2, out int int2))
                                {
                                    return new Tuple<bool, int, int>(true, int1, int2);
                                }
                            }
                        }
                    }
                }
            }
            return new Tuple<bool, int, int>(false, 0, 0);
        }


        public static Tuple<bool, string> Text1AfterEqualsSign(string input)
        {
            if (input == null)
            {
                return new Tuple<bool, string>(false, null);
            }

            if (input.Contains("="))
            {
                int idxAfterEquals = input.IndexOf("=") + 1;
                if (input.Length > idxAfterEquals)
                {
                    string rawValue = input.Substring(idxAfterEquals);
                    string testValue = rawValue.Trim();
                    if (string.IsNullOrEmpty(testValue))
                    {
                        return new Tuple<bool, string>(false, null);
                    }
                    return new Tuple<bool, string>(true, testValue);
                }
            }
            return new Tuple<bool, string>(false, null);
        }

        public static Tuple<bool, string, string> Text2AfterEqualsSign(string input)
        {
            if (input == null)
            {
                return new Tuple<bool, string, string>(false, null, null);
            }

            if (input.Contains("="))
            {
                int idxAfterEquals = input.IndexOf("=") + 1;
                if (input.Length > idxAfterEquals)
                {
                    string rawValue = input.Substring(idxAfterEquals);
                    string testValue = rawValue.Trim();
                    if (testValue.Contains(","))
                    {
                        string[] values = testValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length == 2)
                        {
                            string value1 = values[0].Trim();
                            string value2 = values[1].Trim();
                            if (!string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2))
                            {
                                return new Tuple<bool, string, string>(true, value1, value2);
                            }
                        }
                    }
                }
            }
            return new Tuple<bool, string, string>(false, null, null);
        }


        public static Tuple<bool,int, string, string> Text1Or2AfterEqualsSign(string input)
        {
            if (input == null)
            {
                return new Tuple<bool, int, string, string>(false, 0, null, null);
            }

            if (input.Contains("="))
            {
                int idxAfterEquals = input.IndexOf("=") + 1;
                if (input.Length > idxAfterEquals)
                {
                    string rawValue = input.Substring(idxAfterEquals);
                    string testValue = rawValue.Trim();
                    if (testValue.Contains(","))
                    {
                        string[] values = testValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (values.Length == 2)
                        {
                            string value1 = values[0].Trim();
                            string value2 = values[1].Trim();
                            if (!string.IsNullOrEmpty(value1) && !string.IsNullOrEmpty(value2))
                            {
                                return new Tuple<bool, int, string, string>(true, 2, value1, value2);
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(testValue))
                        {
                            return new Tuple<bool, int, string, string>(false, 0, null, null);
                        }
                        return new Tuple<bool, int, string, string>(true, 1, testValue, null);
                    }
                }
            }
            return new Tuple<bool, int, string, string>(false, 0, null, null);
        }
    }
}