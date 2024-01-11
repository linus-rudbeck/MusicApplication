using EntityFrameworkCore.Testing.Moq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApplication.Controllers;
using MusicApplication.Data;
using MusicApplication.Models;

namespace MusicApplication.Tests
{
    public class SongsControllerTests
    {
        private ApplicationDbContext mockContext;
        private SongsController controller;

        [SetUp]
        public void SetUp()
        {
            // Skapa en mockad DbContext
            mockContext = Create.MockedDbContextFor<ApplicationDbContext>();

            var genre = new Genre() { Id = 1, Name = "Test" };

            // Konfigurera mockad DbSet
            var songs = new List<Song>
            {
                new Song { Id = 1, Title = "Song 1", Artist = "Artist 1", GenreId = 1, Genre = genre },
                new Song { Id = 2, Title = "Song 2", Artist = "Artist 2" }
            };

            genre.Songs.Add(songs[0]);

            // Använd SetupDbSet för att konfigurera DbSet
            mockContext.Set<Song>().AddRange(songs);
            mockContext.SaveChanges();

            mockContext.ChangeTracker.Clear();

            // Skapa controller med mockad DbContext
            controller = new SongsController(mockContext);
        }


        #region Index
        [Test]
        public async Task Index_ReturnsViewResult_WithListOfSongs()
        {
            // Act
            var result = await controller.Index();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());

            var viewResult = result as ViewResult;
            var model = viewResult.Model;
            Assert.That(model, Is.InstanceOf<List<Song>>());

            var songList = model as List<Song>;
            Assert.That(songList.Count, Is.EqualTo(2));
        }
        #endregion


        #region Create
        [Test]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var newSong = new Song { Title = "New Song", Artist = "New Artist" };

            // Act
            var result = await controller.Create(newSong);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            controller.ModelState.AddModelError("Error", "Model error");
            var newSong = new Song { Title = "New Song", Artist = "New Artist" };

            // Act
            var result = await controller.Create(newSong);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
        #endregion


        #region AuthorizeAttributes
        [Test]
        public void Details_Get_HasAllowAnonymousAttribute()
        {
            // Arrange
            var methodInfo = typeof(SongsController).GetMethod("Details", new Type[] { typeof(int?) });
            var attributes = methodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);

            // Assert
            Assert.That(attributes, Is.Not.Empty);
        }

        [Test]
        public void Controller_Has_AuthorizeAttribute()
        {
            // Arrange
            var attributes = typeof(SongsController).GetCustomAttributes(typeof(AuthorizeAttribute), false);

            // Assert
            Assert.That(attributes, Is.Not.Empty);
        }
        #endregion

    }
}