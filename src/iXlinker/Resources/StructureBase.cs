using iXlinkerDtos;
using System;
using Utils;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace iXlinker.Resources
{
    [Serializable()] public class StructureBase
    {
        private string structureName;
        public string StructureName
        {
            get { return this.structureName; }
        }

        private string structureType;
        public string StructureType
        {
            get { return this.structureType; }
        }

        private string baseStructurePrefix;
        public string BaseStructurePrefix
        {
            get { return this.baseStructurePrefix; }
        }

        private string baseStructureName;
        public string BaseStructureName
        {
            get { return this.baseStructureName; }
        }

        private string baseStructureNamespace;
        public string BaseStructureNamespace
        {
            get { return this.baseStructureNamespace; }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    baseStructureNamespace = value;
                }
            }
        }

        private List<string> attributes;
        public List<string> Attributes
        {
            get { return this.attributes ?? (this.attributes = new List<string>()); }
            set
            {
                if (value != null)
                {
                    this.attributes = value;
                }
            }
        }

        //public StructureBase()
        //{

        //}

        public StructureBase(string structureName, string structureType , string baseStructurePrefix , ObservableCollection<PlcStruct> PlcStructuresInPlcLibraries)
        {

            this.structureName = structureName;
            this.structureType = structureType;
            this.baseStructureName = baseStructurePrefix + "_" + CRC32.Calculate_CRC32(baseStructurePrefix).ToString("X8");
            this.baseStructurePrefix = baseStructurePrefix;
            this.baseStructureNamespace = "";
            foreach (PlcStruct plcStruct in PlcStructuresInPlcLibraries)
            {
                if (plcStruct.Name.Equals(baseStructureName))
                {
                    this.baseStructureNamespace = plcStruct.Namespace;
                    break;
                }
            }
        }

        public static StructureBase Build(string structureName, string structureType, string baseStructurePrefix, ObservableCollection<PlcStruct> PlcStructuresInPlcLibraries)
        {
            return new StructureBase(structureName, structureType, baseStructurePrefix, PlcStructuresInPlcLibraries);
        }


        //public StructureBase WithName(string structureName)
        //{
        //    this.structureName = structureName;
        //    return this;
        //}

        public StructureBase AddAttribute(string attribute)
        {
            Attributes.Add(attribute);
            return this;
        }
    }
}
