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

        private string propertyFilter;
        public string PropertyFilter
        {
            get { return this.propertyFilter; }
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

         public StructureBase(string structureName, string structureType , string propertyFilter, string baseStructurePrefix , ObservableCollection<PlcStruct> PlcStructuresInPlcLibraries)
        {
            if (!baseStructurePrefix.Equals("!"))
            {
                this.structureName = structureName;
                this.propertyFilter = propertyFilter;
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
            else
            {
                this.structureName = structureName;
                this.propertyFilter = propertyFilter;
                this.structureType = structureType;
                this.baseStructureName = baseStructurePrefix;
                this.baseStructurePrefix = baseStructurePrefix;
                this.baseStructureNamespace = baseStructurePrefix;
            }
        }
        //
        // Summary:
        //      Create an PLC base structure that will be extended by all the PLC structures matching the structureName, structureType and propertyFilter.
        //      New Base structure is created and named as 'baseStructurePrefix' + "_" + CRC32(baseStructurePrefix). If such a structure already exists in any library,           
        //      the library version is used and it's namespace is added
        //      
        //      
        // Parameters:
        //   structureName:
        //      Name of the PLC structure that is to be extended. 
        //          In case of exact name is used only the exact PLC structure is intended to extend from the base structure created. 
        //          In case of NamePrefix + "*" is used, all the PLC structures with such a prefix are intended to extend from the base structure created. 
        //          In case of "*" is used as a structureName, all the PLC structures are intended to extend from the base structure created.
        //          In case of "!" is used as a structureName, just the base structure is created.
        //              
        //   structureType:
        //      Type of the structure that is to be extended.
        //      
        //   propertyFilter:
        //      Property and value filter for the structure that is to be extended.
        //          In case of exact property name + "=" + value is used only the exact PLC structure is intended to extend from the base structure created. 
        //          In case of property name + "=" + valuePrefix + "*" is used, all the PLC structures with such a property and value matching the criteria are intended to extend from the base structure created. 
        //          In case of "*" is used as a propertyFilter, all the PLC structures are intended to extend from the base structure created.
        //      
        //   baseStructurePrefix:
        //      Prefix of the base structure.
        //          The final base structure name is created as baseStructurePrefix + "_" + CRC32(baseStructurePrefix).
        //          In case of "!" is used as a baseStructurePrefix, matching PLC structures are NOT going to extend from the base structure created nor any created before.
        //      
        //   PlcStructuresInPlcLibraries:
        //      Collection of all Plc structures in all Plc libraries used in the Plc project.
        //          If the base Plc structure that is going to be created is found in some Plc library, the library version is used. 
        //      

        public static StructureBase Build(string structureName, string structureType, string propertyFilter, string baseStructurePrefix, ObservableCollection<PlcStruct> PlcStructuresInPlcLibraries)
        {
            return new StructureBase(structureName, structureType, propertyFilter, baseStructurePrefix, PlcStructuresInPlcLibraries);
        }

        public StructureBase AddAttribute(string attribute)
        {
            Attributes.Add(attribute);
            return this;
        }


    }
}
