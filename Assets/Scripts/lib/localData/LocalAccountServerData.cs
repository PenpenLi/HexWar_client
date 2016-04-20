using UnityEngine;
using System.Collections;

namespace xy3d.tstd.lib.localData{

	public class LocalAccountServerData{

		private static string account;

		private static int serverID;

		public static void SetAccount(string _account){

			account = _account;
		}

		public static void SetServer(int _serverID){

			serverID = _serverID;
		}

		public static void SetFloat(string _key,float _data){
			
			PlayerPrefs.SetFloat(string.Format("{0}:{1}:{2}",account,serverID,_key),_data);
			
			PlayerPrefs.Save();
		}

		public static float GetFloat(string _key){

			return PlayerPrefs.GetFloat(string.Format("{0}:{1}:{2}",account,serverID,_key));
		}
		
		public static void SetInt(string _key,int _data){
			
			PlayerPrefs.SetInt(string.Format("{0}:{1}:{2}",account,serverID,_key),_data);
			
			PlayerPrefs.Save();
		}

		public static int GetInt(string _key){
			
			return PlayerPrefs.GetInt(string.Format("{0}:{1}:{2}",account,serverID,_key));
		}
		
		public static void SetString(string _key,string _data){
			
			PlayerPrefs.SetString(string.Format("{0}:{1}:{2}",account,serverID,_key),_data);
			
			PlayerPrefs.Save();
		}

		public static string GetString(string _key){
			
			return PlayerPrefs.GetString(string.Format("{0}:{1}:{2}",account,serverID,_key));
		}

		public static bool HasKey(string _key){
			
			return PlayerPrefs.HasKey(string.Format("{0}:{1}:{2}",account,serverID,_key));
		}
	}
}