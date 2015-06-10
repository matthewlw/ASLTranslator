using System;
using System.Collections.Generic;
using System.Linq;
namespace ASLSitesClient
{
	public static class WordResolver
	{
		public static DictionaryWord[] Resolve (string input)
		{
			if (input == null)
				throw new ArgumentNullException ("input");
			BaseDictionary[] dicts = { Dictionaries.SigningSavvy, Dictionaries.AslPro };

			//Check to see if it is in the dictionary exactly.
			foreach (BaseDictionary dict in dicts) {
				if (dict.Contains (input)) {
					var dictWord = new DictionaryWord {
						Dictionary = dict,
						Word = input.Trim ().ToUpper ()
					};
					DictionaryWord[] arrayForm = { dictWord };
					return arrayForm;
				}
			}

			//Check to see if the word is partially typed.
			foreach (BaseDictionary dict in dicts) {
				string[] autoCompleted = dict.Autocomplete (input);
				if (autoCompleted.Length != 0) {
					string choice = MainClass.DisplayChoices (Strings.AUTOCOMPLETE, autoCompleted);
					return Resolve (choice);
				}
			}

			//Give up. Ask the user to enter another possible word.
			Console.Write (Strings.WEGIVEUP, input);
			string substitute = Console.ReadLine ();
			DictionaryWord[] empty = { };

			if (substitute != string.Empty)
				return MainClass.Merge (Split (substitute).Select (word => Resolve (word)).ToArray ());
			return empty;

		}

		/// <summary>
		/// Intelligently split up the input into component words,
		/// so the output makes some degree of sense.
		/// 
		/// </summary>
		/// <param name="input">Input.</param>
		public static string[] Split (string input) {
			string[] quoteSplit = input.Split('"');
			var final = new List<string>();
			bool special = false;
			foreach (string segment in quoteSplit) {
				if (segment.Trim() != String.Empty) {
					if (special) {
						final.Add(segment);
					} else {
						final.AddRange(segment.Trim().Split(' '));
					}
				}
				special = !special;
			}
			return final.ToArray();
		}
	}
}