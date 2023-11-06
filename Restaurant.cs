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
                            Console.WriteLine("Table is now taken.");
                        }
                        else
                        {
                            Console.WriteLine("The selected table is already taken.");
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
                                Console.WriteLine("Table has been reserved.");
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


        //insert HOW many of the items. because can order several same items

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
                    List<Item> order = new List<Item>();
                    selectedTable.TotalSum = 0;

                    while (true)
                    {
                        Console.WriteLine("Enter an item to add, '+' to add an item, or '=' to end the order:");
                        string waiterInput = Console.ReadLine();

                        int quantity = 1; // Default quantity is 1

                        switch (waiterInput)
                        {
                            case "+":
                                // Add the prices
                                Console.WriteLine("Enter the name of the item to add:");
                                string itemInputName = Console.ReadLine();

                                Console.WriteLine($"Enter the name quantity of {itemInputName}:");

                                quantity = int.Parse(itemInputName);

                                Item itemToAdd = Item.GetItem(itemInputName, menuManager);

                                if (itemToAdd != null)
                                {
                                    order.Add(itemToAdd);
                                    selectedTable.TotalSum += itemToAdd.Price;
                                    Console.WriteLine($"Added {itemToAdd.Name} to the order. Total: {selectedTable.TotalSum}");
                                }
                                break;

                            case "=":
                                // End order
                                selectedTable.IsTableTaken = false; //chekout
                                double totalSumToCheck = selectedTable.TotalSum;
                                GenerateCheck(order, totalSumToCheck, selectedTable);
                                selectedTable.TotalSum = 0;
                                return;

                            default:
                                // Order single item and add more items
                                itemToAdd = Item.GetItem(waiterInput, menuManager);

                                if (itemToAdd != null)
                                {
                                    order.Add(itemToAdd);
                                    selectedTable.TotalSum += itemToAdd.Price;
                                    Console.WriteLine($"Added {itemToAdd.Name} to the order. Total: {selectedTable.TotalSum}");
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

        public void GenerateCheck(List<Item> order, double totalSumToCheck, Table selectedTable)
        {
            DateTime now = DateTime.Now;
            string currentTime = now.ToString("HH:mm, d, MMMM yyyy");
            Console.WriteLine($"TABLE: {selectedTable.TableNumber}");// TABLE #
            Console.WriteLine(currentTime);                     // CHECKOUT TIME
            order.ToList().ForEach(item => Console.WriteLine($"{item.Name} - {item.Price}")); //ITEM LIST
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
