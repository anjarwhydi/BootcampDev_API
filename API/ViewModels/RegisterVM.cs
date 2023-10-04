using API.Model;

namespace API.ViewModels
{
    public class RegisterVM
    {
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public gender Gender { get; set; }
        public string Password { get; set; }
        public degree Degree { get; set; }
        public string GPA { get; set; }
        public int University_Id { get; set; }
    }
}
