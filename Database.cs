using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Striker_Finale
{
	class Database
	{
		static IMongoCollection<BsonDocument> Collection;
		static MongoClient Client;
		public static void TurnOn()
		{
			var settings = MongoClientSettings.FromConnectionString("mongodb+srv://marco0019:bamboccetti@striker.pq8k4qv.mongodb.net/test");
			settings.ServerApi = new ServerApi(ServerApiVersion.V1);
			Client = new MongoClient(settings);
		}
		public static void Connect(string collection) => Collection = Client.GetDatabase("Striker").GetCollection<BsonDocument>(collection);
		public static List<BsonDocument> AllDoc(string collection) => Client.GetDatabase("Striker").GetCollection<BsonDocument>(collection).Find(new BsonDocument()).ToList();
		public static bool IsPresent(string currentUser)
		{
			Connect("users");
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			return Collection.Find(filter).ToList().Count != 0;
		}
		public static BsonDocument GetUser(string currentUser)
		{
			Connect("users");
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			return Collection.Find(filter).FirstOrDefault();
		}
		public static void Update(string currentUser, int highscore, long time)
		{
			Connect("users");
			var filter = Builders<BsonDocument>.Filter.Eq("username", currentUser);
			Collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("score", highscore));
			Collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("date", DateTime.Now));
			Collection.UpdateOne(filter, Builders<BsonDocument>.Update.Set("time", time));
		}
		public static void Login(ref string currentUser)
		{
			Connect("users");
			Graphic.WindowSize(50, 20);
			Graphic.Draw_Frame(20, 10, 5, 5, setBG: false);
			Graphic.Word(15, 6, "login", 1);
			Graphic.Rect(7, 9, "Username: ", fg: ConsoleColor.White, setBG: false, size: 1);
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
				if (GetUser(currentUser) == null) Console.WriteLine("Error 404: Account not found!");
				else if (GetUser(currentUser)["password"] == password) break;
				else Console.WriteLine("Password or username is invalid! Please enter the correct username or password!!!");
				Console.SetCursorPosition(17, 9);
				Console.Write("                  ");
				Console.SetCursorPosition(17, 11);
				Console.Write("                  ");
			} while (true);
			Graphic.WindowSize(150, 70);
			Graphic.Clear();
		}
		public static void Register(ref string currentUser)
		{

			Connect("classification");
			Graphic.WindowSize(100, 20);
			Graphic.Draw_Frame(20, 10, 5, 5, setBG: false);
			Graphic.Word(10, 6, "register", 1);
			Graphic.Rect(7, 9, "Username: ", fg: ConsoleColor.White, setBG: false, size: 1);
			Graphic.Rect(7, 11, "Password: ", fg: ConsoleColor.White, setBG: false, size: 1);
			string password = "";
			Console.CursorVisible = true;
			do
			{
				Console.SetCursorPosition(17, 9);
				currentUser = Console.ReadLine();
				Console.WriteLine();
				Console.SetCursorPosition(17, 11);
				password = Console.ReadLine();
				if (IsPresent(currentUser))
				{
					Console.SetCursorPosition(13, 12);
					Console.WriteLine($"{currentUser} already exist");
					Console.SetCursorPosition(12, 13);
					Console.Write("You want to login?  Y/N");
					if (Console.ReadKey().Key == ConsoleKey.Y)
					{
						Console.Clear();
						Login(ref currentUser);
						return;
					}
				}
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
			Graphic.Clear();
			Graphic.WindowSize(150, 70);
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
				Graphic.Rect(2, 6 + i, users[users.Count - 1 - i]["username"].ToString(), setBG: false, fg: ConsoleColor.White);
				Graphic.Rect(12, 6 + i, users[users.Count - 1 - i]["score"].ToString(), setBG: false, fg: ConsoleColor.White);
				Graphic.Rect(20, 6 + i, (Convert.ToInt16(users[users.Count - 1 - i]["allShot"]) / Convert.ToDouble(users[users.Count - 1 - i]["allShot"]) * 100).ToString("0.0"), fg: ConsoleColor.White);
				Graphic.Rect(26, 6 + i, (Convert.ToDouble(users[users.Count - 1 - i]["time"]) / Convert.ToDouble(1000)).ToString("0.00s"), setBG: false, fg: ConsoleColor.White);
				Graphic.Rect(32, 6 + i, Convert.ToDateTime(users[users.Count - 1 - i]["date"]).ToString("HH:mm - dd/MM/yy"), setBG: false, fg: ConsoleColor.White);
			}
			Console.SetWindowSize(140, 50);
			Console.SetWindowPosition(0, 0);
			Console.SetWindowSize(140, 50);
		}
		public static void Insert(string currentUser, Player player)
		{
			Connect("multiplayer");
			Collection.InsertOne(new BsonDocument
			{
				{"user", currentUser },
				{"life", player.Life },
				{"kills", player.Kills },
				{"score", player.Score },
				{"posX", player.Position[0] },
				{"posY", player.Position[1] },
				{"shotsX", new BsonArray(SetShotPosition(player, 0)) },
				{"shotsY", new BsonArray(SetShotPosition(player, 1)) }
			});
		}
		public static void Clear(string collection) => Client.GetDatabase("Striker").GetCollection<BsonDocument>(collection).DeleteMany(new BsonDocument() { });
		public static void InsertObs(string[,] map, int width, int height)
		{
			Connect("obstacles");
			Clear("obstacles");
			Graphic.Initialize_Map(map);
			Graphic.Draw_Obstacles_Randomly(map);
			List<List<int>> list = GetObstacles(map, width, height);
			foreach (List<int> item in list)
				Collection.InsertOne(new BsonDocument
				{
					{"x", item[0] },
					{"y", item[1] }
				});
		}
		static List<List<int>> GetObstacles(string[,] map, int width, int height)
		{
			List<List<int>> list = new List<List<int>>();
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					if (map[i, j] == "Obs") list.Add(new List<int> { j, i });
			return list;
		}
		public static void UpdateMap(string[,] map, int width, int height, string currentUser, Player currentPlayer)
		{
			var players = AllDoc("multiplayer");
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
					if (map[j, i] == "Pl" | map[j, i] == "Sh") map[j, i] = "E";

			foreach(var player in players)
			{
				if (player["user"] == currentUser)map[Convert.ToInt16(player["posY"]), Convert.ToInt16(player["posX"])] = "Pl";
				else map[Convert.ToInt16(player["posY"]), Convert.ToInt16(player["posX"])] = "Enem";
				for (int i = 0; i < player["shotsX"].AsBsonArray.Count; i++)
					try
					{
						if (map[Convert.ToInt16(player["shotsY"][i]), Convert.ToInt16(player["shotsX"][i])] == "E" )map[Convert.ToInt16(player["shotsY"][i]), Convert.ToInt16(player["shotsX"][i])] = "Sh";
						if (player["user"] != currentUser)
							if (currentPlayer.Position[0] == Convert.ToInt16(player["shotsX"][i]) & currentPlayer.Position[1] == Convert.ToInt16(player["shotsY"][i])) { currentPlayer.Life--; Graphic.Draw_Life_Bar(currentPlayer.Life);}
					}
					catch(Exception ex) { }
			}
		}
		public static void DeletePlayer(string currentUser)
		{
			Connect("multiplayer");
			var filter = Builders<BsonDocument>.Filter.Eq("user", currentUser);
			Collection.DeleteOne(filter);
		}
		public static void Update(string currentUser, Player player) => Client.GetDatabase("Striker").GetCollection<BsonDocument>("multiplayer").ReplaceOne(Builders<BsonDocument>.Filter.Eq("user", currentUser), new BsonDocument
		{
			{"user", currentUser },
			{"life", player.Life },
			{"kills", player.Kills },
			{"score", player.Score },
			{"posX", player.Position[0] },
			{"posY", player.Position[1] },
			{"shotsX", new BsonArray(SetShotPosition(player, 0)) },
			{"shotsY", new BsonArray(SetShotPosition(player, 1)) }
		});
		static List<int> SetShotPosition(Player player, int index)
		{
			List<int> ints = new List<int>();
			for (int i = 0; i < player.Shots.Count; i++)
				ints.Add(player.Shots[i].Position[index]);
			return ints;
		}
		public static void Lobby(string[,] map, int width, int height, string currentUser, Player player)
		{
			Graphic.Initialize_Map(map);
			Chat(currentUser);
			Insert(currentUser, player);
			if (AllDoc("multiplayer").Count == 1)
				InsertObs(map, width, height);
			else foreach (var item in AllDoc("obstacles")) map[Convert.ToInt16(item["y"]), Convert.ToInt16(item["x"])] = "Obs";
			while (AllDoc("multiplayer").Count < 2 & AllDoc("multiplayer").Count < 11) Console.WriteLine(AllDoc("multiplayer").Count);
		}
		public static void DrawClassification(int x, int y, string currentUser)
		{
			// 11 righe, le prime dieci posizioni più la tua posizione attuale
			Graphic.Rect(x + 6, y, "USER", setBG: false, fg: ConsoleColor.White, size: 1);
			Graphic.Rect(x + 20, y, "LIFE", setBG: false, fg: ConsoleColor.White, size: 1);
			Graphic.Rect(x + 32, y, "KILLS", setBG: false, fg: ConsoleColor.White, size: 1);
			Graphic.Rect(x + 40, y, "SCORE", setBG: false, fg: ConsoleColor.White, size: 1);
			var players = AllDoc("multiplayer");
			players.Sort();
			bool podio = false;
			for(int i = 0; i < players.Count & i < 10; i++)
			{
				if (currentUser == players[i]["user"].ToString()) podio = true;
				if (podio) Graphic.Rect(x + 1, y + i * 2 + 2, "                                            ", bg: ConsoleColor.White, size: 1);
				Graphic.Rect(x + 1, y + i * 2 + 2, "#" + (i + 1).ToString("00"), setBG: podio, fg: !podio ? ConsoleColor.White : ConsoleColor.Black, bg: podio ? ConsoleColor.White : ConsoleColor.Black, size: 1);
				Graphic.Rect(x + 6, y + i * 2 + 2, players[i]["user"].ToString() == currentUser ? "YOU" : players[i]["user"].ToString(), setBG: podio, fg: !podio ? ConsoleColor.White : ConsoleColor.Black, bg: podio ? ConsoleColor.White : ConsoleColor.Black, size: 1);
				Graphic.Draw_Life_Classification(x + 20, y + i * 2 + 2, Convert.ToInt16(players[i]["life"]));
				Graphic.Rect(x + 32, y + i * 2 + 2, players[i]["kills"].ToString(), setBG: podio, fg: !podio ? ConsoleColor.White : ConsoleColor.Black, bg: podio ? ConsoleColor.White : ConsoleColor.Black, size: 1);
				Graphic.Rect(x + 40, y + i * 2 + 2, players[i]["score"].ToString(), setBG: podio, fg: !podio ? ConsoleColor.White : ConsoleColor.Black, bg: podio ? ConsoleColor.White : ConsoleColor.Black, size: 1);
			}
		}
		public static void InsertMessage(string currentUser, string content)
		{
			Connect("chat");
			Collection.InsertOne(new BsonDocument
			{
				{"user", currentUser },
				{"content", content },
				{"date", DateTime.Now},
			});
		}
		public static void Chat(string currentUser)
		{
			Connect("chat");
			var messages = Collection.Find(new BsonDocument() { }).ToList();
			Graphic.Clear(8, 29, 71, 31);
			Graphic.Draw_Frame(40, 3, 60, 10,  fore:ConsoleColor.White, back: ConsoleColor.Black, false);
			Graphic.Rect(11, 61, "                                                                              ", bg: ConsoleColor.White, fg: ConsoleColor.White, size: 1, 0, 0, false);
			bool isUser;
			for(int i = 0; i < messages.Count & i < 10; i++)
			{
				isUser = messages[messages.Count - 1 - i]["user"].ToString() == currentUser;
				int x = isUser ? 71 - (messages[messages.Count - 1 - i]["user"].ToString().Length + messages[messages.Count - 1 - i]["content"].ToString().Length) : 8;

				Graphic.Draw_Frame(((isUser ? 5 : messages[messages.Count - 1 - i]["user"].ToString().Length + 2) + messages[messages.Count - 1 - i]["content"].ToString().Length) / 2 + 5,3, 29 + (9 - i) * 3, x,  setBG:false, fore: isUser ? ConsoleColor.Yellow : ConsoleColor.White);
				Graphic.Rect(x + 1, 30 + (9 - i) * 3, (isUser ? "You" : messages[messages.Count - 1 - i]["user"]) + ": " + messages[messages.Count - 1 - i]["content"].ToString() + "   " + Convert.ToDateTime(messages[messages.Count - 1 - i]["date"]).ToString("h:mm"), setBG: false, size: 1, fg: isUser ? ConsoleColor.Yellow : ConsoleColor.White);
			}
		}
	}
}
