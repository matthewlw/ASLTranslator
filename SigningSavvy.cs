using System;
using System.IO;
using System.Linq;
using CsQuery;
using System.Collections.Generic;

namespace ASLSitesClient
{
	public class SigningSavvy : BaseDictionary
	{
		public override string GetVideoURL(string word) {
			string webpageURL = wordDatabase[word.Trim().ToUpper()];
			CQ page = CQ.CreateFromUrl(webpageURL);
			string urlPart = page["#player1Video > source"].FirstElement().GetAttribute("src");
			return "https://www.signingsavvy.com/" + urlPart;
		}
		public override void CreateDatabase() {
			Console.WriteLine("Creating Signing Savvy database...");
			var writer = new StreamWriter(DataPath);
			const string baseURL = "http://www.signingsavvy.com/browse/";
			var alreadyWritten = new List<string>();
			for (char ch = 'A'; ch <= 'Z'; ch++) {
				alreadyWritten.Clear();
				CQ markup = CQ.CreateFromUrl(baseURL + ch);
				IDomElement[] li_s = markup [".search_results > ul > li"].Elements.ToArray ();
				foreach (var elem in li_s) {
					var word = elem.FirstChild.InnerText + elem.InnerText.Replace("&quot;", "").ToUpper();
					var url = elem.FirstChild.GetAttribute("href");
					if (!alreadyWritten.Contains(word)) {
						writer.WriteLine(word + '\t' + "http://www.signingsavvy.com/" + url);
						alreadyWritten.Add(word);
					}
				}
			}
			writer.Close();
			writer.Dispose();
		}
		public override string DataDirectory {
			get {
				return Paths.SigningSavvyDirectory;
			}
		}
		public override string ToString ()
		{
			return "Signing Savvy";
		}

	}
}