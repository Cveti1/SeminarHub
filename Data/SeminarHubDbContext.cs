﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data.Entities;

namespace SeminarHub.Data
{
    public class SeminarHubDbContext : IdentityDbContext
    {
        public SeminarHubDbContext(DbContextOptions<SeminarHubDbContext> options)
            : base(options)
        {
        }

        public  DbSet<Category> Categories { get; set; } = null!;
        public  DbSet<Seminar> Seminars { get; set; } = null!;
        public  DbSet<SeminarParticipant> SeminarsParticipants { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SeminarParticipant>()
                .HasKey(e => new { e.ParticipantId, e.SeminarId });

            builder.Entity<SeminarParticipant>()
                .HasOne(e => e.Participant)
                .WithMany()
                .HasForeignKey(e => e.ParticipantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SeminarParticipant>()
                .HasOne(e => e.Seminar)
                .WithMany(e => e.SeminarsParticipants)
                .HasForeignKey(e => e.SeminarId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
               .Entity<Category>()
               .HasData(new Category()
               {
                  Id = 1,
                   Name = "Technology & Innovation"
               },
               new Category()
               {
                   Id = 2,
                   Name = "Business & Entrepreneurship"
               },
               new Category()
               {
                   Id = 3,
                   Name = "Science & Research"
               },
               new Category()
               {
                   Id = 4,
                   Name = "Arts & Culture"
               });

            base.OnModelCreating(builder);
        }
    }
}