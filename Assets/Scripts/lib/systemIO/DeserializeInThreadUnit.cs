using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using xy3d.tstd.lib.thread;

namespace xy3d.tstd.lib.systemIO{

	public class DeserializeInThreadUnit<T>{

		private Action<T> callBack;
		private byte[] bytes;
		private T result;

		public void Init(byte[] _bytes,Action<T> _callBack){

			bytes = _bytes;

			callBack = _callBack;

			ThreadScript.Instance.Add(GetData,LoadOver,CheckLoadOver);
		}

		private void GetData(){

			using (Stream stream = new MemoryStream(bytes))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				
//				formatter.Binder = new VersionBinder();
				
				result = (T) formatter.Deserialize(stream);
			}				
		}

		private void LoadOver(){

			callBack(result);
		}

		private bool CheckLoadOver(){

			return result != null;
		}
	}
}