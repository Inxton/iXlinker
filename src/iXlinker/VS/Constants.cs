using System;

namespace iXlinker.TsprojFile.Mapping
{
    internal partial class VS
    {
        private const uint RetryCount = 3;
        private const string VsProgID = "VisualStudio.DTE.16.0";
        private const string minVsSupportedVersionIncluded = "16.0.0.0";
        private const string maxVsSupportedVersionExcluded = "17.0.0.0";
        private const string dotnetcore = "Microsoft.NETCore.App";
        private const string minDotNetCoreSupportedVersionIncluded = "5.0.0";
        private const string maxDotNetCoreSupportedVersionExcluded = "6.0.0";

    }
}
