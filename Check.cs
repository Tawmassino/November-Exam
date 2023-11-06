using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace November_Exam
{
    internal interface ICheck
    {
        void GenerateCheck(List<Item> order, double totalSumToCheck, Table selectedTable);
    }
}
