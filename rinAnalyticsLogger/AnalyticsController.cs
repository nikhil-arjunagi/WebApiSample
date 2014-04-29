using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Globalization;
using System.Net.Http;
using System.Web.Http;

namespace AnalyticsLogger
{
    public class AnalyticsController : ApiController
    {
        public class LogEntity : TableEntity
        {
            public string Content { get; set; }

            public LogEntity(string content)
            {
                this.Content = content;
                this.PartitionKey = this.Content.GetHashCode().ToString() + DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture) + "_partition";
                this.RowKey = this.Content.GetHashCode().ToString() + DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture) + "_row";
            }
        }

        // POST api/Analytics/{id}
        public void Post(string id)
        {
            CloudTableClient tableClient = new CloudTableClient(
                        new Uri("baseUri"),
                        new StorageCredentials("account name", "key"));

            CloudTable table = tableClient.GetTableReference("tableName");
            table.CreateIfNotExists();

            TableOperation insertOperation = TableOperation.Insert(new LogEntity(id));

            // Execute the insert operation.
            table.Execute(insertOperation);
        }

        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent("Hello from OWIN!")
            };
        }

        public HttpResponseMessage Get(int id)
        {
            string msg = String.Format("Hello from OWIN (id = {0})", id);
            return new HttpResponseMessage()
            {
                Content = new StringContent(msg)
            };
        }
    }
}
