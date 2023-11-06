using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace November_Exam
{
    public class MenuManager
    {


        public List<Item> ReadMenuFromFile()
        {
            string filePath = "Menu.txt";
            List<Item> items = new List<Item>();

            try
            {
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);

                    foreach (string line in lines)
                    {
                        string trimmedLine = line.Trim(); // Remove leading and trailing spaces

                        if (!string.IsNullOrEmpty(trimmedLine) && trimmedLine.Contains("*") && trimmedLine.Contains("€"))
                        {
                            string[] parts = trimmedLine.Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries);

                            if (parts.Length == 2)
                            {
                                string itemName = parts[0].Trim();
                                if (double.TryParse(parts[1].Replace("€", "").Trim(), out double itemPrice))
                                {
                                    items.Add(new Item(itemName, itemPrice));
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The test file does not exist.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading test file: " + e.Message);
            }

            return items;
        }

        public void PrintMenuItemsToConsole()//  -------- TRY CATCH!! 
        {
            List<Item> items = ReadMenuFromFile();
            if (items.Count > 0)
            {
                Console.WriteLine("=============== ITEMS ==================");
                int itemNumber = 1;
                foreach (var item in items)
                {
                    Console.WriteLine($"{itemNumber}. {item.Name} - €{item.Price}");
                    itemNumber++;
                }
            }
            else
            {
                Console.WriteLine("No items found in the file.");
            }
        }




        public Item GetItemByName(string itemName)
        {//  -------- TRY CATCH!! 
            List<Item> menu = ReadMenuFromFile();
            return menu.SingleOrDefault(item => item.Name == itemName);
        }
        //-----------------------------------
        #region TestMethods
        public List<Item> ReadTestFile()
        {
            string filePath = "test.txt"; // Assuming the file is in the same directory as your application
            List<Item> items = new List<Item>();

            try
            {
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);

                    foreach (string line in lines)
                    {
                        string trimmedLine = line.Trim(); // Remove leading and trailing spaces

                        if (!string.IsNullOrEmpty(trimmedLine) && trimmedLine.Contains("*") && trimmedLine.Contains("€"))
                        {
                            string[] parts = trimmedLine.Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries);

                            if (parts.Length == 2)
                            {
                                string itemName = parts[0].Trim();
                                if (double.TryParse(parts[1].Replace("€", "").Trim(), out double itemPrice))
                                {
                                    items.Add(new Item(itemName, itemPrice));
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The test file does not exist.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading test file: " + e.Message);
            }

            return items;
        }
        public void PrintTestItemsToConsole()
        {
            List<Item> items = ReadTestFile();
            if (items.Count > 0)
            {
                Console.WriteLine("Items:");
                foreach (var item in items)
                {
                    Console.WriteLine($"{item.Name} - €{item.Price}");
                }
            }
            else
            {
                Console.WriteLine("No items found in the file.");
            }
        }
        #endregion
    }
}