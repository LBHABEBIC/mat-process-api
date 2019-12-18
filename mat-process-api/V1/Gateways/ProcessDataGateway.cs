using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using mat_process_api.V1.Domain;
using mat_process_api.V1.Factories;
using mat_process_api.V1.Gateways;
using mat_process_api.V1.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace mat_process_api.V1.Gateways
{
    public class ProcessDataGateway : IProcessDataGateway
    {
        private IMatDbContext matDbContext;

        public ProcessDataGateway(IMatDbContext _matDbContext)
        {
            matDbContext = _matDbContext;
        }
        public MatProcessData GetProcessData(string processRef)
        {
            //retrieve data by id
            var filter = Builders<BsonDocument>.Filter.Eq("_id", processRef);
            //we will never expect more than one JSON documents matching an ID so we always choose the first/default result
            var result = matDbContext.getCollection().FindAsync(filter).Result.FirstOrDefault();

            return ProcessDataFactory.CreateProcessDataObject(result);
        }
        public string PostInitialProcessDocument(MatProcessData processDoc)
        {
            try
            {
                BsonDocument bsonObject = BsonDocument.Parse(JsonConvert.SerializeObject(processDoc));
                matDbContext.getCollection().InsertOneAsync(bsonObject).Wait(); 
                return processDoc.Id;
            }
            catch(MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    throw new ConflictException();
                }
                throw ex;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }

    public class ConflictException : System.Exception { }
}
