using System;
using System.Diagnostics;
using System.IO;

namespace STBLBuilder {
	public static class Main {
		public static bool Run () {
			if(Entry.PrintHelp) {
				PrintHelp();
			}

			if(Entry.SourceFilePath == null) {
				Console.Error.WriteLine("No source STBL XML file was specified");
				return false;
			}

			STBLXMLFile stblXMLFile = STBLBuilding.ReadSTBLXMLFile(Entry.SourceFilePath);

		
			STBLBuilding.BuildSTBL(stblXMLFile, Entry.TargetDirectoryPath);
			STBLBuilding.BuildIdentifiers(stblXMLFile, Entry.TargetDirectoryPath);

			Entry.Completed = true;
			return true;
		}

		public static void PrintHelp () {
			Console.WriteLine(
				"Builds a Sims 4 STBL file from xml sources. \n" +
				"\n" +
				Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName) + " [-h] [-t[filepath]] [-s[filepath]] [-p] FilePath \n" +
				"\n" +
				" -h\t\t\tPrints this help message. \n" +
				"\n" +
				" -t [directorypath]\tDesignates the directory path the STBL file will \n" +
				"\t\t\tbe saved to. The path can be relative to the \n" +
				"\t\t\tworking directory. \n" +
				"\n" +
				" -p\t\t\tWith this argument, this tool will create source \n" +
				"\t\t\tinfo files for use with the PackageBuilder tool. \n");
		}
	}
}
