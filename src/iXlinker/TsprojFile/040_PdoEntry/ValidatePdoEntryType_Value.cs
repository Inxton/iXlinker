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
            bool isStructured = !String.IsNullOrEmpty(pdoEntry.Name) ? pdoEntry.Name.Contains("__") : false;
            bool isArray = type.StartsWith("ARRAY [");

            if (!string.IsNullOrEmpty(type))
            {
                #region Unstructured PDO entries
                if (!isStructured)
                {
                    #region BIT based unstructured PDO entries
                    //BIT => BOOL
                    if (type.Equals("BIT") || type.Equals("ARRAY [0..0] OF BIT") )
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
                    //ARRAY [n..m] of BIT => ARRAY [n..m] of BOOL
                    else if (isArray && type.EndsWith("BIT"))
                    {
                        type = type.Replace("BIT", "BOOL");
                    }
                    #endregion
                    #region BYTE based unstructured PDO entries
                    //ARRAY [0..0] OF BYTE => BYTE in the case of the not structured PDO Entry
                    else if (type.Equals("ARRAY [0..0] OF BYTE"))
                    {
                        type = "BYTE";
                    }
                    #endregion
                    #region UINT based unstructured PDO entries
                    //UINTxx => ARRAY [0..n] according to the dimension in the case of the not structured PDO Entry
                    else if (type.StartsWith("UINT") && Int32.TryParse(type.Replace("UINT", ""), out Int32 uintDim) && uintDim % 8 == 0)
                    {
                        type = "ARRAY [0.." + (uintDim / 8 - 1).ToString() + "] OF USINT";
                    }
                    #endregion
                    if (!IsValidBaseHwType(type) && !IsArrayOfValidBaseHwType(type))
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine +
                            @"Unexpected type: " + type + " in the terminal : " + pdoEntry.BoxOrderCode + " in the item : " + pdoEntry.VarA.Replace(TcModel.tmpLevelSeparator, TcModel.ioLevelSeparator));
                    }
                }
                #endregion
                #region Structured PDO entries
                else if (isStructured)
                {
                    #region BIT based structured PDO entries
                    //ARRAY [0..0] OF BIT => BIT in the case of the structured PDO Entry
                    if (type.Equals("ARRAY [0..0] OF BIT"))
                    {
                        type = "BIT";
                    }
                    //BITn =>  ARRAY [0..n-1] of BIT in the case of the structured PDO Entry
                    else if (type.StartsWith("BIT") && Int32.TryParse(type.Replace("BIT", ""), out Int32 bitDim))
                    {
                        type = "ARRAY [0.." + (bitDim - 1).ToString() + "] OF BIT";
                    }
                    #endregion
                    #region BYTE based structured PDO entries
                    //ARRAY [0..0] OF BYTE => BYTE in the case of the structured PDO Entry
                    else if (type.Equals("ARRAY [0..0] OF BYTE"))
                    {
                        type = "BYTE";
                    }
                    #endregion
                    if (!IsValidBaseHwTypeOrBit(type) && !IsArrayOfValidBaseHwTypeOrBit(type))
                    {
                        EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine +
                            @"Unexpected type: " + type + " in the terminal : " + pdoEntry.BoxOrderCode + " in the item : " + pdoEntry.VarA.Replace(TcModel.tmpLevelSeparator, TcModel.ioLevelSeparator));
                    }
                }
                #endregion
            }


            if (type != null && !type.Equals(unmodified) && !String.IsNullOrEmpty(pdoEntry.Name))
            {
                try
                {
                    //Stream fs = isStructured ? new FileStream((AppDomain.CurrentDomain.BaseDirectory + "\\modifiedPdoEntryTypeStructured.txt").Replace("\\\\", "\\"), FileMode.Append) : new FileStream((AppDomain.CurrentDomain.BaseDirectory + "\\modifiedPdoEntryTypeUnstructured.txt").Replace("\\\\", "\\"), FileMode.Append);
                    Stream fs = isStructured ? new FileStream("D:\\_tmp\\modifiedPdoEntryTypeStructured.txt", FileMode.Append) : new FileStream("D:\\_tmp\\modifiedPdoEntryTypeUnstructured.txt", FileMode.Append);

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

        private static bool IsValidBaseHwTypeOrBit(string type)
        {
            return IsValidBaseHwType(type) || type.Equals("BIT");
        }

        private static bool IsArrayOfValidBaseHwType(string type)
        {
            string[] separators = { "[", "..", "]", "OF" };
            string[] elements = type.Replace(" ", "").Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return elements.Length == 4 && elements[0].Equals("ARRAY") && Int32.TryParse(elements[1], out Int32 loIndex) && Int32.TryParse(elements[2], out Int32 highIndex) && loIndex == 0 && loIndex < highIndex && IsValidBaseHwType(elements[3]);
        }

        private static bool IsArrayOfValidBaseHwTypeOrBit(string type)
        {
            string[] separators = { "[", "..", "]", "OF" };
            string[] elements = type.Replace(" ", "").Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return elements.Length == 4 && elements[0].Equals("ARRAY") && Int32.TryParse(elements[1], out Int32 loIndex) && Int32.TryParse(elements[2], out Int32 highIndex) && loIndex == 0 && loIndex < highIndex && IsValidBaseHwTypeOrBit(elements[3]);
        }
    }
}

