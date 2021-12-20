using ViewModels;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void GetAllDataTypes()
        {

            DataTypesType dataTypes = Tc.DataTypes;

            foreach (DataTypesTypeDataType dataType in dataTypes.DataType)
            {
                DataTypeViewModel dataTypeViewModel = FillDataType(dataType);
                DataTypes.Add(dataTypeViewModel);
            }

        }
    }
}
