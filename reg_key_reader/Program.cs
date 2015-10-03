using System;
using ntregsharp;
using System.IO;

namespace reg_key_reader
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			RegistryHive hive = new RegistryHive (args [0]);

			string path = "Microsoft|Windows|CurrentVersion|Component Based Servicing|Packages";
			string[] paths = path.Split ('|');

			int i = 0;
			NodeKey key = hive.RootKey;
			while (true) {
				
				foreach (NodeKey k in key.ChildNodes) {
					if (k.Name == paths [i]) {
						key = k;
						break;
					}
				}

				if (i == paths.Length - 1)
					break;
				
				i++;
			}

			//using (FileStream stream = File.Open (hive.Filepath, FileMode.Open)) {
			//	key.EditNodeName (stream, "Packages");
			//}

			Console.WriteLine (key.Name);

			foreach (NodeKey k in key.ChildNodes) {
				Console.WriteLine (k.Name);
			}

			Console.WriteLine (hive.RootKey.Name);
		}
	}
}
