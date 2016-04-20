using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Runtime.Serialization;
using System;
using xy3d.tstd.lib.wwwManager;

namespace xy3d.tstd.lib.systemIO{

	public class SystemIO{

		public static void SaveSerializeFile(string _path, object _obj){

			FileInfo fi = new FileInfo(_path);

			DirectoryInfo dir = fi.Directory;

			if (!dir.Exists){

				dir.Create();
			}

			using (FileStream fs = new FileStream (_path, FileMode.OpenOrCreate)) {
				
				BinaryFormatter formatter = new BinaryFormatter ();
				
				formatter.Serialize (fs, _obj);
				
				fs.Close ();
			}
		}

		public static T LoadSerializeFile<T>(string _path){
			
			using(FileStream fs = new FileStream(_path,FileMode.Open)){
				
				BinaryFormatter formatter = new BinaryFormatter ();

				formatter.Binder = new VersionBinder();

				T data = (T)formatter.Deserialize (fs);
				
				fs.Close();

				return data;
			}
		}

		public static void LoadSerializeFileWithWWW<T>(string _path,Action<T> _callBack){
			
			Action<WWW> callBack = delegate(WWW _www)
			{
				LoadSerializeFileWithWWWOver(_www, _callBack);
			};
			
			WWWManager.Instance.Load(_path, callBack);
		}

		private static void LoadSerializeFileWithWWWOver<T>(WWW _www, Action<T> _callBack)
		{
			DeserializeInThread.Instance.Deserialize(_www.bytes,_callBack);
		}
		
		public static void SaveFile(string _path,byte[] _bytes){

			FileInfo fi = new FileInfo(_path);

			DirectoryInfo dir = fi.Directory;

			if (!dir.Exists){

				dir.Create();
			}

			using (FileStream fs = new FileStream (_path, FileMode.OpenOrCreate)) {

				fs.Write(_bytes,0,_bytes.Length);

				fs.Close();
			}
		}

		public static byte[] LoadFile(string _path){
			
			using (FileStream fs = new FileStream (_path, FileMode.Open)) {
				
				byte[] bytes = new byte[fs.Length];

				fs.Read(bytes,0,(int)fs.Length);

				return bytes;
			}
		}
	}
}