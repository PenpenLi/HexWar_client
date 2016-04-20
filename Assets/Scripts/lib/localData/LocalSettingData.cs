using UnityEngine;
using System.Collections;

namespace xy3d.tstd.lib.localData{

	public class LocalSettingData{

		public static void SetFloat(string _key,float _data){
			
			PlayerPrefs.SetFloat(_key,_data);



			PlayerPrefs.Save();
		}

		public static bool HasKey(string _key){

			return PlayerPrefs.HasKey(_key);
		}
		
		public static float GetFloat(string _key){
			
			return PlayerPrefs.GetFloat(_key);
		}
		
		public static void SetInt(string _key,int _data){
			
			PlayerPrefs.SetInt(_key,_data);
			
			PlayerPrefs.Save();
		}
		
		public static int GetInt(string _key){
			
			return PlayerPrefs.GetInt(_key);
		}
		
		public static void SetString(string _key,string _data){
			
			PlayerPrefs.SetString(_key,_data);
			
			PlayerPrefs.Save();
		}
		
		public static string GetString(string _key){
			
			return PlayerPrefs.GetString(_key);
		}
	}
}