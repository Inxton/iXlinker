namespace iXlinker.TsprojFile.Mapping
{
    partial class VS
    {
        private static bool ValidateActiveTargetPlatform(string activeTargetPlatform)
        {
            bool valid = false;
            string[] validTargetProfiles = { "Debug", "Release" };
            string[] validTargetPlatforms = { "TwinCAT CE7 (ARMV7)", "TwinCAT OS (ARMT2)", "TwinCAT RT (x64)", "TwinCAT RT (x86)" };
            foreach(string profile in validTargetProfiles)
            {
                foreach(string platform  in validTargetPlatforms)
                {
                    if(activeTargetPlatform.Equals(profile + "|" + platform))
                    {
                        valid = true;
                        break;
                    }
                }
                if (valid)
                    break;
            }
            return valid;

        }
    }
}
