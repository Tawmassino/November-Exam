using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace November_Exam
{
    public class Item
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public Item(string itemName, double itemPrice, int quantity)
        {
            Name = itemName;
            Price = itemPrice;
            Quantity = quantity;

        }

        public static Item GetItem(string itemInputName, MenuManager menuManager)
        {
            Item itemSelected = menuManager.GetItemByName(itemInputName);

            if (itemSelected != null)
            {
                return new Item(itemSelected.Name, itemSelected.Price, itemSelected.Quantity);
            }
            else
            {
                return new Item("Item not found", 0.0, 0);
            }
        }

        public static Item GetItemByPlace(int itemPlace, MenuManager menuManager)
        {
            List<Item> menu = menuManager.ReadMenuFromFile();
            if (itemPlace >= 1 && itemPlace <= menu.Count)
            {
                return menu[itemPlace - 1];
            }
            else
            {
                return new Item("Item not found", 0.0, 0);
            }
        }


    }
    // ================== END OF METHODS ==================
}






