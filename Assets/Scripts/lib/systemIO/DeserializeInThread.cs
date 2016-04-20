using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace xy3d.tstd.lib.systemIO{

	public class DeserializeInThread{

		private static DeserializeInThread _Instance;

		public static DeserializeInThread Instance{

			get{

				if(_Instance == null){

					_Instance = new DeserializeInThread();
				}

				return _Instance;
			}
		}

		public void Deserialize<T>(byte[] _bytes,Action<T> _callBack){

			DeserializeInThreadUnit<T> unit = new DeserializeInThreadUnit<T>();

			unit.Init(_bytes,_callBack);
		}
	}
}
