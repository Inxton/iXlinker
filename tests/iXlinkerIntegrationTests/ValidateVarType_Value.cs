using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Reflection;
using iXlinkerDtos;
using iXlinker.TsprojFile;

namespace iXlinkerIntegrationTests
{
    public class ValidateVarType_Value
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test, Order(300)]
        public void T300_BIT()
        {
            PdoEntryViewModel var = new PdoEntryViewModel() { Name = "", Type_Value = "BIT" };
            Assert.AreEqual("BOOL", iXlinker.TsprojFile.Var.Validate.Type_Value(var));

            var.Name = "Item";
            Assert.AreEqual("BOOL", iXlinker.TsprojFile.Var.Validate.Type_Value(var));
        }

        [Test, Order(301)]
        public void T301_BITARR4()
        {
            PdoEntryViewModel var = new PdoEntryViewModel() { Name = "", Type_Value = "BITARR4" };
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.Var.Validate.Type_Value(var));

            var.Name = "Item";
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.Var.Validate.Type_Value(var));
        }

        [Test, Order(302)]
        public void T302_BITARR8()
        {
            PdoEntryViewModel var = new PdoEntryViewModel() { Name = "", Type_Value = "BITARR8" };
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.Var.Validate.Type_Value(var));

            var.Name = "Item";
            Assert.AreEqual("BYTE", iXlinker.TsprojFile.Var.Validate.Type_Value(var));
        }

        [Test, Order(303)]
        public void T303_BITARR16()
        {
            PdoEntryViewModel var = new PdoEntryViewModel() { Name = "", Type_Value = "BITARR16" };
            Assert.AreEqual("WORD", iXlinker.TsprojFile.Var.Validate.Type_Value(var));

            var.Name = "Item";
            Assert.AreEqual("WORD", iXlinker.TsprojFile.Var.Validate.Type_Value(var));
        }

        [Test, Order(304)]
        public void T304_MASTER_MESSAGE()
        {
            PdoEntryViewModel var = new PdoEntryViewModel() { Name = "", Type_Value = "MASTER_MESSAGE" };
            Assert.AreEqual("ARRAY[0..5] OF BYTE", iXlinker.TsprojFile.Var.Validate.Type_Value(var));

            var.Name = "Item";
            Assert.AreEqual("ARRAY[0..5] OF BYTE", iXlinker.TsprojFile.Var.Validate.Type_Value(var));
        }

        [Test, Order(305)]
        public void T305_SLAVE_MESSAGE()
        {
            PdoEntryViewModel var = new PdoEntryViewModel() { Name = "", Type_Value = "SLAVE_MESSAGE" };
            Assert.AreEqual("ARRAY[0..5] OF BYTE", iXlinker.TsprojFile.Var.Validate.Type_Value(var));

            var.Name = "Item";
            Assert.AreEqual("ARRAY[0..5] OF BYTE", iXlinker.TsprojFile.Var.Validate.Type_Value(var));
        }

        [Test, Order(306)]
        public void T306_UINTARR2()
        {
            PdoEntryViewModel var = new PdoEntryViewModel() { Name = "", Type_Value = "UINTARR2" };
            Assert.AreEqual("ARRAY [0..1] OF UINT", iXlinker.TsprojFile.Var.Validate.Type_Value(var));

            var.Name = "Item";
            Assert.AreEqual("ARRAY [0..1] OF UINT", iXlinker.TsprojFile.Var.Validate.Type_Value(var));
        }

        [Test, Order(307)]
        public void T307_NotExistingType()
        {
            PdoEntryViewModel var = new PdoEntryViewModel() { Name = "", Type_Value = "UINT12", BoxOrderCode = "BoxWithNotExistingType", VarA = "Inputs^Channel_1" };
            Assert.AreEqual("UINT12", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(var));

            var.Name = "Item";
            Assert.AreEqual("UINT12", iXlinker.TsprojFile.PdoEntry.Validate.Type_Value(var));
        }
    }
}

