using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using xy3d.tstd.lib.superTween;

namespace xy3d.tstd.lib.textureFactory{

	public class TextureFactory{

		private static TextureFactory _Instance;
		
		public static TextureFactory Instance {
			
			get {
				
				if (_Instance == null) {
					
					_Instance = new TextureFactory ();
				}
				
				return _Instance;
			}
		}

		public Dictionary<string,ITextureFactoryUnit> dic  = new Dictionary<string, ITextureFactoryUnit>();
		public Dictionary<string,ITextureFactoryUnit> dicWillDispose  = new Dictionary<string, ITextureFactoryUnit>();

		public T GetTexture<T> (string _name,Action<T> _callBack,bool _doNotDispose) where T:UnityEngine.Object {
			
			TextureFactoryUnit<T> unit;
			
			Dictionary<string,ITextureFactoryUnit> tmpDic;
			
			if (_doNotDispose) {
				
				tmpDic = dic;
				
			} else {
				
				tmpDic = dicWillDispose;
			}
			
			if (!tmpDic.ContainsKey (_name)) {
				
				unit = new TextureFactoryUnit<T> (_name);
				
				tmpDic.Add (_name, unit);
				
			} else {
				
				unit = tmpDic [_name] as TextureFactoryUnit<T>;
			}
			
			return unit.GetTexture(_callBack);
		}

		public void Dispose(bool _force){

			foreach (ITextureFactoryUnit unit in dicWillDispose.Values) {

				unit.Dispose ();
			}

			dicWillDispose.Clear ();

			if(_force){

				foreach(ITextureFactoryUnit unit in dic.Values){

					unit.Dispose();
				}

				dic.Clear();
			}
		}
	}
}
