using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Striker_finale
{
    class MultiplayerLocale
    {
        public static int Width;
        public static int Height;
        public static string Host_Path = "C:\\Users\\Public\\sharedFile";
        public static string Guest_Path = "C:\\Users\\Public\\sharedFile";
        public static void Initialize_Set(String[,] map)
        {
            StreamWriter sharedFile = new StreamWriter(Host_Path);

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    sharedFile.WriteLine(map[i,j]);
                }
            }

            sharedFile.Close();
        }

        public static void Initialize_Get(String[,] map)
        {
            StreamReader sharedFile = new StreamReader(Guest_Path); //percorso unità condivisa

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    map[i,j] = sharedFile.ReadLine().ToString();
                }
            }

            sharedFile.Close();
        }

        public static void Update(int x, int y)
        {
            StreamWriter sharedFile = new StreamWriter(Host_Path);

            sharedFile.WriteLine(x);
            sharedFile.WriteLine(y);

            sharedFile.Close();
        }

        public static int[] Download()
        {
            int[] position = new int[2];

            StreamReader sharedFile = new StreamReader(Guest_Path);
            position[0] = Convert.ToInt32(sharedFile.ReadLine());
            position[1] = Convert.ToInt32(sharedFile.ReadLine());

            return position;
        }
    }
}
