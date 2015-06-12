using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace ASLSitesClient
{
	public class Downloader
	{
		public delegate void WordDownloadedEventHandler(object sender, WordDownloadedEventArgs e);
		public event WordDownloadedEventHandler WordDownloaded;
		/// <summary>
		/// Download the specified words.
		/// </summary>
		/// <param name="words">An array of words to download</param>
		public string Download(DictionaryWord[] words) {
			var sourcePaths = new List<string>();
			foreach (DictionaryWord word in words) {
				sourcePaths.Add(Download(word));
			}
			string destinationPath = GetFileName();
			Concatenate(destinationPath + ".tmp.mpg", sourcePaths.ToArray());
			RunFFMPEG(String.Format("-i {0}.tmp.mpg {0}", destinationPath));
			File.Delete(destinationPath + ".tmp.mpg");
			return destinationPath;
		}
		string Download(DictionaryWord word) {
			var client = new WebClient();
			var dictionary = word.Dictionary;
			var text = word.Word;
			string finalDestination = dictionary.DataDirectory + text;
			if (!File.Exists(dictionary.DataDirectory + text)) {
				string source = dictionary.GetVideoURL(text);
				string downloadDestination = finalDestination + "-TEMP";
				client.DownloadFile(source, downloadDestination);
				client.Dispose();
			
				const string argumentsFormat = "-i \"{0}\" -f mpeg \"{1}\"";
				var info = new ProcessStartInfo();
				info.FileName = Paths.FFMPEG;
				info.Arguments = String.Format(argumentsFormat, downloadDestination, finalDestination);
				info.RedirectStandardOutput = false;
				info.RedirectStandardError = false;
				info.UseShellExecute = false;
				Process.Start(info).WaitForExit();
				File.Delete(downloadDestination);
				var args = new WordDownloadedEventArgs(word);
				OnWordDownloaded(args);
			}
			client.Dispose();
			return finalDestination;
		}
		static void Concatenate(string destinationPath, params string[] sourcePaths) {
			using (Stream destination = new FileStream(destinationPath, FileMode.CreateNew)) {
				foreach (string sourcePath in sourcePaths) {
					using (Stream source = new FileStream(sourcePath, FileMode.Open)) {
						source.CopyTo(destination);
					}
				}
			}
		}
		static string GetFileName() {
			string currentDirectory = Directory.GetCurrentDirectory();
			string[] files = Directory.EnumerateFiles(currentDirectory).ToArray();
			string[] justNames = files.Select(file => Path.GetFileName(file)).ToArray();
			string[] formattedFiles = justNames.Where(
				file => file.StartsWith("video") && file.EndsWith(".mp4")).ToArray();
			int fileNumber = 0;
			foreach (string file in formattedFiles) {
				string cleaned = file.Replace("video", "").Replace(".mp4", "");
				int temp;
				if (int.TryParse(cleaned, out temp) && temp > fileNumber) fileNumber = temp;
			}
			fileNumber++;
			return "video" + fileNumber + ".mp4";
		}
		static void RunFFMPEG(string arguments) {
			var info = new ProcessStartInfo();
			info.FileName = Paths.FFMPEG;
			info.Arguments = arguments;
			info.RedirectStandardOutput = true;
			info.RedirectStandardError = true;
			info.UseShellExecute = false;
			var process = Process.Start(info);
			System.Threading.Thread.Sleep(15000);
			process.Kill();
		}
		protected virtual void OnWordDownloaded (WordDownloadedEventArgs e) {
			WordDownloaded(this, e);
		}
	}
}
