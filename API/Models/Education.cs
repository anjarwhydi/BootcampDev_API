using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace API.Model
{
    public class Education
    {
        [Key]
        public int Id { get; set; }
        public degree Degree { get; set; }
        public string GPA { get; set; }
        public int University_Id { get; set; }
        public University University { get; set; }
        public ICollection<Profiling> Profilings { get; set; }
    }

    public enum degree
    {
        D3,
        D4,
        S1,
        S2,
        S3
    }
}
