using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CusanellyLibrary.AWS.DynamoDbLibrary
{
    public class DynamoDbMain
    {
        private string _AccessKey { get; set; }
        private string _AccessSecret { get; set; }
        private RegionEndpoint _Region { get; set; }
        private readonly AmazonDynamoDBClient _DynamoClient;
        public DynamoDbMain()
        {

        }
        public DynamoDbMain(AmazonDynamoDBClient client)
        {
            _DynamoClient = client;
        }
        public DynamoDbMain(string acccesskey, string accesssecret, RegionEndpoint region)
        {
            _AccessKey = acccesskey;
            _AccessSecret = accesssecret;
            _Region = region;
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

        public async Task<Document> Insert(string tablename, string dataoriginal, string dataresult)
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
