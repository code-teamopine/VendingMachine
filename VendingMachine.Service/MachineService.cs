using System.Threading.Tasks;
using VendingMachine.Data;

namespace VendingMachine.Service
{
    public class MachineService
    {
        private Logger log;
        public MachineItem MachineItem { get; private set; }

        public MachineService(Logger log,MachineItem machineItem)
        {
            this.log = log;
            this.MachineItem = machineItem;
        }


        /// <summary>
        /// Returns false if it can't get the item
        /// </summary>
        /// <returns>bool</returns>
        public bool RemoveItem()
        {
            if (this.MachineItem.ItemsRemaining > 0)
            {
                this.MachineItem.ItemsRemaining--;
                return true;
            }

            return false;
        }
    }
}