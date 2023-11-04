using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace November_Exam
{
    public class Restaurant 
    {
        //FIELDS

        //PROPERTIES
        
        public int[] Tables { get; set; }
        public bool IsTableTaken { get; set; }
        public double TotalSum { get; set; }


        //CONSTRUCTORS
        public Restaurant(int[] tables, bool isTableTaken, double totalSum)
        {
            Tables = tables;          

            IsTableTaken = isTableTaken;
            TotalSum = totalSum;
        }


        // ======================  METHODS ====================  


        public  void Order(int tableInput)
        {
            //select table
            
            //for (int i = 0; i < Tables.Length; i++)
            //{            //    if (tableInput != Tables[i])
            //    {            //        tableInput = Tables[i];
            //        break;            //    }             //}

            if (tableInput > 0 && tableInput <= Tables.Length && IsTableTaken == false)
            {
                
                IsTableTaken = true;
                Console.WriteLine($"Table {tableInput} has been booked.");
            }
            else
            {
                Console.WriteLine($"Table {tableInput} is not available or invalid.");
            }


            //make order
            List<Item> order = new List<Item>();
            TotalSum = 0;

            while (true)
            {
                string waiterInput = Console.ReadLine();
                switch (waiterInput)
                {
                    case "+":
                        Console.WriteLine("Enter the name of the item to add:");
                        string itemInputName = Console.ReadLine();
                        Item itemToAdd = Item.GetItem(itemInputName);

                        if (itemToAdd != null)
                        {
                            order.Add(itemToAdd);
                            TotalSum += itemToAdd.Price;
                            Console.WriteLine($"Added {itemToAdd.Name} to the order. Total: {TotalSum}");
                        }
                        break;
                    case "=":
                        IsTableTaken = false;
                        double totalSumToCheck = TotalSum;
                        GenerateCheck(order, totalSumToCheck);
                        return;
                    default:
                        itemToAdd = Item.GetItem(waiterInput);

                        if (itemToAdd != null)
                        {
                            order.Add(itemToAdd);
                            TotalSum += itemToAdd.Price;
                            Console.WriteLine($"Added {itemToAdd.Name} to the order. Total: {TotalSum}");
                        }
                        break;
                }
            }

            //var x = product.Name;
            //var y = product.Price;

            //add the prices

        }

        public void UpdateOrder()
        {

        }



        public void GenerateCheck(List<Item> order,double totalSumToCheck)
        {
            DateTime now = DateTime.Now;
            string currentTime = now.ToString("HH:mm, d, MMMM yyyy");
            Console.WriteLine(currentTime);                     
            order.ToList().ForEach(item => Console.WriteLine($"{item.Name} - {item.Price}"));
            Console.WriteLine($"TOTAL: €{totalSumToCheck}");
        }

        public void CheckTableAvailability()
        {
           


        }
        // ================== END OF METHODS ==================

    }
}
