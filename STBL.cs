using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace STBLBuilder {
	public static class STBL {
		public enum Languages {
			English = 0,
			ChineseSimplified = 1,
			ChineseTraditional = 2,
			Czech = 3,
			Danish = 4,
			Dutch = 5,
			Finnish = 6,
			French = 7,
			German = 8,
			Greek = 9,
			Hungarian = 10,
			Italian = 11,
			Japanese = 12,
			Korean = 13,
			Norwegian = 14,
			Polish = 15,
			PortuguesePortugal = 16,
			PortugueseBrazil = 17,
			Russian = 18,
			SpanishSpain = 19,
			SpanishMexico = 20,
			Swedish = 21,
			Thai = 22
		}

		public class LanguageFileName : IFormattable {
			public string Normal {
				get; private set;
			} = "";

			public string Spaced {
				get; private set;
			} = "";

			public string Underscored {
				get; private set;
			} = "";

			public string Hyphenated {
				get; private set;
			} = "";

			public LanguageFileName (Languages language) {
				Normal = language.ToString();
				Spaced = Regex.Replace(Normal, "(\\B[A-Z])", " $1");
				Underscored = Spaced.Replace(" ", "_");
				Hyphenated = Spaced.Replace(" ", "-");
			}

			public override string ToString () {
				return Normal;
			}

			public string ToString (string format, IFormatProvider formatProvider) {
				switch(format) {
					case "Spaced":
						return Spaced;
					case "Underscored":
						return Underscored;
					case "Hyphenated":
						return Hyphenated;
				}

				return Normal;
			}
		}

		public static Random stblKeyGenerator = new Random();

		public static Languages GetLanguage (string languageIdentifier) {
			Languages language = Languages.English;

			if(!Enum.TryParse(languageIdentifier, true, out language)) {
				throw new ArgumentException("'" + languageIdentifier + "' is not a valid language.");
			}

			return language;
		}

		public static Languages GetLanguage (int languageNumber) {
			Languages language = Languages.English;

			if(!Enum.TryParse(languageNumber.ToString(), out language)) {
				throw new ArgumentException("'" + languageNumber.ToString() + "' is not a valid language number.");
			}

			return language;
		}

		public static Languages[] GetAllLanguages () {
			return (Languages[])Enum.GetValues(typeof(Languages));
		}

		public static string[] GetAllLanguageIdentifiers () {
			return Enum.GetNames(typeof(Languages));
		}

		public static int[] GetAllLanguageNumbers () {
			return (int[])Enum.GetValues(typeof(Languages));
		}

		public static bool IsLanguage (string languageIdentifier) {
			Languages language = Languages.English;
			return Enum.TryParse(languageIdentifier, true, out language);
		}

		public static bool IsLanguage (int languageNumber) {
			Languages language = Languages.English;
			return Enum.TryParse(languageNumber.ToString(), out language);
		}

		public static int[] LanguagesToLanguageNumbers (Languages[] languages) {
			int[] languageNumbers = new int[languages.Length];

			for(int languageIndex = 0; languageIndex < languages.Length; languageIndex++) {
				languageNumbers[languageIndex] = (int)languages[languageIndex];
			}

			return languageNumbers;
		}

		public static List<int> LanguagesToLanguageNumbers (List<Languages> languages) {
			List<int> languageNumbers = new List<int>(languages.Count);

			for(int languageIndex = 0; languageIndex < languages.Count; languageIndex++) {
				languageNumbers.Add((int)languages[languageIndex]);
			}

			return languageNumbers;
		}

		public static uint GetRandomUIntKey (uint[] blockedKeys = null) {
			byte[] randomizedValueBytes = new byte[4];
			stblKeyGenerator.NextBytes(randomizedValueBytes);
			uint randomizedValue = BitConverter.ToUInt32(randomizedValueBytes, 0);

			if(blockedKeys != null) {
				if(Array.Exists(blockedKeys, x => x == randomizedValue)) {
					return GetRandomUIntKey(blockedKeys: blockedKeys);
				}
			}

			return randomizedValue;
		}

		public static ulong GetRandomULongKey (ulong[] blockedKeys = null) {
			byte[] randomizedValueBytes = new byte[8];
			stblKeyGenerator.NextBytes(randomizedValueBytes);
			ulong randomizedValue = BitConverter.ToUInt64(randomizedValueBytes, 0);

			if(blockedKeys != null) {
				if(Array.Exists(blockedKeys, x => x == randomizedValue)) {
					return GetRandomULongKey(blockedKeys: blockedKeys);
				}
			}

			return randomizedValue;
		}
	}

	public class STBLXMLFile {
		public int FallbackLanguage {
			get; set;
		}

		public uint STBLGroup {
			get; set;
		}

		public ulong STBLInstance {
			get; set;
		}

		public string STBLName {
			get; set;
		}

		public bool BuildIdentifiers {
			get; set;
		}

		public uint IdentifiersGroup {
			get; set;
		}

		public ulong IdentifiersInstance {
			get; set;
		}

		public string IdentifiersName {
			get; set;
		}

		public List<STBLXMLEntry> Entries {
			get; set;
		}
	}

	public class STBLXMLEntry {
		public string Identifier {
			get; set;
		}

		public uint Key {
			get; set;
		}

		public string English {
			get; set;
		}

		public string ChineseSimplified {
			get; set;
		}

		public string ChineseTraditional {
			get; set;
		}

		public string Czech {
			get; set;
		}

		public string Danish {
			get; set;
		}

		public string Dutch {
			get; set;
		}

		public string Finnish {
			get; set;
		}

		public string French {
			get; set;
		}

		public string German {
			get; set;
		}

		public string Greek {
			get; set;
		}

		public string Hungarian {
			get; set;
		}

		public string Italian {
			get; set;
		}

		public string Japanese {
			get; set;
		}

		public string Korean {
			get; set;
		}

		public string Norwegian {
			get; set;
		}

		public string Polish {
			get; set;
		}

		public string PortuguesePortugal {
			get; set;
		}

		public string PortugueseBrazil {
			get; set;
		}

		public string Russian {
			get; set;
		}

		public string SpanishSpain {
			get; set;
		}

		public string SpanishMexico {
			get; set;
		}

		public string Swedish {
			get; set;
		}

		public string Thai {
			get; set;
		}

		public string GetText (int languageNumber) {
			STBL.Languages language = STBL.Languages.English;

			if(!Enum.TryParse(languageNumber.ToString(), out language)) {
				throw new ArgumentException("'" + languageNumber.ToString() + "' is not a valid language number.");
			}

			return GetText(language);
		}

		public string GetText (string languageIdentifier) {
			STBL.Languages language = STBL.Languages.English;

			if(!Enum.TryParse(languageIdentifier, true, out language)) {
				throw new ArgumentException("'" + languageIdentifier + "' is not a valid language.");
			}

			return GetText(language);
		}

		public string GetText (STBL.Languages language) {
			switch(language) {
				case STBL.Languages.English:
					return English;
				case STBL.Languages.ChineseSimplified:
					return ChineseSimplified;
				case STBL.Languages.ChineseTraditional:
					return ChineseTraditional;
				case STBL.Languages.Czech:
					return Czech;
				case STBL.Languages.Danish:
					return Danish;
				case STBL.Languages.Dutch:
					return Dutch;
				case STBL.Languages.Finnish:
					return Finnish;
				case STBL.Languages.French:
					return French;
				case STBL.Languages.German:
					return German;
				case STBL.Languages.Greek:
					return Greek;
				case STBL.Languages.Hungarian:
					return Hungarian;
				case STBL.Languages.Italian:
					return Italian;
				case STBL.Languages.Japanese:
					return Japanese;
				case STBL.Languages.Korean:
					return Korean;
				case STBL.Languages.Norwegian:
					return Norwegian;
				case STBL.Languages.Polish:
					return Polish;
				case STBL.Languages.PortuguesePortugal:
					return PortuguesePortugal;
				case STBL.Languages.PortugueseBrazil:
					return PortugueseBrazil;
				case STBL.Languages.Russian:
					return Russian;
				case STBL.Languages.SpanishSpain:
					return SpanishSpain;
				case STBL.Languages.SpanishMexico:
					return SpanishMexico;
				case STBL.Languages.Swedish:
					return Swedish;
				case STBL.Languages.Thai:
					return Thai;
				default:
					throw new ArgumentException("'" + language.ToString() + "' is not a valid language");
			}
		}

		public bool HasText (int languageNumber) {
			return GetText(languageNumber) != null;
		}

		public bool HasText (string languageIdentifier) {
			return GetText(languageIdentifier) != null;
		}

		public bool HasText (STBL.Languages language) {
			return GetText(language) != null;
		}

		public void SetText (int languageNumber, string languageText) {
			STBL.Languages language = STBL.Languages.English;

			if(!Enum.TryParse(languageNumber.ToString(), out language)) {
				throw new ArgumentException("'" + languageNumber.ToString() + "' is not a valid language number.");
			}

			SetText(language, languageText);
		}

		public void SetText (string languageIdentifier, string languageText) {
			STBL.Languages language = STBL.Languages.English;

			if(!Enum.TryParse(languageIdentifier, true, out language)) {
				throw new ArgumentException("'" + languageIdentifier + "' is not a valid language.");
			}

			SetText(language, languageText);
		}

		public void SetText (STBL.Languages language, string languageText) {
			switch(language) {
				case STBL.Languages.English:
					English = languageText;
					return;
				case STBL.Languages.ChineseSimplified:
					ChineseSimplified = languageText;
					return;
				case STBL.Languages.ChineseTraditional:
					ChineseTraditional = languageText;
					return;
				case STBL.Languages.Czech:
					Czech = languageText;
					return;
				case STBL.Languages.Danish:
					Danish = languageText;
					return;
				case STBL.Languages.Dutch:
					Dutch = languageText;
					return;
				case STBL.Languages.Finnish:
					Finnish = languageText;
					return;
				case STBL.Languages.French:
					French = languageText;
					return;
				case STBL.Languages.German:
					German = languageText;
					return;
				case STBL.Languages.Greek:
					Greek = languageText;
					return;
				case STBL.Languages.Hungarian:
					Hungarian = languageText;
					return;
				case STBL.Languages.Italian:
					Italian = languageText;
					return;
				case STBL.Languages.Japanese:
					Japanese = languageText;
					return;
				case STBL.Languages.Korean:
					Korean = languageText;
					return;
				case STBL.Languages.Norwegian:
					Norwegian = languageText;
					return;
				case STBL.Languages.Polish:
					Polish = languageText;
					return;
				case STBL.Languages.PortuguesePortugal:
					PortuguesePortugal = languageText;
					return;
				case STBL.Languages.PortugueseBrazil:
					PortugueseBrazil = languageText;
					return;
				case STBL.Languages.Russian:
					Russian = languageText;
					return;
				case STBL.Languages.SpanishSpain:
					SpanishSpain = languageText;
					return;
				case STBL.Languages.SpanishMexico:
					SpanishMexico = languageText;
					return;
				case STBL.Languages.Swedish:
					Swedish = languageText;
					return;
				case STBL.Languages.Thai:
					Thai = languageText;
					return;
				default:
					throw new ArgumentException("'" + language.ToString() + "' is not a valid language.");
			}
		}
	}
}
