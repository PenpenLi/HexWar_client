using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

namespace xy3d.tstd.lib.assetManager{

	public class AssetManagerDataFactory {

		public static void SetData(BinaryWriter _bw,List<string> _assetNames,List<string> _assetBundleNames,Dictionary<string,List<string>> _result){

			_bw.Write(_assetNames.Count);

			for(int i = 0 ; i < _assetNames.Count ; i++){

				_bw.Write(_assetNames[i]);

				_bw.Write(_assetBundleNames[i]);

				List<string> tmpList = _result[_assetNames[i]];

				_bw.Write(tmpList.Count);

				for(int m = 0 ; m < tmpList.Count ; m++){

					_bw.Write(tmpList[m]);
				}
			}
		}

		public static void SetData(BinaryWriter _bw,Dictionary<string,AssetManagerData> _dataDic){

			_bw.Write(_dataDic.Count);

			Dictionary<string,AssetManagerData>.Enumerator enumerator = _dataDic.GetEnumerator();

			while(enumerator.MoveNext()){

				_bw.Write(enumerator.Current.Key);

				AssetManagerData data = enumerator.Current.Value;

				_bw.Write(data.assetBundle);

				if(data.assetBundleDep != null){

					_bw.Write(data.assetBundleDep.Length);

					for(int i = 0 ; i < data.assetBundleDep.Length ; i++){

						_bw.Write(data.assetBundleDep[i]);
					}

				}else{

					_bw.Write(0);
				}
			}
		}

		public static Dictionary<string,AssetManagerData> GetData(byte[] _bytes){

			Dictionary<string,AssetManagerData> dic = new Dictionary<string, AssetManagerData>();

			MemoryStream ms = new MemoryStream(_bytes);

			BinaryReader br = new BinaryReader(ms);

			int assetLength = br.ReadInt32();

			for(int i = 0 ; i < assetLength ; i++){

				string assetName = br.ReadString();

				AssetManagerData unit = new AssetManagerData();

				unit.assetBundle = br.ReadString();

				int length = br.ReadInt32();

				if(length > 0){

					unit.assetBundleDep = new string[length];

					for(int m = 0 ; m < length ; m++){

						unit.assetBundleDep[m] = br.ReadString();
					}
				}

				dic.Add(assetName,unit);
			}

			br.Close();

			ms.Close();

			ms.Dispose();

			return dic;
		}
	}
}