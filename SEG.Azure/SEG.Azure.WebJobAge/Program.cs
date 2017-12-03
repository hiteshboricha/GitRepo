using SEG.Azure.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Azure.WebJobAge
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateEmployeeDOB();
        }

        static int UpdateEmployeeDOB()
        {
            int noofemployeesupdated = 0;

            try
            {
                EmployeeBAL employeebal = new EmployeeBAL();
                noofemployeesupdated = employeebal.UpdateEmployeeDOB();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return noofemployeesupdated;
        }
    }
}
