namespace ViewModels
{
    using System.Collections.ObjectModel;


    public enum DeviceTypes
    {
        IODEVICETYPE_UNKNOWN = 0,                   //	Unknown device 
        IODEVICETYPE_C1220 = 1,                     //	Beckhoff Lightbus-Master 
        IODEVICETYPE_C1200 = 2,                     //	Beckhoff Lightbus-Master 
        IODEVICETYPE_SPC3 = 3,                      //	ProfiBus Slave (Siemens) 
        IODEVICETYPE_CIF30DPM = 4,                  //	ISA ProfiBus-Master 2 kByte (Hilscher) 
        IODEVICETYPE_CIF40IBSM = 5,                 //	ISA Interbus-S-Master 2 kByte (Hilscher) 
        IODEVICETYPE_BKHFPC = 6,                    //	Beckhoff PC C2001
        IODEVICETYPE_CP5412A2 = 7,                  //	ProfiBus-Master (Siemens)
        IODEVICETYPE_SERCANSISA = 8,                //	Sercos Master (Indramat)
        IODEVICETYPE_LPTPORT = 9,                   //	Lpt Port
        IODEVICETYPE_DPRAM = 10,                    //	Generic DPRAM
        IODEVICETYPE_COMPORT = 11,                  //	COM Port
        IODEVICETYPE_CIF30CAN = 12,                 //	ISA CANopen-Master (Hilscher)
        IODEVICETYPE_CIF30PB = 13,                  //	ISA ProfiBus-Master 8 kByte (Hilscher)
        IODEVICETYPE_BKHFCP2030 = 14,               //	Beckhoff CP2030 (Pannel-Link)
        IODEVICETYPE_IBSSCIT = 15,                  //	Interbus-S-Master (Phoenix)
        IODEVICETYPE_CIF30IBM = 16,                 //	ISA Interbus-S-Master (Hilscher)
        IODEVICETYPE_CIF30DNM = 17,                 //	ISA DeviceNet-Master (Hilscher)
        IODEVICETYPE_FCXXXX = 18,                   //	Beckhoff-Filedbus card 
        IODEVICETYPE_CIF50PB = 19,                  //	PCI ProfiBus-Master 8 kByte (Hilscher)
        IODEVICETYPE_CIF50IBM = 20,                 //	PCI Interbus-S-Master (Hilscher)
        IODEVICETYPE_CIF50DNM = 21,                 //	PCI DeviceNet-Master (Hilscher)
        IODEVICETYPE_CIF50CAN = 22,                 //	PCI CANopen-Master (Hilscher)
        IODEVICETYPE_CIF60PB = 23,                  //	PCMCIA ProfiBus-Master (Hilscher)
        IODEVICETYPE_CIF60DNM = 24,                 //	PCMCIA DeviceNet-Master (Hilscher)
        IODEVICETYPE_CIF60CAN = 25,                 //	PCMCIA CANopen-Master (Hilscher)
        IODEVICETYPE_CIF104DP = 26,                 //	PC104 ProfiBus-Master 2 kByte (Hilscher)
        IODEVICETYPE_C104PB = 27,                   //	PC104 ProfiBus-Master 8 kByte (Hilscher)
        IODEVICETYPE_C104IBM = 28,                  //	PC104 Interbus-S-Master 2 kByte (Hilscher)
        IODEVICETYPE_C104CAN = 29,                  //	PC104 CANopen-Master (Hilscher)
        IODEVICETYPE_C104DNM = 30,                  //	PC104 DeviceNet-Master (Hilscher)
        IODEVICETYPE_BKHFCP9030 = 31,               //	Beckhoff CP9030 (Pannel-Link with UPS)
        IODEVICETYPE_SMB = 32,                      //	Motherboard System Management Bus
        IODEVICETYPE_PBMON = 33,                    //	Beckhoff-PROFIBUS-Monitor
        IODEVICETYPE_CP5613 = 34,                   //	PCI ProfiBus-Master (Siemens)
        IODEVICETYPE_CIF60IBM = 35,                 //	PCMCIA Interbus-S-Master (Hilscher)
        IODEVICETYPE_FC200X = 36,                   //	Beckhoff-Lightbus-I/II-PCI-Karte
        IODEVICETYPE_FC3100_OLD = 37,               //	obsolete: dont use
        IODEVICETYPE_FC3100 = 38,                   //	Beckhoff-Profibus-PCI
        IODEVICETYPE_FC5100 = 39,                   //	Beckhoff-CanOpen-PCI
        IODEVICETYPE_FC5200 = 41,                   //	Beckhoff-DeviceNet-PCI
        IODEVICETYPE_BKHFNCBP = 43,                 //	Beckhoff NC back plane
        IODEVICETYPE_SERCANSPCI = 44,               //	Sercos Master (SICAN/IAM PCI)
        IODEVICETYPE_ETHERNET = 45,                 //	Virtual Ethernet Device
        IODEVICETYPE_SERCONPCI = 46,                //	Sercon 410B or 816 Chip Master or Slave (PCI)
        IODEVICETYPE_IBSSCRIRTLK = 47,              //	Interbus-S-Master with Slave-Module LWL Basis (Phoenix)
        IODEVICETYPE_FC7500 = 48,                   //	Beckhoff-SERCOS-PCI
        IODEVICETYPE_CIF30IBS = 49,                 //	ISA Interbus-S-Slave (Hilscher)
        IODEVICETYPE_CIF50IBS = 50,                 //	PCI Interbus-S-Slave (Hilscher)
        IODEVICETYPE_C104IBS = 51,                  //	PC104 Interbus-S-Slave (Hilscher)
        IODEVICETYPE_BKHFCP9040 = 52,               //	Beckhoff CP9040 (CP-PC) 
        IODEVICETYPE_BKHFAH2000 = 53,               //	Beckhoff AH2000 (Hydr. Backplane) 
        IODEVICETYPE_BKHFCP9035 = 54,               //	Beckhoff CP9035 (PCI, Pannel-Link with UPS) 
        IODEVICETYPE_AH2000MC = 55,                 //	Beckhoff-AH2000 with Profibus-MC 
        IODEVICETYPE_FC3100MON = 56,                //	Beckhoff-Profibus-Monitor-PCI 
        IODEVICETYPE_USB = 57,                      //	Virtual USB Device 
        IODEVICETYPE_FC5100MON = 58,                //	Beckhoff-CANopen-Monitor-PCI 
        IODEVICETYPE_FC5200MON = 59,                //	Beckhoff-DeviceNet-Monitor-PCI 
        IODEVICETYPE_FC3100SLV = 60,                //	Beckhoff-Profibus-PCI Slave 
        IODEVICETYPE_FC5100SLV = 61,                //	Beckhoff-CanOpen-PCI Slave 
        IODEVICETYPE_FC5200SLV = 62,                //	Beckhoff-DeviceNet-PCI Slave 
        IODEVICETYPE_IBSSCITPCI = 63,               //	PCI Interbus-S-Master (Phoenix) 
        IODEVICETYPE_IBSSCRIRTLKPCI = 64,           //	PCI Interbus-S-Master with Slave-Modulel LWL Basis (Phoenix) 
        IODEVICETYPE_CX1100_BK = 65,                //	Beckhoff-CX1100 terminal bus power supply 
        IODEVICETYPE_ENETRTMP = 66,                 //	Ethernet real time miniport 
        IODEVICETYPE_CX1500_M200 = 67,              //	PC104 Lightbus-Master 
        IODEVICETYPE_CX1500_B200 = 68,              //	PC104 Lightbus-Slave 
        IODEVICETYPE_CX1500_M310 = 69,              //	PC104 ProfiBus-Master 
        IODEVICETYPE_CX1500_B310 = 70,              //	PC104 ProfiBus-Slave 
        IODEVICETYPE_CX1500_M510 = 71,              //	PC104 CANopen-Master 
        IODEVICETYPE_CX1500_B510 = 72,              //	PC104 CANopen-Slave 
        IODEVICETYPE_CX1500_M520 = 73,              //	PC104 DeviceNet-Master 
        IODEVICETYPE_CX1500_B520 = 74,              //	PC104 DeviceNet-Slave 
        IODEVICETYPE_CX1500_M750 = 75,              //	PC104 Sercos-Master 
        IODEVICETYPE_CX1500_B750 = 76,              //	PC104 Sercos-Slave 
        IODEVICETYPE_BX_BK = 77,                    //	BX terminal bus interface 
        IODEVICETYPE_BX_M510 = 78,                  //	BX SSB-Master 
        IODEVICETYPE_BX_B310 = 79,                  //	BX ProfiBus-Slave 
        IODEVICETYPE_IBSSCRIRTPCI = 80,             //	PCI Interbus-S-Master with slave module copper basis (Phoenix) 
        IODEVICETYPE_BX_B510 = 81,                  //	BX CANopen Slave 
        IODEVICETYPE_BX_B520 = 82,                  //	BX DeviceNet Slave 
        IODEVICETYPE_BC3150 = 83,                   //	BCxx50 ProfiBus Slave 
        IODEVICETYPE_BC5150 = 84,                   //	BCxx50 CANopen Slave 
        IODEVICETYPE_BC5250 = 85,                   //	BCxx50 DeviceNet Slave 
        IODEVICETYPE_EL6731 = 86,                   //	Beckhoff Profibus-EtherCAT Terminal 
        IODEVICETYPE_EL6751 = 87,                   //	Beckhoff CanOpen-EtherCAT Terminal 
        IODEVICETYPE_EL6752 = 88,                   //	Beckhoff DeviceNet-EtherCAT Terminal 
        IODEVICETYPE_COMPB = 89,                    //	COM ProfiBus Master 8 kByte (Hilscher) 
        IODEVICETYPE_COMIBM = 90,                   //	COM Interbus-S Master (Hilscher) 
        IODEVICETYPE_COMDNM = 91,                   //	COM DeviceNet Master (Hilscher) 
        IODEVICETYPE_COMCAN = 92,                   //	COM CANopen Master (Hilscher) 
        IODEVICETYPE_COMIBS = 93,                   //	COM CANopen Slave (Hilscher) 
        IODEVICETYPE_ETHERCAT = 94,                 //	EtherCAT in direct mode 
        IODEVICETYPE_PROFINETIOCONTROLLER = 95,     //	PROFINET Master 
        IODEVICETYPE_PROFINETIODEVICE = 96,         //	PROFINET Slave 
        IODEVICETYPE_EL6731SLV = 97,                //	Beckhoff Profibus Slave EtherCAT Terminal 
        IODEVICETYPE_EL6751SLV = 98,                //	Beckhoff CanOpen Slave EtherCAT Terminal 
        IODEVICETYPE_EL6752SLV = 99,                //	Beckhoff DeviceNet Slave EtherCAT Terminal 
        IODEVICETYPE_C104PPB = 100,                 //	PC104+ ProfiBus Master 8 kByte (Hilscher) 
        IODEVICETYPE_C104PCAN = 101,                //	PC104+ CANopen Master (Hilscher) 
        IODEVICETYPE_C104PDNM = 102,                //	PC104+ DeviceNet Master (Hilscher) 
        IODEVICETYPE_BC8150 = 103,                  //	BCxx50 Serial Slave 
        IODEVICETYPE_BX9000 = 104,                  //	BX9000 Ethernet Slave 
        IODEVICETYPE_CX9000_BK = 105,               //	Beckhoff-CX9000 K-Bus Power Supply 
        IODEVICETYPE_EL6601 = 106,                  //	Beckhoff-RT-Ethernet-EtherCAT-Terminal 
        IODEVICETYPE_BC9050 = 107,                  //	BC9050 Ethernet Slave 
        IODEVICETYPE_BC9120 = 108,                  //	BC9120 Ethernet Slave 
        IODEVICETYPE_ENETADAPTER = 109,             //	Ethernet Miniport Adapter 
        IODEVICETYPE_BC9020 = 110,                  //	BC9020 Ethernet Slave 
        IODEVICETYPE_ETHERCATPROT = 111,            //	EtherCAT Protocol in direct mode 
        IODEVICETYPE_ETHERNETNVPROT = 112,          //	
        IODEVICETYPE_ETHERNETPNMPROT = 113,         //	Profinet Controller 
        IODEVICETYPE_EL6720 = 114,                  //	Beckhoff-Lightbus-EtherCAT-Terminal 
        IODEVICETYPE_ETHERNETPNSPROT = 115,         //	Profinet Device
        IODEVICETYPE_BKHFCP6608 = 116,              //	Beckhoff CP6608(IXP PC) 
        IODEVICETYPE_PTP_IEEE1588 = 117,            //	
        IODEVICETYPE_EL6631SLV = 118,               //	EL6631-0010 Profinet Slave terminal 
        IODEVICETYPE_EL6631 = 119,                  //	EL6631 Profinet Master terminal 
        IODEVICETYPE_CX5000_BK = 120,               //	Beckhoff-CX5100 K-Bus power supply 
        IODEVICETYPE_PCIDEVICE = 121,               //	Generic PCI DPRAM (TCOM) 
        IODEVICETYPE_ETHERNETUPDPROT = 122,         //	UDP Protocol 
        IODEVICETYPE_ETHERNETAUTOPROT = 123,        //	Automation Protocol 
        IODEVICETYPE_CCAT = 124,                    //	CCAT 
        IODEVICETYPE_CPLINK3 = 125,                 //	Virtuelles USB Device (remote via CPLINK3) 
        IODEVICETYPE_EL6632 = 126,                  //	EL6632 
        IODEVICETYPE_CCAT_PBM = 127,                //	CCAT Profibus Master 
        IODEVICETYPE_CCAT_PBS = 128,                //	CCAT Profibus Slave 
        IODEVICETYPE_CCAT_CNM = 129,                //	CCAT CANopen Master 
        IODEVICETYPE_ETHERCATSLAVE = 130,           //	EtherCAT Slave 
        IODEVICETYPE_BACNET = 131,                  //	BACnet device 
        IODEVICETYPE_CCAT_CNS = 132,                //	CCAT CANopen Slave 
        IODEVICETYPE_ETHIP_SCANNER = 133,           //	ETHERNET IP Master 
        IODEVICETYPE_ETHIP_ADAPTER = 134,           //	ETHERNET IP Slave (OLD) 
        IODEVICETYPE_CX8000_BK = 135,               //	Beckhoff-CX8100 Klemmenbus Netzteil - LEGACY use IODEVICETYPE_CX_BK 
        IODEVICETYPE_ETHERNETUDPPROT = 136,         //	Upd Protocol 
        IODEVICETYPE_BC9191 = 137,                  //	BC9191 Etherent Slave 
        IODEVICETYPE_ENETPROTOCOL = 138,            //	Real-Time Ethernet Protocol (BK90xx, AX2000-B900) 
        IODEVICETYPE_ETHIP_ADAPTEREX = 139,         //	ETHERNET IP Slave (NEW) 
        IODEVICETYPE_PNCONTR_CCAT_RT = 140,         //	Profinet Controller CCAT RT 
        IODEVICETYPE_PNCONTR_CCAT_IRT = 141,        //	Profinet Controller CCAT RT + IRT 
        IODEVICETYPE_PNDEV_CCAT_RT = 142,           //	Profinet Device CCAT RT 
        IODEVICETYPE_PNDEV_CCAT_IRT = 143,          //	Profinet Device CCAT RT + IRT 
        IODEVICETYPE_ETHERCATSIMULATION = 144,      //	EtherCAT-Simulation 
        IODEVICETYPE_EL6652SLV = 145,               //	EL6652-0010 
        IODEVICETYPE_PTP_VIA_CCAT = 146,            //	PTP CLock via CCAT 
        IODEVICETYPE_BACNETR9 = 147,                //	BACnet Rev9 device 
        IODEVICETYPE_ETHERCATXFC = 148,             //	EtherCAT in xfc mode 
        IODEVICETYPE_CX2500_0030 = 149,             //	CX2500-0030 RS232 Serial Communication Port 
        IODEVICETYPE_CX2500_0031 = 150,             //	CX2500-0031 RS422/RS485 Serial Communication Port 
        IODEVICETYPE_EL6652MST = 151,               //	EL6652 
        IODEVICETYPE_ENETADSADAPTER = 152,          //	Ethernet Ads Adapter (ET9000) 
        IODEVICETYPE_CCAT_EIPM = 153,               //	CCAT EtherNet/IP Master 
        IODEVICETYPE_CCAT_EIPS = 154,               //	CCAT EtherNet/IP Slave 
        IODEVICETYPE_OPCUADEVICE = 155,			    //	Opc Ua Device 
    }

    public class DeviceViewModel : BaseViewModel
    {
        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }

        private int id;
        public int Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                NotifyPropertyChanged(nameof(Id));
            }
        }

        private DeviceTypes type;
        public DeviceTypes Type
        {

            get { return this.type; }
            set
            {
                this.type = value;
                NotifyPropertyChanged(nameof(Type));
            }
        }

        private string treeViewDesc;
        public string TreeViewDesc
        {
            get 
            {
                this.treeViewDesc = this.Name + @" (Id: " + this.Id + @", Type: " + this.Type + ")";
                return this.treeViewDesc; 
            }
            //set
            //{
            //    if (!string.IsNullOrEmpty(value))
            //    {
            //        this.treeViewDesc = value;
            //        NotifyPropertyChanged(nameof(TreeViewDesc));
            //    }
            //}
        }

        private string amsNetId;
        public string AmsNetId
        {
            get { return this.amsNetId; }
            set
            {
                this.amsNetId = value;
                NotifyPropertyChanged(nameof(AmsNetId));
            }
        }

        private int amsPort;
        public int AmsPort
        {
            get { return this.amsPort; }
            set
            {
                this.amsPort = value;
                NotifyPropertyChanged(nameof(AmsPort));
            }
        }

        private int totalNumberOfBoxes;
        public int TotalNumberOfBoxes
        {
            get { return this.totalNumberOfBoxes; }
            set
            {
                this.totalNumberOfBoxes = value;
                NotifyPropertyChanged(nameof(TotalNumberOfBoxes));
            }
        }

        private int numberOfSubBoxes;
        public int NumberOfSubBoxes
        {
            get { return this.numberOfSubBoxes; }
            set
            {
                this.numberOfSubBoxes = value;
                NotifyPropertyChanged(nameof(NumberOfSubBoxes));
            }
        }

        private ObservableCollection<BoxViewModel> boxes;
        public ObservableCollection<BoxViewModel> Boxes
        {
            get
            {
                return this.boxes ?? (this.boxes = new ObservableCollection<BoxViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.boxes = value;
                    NotifyPropertyChanged(nameof(Boxes));
                }
            }
        }

        private bool infoDataSupport;
        public bool InfoDataSupport
        {
            get { return this.infoDataSupport; }
            set
            {
                this.infoDataSupport = value;
                NotifyPropertyChanged(nameof(InfoDataSupport));
            }
        }

        private bool infoDataId;
        public bool InfoDataId
        {
            get { return this.infoDataId; }
            set
            {
                this.infoDataId = value;
                NotifyPropertyChanged(nameof(InfoDataId));
            }
        }

        private bool infoDataNetId;
        public bool InfoDataNetId
        {
            get { return this.infoDataNetId; }
            set
            {
                this.infoDataNetId = value;
                NotifyPropertyChanged(nameof(InfoDataNetId));
            }
        }

        private bool infoDataChangeCnt;
        public bool InfoDataChangeCnt
        {
            get { return this.infoDataChangeCnt; }
            set
            {
                this.infoDataChangeCnt = value;
                NotifyPropertyChanged(nameof(InfoDataChangeCnt));
            }
        }

        private bool infoDataCfgSlaveCnt;
        public bool InfoDataCfgSlaveCnt
        {
            get { return this.infoDataCfgSlaveCnt; }
            set
            {
                this.infoDataCfgSlaveCnt = value;
                NotifyPropertyChanged(nameof(InfoDataCfgSlaveCnt));
            }
        }

        private bool infoDataDcTimeOffsets;
        public bool InfoDataDcTimeOffsets
        {
            get { return this.infoDataDcTimeOffsets; }
            set
            {
                this.infoDataDcTimeOffsets = value;
                NotifyPropertyChanged(nameof(InfoDataDcTimeOffsets));
            }
        }

        private ObservableCollection<object> infoData;
        public ObservableCollection<object> InfoData
        {
            get
            {
                return this.infoData ?? (this.infoData = new ObservableCollection<object>());
            }
            set
            {
                if (value != null)
                {
                    this.infoData = value;
                    NotifyPropertyChanged(nameof(InfoData));
                }
            }
        }

        private ObservableCollection<SyncUnitViewModel> syncUnits;
        public ObservableCollection<SyncUnitViewModel> SyncUnits
        {
            get
            {
                return this.syncUnits ?? (this.syncUnits = new ObservableCollection<SyncUnitViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.syncUnits = value;
                    NotifyPropertyChanged(nameof(SyncUnits));
                }
            }
        }

        private ObservableCollection<MapableObject> mapableObjects;
        public ObservableCollection<MapableObject> MapableObjects
        {
            get
            {
                return this.mapableObjects ?? (this.mapableObjects = new ObservableCollection<MapableObject>());
            }
            set
            {
                if (value != null)
                {
                    this.mapableObjects = value;
                    NotifyPropertyChanged(nameof(MapableObjects));
                }
            }
        }

        private MapableObject mapableObjectGrouped;
        public MapableObject MapableObjectGrouped
        {
            get
            {
                return this.mapableObjectGrouped ?? (this.mapableObjectGrouped = new MapableObject());
            }
            set
            {
                if (value != null)
                {
                    this.mapableObjectGrouped = value;
                    NotifyPropertyChanged(nameof(MapableObjectGrouped));
                }
            }
        }
    }
}
