using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace November_Exam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; //to show euro sign

            //restaurant menu
            Restaurant myRestaurant = new Restaurant(Table.InitializeTables(), 0, "McCoding");
            MenuManager menuManager = new MenuManager();


            //Table selectedTable = null; // Initialize selectedTable as null
            // Item restaurantProducts = new Item();



            while (true)
            {
                ShowMenu(myRestaurant, menuManager);
            }



        }
        // ================== METHODS ==================
        public static void ShowMenu(Restaurant myRestaurant, MenuManager menuManager)
        {
            List<Item> order = null;
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine(@$"
 _       __     __                        
| |     / /__  / /________  ____ ___  ___ 
| | /| / / _ \/ / ___/ __ \/ __ `__ \/ _ \
| |/ |/ /  __/ / /__/ /_/ / / / / / /  __/
|__/|__/\___/_/\___/\____/_/ /_/ /_/\___/ 
                                          {myRestaurant.RestaurantName}");
            Console.WriteLine("========================================");
            myRestaurant.CheckTableAvailability();

            Console.WriteLine("========== SELECT OPERATION ============");
            Console.WriteLine("1. SELECT TABLE");
            Console.WriteLine("2. FREE TABLE");
            Console.WriteLine("3. ORDER");
            Console.WriteLine("4. UPDATE ORDER");
            Console.WriteLine("5. VIEW ITEMS");
            string waiterMenuChoice = Console.ReadLine().ToLower();

            if (waiterMenuChoice != null)
            {
                if (waiterMenuChoice == "1" || waiterMenuChoice == "table" || waiterMenuChoice == "select")
                {
                    Console.Clear();
                    myRestaurant.SetTable();
                    ReturnToMainMenu(myRestaurant, menuManager);
                }
                else if (waiterMenuChoice == "2" || waiterMenuChoice == "free" || waiterMenuChoice == "free table")
                {
                    Console.Clear();
                    myRestaurant.UnsetTable();
                    ReturnToMainMenu(myRestaurant, menuManager);
                }
                else if (waiterMenuChoice == "3" || waiterMenuChoice == "order")
                {
                    Console.Clear();
                    myRestaurant.Order(menuManager);
                    ReturnToMainMenu(myRestaurant, menuManager);
                }
                else if (waiterMenuChoice == "4" || waiterMenuChoice == "update order" || waiterMenuChoice == "update")
                {
                    if (order != null)
                    {
                        Console.Clear();

                        myRestaurant.UpdateOrder(menuManager, order);
                        ReturnToMainMenu(myRestaurant, menuManager);
                    }
                }
                else if (waiterMenuChoice == "5" || waiterMenuChoice == "view" || waiterMenuChoice == "items" || waiterMenuChoice == "view items")
                {
                    Console.Clear();
                    menuManager.PrintMenuItemsToConsole();
                    ReturnToMainMenu(myRestaurant, menuManager);
                }



            }
        }
        public static void ReturnToMainMenu(Restaurant myRestaurant, MenuManager menuManager)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" -> press any key to return to main menu <-");
            Console.ReadLine();
            Console.ResetColor();
            ShowMenu(myRestaurant, menuManager);
        }

        // ================== END OF METHODS ==================
    }
}