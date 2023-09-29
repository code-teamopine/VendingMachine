using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Data
{
    public class FileHandler
    {
        private const int posItemNumber = 0;
        private const int posItemName = 1;
        private const int posItemPrice = 2;
        private const int posItemType = 3;
        private const int itemStockCoke = 10;
        private const int itemStockMM = 15;
        private const int itemStockWater = 5;
        private const int itemStockSnickers = 7;

        public async Task<Dictionary<string, MachineItem>> GetVendingItems()
        {
            var dataFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\vendingmachine.csv";

            Dictionary<string, MachineItem> items = new Dictionary<string, MachineItem>();
            if (File.Exists(dataFile))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(dataFile))
                    {
                        string line = null;
                        while ((line = await sr.ReadLineAsync()) != null)
                        {
                            string[] itemDetails = line.Split("|");
                            string itemName = itemDetails[posItemName];
                            if (!decimal.TryParse(itemDetails[posItemPrice], out decimal itemPrice))
                            {
                                itemPrice = 0M;
                            }
                            int itemsRemaining = 5;
                            MachineItem item;
                            switch (itemDetails[posItemType])
                            {
                                case "Chip":
                                    item = new Chip(itemName, itemPrice, itemsRemaining);
                                    break;
                                case "Drink":
                                    item = new Drink(itemName, itemPrice, itemsRemaining);
                                    break;
                                case "Gum":
                                    item = new Gum(itemName, itemPrice, itemsRemaining);
                                    break;
                                case "Coke":
                                    item = new Coke(itemName, itemPrice, itemStockCoke);
                                    break;
                                case "MM":
                                    item = new MM(itemName, itemPrice, itemStockMM);
                                    break;
                                case "Water":
                                    item = new Water(itemName, itemPrice, itemStockWater);
                                    break;
                                case "Snickers":
                                    item = new Snickers(itemName, itemPrice, itemStockSnickers);
                                    break;
                                default:
                                    item = new Chip(itemName, itemPrice, itemsRemaining);
                                    break;
                            }
                            items.Add(itemDetails[posItemNumber], item);
                        }
                    }
                }
                catch(FileNotFoundException ex)
                {
                    Console.WriteLine("File Not Found "+ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Input file is missing!! The vending machine will now self destruct.");
                Console.ReadLine();
                //items.Add("A1", new Drink("YOU BROKE IT!", 10000M, 5));
            }
            return items;
        }
    }
}
