namespace November_Exam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //restaurant menu
            Restaurant myRestaurant = new Restaurant(Table.InitializeTables(), 0, "McCoding");

            Console.Clear();
            myRestaurant.CheckTableAvailability();
            //while (true){}
            Console.WriteLine("Select table");
            myRestaurant.SetTable();
            Console.WriteLine("View all items and their prices");

            Console.WriteLine();
        }
    }
}