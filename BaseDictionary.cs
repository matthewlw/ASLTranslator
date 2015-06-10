using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASLSitesClient
{
	public abstract class BaseDictionary
	{
		/// <summary>
		/// The word database. The word is the key, and the URL is the value.
		/// That makes the most sense for what it is being used for.
		/// </summary>
		protected Dictionary<string, string> wordDatabase = new Dictionary<string, string>();
		/// <summary>
		/// Initializes a new instance of the <see cref="ASLSitesClient.BaseDictionary"/> class.
		/// Takes the place of the old "Initialize" method in loading the database
		/// If the database does not exist, it calls CreateDatabase to create it.
		/// </summary>
		public BaseDictionary () {
			if (!File.Exists(DataPath))
				CreateDatabase();
			string[] fileLines = File.ReadAllLines(DataPath);
			foreach (string line in fileLines) {
				string[] lineParts = line.Split('\t');
				wordDatabase.Add(lineParts[0], lineParts[1]);
			}
		}
		/// <summary>
		/// Determines whether the service contains the specified word in its dictionary
		/// The word can be passed in with minimal cleaning -- it is trimmed and upcased
		/// </summary>
		/// <param name="word">Word.</param>
		public bool Contains(string word) {
			return wordDatabase.ContainsKey(word.Trim().ToUpper());
		}
		/// <summary>
		/// Autocompletes the specified fragment.
		/// Word is trimmed and upcased upon reaching this method.
		/// </summary>
		/// <param name="fragment">The fragment to check</param>
		public string[] Autocomplete(string fragment) {
			string goodSegment = fragment.Trim().ToUpper();
			return wordDatabase.Keys.Where(word => word.StartsWith(goodSegment)).ToArray();
		}
		/// <summary>
		/// Gets the path to the database
		/// </summary>
		/// <value>The path to the database</value>
		public string DataPath {
			get {
				return DataDirectory + "database.txt";
			}
		}
		/// <summary>
		/// Gets the video URL
		/// </summary>
		/// <returns>The video URL</returns>
		/// <param name="word">The word to get the URL for</param>
		public abstract string GetVideoURL(string word);
		/// <summary>
		/// Creates the database.
		/// </summary>
		public abstract void CreateDatabase();
		/// <summary>
		/// Gets the path to the data directory (with a trailing directory seperator, for convenience).
		/// </summary>
		/// <value>The path to the data directory</value>
		public abstract string DataDirectory {get;}
	}
}