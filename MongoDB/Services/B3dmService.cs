using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MongoDB.Services
{
    public class B3dmService
    {
        private readonly IMongoCollection<b3dm> _B3DMservice;
        private readonly GridFSBucket gridFSBucket;
        public B3dmService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDBConnString"));
            var database = client.GetDatabase("MongoFile");
            gridFSBucket = new GridFSBucket(database, new GridFSBucketOptions
            {
                BucketName = "fs1",
                ChunkSizeBytes = 1024 * 1024,
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Secondary
            });
           
        }
        public  BsonDocument browseFile()
        {
            var filter = Builders<GridFSFileInfo>.Filter;
            BsonDocument document;
            using (var cursor =  gridFSBucket.Find(filter.Eq(x => x.Filename, "G:\\Map\\Tasks\\6月Task\\13\\13-2-1603\\output\\floor\\tileset.json")))
            {
                var fileInfo = cursor.FirstOrDefault();
                document = fileInfo.BackingDocument;
                
            }
            return (document);
        }
        public async Task<MemoryStream> downTileData(string fileName)
        {
            GridFSFileInfo fileInfo;
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName);
            var options = new GridFSFindOptions
            {
                Limit = 1 
            };
            using (var cursor = gridFSBucket.Find(filter, options))
            {
                fileInfo = cursor.ToList().FirstOrDefault();
            }
            if(fileInfo.Id!=null)
            {
                GridFSDownloadStream gridFSDownloadStream=gridFSBucket.OpenDownloadStream(fileInfo.Id);
                
            }
            
            MemoryStream destination = new MemoryStream();
            destination.Seek(0, SeekOrigin.Begin);
            await gridFSBucket.DownloadToStreamByNameAsync(fileName, destination);
                if(destination!=null)
                {
                  return  destination;
                }
                else
                {
                    return null;
                }
        }
        public async Task<List<b3dm>> Get()
        {
            var result = await _B3DMservice.FindAsync(tile => true);
            return result.ToList();
        }
    }
}
