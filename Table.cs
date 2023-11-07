using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static November_Exam.Table;

namespace November_Exam
{
    public class Table
    {
        //FIELDS

        // PROPERTIES
        public ETable TableNumber { get; set; }
        public bool IsTableTaken { get; set; }
        public DateTime ReservationTime { get; set; }
        public double TotalSum { get; set; }
        public List<Item> Order { get; set; } //track the current order

        public enum ETable
        {
            Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10
        }

        // CONSTRUCTOR
        public Table(ETable tableNumber, bool isTableTaken)
        {
            TableNumber = tableNumber;
            IsTableTaken = isTableTaken;
            TotalSum = 0; // Initialize to 0 euros
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






        //show the reservation one month in advance method

        #region obsoleteMethods
        /*
        public void SetTable()
        {
            Console.WriteLine("Enter the table number: ");
            if (int.TryParse(Console.ReadLine(), out int tableNumber))
            {
                Table selectedTable = Tables.FirstOrDefault(table => (int)table.TableNumber == tableNumber);

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
                            throw new InvalidOperationException("The selected table is already taken.");
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
                                throw new InvalidOperationException("The selected table is already taken.");
                            }
                        }
                        else
                        {
                            throw new FormatException("Invalid reservation time format. Use 'HH:mm dd/MM/yyyy' (hour:minutes day/month/year) .");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid choice. Enter '1' or 'now' to set the table for immediate action or '2' or 'reserve' to reserve it for the future.");
                    }
                }
                else
                {
                    throw new ArgumentException("The selected table doesn't exist.");
                }
            }
            else
            {
                throw new FormatException("Invalid input. Enter a valid number.");
            }
        }


        public bool ChangeAvailability(string tableSelect)


        {
            Table selectedTable = Tables.FirstOrDefault(table => table.TableNumber.ToString() == tableSelect);

            if (selectedTable != null)
            {
                if (selectedTable.IsTableTaken == false)
                {
                    if (selectedTable.ReservationTime == DateTime.MinValue || DateTime.Now >= selectedTable.ReservationTime.AddMinutes(10))
                    {
                        return selectedTable.IsTableTaken = true;
                    }
                    else
                    {
                        throw new InvalidOperationException("The selected table is reserved, and it's less than 10 minutes from the reservation time.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("The selected table is already taken.");
                }
            }
            else
            {
                throw new ArgumentException("The selected table doesn't exist.");
            }
        }


        public void ReserveTable(string tableSelect)
        {
            Table selectedTable = Tables.FirstOrDefault(table => table.TableNumber.ToString() == tableSelect);

            if (selectedTable != null)
            {
                if (!selectedTable.IsTableTaken)
                {
                    Console.WriteLine("Enter reservation time (HH:mm dd/MM/yyyy): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime reservationTime))
                    {
                        selectedTable.ReservationTime = reservationTime;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid reservation time format. Use 'HH:mm dd/MM/yyyy'.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("The selected table is already taken.");
                }
            }
            else
            {
                throw new ArgumentException("The selected table doesn't exist.");
            }
        }


        */
        #endregion




        // ================== END OF METHODS ==================
    }
}


