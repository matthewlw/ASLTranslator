using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace ASLSitesClient
{
	public class ASLPro : BaseDictionary
	{
		public override string GetVideoURL(string word) {
			return wordDatabase[word.Trim().ToUpper()];
		}
		public override void CreateDatabase() {
			Console.WriteLine("Creating ASLPro database...");
			const string beginning = "<option value=";
			var client = new WebClient();
			var writer = new StreamWriter(DataPath);
			var regex = new Regex(Strings.ASLPRO_REGEX);
			var alreadyWritten = new List<string>();
			for (char ch = 'A'; ch <= 'Z'; ch++) {
				alreadyWritten.Clear();
				string rawHTML = client.DownloadString(string.Format(Strings.ASLPRO_BASE_URL, ch));
				string[] rawLines = rawHTML.Split('\n');
				string[] cleanerLines = rawLines.Where(line => line.Trim().StartsWith(beginning)).ToArray();
				foreach (string line in cleanerLines) {
					Match match = regex.Match(line);
					string partialurl = match.Groups[1].Value;
					if (!partialurl.StartsWith("/"))
						partialurl = "/" + partialurl;
					string url = "http://aslpro.com" + partialurl;
					string word = match.Groups[2].Value.ToUpper();
					if (!alreadyWritten.Contains(word)) {
						writer.WriteLine(word + '\t' + url);
						alreadyWritten.Add(word);
					} //end if to determine if already written
				} //end loop over lines
			} //end loop over letters
			writer.Close();
			writer.Dispose();
			client.Dispose();
		}
		public override string DataDirectory {
			get {
				return Paths.ASLProDirectory;
			}
		}
		public override string ToString ()
		{
			return "ASL Pro";
		}
	}
}