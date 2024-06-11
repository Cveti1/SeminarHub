using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.DataConstants;

namespace SeminarHub.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(CategoryNameMax)]
        public string Name { get; set; } = null!;

        public  ICollection<Seminar> Seminars { get; set; } = new List<Seminar>();
    }
}


