using System;
using System.IO;

namespace ASLSitesClient
{
	public static class Paths
	{
		private static char sep = Path.DirectorySeparatorChar;
		public static string Base {
			get {
				string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				return Path.Combine(AppData, "ASLSitesClient");
			}
		}
		public static string FFMPEG {
			get {
				return Path.Combine(Base, "ffmpeg.exe");
			}
		}
		public static string ASLProDirectory {
			get {
				return Path.Combine(Base, "aslpro") + sep;
			}
		}
		public static string ASLProDatabase {
			get {
				return ASLProDirectory + "database.txt";
			}
		}
		public static string SigningSavvyDirectory {
			get {
				return Path.Combine(Base, "signingsavvy") + sep;
			}
		}
		public static string SigningSavvyDatabase {
			get {
				return SigningSavvyDirectory + "database.txt";
			}
		}
	}
}

