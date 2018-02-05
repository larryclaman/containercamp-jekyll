using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AzureReadingList.Models;
using AzureReadingCore.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace AzureReadingList.Controllers
{
    public class ReadingListController : Controller
    { 
        // GET: ReadingList
        public async Task<ActionResult> Index()
        {
            ReadingListViewModel readingListContent = new ReadingListViewModel();

            //get recommendations
            HttpHelper recommedData = new HttpHelper("api/Books/Recommendations");
            String recommendDataResponse = await recommedData.GetResponse();
            readingListContent.LibraryBooks = JsonConvert.DeserializeObject<IEnumerable<Recommendation>>(recommendDataResponse);

            //get user books
            HttpHelper userData = new HttpHelper("api/Books/user");
            String userDataResponse = await userData.GetResponse();
            readingListContent.MyBooks = JsonConvert.DeserializeObject<IEnumerable<Book>>(userDataResponse);
            
            return View(readingListContent);
        }

        // POST: ReadingList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                Book myNewBookToSave = SaveCollectionAsBook(collection);

                HttpHelper postHelper = new HttpHelper("api/Books/User/Create");
                string response = await postHelper.PostRequest(JsonConvert.SerializeObject(myNewBookToSave));
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ErrorViewModel error = new ErrorViewModel()
                {
                    RequestId = ex.Source,
                    ErrorMessage = ex.Message
                };
                return View();
            }
        }

        private static Book SaveCollectionAsBook(IFormCollection collection)
        {
            return new Book()
            {
                id = string.Concat(Settings.readerName, collection["isbn"]),
                title = collection["title"],
                isbn = collection["isbn"],
                description = collection["description"],
                author = collection["author"],
                reader = Settings.readerName
            };
        }

        // GET: ReadingList/Edit/5
        public async Task<ActionResult> Edit(string id)
        {

            HttpHelper editData = new HttpHelper("api/Books/User/Edit/" + id);
            String editDataResponse = await editData.GetResponse();
            Book myBookToEdit = JsonConvert.DeserializeObject<Book>(editDataResponse);

            return View(myBookToEdit);
        }

        // POST: ReadingList/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, IFormCollection collection)
        {
            try
            {
                Book updatedBook = SaveCollectionAsBook(collection);

                HttpHelper postHelper = new HttpHelper("api/Books/User/Create");
                string response = await postHelper.PostRequest(JsonConvert.SerializeObject(updatedBook));
                     
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReadingList/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                HttpHelper deleteHelper = new HttpHelper("api/Books/User/Remove/" + id);
                string deleteHelperResponse = await deleteHelper.GetResponse();

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Index");
        }

        // POST: ReadingList/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}