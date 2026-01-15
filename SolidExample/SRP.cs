using System;
using System.Collections.Generic;
using System.Text;

namespace SolidExample
{
    public class SRP
    {
        // Voilation of SRP
        public class Employee()
        {
            public string? Name { get; set; }
            public string? Age { get; set; }
            public double Salary { get; set; }

            public void CalculateSalary(double hoursWorked, double hourlyRate)
            {
                // implmentation goes here
            }

            public void SaveEmployee()
            {
                // implementation goes here
            }
        }

        // correct implmentation of SRP
        public class EmployeeSRP()
        {
            public string? Name { get; set; }
            public string? Age { get; set; }
            public double Salary { get; set; }
        }

        public class SalaryCalculator()
        {
            public double CalculateSalary(double hoursWorked, double hourlyRate)
            {
                // implmentation goes here
                return hoursWorked * hourlyRate;
            }
        }

        public class EmployeeRepository()
        {
            public void SaveEmployee(EmployeeSRP employee)
            {
                // implementation goes here
            }
        }
    }
}
