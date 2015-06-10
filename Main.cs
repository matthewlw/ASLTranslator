using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace ASLSitesClient
{
	class MainClass
	{
		public static void Main ()
		{
			if (!Directory.Exists(Paths.Base)) {
				Directory.CreateDirectory(Paths.Base);
				Directory.CreateDirectory(Paths.ASLProDirectory);
				Directory.CreateDirectory(Paths.SigningSavvyDirectory);
				Console.WriteLine("Downloading FFMPEG (this may take a while)...");
				var client = new WebClient();
				client.DownloadFile(Strings.FFMPEG_URL, Paths.FFMPEG);
				client.Dispose();
				Console.WriteLine("FFMPEG has finished downloading.");
			}
			Dictionaries.AslPro = new ASLPro();
			Dictionaries.SigningSavvy = new SigningSavvy();
			string lastEntered;
			while(true) {
				DictionaryWord[] resolvedWords;
				Console.Write("What do you want to translate (leave blank to quit)? ");
				lastEntered = Console.ReadLine();
				if (lastEntered == "") break;
				if (lastEntered.ToUpper().Contains("database.txt")) {
					Console.WriteLine("I wouldn't do that if I were you...");
					continue;
				}
				string[] wordsToTranslate = WordResolver.Split(lastEntered);
				resolvedWords = Merge(wordsToTranslate.Select(word => WordResolver.Resolve(word)).ToArray());
				var downloader = new Downloader();
				Action<object, WordDownloadedEventArgs> handler = ((sender, e) => 
					Console.WriteLine (Strings.DOWNLOADED, e.Word.Word, e.Word.Dictionary));
				//MonoDevelop warns that the new expression is redundant. It is not.
				downloader.WordDownloaded += new Downloader.WordDownloadedEventHandler(handler);
				string outFile = downloader.Download(resolvedWords);
				Console.WriteLine("The video is saved at {0}", outFile);
				Console.Write("Would you like to play that now (y/n)? ");
				//LINUX-SPECIFIC
				if (Console.ReadLine() == "y") Process.Start("mplayer", outFile).WaitForExit();
			} 
		}
		
		public static string DisplayChoices(string message, string[] inputData) {
			Console.WriteLine (message);
			for (int i = 0 ; i < inputData.Length; i++) {
				Console.WriteLine("[{0}] {1}", i, inputData[i]);
			}
			Console.Write ("Choice: ");
			try {
				int index = int.Parse (Console.ReadLine ());
				return inputData[index];
			}
			catch (FormatException e) {
				e.ToString();
				Console.WriteLine("Try picking an actual number this time!");
				return DisplayChoices(message, inputData);
			}
			catch (IndexOutOfRangeException e) {
				e.ToString();
				Console.WriteLine("Try selecting an actual choice this time!");
				return DisplayChoices(message, inputData);
			}
		}
		public static T[] Merge<T> (T[][] arrays) {
			var tempList = new List<T>();
			foreach (T[] innerArray in arrays) {
				tempList.AddRange(innerArray);
			}
			return tempList.ToArray();
		}
	}
}
