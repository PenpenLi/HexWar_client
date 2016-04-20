using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using xy3d.tstd.lib.superTween;

namespace xy3d.tstd.lib.textureFactory{

	public class SpriteFactory {

		public static bool SetSprite(Image _img,string _name,bool _doNotDispose,bool _autoReload,bool _alphaIn,Action _loadOverCallBack,Action _reloadOverCallBack){
			
			ImageScript script = null;
			
			if(!_doNotDispose && _autoReload){
				
				script = _img.GetComponent<ImageScript>();
				
				if(script == null){
					
					script = _img.gameObject.AddComponent<ImageScript>();
				}					
				
				script.isLoading = true;
				
				script.imgName = _name;

				script.alphaIn = _alphaIn;

				script.reloadOverCallBack = _reloadOverCallBack;
			}
			
			return SetSpriteReal(_img,script,_name,_doNotDispose,_alphaIn,_loadOverCallBack);
		}
		
		internal static bool SetSpriteReal(Image _img,ImageScript _script,string _name,bool _doNotDispose,bool _alphaIn,Action _loadOverCallBack){

			Action<Sprite> callBack = delegate(Sprite obj) {
				
				GetSprite(_img,_script,_name,obj,_loadOverCallBack);
			};

			Sprite result = TextureFactory.Instance.GetTexture<Sprite>(_name,callBack,_doNotDispose);
			
			if(result == null ){

				if(_alphaIn){
				
					_img.color = new Color(1,1,1,0);
					
					Action<float> tween = delegate(float obj) {
						
						ChangeImageAlpha(_img,obj);
					};
					
					Action<Sprite> startTween = delegate(Sprite obj) {
						
						SuperTween.Instance.To(0,1,0.1f,tween,null);
					};

					TextureFactory.Instance.GetTexture<Sprite>(_name,startTween,_doNotDispose);

				}else{

					_img.color = Color.white;
				}
				
				return true;
				
			}else{
				
				_img.color = Color.white;

//				if(_loadOverCallBack != null){
//
//					_loadOverCallBack();
//				}
				
				return false;
			}
		}
		
		private static void GetSprite(Image _img,ImageScript _script,string _name,Sprite _sp,Action _loadOverCallBack){
			
			if(_img == null){
				
				return;
			}

			if(_script != null){
				
				if(_script.imgName != _name){
					
					return;
				}
				
				_script.isLoading = false;
			}

			_img.sprite = _sp;

			if(_loadOverCallBack != null){

				_loadOverCallBack();
			}
		}
		
		private static void ChangeImageAlpha(Image _img,float _alpha){

			if(_img == null){
				
				return;
			}

			_img.color = new Color(1,1,1,_alpha);
		}
	}
}