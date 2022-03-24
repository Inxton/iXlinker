using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using iXlinkerDtos;

namespace PlcprojFile
{
    public static class ValidatePlcItem
    {
        private static char[]   SpecialCharsName =        { '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', '{', '[', ']', '}', '\\', '|', ';', ':', '\'', '"', ',', '<', '.', '>', '/', '?', '§', ' ' };
        private static char[]   SpecialCharsLink =        { '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', '{',           '}', '\\', '|', ';', ':', '\'', '"', ',', '<', '.', '>', '/', '?', '§', ' ' };
        //private static char[]   SpecialCharsType =        { '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', '{', '[', ']', '}', '\\', '|', ';', ':', '\'', '"', ',', '<', '.', '>', '/', '?', '§', ' ' };
        private static char[] SpecialCharsType = { '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', '{', '[', ']', '}', '\\', '|', ';', ':', '\'', '"', ',', '<', '>', '/', '?', '§', ' ' };
        private static char[]   SpecialCharsArrayType =   { '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', '{',           '}', '\\', '|', ';', ':', '\'', '"', ',', '<', '>', '/', '?', '§' };

        private static char ReplaceChar = '_';

        private static string ReplacePlusMinus(string inStr)
        {
            string ret = inStr;
            if (!string.IsNullOrEmpty(ret))
            {
                if (ret.EndsWith("+"))
                    ret = ret.Substring(0, ret.Length - 1) + "_plus";
                if (ret.EndsWith("-"))
                    ret = ret.Substring(0, ret.Length - 1) + "_minus";
            }

            return ret;
        }

        private static string ReplaceSpecialChars(string inStr, char[] specialChars)
        {
            string ret = inStr;
            if (!string.IsNullOrEmpty(ret))
            {
                foreach (char specialChar in specialChars)
                {
                    ret = ret.Replace(specialChar, ReplaceChar);
                }
            }

            return ret;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string ReplaceKeywords(string inStr)
        {
            string ret = inStr;
            if (!string.IsNullOrEmpty(ret))
            {
                string[] s = ret.Split('[');
                if (    s[0].ToUpper().EndsWith("LIMIT") 
                    ||  s[0].ToUpper().EndsWith("AT"))
                {
                    s[0] = s[0] + "_x";
                }
                if (s.Length > 1)
                {
                    ret = s[0];
                    for (int i = 1; i < s.Length; i++)
                    {
                        ret = ret + "[" + s[i];
                    }
                }
                else
                {
                    ret = s[0];
                }
            }
            return ret;
        }

        private static string ReplaceDoubleUnderscores(string inStr)
        {
            string ret = inStr;
            if (!string.IsNullOrEmpty(ret))
            {
                while (ret.Contains("__")) ret = ret.Replace("__", ReplaceChar.ToString());
                if (ret.EndsWith("_")) ret = ret.Substring(0, ret.Length - 1);
                if (ret.StartsWith("_")) ret = ret.Substring(1);
            }
            return ret;
        }
        private static string AddUnderscorePrefixIfStartsWithNumber(string inStr)
        {
            string ret = inStr;
            if (!string.IsNullOrEmpty(ret))
            {
                if (ret.Length > 0 && char.IsDigit(ret[0]))
                {
                    ret = ReplaceChar + ret;
                }
            }
            return ret;
        }

        private static string AddUnderscorePrefixIfContainsDotNetKeyword(string inStr)
        {
            string [] keywords = new string[]
                {
                    "abstract","add","alias","and","args ","as","ascending","async","await","base",
                    "bool","break","by","byte","case","catch","char","checked","class","const",
                    "continue","decimal","default","delegate","descending","do","double","dynamic","else","enum",
                    "equals","event","explicit","extern","false","finally","fixed","float","for","foreach",
                    "from","get","global","goto","group","if","implicit","in","init","int",
                    "interface","internal","into","is","join","let","lock","long","managed","nameof",
                    "namespace","new","nint","not","notnull","nuint","null","object","on","operator",
                    "or","orderby","out","override","params","partial","private","protected","public","readonly",
                    "record","ref","remove","return","sbyte","sealed","select","set","short","sizeof",
                    "stackalloc","static","string","struct","switch","this","throw","true","try","typeof",
                    "uint","ulong","unchecked","unmanaged","unsafe","ushort","using","value","var","virtual",
                    "void","volatile","when","where","while","with","yield"
                };

            string ret = inStr;
            if (!string.IsNullOrEmpty(ret))
            {
                foreach (string keyword in keywords)
                {
                    if (ret.Equals(keyword))
                    {
                        ret = ReplaceChar + ret;
                        break;
                    }
                }
            }
            return ret;
        }

        public static string Name(string name)
        {
            string ret = name;
            if (!string.IsNullOrEmpty(ret))
            {
                ret = ReplacePlusMinus(ret);
                ret = ReplaceSpecialChars(ret, SpecialCharsName);
                ret = ret.Replace("_" + TcModel.tmpLevelSeparator, TcModel.tmpLevelSeparator);
                ret = ReplaceKeywords(ret);
                ret = RemoveDiacritics(ret);
                ret = ReplaceDoubleUnderscores(ret);
                ret = AddUnderscorePrefixIfStartsWithNumber(ret);
                ret = AddUnderscorePrefixIfContainsDotNetKeyword(ret);
                ret = ret.Replace("_" + TcModel.plcStructSeparator, TcModel.plcStructSeparator);
            }
            return ret;
        }

        public static string Link(string link)
        {
            string ret = link;
            if (!string.IsNullOrEmpty(ret))
            {
                ret = ReplacePlusMinus(ret);
                ret = ReplaceSpecialChars(ret, SpecialCharsLink);
                ret = ret.Replace("_" + TcModel.tmpLevelSeparator, TcModel.tmpLevelSeparator);
                ret = ReplaceKeywords(ret);
                ret = RemoveDiacritics(ret);
                ret = ReplaceDoubleUnderscores(ret);
                ret = AddUnderscorePrefixIfStartsWithNumber(ret);
                ret = AddUnderscorePrefixIfContainsDotNetKeyword(ret);
                ret = ret.Replace("_" + TcModel.plcStructSeparator, TcModel.plcStructSeparator);
            }
            return ret;
        }
        public static string StructurePrefix(string s)
        {
            string ret = ValidatePlcItem.Name(s);
            if (!string.IsNullOrEmpty(ret))
            {
                if (ret.Contains("_") && char.IsDigit(ret[ret.Length - 1]))
                {
                    while (char.IsDigit(ret[ret.Length - 1]) || ret.EndsWith("_"))
                    {
                        ret = ret.Substring(0, ret.Length - 1);
                    }
                }
            }
            return ret;
        }
        public static string Type(string type)
        {
            string ret = type;
            if (ret != null)
            {
                if (ret.ToUpper().Contains("ARRAY"))
                {
                    ret = ReplaceSpecialChars(ret, SpecialCharsArrayType);
                    if (ret.Contains("BIT"))
                    {
                        ret = ret.Replace("BIT","BOOL");
                    }
                }
                else
                {
                    ret = ReplaceSpecialChars(ret, SpecialCharsType);
                }
                ret = ret.Replace("_" + TcModel.tmpLevelSeparator, TcModel.tmpLevelSeparator);
                ret = ReplaceKeywords(ret);
                ret = RemoveDiacritics(ret);
                ret = ReplaceDoubleUnderscores(ret);
                ret = AddUnderscorePrefixIfStartsWithNumber(ret);
                ret = ret.Replace("_" + TcModel.plcStructSeparator, TcModel.plcStructSeparator);

                if (ret == "BIT") ret = "BOOL";
                if (ret != "BIT" && ret.Contains("BIT") && !ret.Contains("ARRAY") && !ret.Contains("BITARR"))
                {
                    try
                    {
                        int dim = Int32.Parse(ret.Replace("BIT", ""));
                        if (dim > 1 && dim <= 8) ret = "BYTE";
                        else if (dim > 8 && dim <= 16) ret = "WORD";
                        else if (dim > 16 && dim <= 32) ret = "DWORD";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                }

                if (ret.Contains("BITARR"))
                {
                    try
                    {
                        int dim = Int32.Parse(ret.Replace("BITARR", ""));
                        if (dim > 1 && dim <= 8) ret = "BYTE";
                        else if (dim > 8 && dim <= 16) ret = "WORD";
                        else if (dim > 16 && dim <= 32) ret = "DWORD";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }

                }

                if (ret != "UINT" && ret.Contains("UINT") && !ret.Contains("ARRAY"))
                {
                    try
                    {
                        int dim = Int32.Parse(ret.Replace("UINT", ""));
                        if (dim > 1 && dim <= 8) ret = "BYTE";
                        else if (dim > 8 && dim <= 16) ret = "INT";
                        else if (dim > 16 && dim <= 32) ret = "DINT";

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }
                }

                if (ret == "MASTER_MESSAGE" || ret == "SLAVE_MESSAGE") ret = "ARRAY[0..5] OF BYTE";
            }
            return ret;
        }
        public static string NameIncludingNamespace(string @namespace, string name)
        {
            if(String.IsNullOrEmpty(@namespace))
            {
                return name;
            }
            else
            {
                return @namespace + "." + name;
            }
        }

    }
}
