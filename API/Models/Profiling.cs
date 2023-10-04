using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model
{
    public class Profiling
    {
        [Key]
        public string NIK { get; set; }
        public Account Account { get; set; }
        public int Education_Id { get; set; }
        public Education Education { get; set; }
    }
}
