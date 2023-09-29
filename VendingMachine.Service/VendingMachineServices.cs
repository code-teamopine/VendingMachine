using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Data;

namespace VendingMachine.Service
{
    public interface IVendingMachineServices
    {

        void DisplayAllItems();

        bool ItemExists(string itemNumber);

        bool RetreiveItem(string itemNumber, string amount = "0", int quantity = 1);

        void DisplaySubMenu();
        
    }
    public class VendingMachineServices: IVendingMachineServices
    {
        private Logger Log = new Logger();
        public Dictionary<string, MachineItem> VendingMachineItems = new Dictionary<string, MachineItem>();
        FileHandler HandleFiles = new FileHandler();
        public MoneyService MoneyService { get; }
        public string NotEnoughMoneyError = "Not enough money in the machine to complete the transaction.";
        public string MessageToUser;

        public VendingMachineServices()
        {
            this.VendingMachineItems = this.HandleFiles.GetVendingItems().Result;
            this.MoneyService = new MoneyService(this.Log);
        }

        public decimal MoneyInMachine
        {
            get
            {
                return this.MoneyService.MoneyInMachine;
            }
        }

        public void DisplayAllItems()
        {
            Console.WriteLine($"{"#".PadRight(5)} {"Stock"} { "Product".PadRight(40) } { "Price".PadLeft(7)}");
            foreach (KeyValuePair<string, MachineItem> kvp in this.VendingMachineItems)
            {
                if (kvp.Value.ItemsRemaining > 0)
                {
                    string itemNumber = kvp.Key.PadRight(5);
                    string itemsRemaining = kvp.Value.ItemsRemaining.ToString().PadRight(5);
                    string productName = kvp.Value.ProductName.PadRight(40);
                    string price = kvp.Value.Price.ToString("C").PadLeft(7);
                    Console.WriteLine($"{itemNumber} {itemsRemaining} {productName}  {price}");
                }
                else
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value.ProductName} IS SOLD OUT.");
                }
            }
        }

        public bool ItemExists(string itemNumber)
        {
            return this.VendingMachineItems.ContainsKey(itemNumber);
        }

        public bool RetreiveItem(string itemNumber, string amount = "0", int quantity = 1)
        {
            // If the item exists (not Q1 or something like that)
            // and we can remove the item
            // and we have more money in the machine than the price
            this.MoneyService.AddMoney(amount);
            if (this.ItemExists(itemNumber)
                && this.MoneyService.MoneyInMachine >= this.VendingMachineItems[itemNumber].Price * quantity
                && this.VendingMachineItems[itemNumber].ItemsRemaining - quantity >= 0
                && this.VendingMachineItems[itemNumber].RemoveItem(quantity))
            {
                // Logging message "CANDYBARNAME A1"
                string message = $"{this.VendingMachineItems[itemNumber].ProductName.ToUpper()} {itemNumber}";

                // Logging before: current money in machine
                decimal before = this.MoneyService.MoneyInMachine;

                // Remove the money
                this.MoneyService.RemoveMoney(this.VendingMachineItems[itemNumber].Price * quantity);

                // Logging after: current money in machine
                decimal after = this.MoneyService.MoneyInMachine;

                // Log the log
                this.Log.Log(message, before, after);

                return true;
            }
            else
            {
                this.MoneyService.RemoveMoney(Convert.ToDecimal(amount));
                return false;
            }
        }
        public void DisplaySubMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Please choose from the following options:");
                Console.WriteLine("1] >> Feed Money");
                Console.WriteLine("2] >> Select Product");
                Console.WriteLine("3] >> Finish Transaction");
                Console.WriteLine("Q] >> Return to Main Menu");
                Console.WriteLine($"Money in Machine: {this.MoneyInMachine.ToString("C")}");
                Console.Write("What option do you want to select? ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.WriteLine(">>> How much do you want to insert?");
                    while (true)
                    {
                        Console.Write("1, 2, 5, 10 dollars? ");
                        string amountToSubmit = Console.ReadLine();
                        if (amountToSubmit == "1"
                            || amountToSubmit == "2"
                            || amountToSubmit == "5"
                            || amountToSubmit == "10")
                        {
                            if (!this.MoneyService.AddMoney(amountToSubmit))
                            {
                                Console.WriteLine("Insert a valid amount.");
                            }
                            else
                            {
                                Console.WriteLine($"Money in Machine: {this.MoneyService.MoneyInMachine.ToString("C")}");
                                break;
                            }
                        }
                    }

                }
                else if (input == "2")
                {
                    while (true)
                    {
                        this.DisplayAllItems();
                        Console.Write(">>> What item do you want? ");
                        string choice = Console.ReadLine();
                        if (this.ItemExists(choice) && this.RetreiveItem(choice))
                        {
                            Console.WriteLine($"Enjoy your {this.VendingMachineItems[choice].ProductName}\n{this.VendingMachineItems[choice].MessageWhenVended}");
                            break;
                        }
                        else if (!this.ItemExists(choice))
                        {
                            Console.Clear();
                            Console.WriteLine("**INVALID ITEM**");
                        }
                        else if (this.ItemExists(choice) && this.MoneyService.MoneyInMachine > this.VendingMachineItems[choice].Price)
                        {
                            Console.WriteLine(this.VendingMachineItems[choice].MessageWhenSoldOut);
                        }
                        else if (this.MoneyService.MoneyInMachine < this.VendingMachineItems[choice].Price)
                        {
                            Console.WriteLine(this.NotEnoughMoneyError);
                            break;
                        }
                    }
                }
                else if (input == "3")
                {
                    Console.WriteLine("Finishing Transaction");
                    Console.WriteLine(this.MoneyService.GiveChange());
                }
                else if (input.ToUpper() == "Q")
                {
                    Console.WriteLine("Returning to main menu");
                    break;
                }
                else
                {
                    Console.WriteLine("Please try again");
                }

                Console.ReadLine();
            }
        }
    }
}
