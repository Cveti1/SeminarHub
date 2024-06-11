using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.DataConstants;

namespace SeminarHub.Data.Entities
{
    public class Seminar
    { 
        [Key]
       public int Id { get; set; }


        [Required]
        [StringLength(TopicMaxName)]
        public string Topic { get; set; } = null!;

        

        [Required]
        [StringLength(LecturerMaxName)]
        public string Lecturer { get; set; } = null!;


        [Required]
        [StringLength(DetailsMax)]
        public string Details { get; set; } = null!;



        [Required]
        public string OrganizerId { get; set; } = null!;


       
        [ForeignKey(nameof(OrganizerId))]
        public IdentityUser Organizer { get; set; } = null!;


        public DateTime DateAndTime { get; set; }


        [MaxLength(180)]
        public int Duration { get; set; }


        [Required]
        public int CategoryId { get; set; }


       
        [ForeignKey(nameof(CategoryId))]
        public  Category Category { get; set; } = null!;

        public  ICollection<SeminarParticipant> SeminarsParticipants { get; set; } = new List<SeminarParticipant>();
    }
}



