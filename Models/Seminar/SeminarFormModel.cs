using Microsoft.AspNetCore.Identity;
using SeminarHub.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SeminarHub.Models.Category;
using static SeminarHub.Data.DataConstants;
using System.Configuration;
using Microsoft.VisualBasic;

namespace SeminarHub.Models.Seminar
{
    public class SeminarFormModel
    {
       
        [Required]
        [StringLength(TopicMaxName, MinimumLength = TopicMinName)]
        public string Topic { get; set; } = null!;



        [Required]
        [StringLength(LecturerMaxName, MinimumLength = LecturerMinName)]
        public string Lecturer { get; set; } = null!;


        [Required]
        [StringLength(DetailsMax, MinimumLength = DetailsMin)]
        public string Details { get; set; } = null!;


        
        [Required]
        public DateTime DateAndTime { get; set; } 



        [Range(DurationMin, DurationMax)]
        public int Duration { get; set; }


        [Required]
        public int CategoryId { get; set; }


        public virtual IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}

