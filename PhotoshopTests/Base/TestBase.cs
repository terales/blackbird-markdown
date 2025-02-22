using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Extensions.Configuration;
using PhotoshopApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PhotoshopTests.Base
{
    public class TestBase
    {
        public IEnumerable<AuthenticationCredentialsProvider> Creds { get; set; }

        public InvocationContext InvocationContext { get; set; }

        public FileManager FileManager { get; set; }

        public TestBase()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var folderLocation = config.GetSection("TestFolder").Value 
                ?? throw new InvalidOperationException("TestFolder configuration is required");

            Creds = new List<AuthenticationCredentialsProvider>();

            InvocationContext = new InvocationContext
            { 
                AuthenticationCredentialsProviders = Creds,
            };

            FileManager = new FileManager(folderLocation);
        }
    }
}