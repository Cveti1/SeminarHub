using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SeminarHub.Data.Entities;

namespace SeminarHub.Data.Entities
{
    public class SeminarParticipant
    {
        [Required]
        public int SeminarId { get; set; }


        [ForeignKey(nameof(SeminarId))]
        public  Seminar Seminar { get; set; } = null!;


       [Required]
       public string ParticipantId { get; set; } = null!;


       [ForeignKey(nameof(ParticipantId))]
        public  IdentityUser Participant { get; set; } = null!;
    }
}



