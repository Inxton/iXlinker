namespace iXlinkerExt
{
    partial class Prerequisites
    {
        internal static bool CheckPrerequisites(out string devenvPath)
        {
            bool prerequisitesOK = false;
            string _devenvPath = "";

            prerequisitesOK = CheckDotNetCore();

            prerequisitesOK = prerequisitesOK && CheckVs(out _devenvPath);

            devenvPath = _devenvPath;

            return prerequisitesOK;
        }
    }
}
