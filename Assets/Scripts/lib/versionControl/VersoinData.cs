using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace xy3d.tstd.lib.versionControl{

	public class VersionData{

		public static VersionData LoadFromFile(string _path){

			using(FileStream fs = new FileStream(_path,FileMode.Open)){

				BinaryReader br = new BinaryReader(fs);

				VersionData data = new VersionData();

				data.version = br.ReadInt32();

				int length = br.ReadInt32();

				for(int i = 0 ; i < length ; i++){

					string fileName = br.ReadString();

					int fileVersion = br.ReadInt32();

					data.dic.Add(fileName,fileVersion);
				}

				br.Close();

				fs.Close();

				return data;
			}
		}

		public static void SaveToFile(string _path,VersionData _data){
			
			FileInfo fi = new FileInfo(_path);
			
			DirectoryInfo dir = fi.Directory;
			
			if (!dir.Exists){
				
				dir.Create();
			}
			
			using (FileStream fs = new FileStream (_path, FileMode.OpenOrCreate)) {
				
				BinaryWriter bw = new BinaryWriter(fs);

				bw.Write(_data.version);

				bw.Write(_data.dic.Count);

				foreach(KeyValuePair<string,int> pair in _data.dic){

					bw.Write(pair.Key);

					bw.Write(pair.Value);
				}

				bw.Close();
				
				fs.Close ();
			}
		}

		public int version;
		public Dictionary<string,int> dic = new Dictionary<string, int>();

	}
}