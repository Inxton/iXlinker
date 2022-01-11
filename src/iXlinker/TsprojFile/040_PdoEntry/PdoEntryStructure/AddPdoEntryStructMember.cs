using ViewModels;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private PdoEntryStructMemberViewModel AddPdoEntryStructMember(PdoEntryViewModel pdoEntry)
        {
            string pdoEntryName = pdoEntry.Name.Split('_')[2];
            string pdoEntryType = pdoEntry.Type_Value;
            PdoEntryStructMemberViewModel pdoEntryStructMemberViewModel = new PdoEntryStructMemberViewModel() { Name = pdoEntryName , Type_Value = pdoEntryType };

            return pdoEntryStructMemberViewModel;

        }
    }
}
