namespace AzureReadingCore.Models
{
    public class Recommendation
    {
        public string id;
        public string type = "recommendation";
        public string isbn;
        public string title;
        public string author;
        public string description;
        public string imageURL;
    }
}
