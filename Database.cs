using MongoDB.Driver;
using MongoDB.Bson;// To write in the cluster
using System;
using System.Collections.ObjectModel;
using Amazon.Runtime.Documents;

namespace Striker_finale
{
	class Database
	{
		static IMongoCollection<BsonDocument> Collection;
		static MongoClient Client;
		static BsonDocument Schema = new BsonDocument
		{
			{"username", "" },
			{"score", 0 },
			{"date", DateTime.Now }
		};
		public static void Connect()
		{
			var settings = MongoClientSettings.FromConnectionString("mongodb+srv://marco0019:bamboccetti@striker.vslyafa.mongodb.net/?retryWrites=true&w=majority");
			settings.ServerApi = new ServerApi(ServerApiVersion.V1);
			Client = new MongoClient(settings);
			Collection = Client.GetDatabase("Striker").GetCollection<BsonDocument>("classification");
		}
		public static void Test()
		{
			Connect();
			var documents = Collection.Find(new BsonDocument()).ToList();
			foreach (var db in documents)
			{
				Console.WriteLine($"Username: {db["username"]}, Score: {db["score"]}, date: {db["date"]}");
			}
			Console.WriteLine();
			Console.Write(Read("Marco Campione")["score"]);
			//Update("Marco Campione", 1000);
		}
		public static BsonDocument Read(string currentUser)
		{
			Connect();
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			return Collection.Find(filter).FirstOrDefault();
		}
		public static void Sort()
		{
			Connect();
			var sort = Builders<BsonDocument>.Sort.Descending("score");
		}
		public static void Update(string currentUser, int highscore)
		{
			Connect();
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			var update = Builders<BsonDocument>.Update.Set("score", highscore);
			Collection.UpdateOne(filter, update);
		}
		public static void Insert(string currentUser)
		{
			Connect();
			Collection.InsertOne(new BsonDocument
			{
				{ "username", currentUser },
				{ "score", 0 },
				{ "date", DateTime.Now }
			});
		}
	}
}
