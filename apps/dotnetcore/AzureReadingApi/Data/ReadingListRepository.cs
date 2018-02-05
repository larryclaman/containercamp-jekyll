using System;
using System.Collections.Generic;
using System.Configuration;
using AzureReadingCore.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace AzureReadingApi.Data
{
    public static class ReadingListRepository<T> where T : class
    {
        public static readonly string DatabaseId = Settings.DatabaseId;
        public static readonly string CollectionId = Settings.CollectionId;
        private static string endpoint = Settings.EndPointDocDb;
        private static string authKey = Settings.ReadWriteAuthKey;

        private static DocumentClient client;

        public static void Initialize()
        {
            client = new DocumentClient(new Uri(endpoint), authKey);
        }

        public static async Task StartUpMode()
        {
            //Load data into the new collection.
            IEnumerable<Recommendation> libraryBooks = new List<Recommendation>()
            {
                new Recommendation() {
                    author ="Johnathan Baier",
                    description ="Learn Kubernetes the Right Way.",
                    id ="1", isbn="01234",
                    title ="Get Started with Kubernetes",
                    imageURL = "https://mtchouimages.blob.core.windows.net/books/Kubernetes.jpg" },
                new Recommendation() {
                    author ="Rajdeep Das",
                    description ="Docker Networking Deep Dive",
                    id ="2", isbn="95201",
                    title ="Learn Docker Networking",
                    imageURL ="https://mtchouimages.blob.core.windows.net/books/DockerNetworking.jpg"  },
                new Recommendation() {
                    author ="Rajesh RV",
                    description ="Build scalable microservices with Spring and Docker",
                    id ="3", isbn="090923",
                    title ="Spring Microservices",
                    imageURL = "https://mtchouimages.blob.core.windows.net/books/SpringMicroServices.jpg" },
                new Recommendation() {
                    author ="Aleksandar Prokopec",
                    description ="Learn the art of building concurrent applications!",
                    id ="4", isbn="342421",
                    title ="Learn Concurrent Programming in Scala",
                    imageURL = "https://mtchouimages.blob.core.windows.net/books/Scala.jpg" },
                new Recommendation() {
                    author ="Vitorrio Bertocci",
                    description ="Azure Active Directory capabilities from the master!",
                    id ="5", isbn="472891",
                    title ="Modern Authentication with AzureAD",
                    imageURL = "https://mtchouimages.blob.core.windows.net/books/AzureAD.jpg" },
                new Recommendation() {
                    author ="Leoanrd G. Lobel",
                    description ="Step by step guide for developers!",
                    id ="6", isbn="291920",
                    title ="Microsoft Azure SQL",
                    imageURL = "https://mtchouimages.blob.core.windows.net/books/AzureSQL.jpg" },
                new Recommendation() {
                    author ="Rajdeep Das",
                    description ="Exam Ref 70-487",
                    id ="7", isbn="9788652",
                    title ="Developing Azure and Web Services",
                    imageURL = "https://mtchouimages.blob.core.windows.net/books/AzureCert.jpg" },
                new Recommendation() {
                    author ="Haishi Bai",
                    description ="Service fabric for developers!",
                    id ="8", isbn="667263",
                    title ="Programming Microsoft Azure Service Fabric",
                    imageURL = "https://mtchouimages.blob.core.windows.net/books/ServiceFabric.jpg" },
            } as IEnumerable<Recommendation>;

            foreach (var doc in libraryBooks)
            {
                try
                {
                    await client.UpsertDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    doc);
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static async Task<IEnumerable<T>> GetBooks(Expression<Func<T, bool>> predicate)
        { 
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<IEnumerable<Book>> GetBooksForUser(Expression<Func<Book, bool>> predicate)
        {
            List<Book> results = new List<Book>();

            try
            {
                IDocumentQuery<Book> query = client.CreateDocumentQuery<Book>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<Book>());
                }
            }
            catch (Exception ex)
            {

            }

            return results;
        }

        public static async Task UpsertBookForUser(Book myNewBook)
        {
            try
            {
                await client.UpsertDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                myNewBook);
            }
            catch (Exception ex)
            {

            }
        }

        public static async Task RemoveBookForUser(string bookId)
        {
            try
            {
                await client.DeleteDocumentAsync(
                    UriFactory.CreateDocumentUri(DatabaseId, CollectionId, bookId));
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}