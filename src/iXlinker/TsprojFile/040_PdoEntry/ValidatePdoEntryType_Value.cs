using System;
using iXlinkerDtos;
using iXlinker.Utils;
using System.IO;


namespace iXlinker.TsprojFile.PdoEntry
{
    public class Validate
    {
        public static string Type_Value(PdoEntryViewModel pdoEntry)
        {
            string unmodified = pdoEntry.Type_Value;
            string type = pdoEntry.Type_Value;

            if (!string.IsNullOrEmpty(type))
            {
                #region BIT based PDO entries
                //BIT => BOOL
                if (type.Equals("BIT"))
                {
                    type = "BOOL";
                }
                //BITxx => BYTE, WORD, DWORD, LWORD according to the dimension
                else if (type.StartsWith("BIT") && Int32.TryParse(type.Replace("BIT", ""), out Int32 bitDim))
                {
                    if (bitDim > 1 && bitDim <= 8) type = "BYTE";
                    else if (bitDim > 8 && bitDim <= 16) type = "WORD";
                    else if (bitDim > 16 && bitDim <= 32) type = "DWORD";
                    else if (bitDim > 32 && bitDim <= 64) type = "LWORD";
                }
                #endregion
                #region UINT based PDO entries
                //UINTxx => ARRAY [0..n] according to the dimension in the case of the not structured PDO Entry
                else if (type.StartsWith("UINT") && Int32.TryParse(type.Replace("UINT", ""), out Int32 uintDim) && uintDim % 8 == 0)
                {
                    type = "ARRAY [0.." + (uintDim / 8 - 1).ToString() + "] OF USINT";
                }
                #endregion

                if (!IsValidBaseHwType(type) && !IsValidArrayOfTheHwType(type))
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine +
                        @"Unexpected type: " + type + " in the terminal : " + pdoEntry.BoxOrderCode + " in the item : " + pdoEntry.VarA.Replace(TcModel.tmpLevelSeparator, TcModel.ioLevelSeparator));
                }
            }

            if (type != null && !type.Equals(unmodified) && !String.IsNullOrEmpty(pdoEntry.Name))
            {
                try
                {
                    //Stream fs = isStructured ? new FileStream((AppDomain.CurrentDomain.BaseDirectory + "\\modifiedPdoEntryTypeStructured.txt").Replace("\\\\", "\\"), FileMode.Append) : new FileStream((AppDomain.CurrentDomain.BaseDirectory + "\\modifiedPdoEntryTypeUnstructured.txt").Replace("\\\\", "\\"), FileMode.Append);
                    Stream fs = new FileStream("D:\\_tmp\\modifiedPdoEntryType.txt", FileMode.Append);

                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(@"{0};{1};{2};{3};{4}", pdoEntry.BoxOrderCode, (pdoEntry.VarA).Replace(TcModel.tmpLevelSeparator, TcModel.ioLevelSeparator), pdoEntry.Name, unmodified, type);
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }
            }
            return type;
        }

        private static bool IsValidBaseHwType(string type)
        {
            //All of these types has been found during testing, except DWORD and LREAL
            string[] basetypes = { "BOOL", "BYTE", "WORD", "DWORD", "LWORD", "SINT", "USINT", "INT", "UINT", "DINT", "UDINT", "LINT", "ULINT", "REAL", "LREAL" };
            bool isBaseType = false;
            foreach (string basetype in basetypes)
            {
                if (basetype.Equals(type))
                {
                    isBaseType = true;
                    break;
                }
            }
            return isBaseType;
        }

        private static bool IsValidArrayHwType(string type)
        {
            string[] basetypes = { "BYTE" , "USINT" };
            bool isValidArrayType = false;
            foreach (string basetype in basetypes)
            {
                if (basetype.Equals(type))
                {
                    isValidArrayType = true;
                    break;
                }
            }
            return isValidArrayType;
        }

        private static bool IsValidArrayOfTheHwType(string type)
        {
            string[] separators = { "[", "..", "]", "OF" };
            string[] elements = type.Replace(" ", "").Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return elements.Length == 4 && elements[0].Equals("ARRAY") && Int32.TryParse(elements[1], out Int32 loIndex) && Int32.TryParse(elements[2], out Int32 highIndex) && loIndex == 0 && loIndex < highIndex && IsValidArrayHwType(elements[3]);
        }
    }
}

