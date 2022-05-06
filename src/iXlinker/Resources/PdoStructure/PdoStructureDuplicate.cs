using iXlinkerDtos;
using System;
using Utils;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace iXlinker.Resources
{
    [Serializable()] public class PdoStructureDuplicate
    {
        private string originalName;
        public string OriginalName
        {
            get { return this.originalName; }
        }

        private string replacementName;
        public string ReplacementName
        {
            get { return this.replacementName; }
        }

         public PdoStructureDuplicate(string originalType, string replacementType)
        {
            this.originalName = originalType;
            this.replacementName = replacementType;
        }
    }
}
