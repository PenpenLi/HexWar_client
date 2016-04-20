using UnityEngine;
using System.Collections;

namespace xy3d.tstd.lib.localData{

	public class LocalAccountData{

		private static string account;

		public static void SetAccount(string _account){

			account = _account;

			LocalAccountServerData.SetAccount(account);
		}

		public static void SetFloat(string _key,float _data){

			PlayerPrefs.SetFloat(string.Format("{0}:{1}",account,_key),_data);

			PlayerPrefs.Save();
		}

		public static float GetFloat(string _key){

			return PlayerPrefs.GetFloat(string.Format("{0}:{1}",account,_key));
		}

		public static void SetInt(string _key,int _data){

			PlayerPrefs.SetInt(string.Format("{0}:{1}",account,_key),_data);

			PlayerPrefs.Save();
		}

		public static float GetInt(string _key){
			
			return PlayerPrefs.GetInt(string.Format("{0}:{1}",account,_key));
		}

		public static void SetString(string _key,string _data){
			
			PlayerPrefs.SetString(string.Format("{0}:{1}",account,_key),_data);

			PlayerPrefs.Save();
		}

		public static string GetString(string _key){
			
			return PlayerPrefs.GetString(string.Format("{0}:{1}",account,_key));
		}

		public static bool HasKey(string _key){
			
			return PlayerPrefs.HasKey(string.Format("{0}:{1}",account,_key));
		}
	}
}