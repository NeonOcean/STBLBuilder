using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Xml.Serialization;

namespace STBLBuilder {
	public static class STBLBuilding {
		public const string STBLFileExtension = "stbl";
		public const string IdentifiersFileExtension = "xml";
		public const string SourceInfoFileExtension = "sourceinfo";

		public class SourceInfo {
			[XmlElement(IsNullable = true)]
			public string Name {
				get; set;
			} = null;

			public uint TypeID {
				get; set;
			} = 0;

			public uint GroupID {
				get; set;
			} = 0;

			public ulong InstanceID {
				get; set;
			} = 0;
		}

		public static STBLXMLFile ReadSTBLXMLFile (string sourcePath) {
			if(!File.Exists(sourcePath)) {
				throw new FileNotFoundException("File does not exist.", sourcePath);
			}

			STBLXMLFile file = null;

			try {
				file = (STBLXMLFile)Tools.ReadXML<STBLXMLFile>(sourcePath);
			} catch(Exception e) {
				throw new Exception("Failed to read language source file.", e);
			}

			return file;
		}

		public static void BuildSTBL (STBLXMLFile file, string buildPath) {
			Directory.CreateDirectory(buildPath);

			STBL.Languages fallbackLanguage = STBL.GetLanguage(file.FallbackLanguage);

			foreach(STBL.Languages language in STBL.GetAllLanguages()) {
				string languageFileName = string.Format(file.STBLName, new STBL.LanguageFileName(language));

				string stblFileName = languageFileName + "." + STBLFileExtension;
				string stblFilePath = Path.Combine(buildPath, stblFileName);

				using(BinaryWriter writer = new BinaryWriter(new FileStream(stblFilePath, FileMode.Create))) {
					string[] texts = new string[file.Entries.Count];
					ushort[] textsByteCounts = new ushort[file.Entries.Count];
					uint entryByteCount = 0;

					for(int entryIndex = 0; entryIndex < file.Entries.Count; entryIndex++) {
						string entryLanguageText = file.Entries[entryIndex].GetText(language);

						if(entryLanguageText == null) {
							entryLanguageText = file.Entries[entryIndex].GetText(fallbackLanguage);
						}

						if(entryLanguageText == null) {
							entryLanguageText = file.Entries[entryIndex].Identifier;
						}

						texts[entryIndex] = entryLanguageText;

						textsByteCounts[entryIndex] = (ushort)Encoding.UTF8.GetByteCount(texts[entryIndex]);
						entryByteCount += textsByteCounts[entryIndex] + 1u;
					}

					writer.Write(Encoding.UTF8.GetBytes("STBL"));
					writer.Write((byte)5);
					writer.Write((ushort)0);
					writer.Write((uint)file.Entries.Count);
					writer.Write(0u);
					writer.Write((ushort)0);
					writer.Write(entryByteCount);

					for(int textsIndex = 0; textsIndex < file.Entries.Count; textsIndex++) {
						writer.Write(file.Entries[textsIndex].Key);
						writer.Write((byte)0);
						writer.Write(textsByteCounts[textsIndex]);
						writer.Write(texts[textsIndex].ToCharArray());
					}
				}

				if(STBLBuilder.Entry.BuildSourceInfoFile) {
					string sourceInfoFilePath = stblFilePath + "." + SourceInfoFileExtension;

					string languageInstanceHexadecimal = ((int)language).ToString("x2") + file.STBLInstance.ToString("x").Substring(2);
					ulong languageInstance = ulong.Parse(languageInstanceHexadecimal, NumberStyles.HexNumber);

					Tools.WriteXML(sourceInfoFilePath, new SourceInfo() {
						Name = languageFileName,
						TypeID = 570775514,
						GroupID = file.STBLGroup,
						InstanceID = languageInstance
					});
				}
			}
		}

		public static void BuildIdentifiers (STBLXMLFile file, string buildPath) {
			if(!file.BuildIdentifiers) {
				return;
			}

			Directory.CreateDirectory(buildPath);

			string identifiersFileName = file.IdentifiersName + "." + IdentifiersFileExtension;
			string identifiersFilePath = Path.Combine(buildPath, identifiersFileName);

			string identifiersText = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
			identifiersText += "<I n=\"" + SecurityElement.Escape(file.IdentifiersName) + "\" s=\"" + file.IdentifiersInstance.ToString("0") + "\" i=\"snippet\" m=\"snippets\" c=\"NeonoceanGlobalLanguageIdentifiers\">\n";
			identifiersText += "\t<L n=\"value\">\n";

			for(int entryIndex = 0; entryIndex < file.Entries.Count; entryIndex++) {
				identifiersText += "\t\t<U>\n";
				identifiersText += "\t\t\t<T n=\"key\">" + SecurityElement.Escape(file.Entries[entryIndex].Identifier) + "</T>\n";
				identifiersText += "\t\t\t<T n=\"value\">" + file.Entries[entryIndex].Key.ToString("0") + "</T>\n";
				identifiersText += "\t\t</U>\n";
			}

			identifiersText += "\t</L>\n</I>";

			using(StreamWriter writer = new StreamWriter(identifiersFilePath)) {
				writer.Write(identifiersText);
			}

			if(STBLBuilder.Entry.BuildSourceInfoFile) {
				string sourceInfoFilePath = identifiersFilePath + "." + SourceInfoFileExtension;

				Tools.WriteXML(sourceInfoFilePath, new SourceInfo() {
					Name = file.IdentifiersName,
					TypeID = 2113017500,
					GroupID = file.IdentifiersGroup,
					InstanceID = file.IdentifiersInstance
				});
			}
		}
	}
}
