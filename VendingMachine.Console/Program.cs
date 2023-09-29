using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using VendingMachine.Data;
using VendingMachine.Service;

namespace VendingMachine.Consoles
{
    class Program
    {
        static void Main(string[] args)
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            ServiceCollection collection = new ServiceCollection();
            collection.AddScoped<IVendingMachineServices, VendingMachineServices>();
            IServiceProvider serviceProvider = collection.BuildServiceProvider();
            VendingMachineServices wms = (VendingMachineServices)serviceProvider.GetService<IVendingMachineServices>();
            while (true)
            {
                tostart:
                Console.WriteLine();
                Console.WriteLine("Main Menu");
                Console.WriteLine("Command 1: inv");
                Console.WriteLine("Command 2: Order <amount> <item_number> <Quntity>");
                Console.WriteLine($"Money in Machine: {wms.MoneyInMachine.ToString("C")}");
                Console.Write("What option do you want to select? ");
                string input = Console.ReadLine();

                if (input.ToLower().StartsWith("order"))
                {
                    if (input.Split(" ").Length == 4)
                    {
                        var paramaers = input.Split(" ");
                        if (wms.ItemExists(paramaers[2]) && wms.RetreiveItem(paramaers[2], paramaers[1], Convert.ToInt32(paramaers[3])))
                        {
                            Console.WriteLine($"Enjoy your {wms.VendingMachineItems[paramaers[2]].ProductName}\n{wms.VendingMachineItems[paramaers[2]].MessageWhenVended}");
                            Console.WriteLine(wms.MoneyService.GiveChange());
                        }
                        else if (!wms.ItemExists(paramaers[2]))
                        {
                            Console.WriteLine("**INVALID ITEM**");
                        }
                        else if (wms.ItemExists(paramaers[2]) && wms.VendingMachineItems[paramaers[2]].ItemsRemaining < Convert.ToInt32(paramaers[3]))
                        {
                            Console.WriteLine(wms.VendingMachineItems[paramaers[2]].MessageWhenSoldOut);
                        }
                        else if (wms.MoneyService.MoneyInMachine + Convert.ToDecimal(paramaers[1]) < wms.VendingMachineItems[paramaers[2]].Price * Convert.ToInt32(paramaers[3]))
                        {
                            Console.WriteLine(wms.NotEnoughMoneyError);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input");
                    }
                    goto tostart;
                }
                else if (input == "inv")
                {
                    Console.WriteLine("Items in Stock");
                    wms.DisplayAllItems();
                    goto tostart;

                }
                else
                {
                    Console.WriteLine("Please try again");
                }
                Console.Write("Press any key to start");
                Console.ReadLine();
            }
        }
        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger log = new Logger();
            log.Log(e.ExceptionObject.ToString(), 0, 0);
            Console.WriteLine("Unknown error occurred.");
            Environment.Exit(0);
        }
    }
}