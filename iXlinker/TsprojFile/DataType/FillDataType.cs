using System;
using ViewModels;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private DataTypeViewModel FillDataType(DataTypesTypeDataType dataType)
        {
            DataTypeViewModel dataTypeViewModel = new DataTypeViewModel();

            TypeType typeType = new TypeType();
            ArrayInfoType arrayInfoType = new ArrayInfoType();

            try
            {
                typeType = (TypeType) dataType.Items[0];
                arrayInfoType = (ArrayInfoType)dataType.Items[1];
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
            }

            dataTypeViewModel.Name = dataType.Name.Value ?? "";
            dataTypeViewModel.GUID = dataType.Name.GUID ?? "";
            dataTypeViewModel.BaseType = typeType.Value;
            dataTypeViewModel.IecbaseType = dataType.Name.IecBaseTypeSpecified && dataType.Name.IecBaseType;
            dataTypeViewModel.AutoDelete = dataType.Name.AutoDeleteTypeSpecified && dataType.Name.AutoDeleteType;
            dataTypeViewModel.HideSubItems = dataType.Name.HideSubItemsSpecified && dataType.Name.HideSubItems;
            dataTypeViewModel.BitSize = (uint)(dataType.BitSize.Value);
            dataTypeViewModel.Lbound = UInt32.Parse(arrayInfoType.LBound.Value);
            dataTypeViewModel.Elements = UInt32.Parse(arrayInfoType.Elements.Value);

            return dataTypeViewModel;
        }
    }
}
