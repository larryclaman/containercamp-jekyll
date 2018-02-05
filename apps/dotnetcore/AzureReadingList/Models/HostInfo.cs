using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AzureReadingList.Models
{
    public class HostInfo
    {
        private static HostInfo instance;
        public string HostIpAddress;

        private HostInfo()
        {
            IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = iPHostEntry.AddressList[0];
            HostIpAddress = ipAddress.ToString();
        }

        public static HostInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HostInfo();
                }
                return instance;
            }
        }
    }
}