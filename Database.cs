using MongoDB.Driver;
using MongoDB.Bson;// To write in the cluster
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Collections.ObjectModel;
using System.Linq;

namespace Striker
{
	class Database
	{
		static IMongoCollection<BsonDocument> Collection;
		static MongoClient Client;
		public static void Connect(string collection)
		{
			var settings = MongoClientSettings.FromConnectionString("mongodb+srv://marco0019:bamboccetti@striker.pq8k4qv.mongodb.net/test");
			settings.ServerApi = new ServerApi(ServerApiVersion.V1);
			Client = new MongoClient(settings);
			Collection = Client.GetDatabase("Striker").GetCollection<BsonDocument>(collection);
		}
		public static List<BsonDocument> AllDoc(string collection)
		{
			Connect(collection);
			return Collection.Find(new BsonDocument()).ToList();
		}
		public static bool IsPresent(string currentUser)
		{
			Connect("classification");
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			return Collection.Find(filter).ToList().Count != 0;
		}
		public static BsonDocument Read(string currentUser)
		{
			Connect("classification");
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			return Collection.Find(filter).FirstOrDefault();
		}
		public static void Update(string currentUser, int highscore, long time)
		{
			Connect("classification");
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
		public static void SetInitialMap(string[,] map, int width, int height)
		{
			var obs = AllDoc("obstacles");
			foreach(var item in obs)map[Convert.ToInt16(item["position"][1]), Convert.ToInt16(item["position"][0])] = "Obs";
		}
		public static void Login(ref string currentUser)
		{
			Connect("classification");
			Graphic.SetWindowSize(50, 20);
			Graphic.Draw_Frame(20, 10, 5, 5, setBG:false);
			Graphic.Word(15, 6, "login", 1);
			Graphic.Rect(7, 9, "Username: ", fg:ConsoleColor.White, setBG: false, size: 1);
			Graphic.Rect(7, 11, "Password: ", fg: ConsoleColor.White, setBG: false, size: 1);
			string password = "";
			do
			{
				Console.CursorVisible = true;
				Console.SetCursorPosition(17, 9);
				currentUser = Console.ReadLine();
				Console.SetCursorPosition(17, 11);
				password = Console.ReadLine();
				Console.SetCursorPosition(7, 13);
				if (Read(currentUser) == null) Console.WriteLine("Error 404: Account not found!");
				else if (Read(currentUser)["password"] == password) break;
				else Console.WriteLine("Password or username is invalid! Please enter the correct username or password!!!");
				Console.SetCursorPosition(17, 9);
				Console.Write("                  ");
				Console.SetCursorPosition(17, 11);
				Console.Write("                  ");
			} while (true);
		}
		public static void Register(ref string currentUser)
		{

			Connect("classification");
			Graphic.SetWindowSize(50, 20);
			Graphic.Draw_Frame(20, 10, 5, 5, setBG: false);
			Graphic.Word(15, 6, "register", 1);
			Graphic.Rect(7, 9, "Username: ", fg: ConsoleColor.White, setBG: false, size: 1);
			Graphic.Rect(7, 11, "Password: ", fg: ConsoleColor.White, setBG: false, size: 1);
			string password = "";
			do
			{
				Console.SetCursorPosition(17, 9);
				currentUser = Console.ReadLine();
				Console.WriteLine();
				Console.SetCursorPosition(17, 11);
				password = Console.ReadLine();
				if (IsPresent(currentUser)) Console.WriteLine($"{currentUser} already exist");
				else if (currentUser.Length > 12 | password.Length > 12) Console.WriteLine("The maximum length is 12!!!");
				else if (currentUser.Length < 5 | password.Length < 5) Console.WriteLine("The minimum length is 5");
				else break;
				Console.SetCursorPosition(17, 9);
				Console.Write("                  ");
				Console.SetCursorPosition(17, 11);
				Console.Write("                  ");
			} while (true);

			Collection.InsertOne(new BsonDocument
			{
				{ "username", currentUser },
				{ "password", password },
				{ "score", 0 },
				{ "date", DateTime.Now },
				{ "time", 0 },
				{ "allShot", 0 },
				{ "shotMissed", 0 }
			});
		}
		public static void Delete(string collection, string currentUser)
		{
			Connect(collection);
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			Collection.DeleteOne(filter);
		}
		public static void DrawClassification()
		{
			Connect("classification");
			Graphic.Clear(0, 0);
			Console.SetWindowSize(140, 50);
			Console.SetWindowPosition(0, 0);
			Console.SetWindowSize(140, 50);
			Graphic.Word(0, 0, "Classification", 1);
			var users = AllDoc("classification");
			Graphic.Rect(0, 4, "Username", setBG: false, fg: ConsoleColor.White);
			Graphic.Rect(12, 4, "Score", setBG: false, fg: ConsoleColor.White);
			Graphic.Rect(22, 4, "Time", setBG: false, fg: ConsoleColor.White);
			Graphic.Rect(28, 4, "Last game", setBG: false, fg: ConsoleColor.White);
			for (int i = 0; i < users.Count; i++)
			{
				Graphic.Rect(0, 6 + i, (1 + i).ToString(" 0"), fg: ConsoleColor.White);
				Graphic.Rect(2, 6 + i, users[users.Count - 1 - i]["username"].ToString(), setBG: false, fg:ConsoleColor.White);
				Graphic.Rect(12, 6 + i, users[users.Count - 1 - i]["score"].ToString(), setBG: false, fg: ConsoleColor.White);
				Graphic.Rect(20, 6 + i, (Convert.ToInt16(users[users.Count - 1 - i]["allShot"]) / Convert.ToDouble(users[users.Count - 1 - i]["allShot"]) * 100).ToString("0.0"), fg: ConsoleColor.White);
				Graphic.Rect(26, 6 + i, (Convert.ToDouble(users[users.Count - 1 - i]["time"]) / Convert.ToDouble(1000)).ToString("0.00s"), setBG: false, fg: ConsoleColor.White);
				Graphic.Rect(32, 6 + i, Convert.ToDateTime(users[users.Count - 1 - i]["date"]).ToString("HH:mm - dd/MM/yy"), setBG: false, fg: ConsoleColor.White);
			}
			Console.SetWindowSize(140, 50);
			Console.SetWindowPosition(0, 0);
			Console.SetWindowSize(140, 50);
		}
		public static void InsertObs(string[,] map, int width, int height)
		{
			Graphic.Initialize_Map(map);
			Graphic.Draw_Obstacles_Randomly(map);
			Connect("obstacles");
			List<List<int>> list = GetObstacles(map,width, height );
			foreach(List<int> item in list)
			{
				Collection.InsertOne(new BsonDocument
				{
					{"position", new BsonArray(item) }
				});
			}
		}
		static List<List<int>> GetObstacles(string[,] map, int width, int height)
		{
			List<List<int>> list = new List<List<int>>();
			for(int i = 0; i < height; i++)
				for(int j = 0; j < width; j++)
					if (map[i, j] == "Obs")list.Add(new List<int> { j, i});
			return list;
		}
		public static void UpdatePlayers(string[,] map, int width, int height)
		{
			var players = AllDoc("multiplayer");
			ClearMap(map, width, height);
			foreach(var player in players)
			{
				map[Convert.ToInt16(player["posY"]), Convert.ToInt16(player["posX"])] = "Pl";
				for (int i = 0; i < player["shotsY"].ToBson().Length; i++)
					map[Convert.ToInt16(player["shotsY"][i]), Convert.ToInt16(player["shotsX"][i])] = "Sh";
			}
		}
		public static void ClearMap(string[,] map, int width, int height){
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++) if (map[i, j] == "Pl" | map[i, j] == "Sh") map[i, j] = "E";
		}
		public static void Insert(string currentUser, Player player)
		{
			Connect("multiplayer");
			Collection.InsertOne(new BsonDocument
			{
				{"user", currentUser },
				{"posX", player.Position[0] },
				{"posY", player.Position[1] },
				{"shotsX", new BsonArray(SetShotPosition(player, 0)) },
				{"shotsY", new BsonArray(SetShotPosition(player, 1)) }
			});
		}
		public static void Update(string currentUser, Player player)
		{
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			var update = Builders<BsonDocument>.Update.Set("posX", player.Position[0]);
			Collection.UpdateOne(filter, update);
			update = Builders<BsonDocument>.Update.Set("posY", player.Position[1]);
			Collection.UpdateOne(filter, update);
			update = Builders<BsonDocument>.Update.Set("shotsX", new BsonArray(SetShotPosition(player, 0)));
			Collection.UpdateOne(filter, update);
			update = Builders<BsonDocument>.Update.Set("shotsY", new BsonArray(SetShotPosition(player, 1)));
			Collection.UpdateOne(filter, update);
		}
		public static void Lobby(string[,] map, int width, int height, string currentUser, Player player)
		{
			Insert(currentUser, player);
			var players = AllDoc("multiplayer");
			if (players.Count == 1)
			{
				Graphic.Initialize_Map(map);
				Graphic.Draw_Obstacles_Randomly(map);
			}
			else SetInitialMap(map, width, height);
			Stopwatch time = new Stopwatch();
			time.Start();
			while (AllDoc("multiplayer").Count < 2 | time.ElapsedMilliseconds / 1000 < 10)
			{
				Console.SetCursorPosition(0, 0);
				Console.Write(time.ElapsedMilliseconds);
			}
		}
		static List<int> SetShotPosition(Player player, int index)
		{
			List<int> ints = new List<int>();
			for(int i = 0; i < player.Shots.Count; i++)
			{
				ints.Add(player.Shots[i].Position[index]);
			}
			return ints;
		}
	}
}
