using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace November_Exam
{
    public class Item
    {
        //FIELDS

        //PROPERTIES

        public string Name { get; set; }
        public double Price { get; set; }

        //CONSTRUCTORS
        public Item(string itemName, double itemPrice)
        {
            Name = itemName;
            Price = itemPrice;
        }

        // ======================  METHODS ====================  

        //List<Item> menuList = ReadMenu().ToList();
        //itemSelected = menuList.SingleOrDefault(item => menuList.Contains == itemInputName);

        public static Item GetItem(string itemInputName)
        {
            List<Item> menuList = ReadMenu();
            Item itemSelected = menuList.SingleOrDefault(item => item.Name == itemInputName);

            if (itemSelected != null)
            {
                return new Item(itemSelected.Name, itemSelected.Price);
            }
            else
            {               
                return new Item("Item not found", 0.0);
            }
        }

        public static List<Item> ReadMenu()
        {
            string menu = $"Menu.txt";
            List<Item> menuFromFile = new List<Item>();

            try
            {
                if (File.Exists(menu))
                {
                    string[] menuLines = File.ReadAllLines(menu);

                    foreach (string line in menuLines)
                    {
                        // format = "1. NAME: product |*| PRICE: €0.00"
                        string[] parts = line.Split(new[] { "NAME: ", " |*| PRICE: €" }, StringSplitOptions.None);
                        if (parts.Length == 2)
                        {
                            string itemName = parts[0];
                            if (double.TryParse(parts[1], out double itemPrice))
                            {
                                Item item = new Item(itemName, itemPrice);
                                menuFromFile.Add(item);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Menu file does not exist.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading menu: " + e.Message);
            }

            return menuFromFile;
        }
    }


    // ================== END OF METHODS ==================

}

