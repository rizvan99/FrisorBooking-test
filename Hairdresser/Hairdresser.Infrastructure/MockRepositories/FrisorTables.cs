using Hairdresser.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hairdresser.Infrastructure.MockRepositories
{
    public class FrisorTables
    {
        public List<Employee> Employees { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Treatment> Treatments { get; set; }
        public static FrisorTables MockInstance = null;
        private FrisorTables()
        {
            Employees = new List<Employee>();
            Appointments = new List<Appointment>();
            Customers = new List<Customer>();
            Treatments = new List<Treatment>();
        }

        public Employee CreateEmployee(Employee e)
        {
            Employees.Add(e);
            return e;
        }
        public List<Employee> ReadEmployees()
        {
            return Employees;
        }
        public Employee UpdateEmployee(Employee e)
        {
            foreach (Employee ex in Employees)
            {
                if (ex.UserID == e.UserID)
                {
                    e = ex;
                    return ex;
                }
            }
            return null;
        }
        public Employee DeleteEmployee(Employee e)
        {
            foreach (Employee ex in Employees)
            {
                if (ex.UserID == e.UserID)
                {
                    Employees.Remove(ex);
                    return ex;
                }
            }
            return null;
        }

        public Customer CreateCustomer(Customer e)
        {
            Customers.Add(e);
            return e;
        }
        public List<Customer> ReadCustomers()
        {
            return Customers;
        }
        public Customer UpdateCustomer(Customer e)
        {
            foreach (Customer ex in Customers)
            {
                if (ex.UserID == e.UserID)
                {
                    e = ex;
                    return ex;
                }
            }
            return null;
        }
        public Customer DeleteCustomer(Customer e)
        {
            foreach (Customer ex in Customers)
            {
                if (ex.UserID == e.UserID)
                {
                    Customers.Remove(ex);
                    return ex;
                }
            }
            return null;
        }

        public Appointment CreateAppontment(Appointment e)
        {
            Appointments.Add(e);
            return e;
        }
        public List<Appointment> ReadAppointments()
        {
            return Appointments;
        }
        public Appointment UpdateAppointment(Appointment e)
        {
            foreach (Appointment ex in Appointments)
            {
                if (ex.AppointmentDateTime == e.AppointmentDateTime)
                {
                    e = ex;
                    return ex;
                }
            }
            return null;
        }
        public Appointment DeleteAppointment(Appointment e)
        {
            foreach (Appointment ex in Appointments)
            {
                if (ex.AppointmentDateTime == e.AppointmentDateTime)
                {
                    Appointments.Remove(ex);
                    return ex;
                }
            }
            return null;
        }

        public Treatment CreateService(Treatment e)
        {
            Treatments.Add(e);
            return e;
        }
        public List<Treatment> ReadServices()
        {
            return Treatments;
        }
        public Treatment UpdateService(Treatment e)
        {
            foreach (Treatment ex in Treatments)
            {
                if (ex.TreatmentID == e.TreatmentID)
                {
                    e = ex;
                    return ex;
                }
            }
            return null;
        }
        public Treatment DeleteService(Treatment e)
        {
            foreach (Treatment ex in Treatments)
            {
                if (ex.TreatmentID == e.TreatmentID)
                {
                    Treatments.Remove(ex);
                    return ex;
                }
            }
            return null;
        }
        public Employee GetEmployee(int Id)
        {
            foreach (Employee ex in Employees)
            {
                if (ex.UserID == Id)
                {
                    return ex;
                }
            }
            return null;
        }
        public Customer GetCustomer(int Id)
        {
            foreach (Customer ex in Customers)
            {
                if (ex.UserID == Id)
                {
                    return ex;
                }
            }
            return null;
        }

        public Treatment GetService(int Id)
        {
            foreach (Treatment ex in Treatments)
            {
                if (ex.TreatmentID == Id)
                {
                    return ex;
                }
            }
            return null;
        }

        public static FrisorTables GetInstance()
        {
            if (MockInstance == null)
            {
                return new FrisorTables();
            }
            return MockInstance;
        }
    }
}
