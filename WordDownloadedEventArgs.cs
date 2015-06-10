using System;

namespace ASLSitesClient
{
	public class WordDownloadedEventArgs : EventArgs
	{
		DictionaryWord word;
		public WordDownloadedEventArgs (DictionaryWord word)
		{
			this.word = word;
		}
		/// <summary>
		/// Gets the word that was downloaded.
		/// </summary>
		/// <value>The word</value>
		public DictionaryWord Word {
			get {
				return word;
			}
		}
	}
}

