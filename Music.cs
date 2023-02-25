using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Media;

namespace Striker_Finale
{
    public class Music
    {

        static string cartella = Directory.GetCurrentDirectory();
        static string file = cartella + "\\src\\sottofondo.wav";
        static SoundPlayer sottofondo = new SoundPlayer(file);
        public static void Title()
        {
            var file = cartella + "\\src\\smb_coin.wav";
            SoundPlayer sottofondo = new SoundPlayer(file);
            sottofondo.Play();
        }
        public static void SoundTrack()
        {
            sottofondo.Play();
        }
        public static void SoundTrack(bool stop)
        {
            sottofondo.Stop();
        }
        public static void Shoot()
        {
            var file = cartella + "\\src\\shoot.wav";
            SoundPlayer sottofondo = new SoundPlayer(file);
            sottofondo.Play();
        }
        public static void GameOver()
        {
            var file = cartella + "\\src\\gameover.wav";
            SoundPlayer sottofondo = new SoundPlayer(file);
            sottofondo.Play();
        }
		public static void Sound(string file)=> new SoundPlayer($"{Directory.GetCurrentDirectory()}\\src\\{file}.wav").Play();
        public static void level()
        {
            var file = cartella + "\\src\\level.wav";
            SoundPlayer sottofondo = new SoundPlayer(file);
            sottofondo.Play();
        }
        public static void Error()
        {
            var file = cartella + "\\src\\Error.wav";
            SoundPlayer sottofondo = new SoundPlayer(file);
            sottofondo.Play();
        }
    }
}
