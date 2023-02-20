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
        public static string Guest_Path = "F:\\Users\\Public\\sharedFile";
        public static void Initialize_Set(String[,] map)
        {
            try
            {
                using (FileStream stream = new FileStream(Host_Path,FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    StreamWriter sharedFile = new StreamWriter(Host_Path);

                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            sharedFile.WriteLine(map[i, j]);
                        }
                    }

                    sharedFile.Close();
                }
            }
            catch (IOException)
            {
                Striker_Finale.Striker.LM_Game();
            }            
        }

        public static void Initialize_Get(String[,] map)
        {
            try
            {
                using (FileStream stream = new FileStream(Host_Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {

                    StreamReader sharedFile = new StreamReader(Guest_Path); //percorso unità condivisa

                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            map[i, j] = sharedFile.ReadLine().ToString();
                        }
                    }

                    sharedFile.Close();
                }
            }
            catch (IOException)
            {
                Striker_Finale.Striker.LM_Game();
            }
        }

        public static void Update(int x, int y)
        {
            try
            {
                using (FileStream stream = new FileStream(Host_Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    StreamWriter sharedFile = new StreamWriter(Host_Path);

                    sharedFile.WriteLine(x);
                    sharedFile.WriteLine(y);

                    sharedFile.Close();
                }
            }
            catch (IOException)
            {
                Striker_Finale.Striker.LM_Game();
            }
        }

        public static void Enemy_Update(String[,] map)
        {
            bool libero = false;
            do
            {
                int[] pos = Download_Position();
                if (pos[0] != -1)
                {
                    libero = true;
                    map[Find_Enem(map)[0], Find_Enem(map)[1]] = "E";
                    map[pos[0], pos[1]] = "Enem";
                }
                else
                {
                    libero = false;
                }
            }
            while (!libero);
            
        }

        static int[] Find_Enem(String[,] map)
        {
            int[] position = { -1, -1 };
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (map[i, j] == "Enem") position[0] = i; position[1] = j;
                }
            }
            return position;            
        }

        static int[] Download_Position()
        {
            try
            {
                using (FileStream stream = new FileStream(Host_Path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    int[] position = new int[2];

                    StreamReader sharedFile = new StreamReader(Guest_Path);
                    position[0] = Convert.ToInt32(sharedFile.ReadLine());
                    position[1] = Convert.ToInt32(sharedFile.ReadLine());

                    return position;
                }
            }
            catch (IOException)
            {
                int[] ritorna = { -1, -1 };
                return ritorna;
            }            
        }
    }
}
