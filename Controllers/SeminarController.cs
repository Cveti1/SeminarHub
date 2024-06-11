using System.Globalization;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Data;
using SeminarHub.Data.Entities;
using SeminarHub.Models.Category;
using SeminarHub.Models.Seminar;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {
        private readonly SeminarHubDbContext data;

        public SeminarController(SeminarHubDbContext _data)
        {
            data = _data;
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            var seminar = await data
                .Seminars
                .Select(e => new SeminarViewShortModel()
                {
                    Id = e.Id,
                    Topic = e.Topic,
                    Lecturer = e.Lecturer,
                    DateAndTime = e.DateAndTime.ToString("dd/MM/yyyy H:mm"),
                    Category = e.Category.Name,
                    Organizer = e.Organizer.UserName
                })
                .ToListAsync();

            return View(seminar);
        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            SeminarFormModel seminar = new SeminarFormModel()
            {
                Categories = GetCategory()
            };

            return View(seminar);
        }


        [HttpPost]
        public async Task<IActionResult> Add(SeminarFormModel seminar)
        {
            
            if (!ModelState.IsValid)
            {
                return View(seminar);
            }
            

            string currentUserId = GetUserId();

            var seminarToAdd = new Seminar()
            {
                Topic = seminar.Topic,
                Lecturer = seminar.Lecturer,
                Details = seminar.Details,
                DateAndTime = seminar.DateAndTime,
                CategoryId = seminar.CategoryId,
                OrganizerId = currentUserId,
                Duration = seminar.Duration
            };

            await data.Seminars.AddAsync(seminarToAdd);
            await data.SaveChangesAsync();

            return RedirectToAction("All", "Seminar");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var seminarToEdit = await data.Seminars.FindAsync(id);

            if (seminarToEdit == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            if (currentUserId != seminarToEdit.OrganizerId)
            {
                return Unauthorized();
            }

            SeminarFormModel seminar = new SeminarFormModel()
            {
                Topic = seminarToEdit.Topic,
                Lecturer = seminarToEdit.Lecturer,
                Details = seminarToEdit.Details,
                Duration = seminarToEdit.Duration,
                DateAndTime = seminarToEdit.DateAndTime,
                CategoryId = seminarToEdit.CategoryId,
                Categories = GetCategory()
            };

            return View(seminar);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SeminarFormModel newSeminar)
        {
            var seminarToEdit = await data.Seminars.FindAsync(id);

            if (seminarToEdit == null)
            {
                return BadRequest();
            }

            string currentUser = GetUserId();

            if (currentUser != seminarToEdit.OrganizerId)
            {
                return Unauthorized();
            }


            seminarToEdit.Topic = newSeminar.Topic;
            seminarToEdit.Lecturer = newSeminar.Lecturer;
            seminarToEdit.Details = newSeminar.Details;
            seminarToEdit.Duration = newSeminar.Duration;
            seminarToEdit.DateAndTime = newSeminar.DateAndTime;
            seminarToEdit.CategoryId = newSeminar.CategoryId;

            await data.SaveChangesAsync();
            return RedirectToAction("All", "Seminar");
        }

       

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var seminar = await data
                .Seminars
                .Where(e => e.Id == id)
                .Select(e => new SeminarViewDetailModel()
                {
                    Id = e.Id,
                    Topic = e.Topic,
                    Lecturer = e.Lecturer,
                    DateAndTime = e.DateAndTime.ToString("dd/MM/yyyy H:mm"),
                    Organizer = e.Organizer.UserName,
                    Category = e.Category.Name,
                    Duration = e.Duration,
                    Details = e.Details
                })
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                return BadRequest();
            }

            return View(seminar);
        }



        public async Task<IActionResult> Join(int id)
        {
            var seminarToAdd = await data
                .Seminars
                .FindAsync(id);

            if (seminarToAdd == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            var entry = new SeminarParticipant()
            {
                SeminarId = seminarToAdd.Id,
                ParticipantId = currentUserId,
            };

            if (await data.SeminarsParticipants.ContainsAsync(entry))
            {
                return RedirectToAction("All", "Seminar");
            }

            await data.SeminarsParticipants.AddAsync(entry);
            await data.SaveChangesAsync();

            return RedirectToAction("Joined", "Seminar");
        }



        public async Task<IActionResult> Joined()
        {
            string currentUserId = GetUserId();

            var userSeminars = await data
                .SeminarsParticipants
                .Where(s => s.ParticipantId == currentUserId)
                .Select(s => new SeminarJoinedModel()
                {
                    Id = s.SeminarId,
                    Topic = s.Seminar.Topic,
                    Lecturer = s.Seminar.Lecturer,
                    DateAndTime = s.Seminar.DateAndTime.ToString("dd/MM/yyyy HH:mm"),
                    Category = s.Seminar.Category.Name,
                    Organizer = s.Seminar.Organizer.UserName,
                })
                .ToListAsync();

            return View(userSeminars);
        }


        public async Task<IActionResult> Leave(int id)
        {
            var eventId = id;
            var currentUser = GetUserId();

            var eventToLeave = data.Seminars.FindAsync(eventId);

            if (eventToLeave == null)
            {
                return BadRequest();
            }

            var entry = await data.SeminarsParticipants
                .FirstOrDefaultAsync(ep => ep.ParticipantId == currentUser && ep.SeminarId == eventId);
            data.SeminarsParticipants.Remove(entry);
            await data.SaveChangesAsync();

            return RedirectToAction("Joined", "Seminar");
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();


            var seminar = await data
                .Seminars
                .FindAsync(id);

           

            if (seminar == null || seminar.OrganizerId != userId)
            {
                return RedirectToAction("All", "Seminar");
            }

            var model = new DeleteViewModel()
            {
                Id = seminar.Id,
                Topic = seminar.Topic,
                DateAndTime = seminar.DateAndTime.ToString("dd/MM/yyyy H:mm")
            };

            return View(model);
        }

        [HttpPost]
     
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetUserId();

            var seminar = await data
                .Seminars
                .FindAsync(id);

            if (seminar == null || seminar.OrganizerId != userId)
            {
                return RedirectToAction("All", "Seminar");
            }
            
            data.Seminars.Remove(seminar);
           await data.SaveChangesAsync();

            return RedirectToAction(nameof(All));
        }

     
        //Helpers
        private IEnumerable<CategoryViewModel> GetCategory()
            => data
                .Categories
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                });



        private string GetUserId()
            => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
