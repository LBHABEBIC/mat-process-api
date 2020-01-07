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

        public MatProcessData UpdateProcessData(UpdateDefinition<BsonDocument> updateDefinition, string id)
        {
            //return updated document
            var options = new FindOneAndUpdateOptions<BsonDocument>();
            options.ReturnDocument = ReturnDocument.After;
            //query by
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            //find and update
            var result = matDbContext.getCollection().FindOneAndUpdate(filter, updateDefinition, options);
            //if empty result, the document to be updated was not found
            if(result == null)
            {
                throw new DocumentNotFound();
            }
            return ProcessDataFactory.CreateProcessDataObject(result);
        }
    }

    public class DocumentNotFound : Exception {}
}
