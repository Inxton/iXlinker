using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private string GetBoxNameFromProductCode(string box_product_code, string box_revision_number)
        {
            string productcode="";
            string numberCode = box_product_code.ToUpper().Replace("#X", "");
            string characterCode = numberCode.Substring(numberCode.Length - 4).ToUpper();
            numberCode = numberCode.Substring(0, numberCode.Length - 4);
            uint productNumberCode= uint.Parse(numberCode.ToUpper(), System.Globalization.NumberStyles.HexNumber);
            string prefix = "";
            string suffix = "";
            switch (characterCode)
            {
                case "2C22":
                    prefix = "BK";
                    suffix = "0000";
                    break;
                case "4032":
                    prefix = "CPXXXX-BK";
                    suffix = "";
                    break;
                case "3094":
                    prefix = "IL";
                    suffix = "B110";
                    break;

                default:
                    break;
            }
            if (suffix != "")
            {
                productcode = prefix + productNumberCode.ToString("0000") + "-" + suffix + "-" + box_revision_number;
            }
            else
            {
                productcode = prefix + productNumberCode.ToString("0000") + "-" + box_revision_number;
            }
            return productcode;
        }
    }
}
