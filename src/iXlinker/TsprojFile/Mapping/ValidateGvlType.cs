using System;
using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private string ValidateGvlType(string gvlType)
        {
            string ret = gvlType;

            if (gvlType == "BIT") ret = "BOOL";
            if (gvlType != "BIT" && gvlType.Contains("BIT") && !gvlType.Contains("ARRAY") && !gvlType.Contains("BITARR"))
            {
                try
                {
                    int dim = Int32.Parse(gvlType.Replace("BIT", ""));
                    if (dim > 1 && dim <= 8) ret = "BYTE";
                    else if (dim > 8 && dim <= 16) ret = "WORD";
                    else if (dim > 16 && dim <= 32) ret = "DWORD";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

            }

            if (gvlType.Contains("BITARR"))
            {
                try
                {
                    int dim = Int32.Parse(gvlType.Replace("BITARR", ""));
                    if (dim > 1 && dim <= 8) ret = "BYTE";
                    else if (dim > 8 && dim <= 16) ret = "WORD";
                    else if (dim > 16 && dim <= 32) ret = "DWORD";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

            }

            if (gvlType != "UINT" && gvlType.Contains("UINT") && !gvlType.Contains("ARRAY") )
            {
                try
                {
                    int dim = Int32.Parse(gvlType.Replace("UINT", ""));
                    if (dim > 1 && dim <= 8) ret = "BYTE";
                    else if (dim > 8 && dim <= 16) ret = "INT";
                    else if (dim > 16 && dim <= 32) ret = "DINT";

                }
                catch (Exception ex)
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                }

            }

            if (gvlType == "MASTER_MESSAGE" || gvlType == "SLAVE_MESSAGE") ret = "ARRAY[0..5] OF BYTE";

            return ret;
        }
    }
}
