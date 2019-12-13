using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon;

namespace DynamoDBLibrary
{
    public interface IDynamoContext
    {
        Task CheckTable(string tablename);
        Task<Document> Insert(string tablename, string dataoriginal, string dataresult, Dictionary<string, DynamoDBEntry> values = null);
    }
    public class DynamoContext : IDynamoContext
    {
        private string _AccessKey { get; set; }
        private string _AccessSecret { get; set; }
        private RegionEndpoint _Region { get; set; }
        private readonly AmazonDynamoDBClient _DynamoClient;
        public DynamoContext()
        {

        }
        public DynamoContext(AmazonDynamoDBClient client)
        {
            _DynamoClient = client;
        }
        public DynamoContext(string acccesskey, string accesssecret, string region)
        {
            _AccessKey = acccesskey;
            _AccessSecret = accesssecret;
            _Region = RegionEndpoint.GetBySystemName(region);
            _DynamoClient = new AmazonDynamoDBClient(_AccessKey, _AccessSecret, _Region);
        }
        public async Task CheckTable(string tablename)
        {
            var table = await _DynamoClient.ListTablesAsync();
            if (table.HttpStatusCode.Equals(HttpStatusCode.OK))
            {
                if (!table.TableNames.Contains(tablename))
                {
                    CreateDynamoTable(tablename);
                }
            }
        }
        private void CreateDynamoTable(string tablename)
        {
            CreateTableRequest request = new CreateTableRequest()
            {
                TableName = tablename,
                ProvisionedThroughput = new ProvisionedThroughput(25, 25),
                KeySchema = new List<KeySchemaElement>() {
                    new KeySchemaElement{
                        AttributeName = "Id",
                        KeyType = KeyType.HASH
                    },
                    new KeySchemaElement{
                        AttributeName = "DateCreated",
                        KeyType = KeyType.RANGE
                    }
                },
                AttributeDefinitions = new List<AttributeDefinition>() {
                    new AttributeDefinition{
                        AttributeName = "Id",
                        AttributeType = ScalarAttributeType.N
                    },
                    new AttributeDefinition{
                        AttributeName = "DateCreated",
                        AttributeType = ScalarAttributeType.N
                    }
                }
            };
            try
            {
                CreateTableResponse response = _DynamoClient.CreateTableAsync(request).Result;
                string status = String.Empty;
                do
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    status = response.TableDescription.TableStatus;
                } while (status != TableStatus.ACTIVE);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Document> Insert(string tablename, string dataoriginal, string dataresult, Dictionary<string,DynamoDBEntry> values = null)
        {
            Table table = Table.LoadTable(_DynamoClient, tablename);
            var data = new Document();
            var datestring = DateTime.Now.ToString("yyyyMMddHHmmss");
            datestring = datestring.Replace("/", "");
            var date = Int64.Parse(datestring);
            data["Id"] = date;
            data["DateCreated"] = date;
            data["DateModified"] = date;
            data["Status"] = 1;
            data["DataOriginal"] = dataoriginal;
            data["DataResult"] = dataresult;

            var result = await table.PutItemAsync(data);
            return result;
        }
    }
}
