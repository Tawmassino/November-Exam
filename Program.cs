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
            // Item restaurantProducts = new Item();



            while (true)
            {
                ShowMenu(myRestaurant, menuManager);
            }



        }
        // ================== METHODS ==================
        public static void ShowMenu(Restaurant myRestaurant, MenuManager menuManager)
        {
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
            Console.WriteLine("2. ORDER");
            Console.WriteLine("3. VIEW ITEMS");
            string waiterMenuChoice = Console.ReadLine().ToLower();

            if (waiterMenuChoice != null)
            {
                if (waiterMenuChoice == "1" || waiterMenuChoice == "table")
                {
                    Console.Clear();
                    myRestaurant.SetTable();
                    ReturnToMainMenu(myRestaurant, menuManager);
                }
                else if (waiterMenuChoice == "2" || waiterMenuChoice == "order")
                {
                    Console.Clear();
                    //myRestaurant.Order(menuManager);
                    ReturnToMainMenu(myRestaurant, menuManager);
                }
                else if (waiterMenuChoice == "3" || waiterMenuChoice == "view" || waiterMenuChoice == "items" || waiterMenuChoice == "view items")
                {
                    Console.Clear();
                    menuManager.PrintMenuItemsToConsole();
                    // menuManager.PrintMenuToConsole();
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