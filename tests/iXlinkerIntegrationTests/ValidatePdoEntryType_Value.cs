using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Reflection;
using iXlinkerDtos;
using iXlinker.TsprojFile;

namespace iXlinkerIntegrationTests
{
    public class ValidatePdoEntryType_Value
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test, Order(100)]
        public void T100_Structured_ArrayZeroToZeroOfBit()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "Structure__", Type_Value = "ARRAY [0..0] OF BIT" };
            Assert.AreEqual("BIT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Structure__item";
            Assert.AreEqual("BIT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(101)]
        public void T101_Structured_ArrayZeroToZeroOfByte()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "Structure__", Type_Value = "ARRAY [0..0] OF BYTE" };
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Structure__item";
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(102)]
        public void T102_Structured_BIT2()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "Structure__", Type_Value = "BIT2" };
            Assert.AreEqual("ARRAY [0..1] OF BIT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Structure__item";
            Assert.AreEqual("ARRAY [0..1] OF BIT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(103)]
        public void T103_Structured_BIT3()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "Structure__", Type_Value = "BIT3" };
            Assert.AreEqual("ARRAY [0..2] OF BIT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Structure__item";
            Assert.AreEqual("ARRAY [0..2] OF BIT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(104)]
        public void T104_Structured_BIT4()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "Structure__", Type_Value = "BIT4" };
            Assert.AreEqual("ARRAY [0..3] OF BIT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Structure__item";
            Assert.AreEqual("ARRAY [0..3] OF BIT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(105)]
        public void T105_Structured_BIT5()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "Structure__", Type_Value = "BIT5" };
            Assert.AreEqual("ARRAY [0..4] OF BIT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Structure__item";
            Assert.AreEqual("ARRAY [0..4] OF BIT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(106)]
        public void T106_Structured_NotExistingType()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "", Type_Value = "UINT12", BoxOrderCode = "BoxWithNotExistingType", VarA = "Inputs^Channel_1__Counter" };
            Assert.AreEqual("UINT12", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Item";
            Assert.AreEqual("UINT12", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(200)]
        public void T200_Unstructured_ArrayZeroToZeroOfByte()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "", Type_Value = "ARRAY [0..0] OF BYTE" };
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Item";
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(201)]
        public void T201_Unstructured_ArrayOfBit()
        {
            for(ushort i = 1;i <= 10; i++)
            {
                PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "", Type_Value = "ARRAY [0.." + i.ToString() + "] OF BIT" };
                Assert.AreEqual("ARRAY [0.." + i.ToString() + "] OF BOOL", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

                pdoEntry.Name = "Item";
                Assert.AreEqual("ARRAY [0.." + i.ToString() + "] OF BOOL", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
            }
        }

        [Test, Order(202)]
        public void T202_Unstructured_BIT()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "", Type_Value = "BIT" };
            Assert.AreEqual("BOOL", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Item";
            Assert.AreEqual("BOOL", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(203)]
        public void T203_Unstructured_BIT2()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "", Type_Value = "BIT2" };
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Item";
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(204)]
        public void T204_Unstructured_BIT3()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "", Type_Value = "BIT3" };
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Item";
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(205)]
        public void T205_Unstructured_UINT16()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "", Type_Value = "UINT16" };
            Assert.AreEqual("ARRAY [0..1] OF USINT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Item";
            Assert.AreEqual("ARRAY [0..1] OF USINT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(206)]
        public void T206_Unstructured_UINT24()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "", Type_Value = "UINT24" };
            Assert.AreEqual("ARRAY [0..2] OF USINT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Item";
            Assert.AreEqual("ARRAY [0..2] OF USINT", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }

        [Test, Order(207)]
        public void T207_Unstructured_NotExistingType()
        {
            PdoEntryViewModel pdoEntry = new PdoEntryViewModel() { Name = "", Type_Value = "UINT12", BoxOrderCode = "BoxWithNotExistingType" , VarA = "Inputs^Channel_1"};
            Assert.AreEqual("UINT12", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));

            pdoEntry.Name = "Item";
            Assert.AreEqual("UINT12", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(pdoEntry));
        }
    }
}