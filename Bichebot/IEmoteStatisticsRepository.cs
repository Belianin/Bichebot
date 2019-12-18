using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Bichebot
{
    public interface IEmoteStatisticsRepository
    {
        Task IncrementAsync(string emoteName);

        Task<IEnumerable<EmoteStatistic>> GetAllStatisticsAsync();
    }

    public class EmoteStatistic
    {
        [BsonId]
        public string Name { get; set; }
        
        public int MessageCount { get; set; }
        
        public int ReactionCount { get; set; }
    }
    
    public class MongoEmoteStatisticsRepository : IEmoteStatisticsRepository
    {
        private readonly IMongoClient client;

        private readonly IMongoDatabase database;

        public MongoEmoteStatisticsRepository(string password)
        {
            client = new MongoClient(
                $"mongodb+srv://Aler:{password}@bichebot-puhh4.azure.mongodb.net/test?retryWrites=true&w=majority");

            database = client.GetDatabase("bichebot");
        }

        public async Task IncrementAsync(string emoteName)
        {
            try
            {

                // UPDATE METHOD
                Console.WriteLine("Incrementing " + emoteName);
                var collection = database.GetCollection<EmoteStatistic>("emotes");
                var items = await collection.FindAsync(p => p.Name == emoteName).ConfigureAwait(false);
                var item = items?.ToEnumerable().FirstOrDefault();
            
                if (item != null)
                {
                    Console.WriteLine("REplaceing");
                    item.ReactionCount += 1;
                    var replaceOneResult = await collection.ReplaceOneAsync(i => i.Name == emoteName, item).ConfigureAwait(false);
                }
                else
                {
                    Console.WriteLine("Inserting");
                    await collection.InsertOneAsync(
                        new EmoteStatistic
                        {
                            Name = emoteName,
                            MessageCount = 0,
                            ReactionCount = 1
                        });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<EmoteStatistic>> GetAllStatisticsAsync()
        {
            var collection = database.GetCollection<EmoteStatistic>("emotes");
            var result = await collection.FindAsync(FilterDefinition<EmoteStatistic>.Empty).ConfigureAwait(false);
            return result.ToEnumerable();
        }
    }
}