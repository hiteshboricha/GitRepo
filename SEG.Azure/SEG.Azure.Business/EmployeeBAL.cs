﻿using SEG.Azure.Data;
using SEG.Azure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Azure.Business
{
    public class EmployeeBAL : BusinessAccessBase
    {
        public int InsertEmployee(Employee employee)
        {
            int employeeid = new int();
            try
            {
                IEmployeeData employeedal = new EmployeeData();
                employeeid = employeedal.InsertEmployee(employee);
            }
            catch (Exception ex)
            {
                LogAndThrowApplicationException("Error saving to datasoure", ex);
            }

            return employeeid;
        }

        public List<Employee> GetEmployees(int employeeid)
        {
            List<Employee> employeelist = new List<Employee>();
            try
            {
                IEmployeeData employeedal = new EmployeeData();
                employeelist = employeedal.GetEmployees(employeeid);
            }
            catch (Exception ex)
            {
                LogAndThrowApplicationException("Error saving to datasoure", ex);
            }

            return employeelist;
        }

        public int UpdateEmployeeDOB()
        {
            int noofemployeesupdated = new int();
            try
            {
                IEmployeeData employeedal = new EmployeeData();
                noofemployeesupdated = employeedal.UpdateEmployeeDOB();
            }
            catch (Exception ex)
            {
                LogAndThrowApplicationException("Error updating datasoure", ex);
            }

            return noofemployeesupdated;
        }

        public int DeleteEmployee(int employeeid)
        {
            int noofemployeesupdated = 0;
            try
            {
                IEmployeeData employeedal = new EmployeeData();
                noofemployeesupdated = employeedal.DeleteEmployee(employeeid);
            }
            catch (Exception ex)
            {
                LogAndThrowApplicationException("Error deleting from datasoure", ex);
            }

            return noofemployeesupdated;
        }
    }
}