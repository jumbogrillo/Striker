using MongoDB.Driver;
using MongoDB.Bson;// To write in the cluster
using System;
using System.Collections.Generic;
using Stricker;
using System.Diagnostics;

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
			Sort();
		}
		public static List<BsonDocument> AllDoc()
		{
			Connect();
			return Collection.Find(new BsonDocument()).ToList();
		}
		public static bool IsPresent(string currentUser)
		{
			Connect();
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			return Collection.Find(filter).ToList().Count != 0;
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
			var documents = Collection.Find(new BsonDocument()).ToList();
			int[] scores = ScoreSorted();
			for(int i = 0; i < scores.Length; i++)
			{
				Console.WriteLine(documents[scores[i]]["username"] + " " + documents[scores[i]]["score"]);
			}
		}
		private static int[] ScoreSorted()
		{
			var documents = Collection.Find(new BsonDocument()).ToList();
			int[] indexes = new int[documents.Count];
			for (int i = 0; i < documents.Count - 1; i++)
				for (int j = i + 1; j < documents.Count; j++)
					if (documents[i]["score"] > documents[j]["score"])
					{
						var appo = documents[i];
						documents[i] = documents[j];
						documents[j] = appo;
						indexes[i] = j;
					}
					else if (String.Compare(documents[i]["username"].ToString(), documents[j]["username"].ToString()) > 0)
					{

					}
			return indexes;
		}
		private static void Invert(List<BsonDocument> docs, int index1, int index2, int[] indexes)
		{
			var appo = docs[index1];
			docs[index1] = docs[index2];
			docs[index2] = appo;
			indexes[index1] = index2;
		}
		public static void Update(string currentUser, int highscore, long time)
		{
			Connect();
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			var update = Builders<BsonDocument>.Update.Set("score", highscore);
			Collection.UpdateOne(filter, update);
			filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			update = Builders<BsonDocument>.Update.Set("date", DateTime.Now);
			Collection.UpdateOne(filter, update);
			filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			update = Builders<BsonDocument>.Update.Set("time", time);
			Collection.UpdateOne(filter, update);
		}
		public static void Insert(ref string currentUser)
		{
			Console.WriteLine();
			Connect();
			do
			{
				Console.Write("Enter your username: ");
				currentUser = Console.ReadLine();
				if (IsPresent(currentUser)) Console.WriteLine($"{currentUser} already exist");
				else if (currentUser.Length > 12) Console.WriteLine("The maximum length is 12!!!");
				else if (currentUser.Length < 5) Console.WriteLine("The minimum length is 5");
				else break;
			} while (true);

			Collection.InsertOne(new BsonDocument
			{
				{ "username", currentUser },
				{ "score", 0 },
				{ "date", DateTime.Now },
				{ "time", 0 }
			});
		}
		public static void Delete(string currentUser)
		{
			Connect();
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			Collection.DeleteOne(filter);
		}
		public static void Clear()
		{
			Connect();
			var documents = AllDoc();
			foreach(var item in documents)
			{
				var filter = Builders<BsonDocument>.Filter.Eq("score", 0);
				Collection.DeleteOne(filter);
			}
		}
		public static void DrawClassification()
		{
			Connect();
			Graphic.Clear(0, 0);
			Console.SetWindowSize(140, 50);
			Console.SetWindowPosition(0, 0);
			Console.SetWindowSize(140, 50);
			Graphic.Word(0, 0, "Classification", 1);
			var users = AllDoc();
			Graphic.Rect(0, 4, "Username", setBG: false, fg: ConsoleColor.White);
			Graphic.Rect(12, 4, "Score", setBG: false, fg: ConsoleColor.White);
			Graphic.Rect(20, 4, "Last game", setBG: false, fg: ConsoleColor.White);
			for (int i = 0; i < users.Count; i++)
			{
				Graphic.Rect(0, 6 + i, i.ToString("00"), fg: ConsoleColor.White);
				Graphic.Rect(2, 6 + i, users[users.Count - 1 - i]["username"].ToString(), setBG: false, fg:ConsoleColor.White);
				Graphic.Rect(14, 6 + i, users[users.Count - 1 - i]["score"].ToString(), setBG: false, fg: ConsoleColor.White);
				Graphic.Rect(22, 6 + i, users[users.Count - 1 - i]["time"].ToString(), setBG: false, fg: ConsoleColor.White);
				Graphic.Rect(22, 6 + i, Convert.ToDateTime(users[users.Count - 1 - i]["date"]).ToString("HH:mm - dd/MM/yy"), setBG: false, fg: ConsoleColor.White);
			}
		}
	}
}
