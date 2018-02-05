using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AzureReadingCore.Models;
using AzureReadingApi.Data;
using Newtonsoft.Json;

namespace AzureReadingApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Books")]
    public class BooksController : Controller
    {
        [Route("Index")]
        public IEnumerable<Book> Index()
        {
            Book book = new Book()
            {
                title = "test",
                description = "some long and winding description",
                author = "all work no money",
                id = "9999",
                isbn = "484828222234"
            };

            List<Book> myBooks = new List<Book>();
            myBooks.Add(book);

            return myBooks.AsEnumerable<Book>();            
        }
        
        [HttpGet]
        [Route("User")]
        public async Task<IEnumerable<Book>> GetUsersBooks()
        {
            ReadingListRepository<Book>.Initialize();
            IEnumerable<Book> myBooks = (IEnumerable<Book>) await ReadingListRepository<Book>.GetBooksForUser(b => b.reader == Settings.readerName);
            return myBooks;
        }

        [Route("Recommendations")]
        public async Task<IEnumerable<Recommendation>> GetRecommendations()
        {
            ReadingListRepository<Recommendation>.Initialize();
            IEnumerable<Recommendation> recomms = (IEnumerable<Recommendation>) await ReadingListRepository<Recommendation>.GetBooks(d => d.type == "recommendation");
            return recomms;
        }

        [Route("User/Create")]
        public async Task<string> SaveBook([FromBody] string content)
        {
            ReadingListRepository<Book>.Initialize();

            Book myBookToSave = JsonConvert.DeserializeObject<Book>(content);

            await ReadingListRepository<Book>.UpsertBookForUser(myBookToSave);

            return "Success";

        }

        [Route("User/Edit/{id}")]
        public async Task<Book> GetBook(string id)
        {
            //get the requested record.
            ReadingListRepository<Book>.Initialize();

            IEnumerable<Book> myBooks = (IEnumerable<Book>)await ReadingListRepository<Book>.GetBooksForUser(b => b.id == id.ToString());

            return myBooks.First();
        }

        [Route("User/Remove/{id}")]
        public async Task<string> DeleteBook(string id)
        {
            ReadingListRepository<Book>.Initialize();
            await ReadingListRepository<Book>.RemoveBookForUser(id);

            return "success";
        }

        [Route("StartUp")]
        public async Task<string> StartUp()
        {
            ReadingListRepository<Recommendation>.Initialize();

            await ReadingListRepository<Recommendation>.StartUpMode();

            return "success";
        }
    }
}