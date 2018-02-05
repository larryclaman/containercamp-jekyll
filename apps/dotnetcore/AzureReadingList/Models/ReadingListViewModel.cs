using AzureReadingCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureReadingList.Models
{
    public class ReadingListViewModel
    {
        public IEnumerable<Book> MyBooks { get; set; }
        public IEnumerable<Recommendation> LibraryBooks { get; set; }
    }
}
