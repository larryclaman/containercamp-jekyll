using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureReadingCore.Models
{
    public class Settings
    {
        public static string DatabaseId = Environment.GetEnvironmentVariable("DatabaseId");
        public static string CollectionId = Environment.GetEnvironmentVariable("CollectionId");
        public static string readerName = Environment.GetEnvironmentVariable("readerName");
        public static string EndPointDocDb = Environment.GetEnvironmentVariable("EndPointDocDb");
        public static string EndPoint = Environment.GetEnvironmentVariable("EndPoint");
        public static string ReadWriteAuthKey = Environment.GetEnvironmentVariable("ReadWriteAuthKey"); //read-write
        public static string ReadOnlyAuthKey = Environment.GetEnvironmentVariable("ReadOnlyAuthKey"); //read only
    }
}