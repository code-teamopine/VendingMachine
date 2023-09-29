using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Service;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineTest
    {
        [DataTestMethod]
        [DataRow("5", 5)]
        [DataRow("10", 10)]
        [DataRow("2", 2)]
        [DataRow("1", 1)]
        public void TestsIfDepositingCashWorksCorrectly(string input, int expected)
        {
            VendingMachineServices vm = new VendingMachineServices();
            vm.MoneyService.AddMoney(input);
            decimal result = vm.MoneyService.MoneyInMachine;

            Assert.AreEqual((decimal)expected, result);
        }

        [TestMethod]
        public void TestsIfReturnCashAsExpected()
        {
            VendingMachineServices vm = new VendingMachineServices();
            vm.MoneyService.AddMoney("1.35");
            string result = vm.MoneyService.GiveChange();

            Assert.AreEqual(result, "Your change is 5 quarters, 1 dimes, and 0 nickels");

        }

        [TestMethod]
        public void TestsIfWillReturnOutOfStockIfSold5orMore()
        {
            VendingMachineServices vm = new VendingMachineServices();
            vm.MoneyService.AddMoney("10");
            vm.RetreiveItem("A4");
            vm.RetreiveItem("A4");
            vm.RetreiveItem("A4");
            vm.RetreiveItem("A4");
            vm.RetreiveItem("A4");
            vm.RetreiveItem("A4");
            string result = vm.VendingMachineItems["A4"].MessageWhenSoldOut;
            string expected = "Sold out of Zapp's Blood Moon Chip!\nBuy something else!";

            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void TestsIfNotEnoughMoneyEnteredToPurchaseItem()
        {
            VendingMachineServices vm = new VendingMachineServices();
            vm.RetreiveItem("A1");
            string result = vm.MessageToUser;
            Assert.AreEqual("Not enough money in the machine to complete the transaction.", result);
        }

    }
}
