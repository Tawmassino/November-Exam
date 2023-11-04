using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace November_Exam
{
    internal class Table
    {
        //FIELDS

        // PROPERTIES
        public TablesEnum TableNumber { get; set; }
        public bool IsTableTaken { get; set; }
        public List<Table> Tables { get; set; } = new List<Table>();
        public DateTime ReservationTime { get; set; }


        public enum TablesEnum
        {
            Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10
        }



        // CONSTRUCTOR
        public Table(TablesEnum tableNumber, bool isTableTaken)
            {
                TableNumber = tableNumber;
                IsTableTaken = isTableTaken;
            }
        // ======================  METHODS ====================  

        public static List<Table> InitializeTables()
        {
            List<Table> tables = new List<Table>();

            foreach (TablesEnum tableEnum in Enum.GetValues(typeof(TablesEnum)))
            {
                tables.Add(new Table(tableEnum, false));
            }

            return tables;
        }

        public static void InitialAvailability(List<Table> tables)
        {
            foreach (Table table in tables)
            {
                table.IsTableTaken = false;
            }
        }
    




    public void CheckTableAvailability()
        {
            //TablesEnum tables.ToList().ForEach(table => table.IsTableTaken == false);



        }
        // ================== END OF METHODS ==================
    }
}

