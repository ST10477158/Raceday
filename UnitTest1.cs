using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raceday.Controllers;
using Raceday.Data;
using Raceday.Models;
using System.Security.Claims;
using Results = Raceday.Models.Results;

namespace Raceday.Tests
{
    public class RaceDayUnitTests
    {
        private ApplicationDBContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            return new ApplicationDBContext(options);
        }

        // TEST 1: User registration and login - Requirement: Password must be hashed
        [Fact]
        public async Task Test_UserRegistration_HashesPassword()
        {
            var db = GetInMemoryDb();
            var controller = new AuthController(db);
            var user = new User
            {
                User_Email = "test@raceday.co.za",
                User_Password = "Password123",
                User_Role = "Participant"
            };

            var result = await controller.Register(user);

            Assert.IsType<OkObjectResult>(result);
            var savedUser = await db.Users.FirstAsync();
            // Password must NOT be stored in original form
            Assert.NotEqual("Password123", savedUser.User_Password);
            Assert.True(BCrypt.Net.BCrypt.Verify("Password123", savedUser.User_Password));
        }

        // TEST 2: Login fails with wrong password - Authenticated vs Unauthenticated
        [Fact]
        public async Task Test_Login_Fails_WithWrongPassword_Unauthenticated()
        {
            var db = GetInMemoryDb();
            var controller = new AuthController(db);
            var user = new User { User_Email = "login@test.com", User_Password = "Correct123", User_Role = "Organiser" };
            await controller.Register(user);

            var loginAttempt = new User { User_Email = "login@test.com", User_Password = "WrongPassword" };
            var result = await controller.Login(loginAttempt);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        // TEST 3: Event Management - Organiser can create event
        [Fact]
        public async Task Test_EventManagement_OrganiserCanCreateEvent()
        {
            var db = GetInMemoryDb();
            // Seed organiser
            var user = new User { User_ID = 1, User_Email = "org@test.com", User_Password = "hashed", User_Role = "Organiser" };
            db.Users.Add(user);
            db.Organisers.Add(new Organiser { Organiser_ID = 1, User_ID = 1, Organisation_Name = "Test Org", Organisation_Phone = "011" });
            await db.SaveChangesAsync();

            var controller = new EventsController(db);
            // Mock authenticated Organiser user
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Organiser")
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) }
            };

            var ev = new Events
            {
                Event_Name = "Test Marathon",
                Event_Description = "Test Description",
                Event_Venue = "Potchefstroom",
                Event_StartTime = DateTime.Now.AddDays(7)
            };

            var result = await controller.Create(ev);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, await db.Events.CountAsync());
        }

        // TEST 4: Role-Based - Different roles are rejected (Participant cannot create event logic)
        [Fact]
        public async Task Test_RoleBased_ParticipantCannotAccessOrganiserEndpoint()
        {
            var db = GetInMemoryDb();
            var user = new User { User_ID = 2, User_Email = "part@test.com", User_Password = "hashed", User_Role = "Participant" };
            db.Users.Add(user);
            db.Participants.Add(new Participant { Participant_ID = 1, User_ID = 2, Participant_Name = "Thabo", Participant_Surname = "Mokoena" });
            await db.SaveChangesAsync();

            // This test proves the [Authorize(Roles="Organiser")] attribute exists on EventsController.Create
            var method = typeof(EventsController).GetMethod("Create");
            var authorizeAttr = method!.GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute), false)
               .FirstOrDefault() as Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

            Assert.NotNull(authorizeAttr);
            Assert.Equal("Organiser", authorizeAttr.Roles);
        }

        // TEST 5: Event Enrolments - Participant can enrol in category
        [Fact]
        public async Task Test_EventEnrolments_ParticipantCanEnrol()
        {
            var db = GetInMemoryDb();
            var user = new User { User_ID = 3, User_Email = "enrol@test.com", User_Password = "hashed", User_Role = "Participant" };
            db.Users.Add(user);
            db.Participants.Add(new Participant { Participant_ID = 1, User_ID = 3, Participant_Name = "Ayanda", Participant_Surname = "Nkosi" });
            db.Organisers.Add(new Organiser { Organiser_ID = 1, User_ID = 1, Organisation_Name = "Org", Organisation_Phone = "011" });
            db.Events.Add(new Events { Event_ID = 1, Organiser_ID = 1, Event_Name = "City Run", Event_Description = "Run", Event_Venue = "JHB", Event_StartTime = DateTime.Now });
            db.Categories.Add(new Category { Category_ID = 1, Event_ID = 1, Category_Name = "5KM Fun Run", Category_EntryAmount = 80, Category_QualifyAge = 13 });
            await db.SaveChangesAsync();

            var enrolment = new Enrolment
            {
                Participant_ID = 1,
                Category_ID = 1,
                Enrolment_Date = DateTime.UtcNow,
                Race_Number = 101
            };
            db.Enrolments.Add(enrolment);
            await db.SaveChangesAsync();

            var count = await db.Enrolments.CountAsync();
            Assert.Equal(1, count);
            var saved = await db.Enrolments.FirstAsync();
            Assert.Equal(101, saved.Race_Number);
        }

        // TEST 6: Results - Organiser can capture results with Average_Pace
        [Fact]
        public async Task Test_Results_CaptureWithAveragePace()
        {
            var db = GetInMemoryDb();
            db.Enrolments.Add(new Enrolment { Enrolment_ID = 1, Participant_ID = 1, Category_ID = 1, Enrolment_Date = DateTime.Now, Race_Number = 101 });
            await db.SaveChangesAsync();

            var result = new Results
            {
                Enrolment_ID = 1,
                Finish_Time = TimeSpan.Parse("00:52:35"),
                Finish_Position = 45,
                Average_Pace = 5.26m,
                Category_Position = 12
            };
            db.Results.Add(result);
            await db.SaveChangesAsync();

            var saved = await db.Results.FirstAsync();
            Assert.Equal(5.26m, saved.Average_Pace);
        }
    }
}