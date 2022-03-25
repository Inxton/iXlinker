using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private PdoEntryStructMemberViewModel AddPdoEntryStructMember(PdoEntryViewModel pdoEntry)
        {
            string pdoEntryName = pdoEntry.Name.Split('_')[2];

            PdoEntryStructMemberViewModel pdoEntryStructMemberViewModel = new PdoEntryStructMemberViewModel() { Name = pdoEntryName , Type_Value = pdoEntry.Type_Value ,TypeNamespace = pdoEntry.TypeNamespace};

            return pdoEntryStructMemberViewModel;

        }
    }
}
