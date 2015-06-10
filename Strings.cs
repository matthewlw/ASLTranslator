
//There is no good reason for this class to exist
//However, the hard-coded strings irritate me.
public static class Strings
{

	/// <summary>
	/// The URL of a Windows copy of FFMPEG.
	/// </summary>
	public readonly static string FFMPEG_URL = 
		"https://github.com/DFKnight/encoder/blob/master/bin/ffmpeg.exe?raw=true";
	//Fake: https://raw.githubusercontent.com/DFKnight/encoder/master/README.md
	//Real: https://github.com/DFKnight/encoder/blob/master/bin/ffmpeg.exe?raw=true
	public readonly static string DICTIONARY_QUESTION = 
		"What dictionary would you like to use?";
	public readonly static string USER_AGENT = 
		"Mozilla/5.0 (iPhone; U; CPU iPhone OS 3_0 like Mac OS X; en-us) AppleWebKit/528.18 (KHTML, like Gecko) Version/4.0 Mobile/7A341 Safari/528.16";
	public readonly static string ASLPRO_BASE_URL = 
		"http://www.aslpro.com/cgi-bin/aslpro/aslpro.cgi?dictionary=Main&letter={0}";
	public readonly static string ASLPRO_REGEX = 
		@"<option value=\""(.*)\"">(.*)</option>";
	public readonly static string ASLPRO_BEGINNING = 
		"<option value=";
	public readonly static string AUTOCOMPLETE = 
		"Which word do you want to use?";
	public readonly static string WEGIVEUP = 
		@"Failed to find ""{0}"" in either dictionary. 
If you would like to omit the word, press Enter.
Otherwise, enter a substitute: ";
	public readonly static string DOWNLOADED = 
		"Word {0} downloaded from dictionary {1}";

}

