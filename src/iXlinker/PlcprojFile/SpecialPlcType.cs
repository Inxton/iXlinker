using iXlinkerDtos;
using System;
using Utils;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace iXlinker.Resources
{
    [Serializable()] public class SpecialPlcType
    {
        private string originalType;
        public string OriginalType
        {
            get { return this.originalType; }
        }

        private string replacementType;
        public string ReplacementType
        {
            get { return this.replacementType; }
        }

        private string replacementTypeNamespace;
        public string ReplacementTypeNamespace
        {
            get { return this.replacementTypeNamespace; }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    replacementTypeNamespace = value;
                }
            }
        }

         public SpecialPlcType(string originalType, string replacementType, ObservableCollection<PlcStruct> PlcStructuresInPlcLibraries)
        {

            this.originalType = originalType;
            this.replacementType = replacementType;
            this.replacementTypeNamespace = "";
            foreach (PlcStruct plcStruct in PlcStructuresInPlcLibraries)
            {
                if (plcStruct.Name.Equals(replacementType))
                {
                    this.replacementTypeNamespace = plcStruct.Namespace;
                    break;
                }
            }
        }
    }
}
