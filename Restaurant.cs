using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static November_Exam.Table;

namespace November_Exam
{
    public class Restaurant : ICheck
    {
        //FIELDS
        public string RestaurantName { get; set; }
        // PROPERTIES
        public List<Table> Tables { get; set; }
        public double TotalSum { get; set; }

        // CONSTRUCTORS
        public Restaurant(List<Table> tables, double totalSum, string restaurantName)
        {
            Tables = tables;
            TotalSum = totalSum;
            RestaurantName = restaurantName;
        }


        // ======================  METHODS ====================  
        public static List<Table> InitializeTables()
        {
            List<Table> tables = new List<Table>();

            foreach (ETable tableEnum in Enum.GetValues(typeof(ETable)))
            {
                tables.Add(new Table(tableEnum, false));
            }

            return tables;
        }

        public void SetTable()
        {
            Console.WriteLine("Enter the table number: ");
            if (int.TryParse(Console.ReadLine(), out int tableNumber))
            {
                Table selectedTable = Tables.FirstOrDefault(table => (int)table.TableNumber == tableNumber - 1);


                if (selectedTable != null)
                {
                    Console.WriteLine("1. NOW or 2.RESERVE?");
                    string choice = Console.ReadLine().ToLower();

                    if (choice == "1" || choice == "now")
                    {
                        if (selectedTable.IsTableTaken == false)
                        {
                            selectedTable.IsTableTaken = true;
                            selectedTable.Order = new List<Item>();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Table is now taken.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("The selected table is already taken.");
                            Console.ResetColor();
                        }
                    }
                    else if (choice == "2" || choice == "reserve")
                    {
                        Console.WriteLine("Enter reservation time (e.g., 'HH:mm dd/MM/yyyy'): ");
                        if (DateTime.TryParseExact(Console.ReadLine(), "HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime reservationTime))
                        {
                            if (!selectedTable.IsTableTaken)
                            {
                                selectedTable.ReservationTime = reservationTime;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Table has been reserved.");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("The selected table is already taken.");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid reservation time format. Use 'HH:mm dd/MM/yyyy' (hour:minutes day/month/year).");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Enter '1' or 'now' to set the table for immediate action or '2' or 'reserve' to reserve it for the future.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The selected table doesn't exist.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Enter a valid number.");
                Console.ResetColor();
            }
        }

        public void UnsetTable()
        {
            Console.WriteLine("Enter the table number to mark as not taken: ");
            if (int.TryParse(Console.ReadLine(), out int tableNumber))
            {
                Table selectedTable = Tables.FirstOrDefault(table => (int)table.TableNumber == tableNumber - 1);

                if (selectedTable != null)
                {
                    if (selectedTable.IsTableTaken)
                    {
                        selectedTable.IsTableTaken = false;
                        selectedTable.Order = null; // clear the order when marking the table as not taken
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Table is now marked as not taken.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("The selected table is already not taken.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The selected table doesn't exist.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Enter a valid number.");
                Console.ResetColor();
            }
        }



        public void Order(MenuManager menuManager)
        {
            // Select table
            Console.WriteLine("Enter the table number: ");
            if (int.TryParse(Console.ReadLine(), out int tableNumber))
            {

                Table selectedTable = Tables.FirstOrDefault(table => (int)table.TableNumber == tableNumber - 1);


                if (selectedTable != null)//&& selectedTable.IsTableTaken
                {
                    // Make order
                    List<Item> order = selectedTable.Order;
                    selectedTable.TotalSum = 0;

                    int itemNumber;


                    while (true)
                    {
                        Console.WriteLine("Enter an item to add, '+' to add an item, or '=' to end the order:");
                        string waiterInput = Console.ReadLine();



                        switch (waiterInput)
                        {
                            case "+":
                                // Add the prices
                                Console.WriteLine("Enter the NAME or the NUMBER of the item to add:");
                                Console.WriteLine("Type / to update the order");
                                string itemInputName = Console.ReadLine();

                                Console.WriteLine($"Enter the quantity of item {itemInputName}:");
                                if (int.TryParse(Console.ReadLine(), out int quantity))
                                {
                                    // Parsing was successful, quantity is set
                                }
                                else
                                {
                                    Console.WriteLine("Invalid quantity. Defaulting to 1.");
                                    quantity = 1;
                                }


                                //Item itemToAdd = Item.GetItem(itemInputName, menuManager);
                                Item itemToAdd = null;

                                if (int.TryParse(itemInputName, out itemNumber))
                                {
                                    itemToAdd = Item.GetItemByPlace(itemNumber, menuManager);
                                }
                                else
                                {
                                    itemToAdd = Item.GetItem(itemInputName, menuManager);
                                }

                                itemToAdd.Quantity = quantity;//set the input quantity to the object

                                if (itemToAdd != null)
                                {
                                    order.Add(itemToAdd);
                                    selectedTable.TotalSum += itemToAdd.Price * quantity; // Update the total sum
                                    Console.WriteLine($"Added {quantity} {itemToAdd.Name} to the order. Total: {selectedTable.TotalSum}");
                                    Console.WriteLine("----------------------------------------");
                                }
                                break;

                            case "=":
                                // End order
                                selectedTable.IsTableTaken = false; //chekout
                                double totalSumToCheck = selectedTable.TotalSum;
                                GenerateCheck(order, totalSumToCheck, selectedTable);
                                selectedTable.TotalSum = 0;
                                return;

                            case "/":
                                UpdateOrder(menuManager, order);
                                break;

                            default:
                                // Order single item and add more items
                                //itemToAdd = Item.GetItem(waiterInput, menuManager);
                                if (selectedTable.IsTableTaken)
                                {
                                    // Handle single item order when the table is taken
                                    itemToAdd = null;
                                    if (int.TryParse(waiterInput, out itemNumber))
                                    {
                                        itemToAdd = Item.GetItemByPlace(itemNumber, menuManager);
                                    }
                                    else
                                    {
                                        itemToAdd = Item.GetItem(waiterInput, menuManager);
                                    }
                                    itemToAdd.Quantity = 1;

                                    if (itemToAdd != null)
                                    {
                                        order.Add(itemToAdd);
                                        selectedTable.TotalSum += itemToAdd.Price;
                                        Console.WriteLine($"Added {itemToAdd.Quantity} {itemToAdd.Name} to the order. Total: {selectedTable.TotalSum}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Table is not taken. You can only add items if the table is taken.");
                                }
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Table {tableNumber} is not available or invalid.");
                }
            }
        }

        public void UpdateOrder(MenuManager menuManager, List<Item> order)
        {
            // Select table
            Console.WriteLine("Enter the table number: ");
            if (int.TryParse(Console.ReadLine(), out int tableNumber))
            {
                Table selectedTable = Tables.FirstOrDefault(table => (int)table.TableNumber == tableNumber - 1);

                if (selectedTable != null)//&& selectedTable.IsTableTaken
                {
                    // Update the order
                    order = selectedTable.Order;
                    int itemNumber;

                    while (true)
                    {
                        order.ToList().ForEach(item => Console.WriteLine($"{item.Name} x {item.Quantity} -> {item.Price}")); //ITEM LIST
                        Console.WriteLine("========================================");
                        Console.WriteLine("Enter an item to add, '+' to add an item, '-' to remove an item, or '=' to end the order:");
                        string waiterInput = Console.ReadLine();

                        switch (waiterInput)
                        {
                            case "+":
                                // Add the prices
                                Console.WriteLine("Enter the NAME or the NUMBER of the item to add:");
                                string itemInputName = Console.ReadLine();
                                Console.WriteLine($"Enter the name quantity of {itemInputName}:");
                                if (int.TryParse(Console.ReadLine(), out int quantity))
                                {
                                    // Parsing was successful, quantity is set
                                }
                                else
                                {
                                    Console.WriteLine("Invalid quantity. Defaulting to 1.");
                                    quantity = 1;
                                }

                                Item itemToAdd = null;

                                if (int.TryParse(itemInputName, out itemNumber))
                                {
                                    itemToAdd = Item.GetItemByPlace(itemNumber, menuManager);
                                }
                                else
                                {
                                    itemToAdd = Item.GetItem(itemInputName, menuManager);
                                }

                                itemToAdd.Quantity = quantity;

                                if (itemToAdd != null)
                                {
                                    order.Add(itemToAdd);
                                    selectedTable.TotalSum += itemToAdd.Price * quantity;
                                    Console.WriteLine($"Added {quantity} {itemToAdd.Name} to the order. Total: {selectedTable.TotalSum}");
                                    Console.WriteLine("----------------------------------------");
                                }
                                break;

                            case "-":
                                // Remove an item from the order
                                Console.WriteLine("Enter the index of the item to remove:");
                                if (int.TryParse(Console.ReadLine(), out int removeIndex) && removeIndex >= 0 && removeIndex < order.Count)
                                {
                                    Item removedItem = order[removeIndex];
                                    order.RemoveAt(removeIndex);
                                    selectedTable.TotalSum -= removedItem.Price * removedItem.Quantity;
                                    Console.WriteLine($"Removed {removedItem.Quantity} {removedItem.Name} from the order. Total: {selectedTable.TotalSum}");
                                    Console.WriteLine("----------------------------------------");
                                    // Display the updated order
                                    order.ToList().ForEach(item => Console.WriteLine($"{item.Name} x {item.Quantity} -> {item.Price}"));
                                    Console.WriteLine("----------------------------------------");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid item index.");
                                }
                                break;

                            case "=":
                                // End order
                                selectedTable.IsTableTaken = false; //checkout
                                double totalSumToCheck = selectedTable.TotalSum;
                                GenerateCheck(order, totalSumToCheck, selectedTable);
                                selectedTable.TotalSum = 0;
                                return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Table {tableNumber} is not available or invalid.");
                }
            }
        }


        public void GenerateCheck(List<Item> order, double totalSumToCheck, Table selectedTable)
        {
            DateTime now = DateTime.Now;
            string currentTime = now.ToString("HH:mm, d, MMMM yyyy");
            Console.WriteLine($"TABLE: {selectedTable.TableNumber}");// TABLE #
            Console.WriteLine(currentTime);                     // CHECKOUT TIME
            Console.WriteLine("----------------------------------------");
            order.ToList().ForEach(item => Console.WriteLine($"{item.Name} x {item.Quantity} -> {item.Price}")); //ITEM LIST
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"TOTAL: €{totalSumToCheck}");
        }

        public void CheckTableAvailability()
        {
            Console.WriteLine("============= TABLE STATUS =============");
            foreach (var table in Tables)
            {
                string tableStatus = table.IsTableTaken ? "Taken" : "Available";

                Console.Write($"{table.TableNumber} : ");
                ColorizeAndPrintTableStatus(tableStatus);
                Console.WriteLine(tableStatus);
                Console.ResetColor();
            }
            Console.WriteLine("========================================");
        }

        private void ColorizeAndPrintTableStatus(string status)
        {
            if (status == "Taken")
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (status == "Available")
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        //public void CheckTableAvailability()
        //{
        //    Console.WriteLine("============= TABLE STATUS =============");
        //    foreach (var table in Tables)
        //    {
        //        string tableStatus = table.IsTableTaken ? "Taken" : "Available";
        //        Console.WriteLine($"{table.TableNumber}: {tableStatus}");
        //    }
        //    Console.WriteLine("========================================");
        //}

        //public void UpdateOrder()
        //{

        //}


        // ================== END OF METHODS ==================

    }
}
