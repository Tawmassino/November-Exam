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
                        DateTime currentTime = DateTime.Now;

                        if (!selectedTable.IsTableTaken)
                        {
                            bool hasNearbyReservation = selectedTable.ReservationTime != null && (selectedTable.ReservationTime - currentTime).TotalMinutes <= 10;

                            if (!hasNearbyReservation)
                            {
                                Console.WriteLine("Enter reservation time (e.g., 'HH:mm dd/MM/yyyy'): ");
                                if (DateTime.TryParseExact(Console.ReadLine(), "HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime reservationTime))
                                {
                                    if (currentTime < reservationTime)
                                    {
                                        selectedTable.ReservationTime = reservationTime;
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Table has been reserved.");
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("You cannot reserve in the past or overlap with an earlier reservation.");
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
                                Console.WriteLine("The selected table cannot be reserved as there is a reservation within the next 10 minutes.");
                                Console.ResetColor();
                            }
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


        // ------------ order methods ------------
        public void Order(MenuManager menuManager)
        {
            int tableNumber = SelectTable();
            if (tableNumber <= 0)// Invalid table number
            {
                Console.WriteLine("Invalid table number. Please select a valid table.");
                return;
            }

            Table selectedTable = Tables.FirstOrDefault(table => (int)table.TableNumber == tableNumber - 1);
            if (selectedTable == null)// Invalid table
            {
                Console.WriteLine("Invalid table. Please select a valid table.");
                return;
            }

            if (selectedTable.IsTableTaken == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The selected table is not set as TAKEN. Please set the table before ordering.");
                Console.ResetColor();
                return;
            }


            List<Item> order = selectedTable.Order;
            selectedTable.TotalSum = 0;

            while (true)
            {
                ShowOperationOptions();

                string waiterInput = Console.ReadLine();

                switch (waiterInput)
                {
                    case "q":
                        return;

                    case "+":
                        AddItemToOrder(menuManager, order, selectedTable);
                        break;

                    case "=":
                        EndOrder(order, selectedTable);
                        return;

                    case "-":
                        RemoveItemFromOrder(order, selectedTable);
                        break;

                    case "...":
                        Console.WriteLine($"==== {selectedTable.TableNumber} ===== ");
                        Console.WriteLine("----------------------------------------");
                        order.ToList().ForEach(item => Console.WriteLine($"{item.Name} x {item.Quantity} -> {item.Price}"));
                        break;

                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            }
        }

        private int SelectTable()
        {
            Console.WriteLine("Enter the table number: ");
            if (int.TryParse(Console.ReadLine(), out int tableNumber))
            {
                return tableNumber;
            }
            else
            {
                Console.WriteLine("Invalid table number.");
                return -1;
            }
        }

        private void AddItemToOrder(MenuManager menuManager, List<Item> order, Table selectedTable)
        {
            Console.WriteLine("Enter the NAME or the NUMBER of the item to add:");
            string itemInputName = Console.ReadLine();
            if (itemInputName != null)
            {
                Item itemToAdd = null;
                int itemNumber;

                if (int.TryParse(itemInputName, out itemNumber))
                {
                    itemToAdd = Item.GetItemByPlace(itemNumber, menuManager);
                }
                else
                {
                    itemToAdd = Item.GetItem(itemInputName, menuManager);
                }

                if (itemToAdd != null)
                {
                    Console.WriteLine($"Enter the quantity of {itemToAdd.Name}:");
                    if (int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        // Parsing was successful, and quantity is set
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity. Defaulting to 1.");
                        quantity = 1;
                    }

                    itemToAdd.Quantity = quantity;
                    Console.Clear();
                    Console.WriteLine("----------------------------------------");
                    order.Add(itemToAdd);
                    selectedTable.TotalSum += itemToAdd.Price * quantity;
                    Console.WriteLine($"Added {quantity} {itemToAdd.Name} to the order. Total: €{selectedTable.TotalSum.ToString("F2")}");
                    Console.WriteLine("----------------------------------------");
                    order.ForEach(item => Console.WriteLine($"{item.Name} x {item.Quantity} - {item.Price}"));
                    Console.WriteLine("----------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("invalid input");
            }
        }

        private void EndOrder(List<Item> order, Table selectedTable)
        {
            selectedTable.IsTableTaken = false; // Checkout
            double totalSumToCheck = selectedTable.TotalSum;
            GenerateCheck(order, totalSumToCheck, selectedTable);
            selectedTable.TotalSum = 0;
        }

        private void RemoveItemFromOrder(List<Item> order, Table selectedTable)
        {
            if (order.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("----------------------------------------");
                order.ToList().ForEach(item => Console.WriteLine($"{order.IndexOf(item) + 1}.{item.Name} x {item.Quantity} -> €{item.Price}"));
                Console.WriteLine("Enter the index of the item to remove:");

                if (int.TryParse(Console.ReadLine(), out int removeIndex) && removeIndex > 0 && removeIndex <= order.Count)
                {
                    int humanIndex = removeIndex - 1; // start from 1, not 0
                    if (humanIndex >= 0 && humanIndex < order.Count)
                    {
                        Item removedItem = order[humanIndex];
                        order.RemoveAt(humanIndex);
                        selectedTable.TotalSum -= removedItem.Price * removedItem.Quantity;
                        Console.WriteLine($"Removed {removedItem.Quantity} {removedItem.Name} from the order. Total: {selectedTable.TotalSum}");
                        Console.WriteLine("----------------------------------------");
                        order.ForEach(item => Console.WriteLine($"{item.Name} x {item.Quantity} -> €{item.Price}"));
                        Console.WriteLine("----------------------------------------");
                    }
                    else
                    {
                        Console.WriteLine("Error: Index out of range.");
                    }
                }

                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid item index.");
                }
            }
            else
            {
                Console.WriteLine("The order is empty");
            }
        }
        // ------------ end of order ------------

        public void ShowOperationOptions()
        {
            Console.WriteLine("========================================================");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("+");
            Console.ResetColor();

            Console.Write(" ADD ITEM || ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("-");
            Console.ResetColor();

            Console.Write(" REMOVE ITEM || ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("=");
            Console.ResetColor();
            Console.Write(" CHECKOUT || ");


            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" q ");
            Console.ResetColor();
            Console.Write(" QUIT || ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("...");
            Console.ResetColor();
            Console.WriteLine(" VIEW ORDER:");
            Console.WriteLine("--------------------------------------------------------");
        }

        public void GenerateCheck(List<Item> order, double totalSumToCheck, Table selectedTable)
        {
            order.ToList().ForEach(item => Console.WriteLine($"{item.Name} x {item.Quantity} -> €{item.Price}")); //ITEM LIST

            Console.WriteLine("-------------- Checkout? ---------------");
            AreYouSure();// y/n?

            string checkoutChoice = Console.ReadLine().ToLower();

            if (checkoutChoice != null)
            {
                if (checkoutChoice == "y" || checkoutChoice == "yes")
                {
                    Console.Clear();
                    Console.WriteLine("----------------------------------------");
                    DateTime now = DateTime.Now;
                    string currentTime = now.ToString("HH:mm, d, MMMM yyyy");
                    Console.WriteLine($"TABLE: {selectedTable.TableNumber}");// TABLE #
                    Console.WriteLine(currentTime);                     // CHECKOUT TIME
                    Console.WriteLine("----------------------------------------");
                    order.ToList().ForEach(item => Console.WriteLine($"{order.IndexOf(item) + 1}.{item.Name} x {item.Quantity} -> €{item.Price.ToString("F2")}")); //ITEM LIST
                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine($"TOTAL: €{totalSumToCheck.ToString("F2")}");
                }
                else if (checkoutChoice == "n" | checkoutChoice == "no")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("invalid input");
                    return;
                }
            }

        }
        public void AreYouSure()
        {
            Console.Write("-----------------"); Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Y"); Console.ResetColor();
            Console.Write("/"); Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("N"); Console.ResetColor();
            Console.Write("-----------------");
            Console.WriteLine();
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
