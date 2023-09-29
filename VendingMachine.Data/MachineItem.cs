using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Data
{
    public abstract class MachineItem
    {

        /// <summary>
        /// The name of the VendingItem
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// The price of the VendingItem
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// How many of each VendingItem remains
        /// </summary>
        public int ItemsRemaining { get; set; }

        /// <summary>
        /// What the menu displays when the VendingItem is vended
        /// </summary>
        public string MessageWhenVended { get; set; }

        public string MessageWhenSoldOut { get; set; }

        public MachineItem()
        {

        }

        public MachineItem(string productName, decimal price, int itemsRemaining, string messageWhenVended)
        {
            this.ProductName = productName;
            this.Price = price;
            this.ItemsRemaining = itemsRemaining;
            this.MessageWhenVended = messageWhenVended;
            this.MessageWhenSoldOut = $"Sold out of {this.ProductName}!\nBuy something else!";

        }



        /// <summary>
        /// Returns false if it can't get the item
        /// </summary>
        /// <returns>bool</returns>
        public bool RemoveItem(int noItemtoRemove)
        {
            if (this.ItemsRemaining > 0 && this.ItemsRemaining - noItemtoRemove >= 0)
            {
                this.ItemsRemaining -= noItemtoRemove;
                return true;
            }

            return false;
        }
    }
    public class Candy : MachineItem
    {
        public const string Message = "Munch, Munch, Yum!";

        public Candy(
            string productName,
            decimal price,
            int itemsRemaining)
                : base(
                productName,
                price,
                itemsRemaining,
                Message)
        {
        }
    }
    public class Chip : MachineItem
    {
        public const string Message = "Crunch, Crunch, Yum!";

        public Chip(
            string productName,
            decimal price,
            int itemsRemaining)
                : base(
                productName,
                price,
                itemsRemaining,
                Message)
        {
        }
    }
    public class Coke : MachineItem
    {
        public const string Message = "Glug, Glug, Yum!";

        public Coke(
            string productName,
            decimal price,
            int itemsRemaining)
                : base(
                productName,
                price,
                itemsRemaining,
                Message)
        {
        }
    }
    public class Drink : MachineItem
    {
        public const string Message = "Glug, Glug, Yum!";

        public Drink(
            string productName,
            decimal price,
            int itemsRemaining)
                : base(
                productName,
                price,
                itemsRemaining,
                Message)
        {
        }
    }
    public class Gum : MachineItem
    {
        public const string Message = "Chew, Chew, Yum!";

        public Gum(
            string productName,
            decimal price,
            int itemsRemaining)
                : base(
                productName,
                price,
                itemsRemaining,
                Message)
        {
        }
    }
    public class MM : MachineItem
    {
        public const string Message = "Glug, Glug, Yum!";

        public MM(
            string productName,
            decimal price,
            int itemsRemaining)
                : base(
                productName,
                price,
                itemsRemaining,
                Message)
        {
        }
    }
    public class Snickers : MachineItem
    {
        public const string Message = "Glug, Glug, Yum!";

        public Snickers(
            string productName,
            decimal price,
            int itemsRemaining)
                : base(
                productName,
                price,
                itemsRemaining,
                Message)
        {
        }
    }
    public class Water : MachineItem
    {
        public const string Message = "Glug, Glug, Yum!";

        public Water(
            string productName,
            decimal price,
            int itemsRemaining)
                : base(
                productName,
                price,
                itemsRemaining,
                Message)
        {
        }
    }
}