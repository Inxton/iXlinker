namespace TwincatXmlSchemas.TcSmProject
{
    public interface IBox
    {
        bool DisabledSpecified { get; set; }
        bool Disabled { get; set; }
        string Name { get; set; }

        object Item { get; set; }
        string Id { get; set; }

        BusCoupler BusCoupler { get; }

        int BoxType { get; }
        TcSmBoxDefBox[] Box { get; set; }

        TcSmVarGrpDef[] Vars { get; set; }

    }

    public partial class TcSmBoxDefBox : IBox
    {

    }

    public partial class TcSmDevDefBox : IBox 
    {

    }

    public partial class TcSmBoxDef : IBox
    {

    }

    

}