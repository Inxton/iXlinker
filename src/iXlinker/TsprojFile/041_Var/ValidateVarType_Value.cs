using System;
using iXlinkerDtos;
using iXlinker.Utils;
using System.Text;
using System.Globalization;
using System.IO;

namespace iXlinker.TsprojFile.Var
{
    public class Validate
    {
        public static string Type_Value(PdoEntryViewModel pdoEntry)
        {
            string unmodified = pdoEntry.Type_Value;
            string type = pdoEntry.Type_Value;

            if (type != null)
            {
                //		
                //	BIT => BOOL	
                //	BITARR4	=> BYTE
                //	BITARR8	=> BYTE
                //	BITARR16 => WORD
                //	MASTER_MESSAGE => ARRAY[0..5] OF BYTE
                //	SLAVE_MESSAGE => ARRAY[0..5] OF BYTE
                //	UINTARR2 => ARRAY [0..1] OF UINT
                //		

                if (type.Equals("BIT")) type = "BOOL";
                else if (type.Equals("BITARR4") || type.Equals("BITARR8")) type = "BYTE";
                else if (type.Equals("BITARR16")) type = "WORD";
                else if (type.Equals("MASTER_MESSAGE") || type.Equals("SLAVE_MESSAGE")) type = "ARRAY[0..5] OF BYTE";
                else if (type.Equals("UINTARR2")) type = "ARRAY [0..1] OF UINT";

                if (!IsValidBaseHwType(type) && !IsArrayOfValidBaseHwType(type))
                {
                    EventLogger.Instance.Logger.Error(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine +
                        @"Unexpected type: " + type + " in the terminal : " + pdoEntry.BoxOrderCode + " in the item : " + pdoEntry.VarA.Replace(TcModel.tmpLevelSeparator, TcModel.ioLevelSeparator));
                }
            }
            return type;
        }

        private static bool IsValidBaseHwType(string type)
        {
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


        private static bool IsArrayOfValidBaseHwType(string type)
        {
            string[] separators = { "[", "..", "]", "OF" };
            string[] elements = type.Replace(" ", "").Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return elements.Length == 4 && elements[0].Equals("ARRAY") && Int32.TryParse(elements[1], out Int32 loIndex) && Int32.TryParse(elements[2], out Int32 highIndex) && loIndex == 0 && loIndex < highIndex && IsValidBaseHwType(elements[3]);
        }
    }
}

