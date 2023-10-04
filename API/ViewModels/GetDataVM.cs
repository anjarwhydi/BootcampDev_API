using API.Model;

namespace API.ViewModels
{
    public class GetDataVM
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public gender Gender { get; set; }
        public degree Degree { get; set; }
        public string GPA { get; set; }
        public string UniversityName { get; set; }
    }
}
