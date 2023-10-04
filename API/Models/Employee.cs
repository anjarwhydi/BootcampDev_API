using API.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Employee
{
    [Key]
    public string NIK { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public DateTime Birthday { get; set; }
    public int Salary { get; set; }
    public string Email { get; set; }
    public gender Gender { get; set; }
    public Account Account { get; set; }
}

public enum gender
{
    Male,
    Female
}