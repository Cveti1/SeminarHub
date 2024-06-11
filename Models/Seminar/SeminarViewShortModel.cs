using Microsoft.AspNetCore.Identity;
using SeminarHub.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SeminarHub.Models.Seminar
{
    public class SeminarViewShortModel
    {
        public int Id { get; set; }

        public string Topic { get; set; } = null!;

        public string Lecturer { get; set; } = null!;

        public string DateAndTime { get; set; } = null!;

        public string Category { get; init; } = null!;
        public string Organizer { get; init; } = null!;
    }
}







