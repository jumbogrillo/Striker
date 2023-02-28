using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

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
		public static void Insert(string currentUser, int score, double accuracy, long time)
		{
			Connect("classification");
			Collection.InsertOne(new BsonDocument
			{
				{"username", currentUser},
				{"score", score },
				{"accuracy", accuracy },
				{"time",  time},
				{"date", DateTime.Now}
			});
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
			Connect("users");
			Graphic.WindowSize(50, 20);
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
				{ "password", password }
			});
			Graphic.Clear();
			Graphic.WindowSize(150, 70);
		}
		public static void DrawClassification()
		{
			Graphic.WindowSize(140, 70);
			Graphic.Clear(0, 0);
			var users = Client.GetDatabase("Striker").GetCollection<BsonDocument>("classification").Find(new BsonDocument()).ToList();
			Graphic.Draw_Frame(65, users.Count + 4, 0, 0, setBG: false);
			Graphic.Word(0, 1, "Classification", 1);
			int[] indexes = SortPlayers(users);
			Graphic.Rect(1, 5, "Username", setBG: false, fg: ConsoleColor.White);
			Graphic.Rect(13, 5, "Score", setBG: false, fg: ConsoleColor.White);
			Graphic.Rect(21, 5, "Accuracy", setBG: false, fg: ConsoleColor.White);
			Graphic.Rect(27, 5, "Time", setBG: false, fg: ConsoleColor.White);
			Graphic.Rect(33, 5, "Date", setBG: false, fg: ConsoleColor.White);
			for (int i = 0; i < users.Count; i++)
			{
				Graphic.Rect(1, 7 + i, (1 + i).ToString(" 0"), fg: ConsoleColor.White);
				Graphic.Rect(3, 7 + i, users[indexes[i]]["username"].ToString(), setBG: false, fg: ConsoleColor.White);
				Graphic.Rect(13, 7 + i, users[indexes[i]]["score"].ToString(), setBG: false, fg: ConsoleColor.White);
				Graphic.Rect(21, 7 + i, Convert.ToDouble(users[indexes[i]]["accuracy"]).ToString("0.00") + "%", fg: ConsoleColor.White);
				Graphic.Rect(27, 7 + i, (Convert.ToDouble(users[indexes[i]]["time"]) / Convert.ToDouble(1000)).ToString("0.00s"), setBG: false, fg: ConsoleColor.White);
				Graphic.Rect(33, 7 + i, Convert.ToDateTime(users[indexes[i]]["date"]).ToString("HH:mm - dd/MM/yy"), setBG: false, fg: ConsoleColor.White);
			}
			Graphic.WindowSize(140, 70);
		}
		public static void Insert(string currentUser, Player player)
		{
			Connect("multiplayer");
			Collection.InsertOne(new BsonDocument
			{
				{"user", currentUser },
				{"life", player.Life },
				{"kills", 0 },
				{"score", player.Score },
				{"posX", player.Position[0] },
				{"posY", player.Position[1] },
				{"shotsX", new BsonArray(SetShotPosition(player, 0)) },
				{"shotsY", new BsonArray(SetShotPosition(player, 1)) }
			});
		}
		public static void Clear(string collection) => Client.GetDatabase("Striker").GetCollection<BsonDocument>(collection).DeleteMany(Builders<BsonDocument>.Filter.Empty);
		public static void InsertObs(string[,] map, int width, int height)
		{
			Clear("obstacles");
			Connect("obstacles");
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
		public static void UpdateMap(string[,] map, int width, int height, string currentUser, Player currentPlayer, ref bool winner)
		{
			var players = AllDoc("multiplayer");
			if (players.Count == 1) winner = true;
			else
			{
				DrawClassification(87, 5, currentUser, players);
				for (int i = 0; i < width; i++)
					for (int j = 0; j < height; j++)
						if (map[j, i] == "Pl" | map[j, i] == "Sh" | map[j, i] == "Enem") map[j, i] = "E";
				foreach(var player in players)
				{
					if (player["user"] == currentUser)map[Convert.ToInt16(player["posY"]), Convert.ToInt16(player["posX"])] = "Pl";
					else map[Convert.ToInt16(player["posY"]), Convert.ToInt16(player["posX"])] = "Enem";
					for (int i = 0; i < player["shotsX"].AsBsonArray.Count; i++)
						try
						{
							if (map[Convert.ToInt16(player["shotsY"][i]), Convert.ToInt16(player["shotsX"][i])] == "E" )map[Convert.ToInt16(player["shotsY"][i]), Convert.ToInt16(player["shotsX"][i])] = "Sh";
							if (player["user"] != currentUser)
								if (currentPlayer.Position[0] == Convert.ToInt16(player["shotsX"][i]) & currentPlayer.Position[1] == Convert.ToInt16(player["shotsY"][i]))
								{
									currentPlayer.Life--;
									Graphic.Draw_Life_Bar(currentPlayer.Life);
									if (currentPlayer.Life <= 0) UpdateEnemy(player["user"].ToString(), Convert.ToInt16(player["kills"]) + 1);
								}
						}
						catch(Exception ex) { }
				}
			}
		}
		public static void UpdateEnemy(string user, int kills) => Client.GetDatabase("Striker").GetCollection<BsonDocument>("multiplayer").UpdateOne(Builders<BsonDocument>.Filter.Eq("user", user), Builders<BsonDocument>.Update.Set("kills", kills));
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
			{"kills", Client.GetDatabase("Striker").GetCollection<BsonDocument>("multiplayer").Find(Builders<BsonDocument>.Filter.Eq("user", currentUser)).FirstOrDefault()["kills"] },
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
			if (AllDoc("multiplayer").Count < 11)
			{
				Graphic.Initialize_Map(map);
				Insert(currentUser, player);
				if (AllDoc("multiplayer").Count < 2)
					InsertObs(map, width, height);// Insert the obstacles into DB
				else foreach (var item in AllDoc("obstacles")) map[Convert.ToInt16(item["y"]), Convert.ToInt16(item["x"])] = "Obs";// insert the obstacles into the map
				Stopwatch clock = new Stopwatch();
				clock.Start();
				while (AllDoc("multiplayer").Count < 2) Graphic.Draw_Progress_Bar(4, 4, (AllDoc("multiplayer").Count - 1) * 50 + (clock.ElapsedMilliseconds / Convert.ToDouble(1200) > 50 ? 50 : clock.ElapsedMilliseconds / Convert.ToDouble(1200)), 100);
				Graphic.Draw_Progress_Bar(4, 4, (AllDoc("multiplayer").Count - 1) * 50 + (clock.ElapsedMilliseconds / Convert.ToDouble(1200) > 50 ? 50 : clock.ElapsedMilliseconds / Convert.ToDouble(1200)), 100);
				Thread.Sleep(1000);
				Graphic.Clear(1);
				Chat(currentUser);
				DrawClassification(87, 5, currentUser, AllDoc("multiplayer"));
			}
			else
			{
				Graphic.Word(0, 0, "Too much player", 1);
				Graphic.Clear(1, 1);
				Striker.Main(new string[] { });
			}
		}
		public static int[] SortPlayers(List<BsonDocument> players)
		{
			int[] scores = new int[players.Count];
			int[] indexes = new int[players.Count];
			for (int i = 0; i < players.Count; i++)
			{
				scores[i] = Convert.ToInt16(players[i]["score"]);
				indexes[i] = i;
			}
			bool change = true;
			int cycle = 0;
			while (change)
			{
				change = false;
				for(int i = 0; i < indexes.Length - cycle - 1; i++)
				{
					if (scores[i] < scores[i + 1])
					{
						change = true;
						var score = scores[i];
						scores[i] = scores[i + 1];
						scores[i + 1] = score;
						score = indexes[i];
						indexes[i] = indexes[i + 1];
						indexes[i + 1] = score;
					}
				}
				cycle++;
			}
			return indexes;
		}
		public static void DrawClassification(int x, int y, string currentUser, List<BsonDocument> players)
		{
			Graphic.Rect(x + 7, y, "USER", setBG: false, fg: ConsoleColor.White, size: 1);
			Graphic.Rect(x + 21, y, "LIFE", setBG: false, fg: ConsoleColor.White, size: 1);
			Graphic.Rect(x + 33, y, "KILLS", setBG: false, fg: ConsoleColor.White, size: 1);
			Graphic.Rect(x + 41, y, "SCORE", setBG: false, fg: ConsoleColor.White, size: 1);
			bool isUser = false;
			var indexes = SortPlayers(players);
			int indexOfUser = -1;
			for(int i = 0; i < indexes.Length & indexOfUser == -1; i++)
				if (players[indexes[i]]["user"].ToString() == currentUser) indexOfUser = i;
			for(int i = 0; i < players.Count & i < 10; i++)
			{
				if (currentUser == players[indexes[i]]["user"].ToString()) isUser = true;
				else isUser = false;
				Graphic.Rect(x + 1, y + i * 2 + 2, "                                              ", bg: isUser ? ConsoleColor.Gray : ConsoleColor.Black, size: 1);
				Graphic.Rect(x + 2, y + i * 2 + 2, "#" + (i + 1).ToString("00"), fg: !isUser ? ConsoleColor.White : ConsoleColor.Black, bg: isUser ? ConsoleColor.Gray : ConsoleColor.Black, size: 1);
				Graphic.Rect(x + 7, y + i * 2 + 2, isUser ? "YOU" : players[indexes[i]]["user"].ToString(), fg: !isUser ? ConsoleColor.White : ConsoleColor.Black, bg: isUser ? ConsoleColor.Gray : ConsoleColor.Black, size: 1);
				Graphic.Draw_Life_Classification(x + 20, y + i * 2 + 2, Convert.ToInt16(players[indexes[i]]["life"]));
				Graphic.Rect(x + 37, y + i * 2 + 2, players[indexes[i]]["kills"].ToString(), fg: !isUser ? ConsoleColor.White : ConsoleColor.Black, bg: isUser ? ConsoleColor.Gray : ConsoleColor.Black, size: 1);
				Graphic.Rect(x + 46 - players[indexes[i]]["score"].ToString().Length, y + i * 2 + 2, players[indexes[i]]["score"].ToString(), fg: !isUser ? ConsoleColor.White : ConsoleColor.Black, bg: isUser ? ConsoleColor.Gray : ConsoleColor.Black, size: 1);
			}
			Graphic.Rect(x + 1, y + 23, "                                              ", bg: ConsoleColor.White, size: 1);
			Graphic.Rect(x + 2, y + 23, "#" + (indexOfUser + 1).ToString("00"), fg: ConsoleColor.Black, bg: ConsoleColor.White, size: 1);
			Graphic.Rect(x + 7, y + 23, isUser ? "YOU" : players[indexes[indexOfUser]]["user"].ToString(), fg: ConsoleColor.Black, bg: ConsoleColor.White, size: 1);
			Graphic.Draw_Life_Classification(x + 20, y + 23, Convert.ToInt16(players[indexes[indexOfUser]]["life"]));
			Graphic.Rect(x + 37, y + 23, players[indexes[indexOfUser]]["kills"].ToString(), fg: ConsoleColor.Black, bg: ConsoleColor.White, size: 1);
			Graphic.Rect(x + 46 - players[indexes[indexOfUser]]["score"].ToString().Length, y + 23, players[indexes[indexOfUser]]["score"].ToString(), fg: ConsoleColor.Black, bg: ConsoleColor.White, size: 1);
			if(players.Count < 10)
                Graphic.Rect(x + 1, y + (players.Count + 2) * 2, "                                              ", setBG:false, size: 1);
        }
		public static void InsertMessage(string currentUser, string content)
		{
			Console.CursorVisible = false;
			Connect("chat");
			Collection.InsertOne(new BsonDocument
			{
				{"user", currentUser },
				{"content", content.Length > 50 ? content.Substring(0, 47) + "..." : content },
				{"date", DateTime.Now},
			});
			Chat(currentUser);
		}
		public static void Chat(string currentUser)
		{
			var messages = (Client.GetDatabase("Striker").GetCollection<BsonDocument>("chat").Find(new BsonDocument() { })).ToList();
			Graphic.Draw_Frame(40, 3, 60, 10,  fore:ConsoleColor.White, back: ConsoleColor.Black, false);
			Graphic.Clear(11, 61, 78, 1);
			//Graphic.Clear(8, 29, 71, 30);
			bool isUser;
			for(int i = 0; i < messages.Count & i < 10; i++)
			{
				isUser = messages[messages.Count - 1 - i]["user"].ToString() == currentUser;
				int x = isUser ? 71 - (messages[messages.Count - 1 - i]["user"].ToString().Length + messages[messages.Count - 1 - i]["content"].ToString().Length) : 8;
				Graphic.Clear(8, 29 + (9 - i) * 3, 78, 3);
				Graphic.Draw_Frame(((isUser ? 5 : messages[messages.Count - 1 - i]["user"].ToString().Length + 2) + messages[messages.Count - 1 - i]["content"].ToString().Length) / Convert.ToDouble(2) + 6,3, 29 + (9 - i) * 3, x,  setBG:false, fore: isUser ? ConsoleColor.Yellow : ConsoleColor.White);
				Graphic.Rect(x + 1, 30 + (9 - i) * 3, (isUser ? "You" : messages[messages.Count - 1 - i]["user"]) + ": " + messages[messages.Count - 1 - i]["content"].ToString() + "    " + Convert.ToDateTime(messages[messages.Count - 1 - i]["date"]).ToString("HH:mm"), setBG: false, size: 1, fg: isUser ? ConsoleColor.Yellow : ConsoleColor.White);
			}
		}
	}
}
