using API.Model;
using API.Repository.Interface;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext context;

        public EmployeeRepository(MyContext context)
        {
            this.context = context;
        }

        public int Delete(string NIK)
        {
            var employee = context.Employees.Find(NIK);
            if (employee == null)
            {
                throw new ArgumentException("Data not found.");
            }

            context.Employees.Remove(employee);
            return context.SaveChanges();
        }

        public IEnumerable<GetDataVM> Get()
        {
            var result = (from e in context.Employees
                          join a in context.Accounts on e.NIK equals a.NIK
                          join p in context.Profilings on a.NIK equals p.NIK
                          join ed in context.Educations on p.Education_Id equals ed.Id
                          join u in context.Universities on ed.University_Id equals u.Id
                          select new GetDataVM
                          {
                              FullName = e.FirstName + " " + e.LastName,
                              Phone = e.Phone,
                              Birthday = e.Birthday,
                              Salary = e.Salary,
                              Email = e.Email,
                              Gender = e.Gender,
                              Degree = ed.Degree,
                              GPA = ed.GPA,
                              UniversityName = u.Name
                          }).ToList();

            return result;
        }

        public Employee Get(string NIK)
        {
            return context.Employees.FirstOrDefault(e => e.NIK == NIK);
        }

        public int Update(Employee employee)
        {
            var existingEmployee = context.Employees.FirstOrDefault(e => e.NIK == employee.NIK);
            if (existingEmployee == null)
            {
                throw new ArgumentException("Data not found.");
            }

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.Birthday = employee.Birthday;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.Email = employee.Email;
            existingEmployee.Gender = employee.Gender;

            context.Entry(existingEmployee).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public bool IsEmailUnique(string email)
        {
            return !context.Employees.Any(e => e.Email == email);
        }

        public bool IsPhoneUnique(string phone)
        {
            return !context.Employees.Any(e => e.Phone == phone);
        }

        public int Insert(RegisterVM register)
        {
            // Validasi alamat email unik
            if (!IsEmailUnique(register.Email))
            {
                throw new ArgumentException("Email already exists in the database.");
            }

            // Validasi nomor telepon unik
            if (!IsPhoneUnique(register.Phone))
            {
                throw new ArgumentException("Phone number already exists in the database.");
            }
            // Sisipkan data ke tabel Employees
            var employee = new Employee
            {
                NIK = GenNIK(),
                FirstName = register.FirstName,
                LastName = register.LastName,
                Phone = register.Phone,
                Birthday = register.Birthday,
                Salary = register.Salary,
                Email = register.Email,
                Gender = register.Gender,
            };

            context.Employees.Add(employee);
            context.SaveChanges();

            var account = new Account
            {
                NIK = employee.NIK,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(register.Password)
            };

            context.Accounts.Add(account);
            context.SaveChanges();

            var education = new Education
            {
                Degree = register.Degree,
                GPA = register.GPA,
                University_Id = register.University_Id
            };

            context.Educations.Add(education);
            context.SaveChanges();

            var profiling = new Profiling
            {
                NIK = employee.NIK,
                Education_Id = education.Id
            };

            context.Profilings.Add(profiling);
            context.SaveChanges();

            return 1;
        }


        public GetDataVM GetData(string NIK)
        {
            var result = (from e in context.Employees
                          join a in context.Accounts on e.NIK equals a.NIK
                          join p in context.Profilings on a.NIK equals p.NIK
                          join ed in context.Educations on p.Education_Id equals ed.Id
                          join u in context.Universities on ed.University_Id equals u.Id
                          where e.NIK == NIK
                          select new GetDataVM
                          {
                              FullName = e.FirstName + " " + e.LastName,
                              Phone = e.Phone,
                              Birthday = e.Birthday,
                              Salary = e.Salary,
                              Email = e.Email,
                              Gender = e.Gender,
                              Degree = ed.Degree,
                              GPA = ed.GPA,
                              UniversityName = u.Name
                          }).FirstOrDefault();

            return result;
        }

        public string GenNIK()
        {
            string currentDate = DateTime.Now.ToString("ddMMyy");

            int existingCount = context.Employees.Count(e => e.NIK.StartsWith(currentDate));

            string newNIK = $"{currentDate}{existingCount + 1:D3}";

            return newNIK;
        }
    }
}
