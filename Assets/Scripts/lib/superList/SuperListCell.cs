using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using xy3d.tstd.lib.superTween;
using System;

namespace xy3d.tstd.lib.superList{

	public class SuperListCell : MonoBehaviour,IPointerClickHandler {

		protected object data;
		protected bool selected;

		public CanvasGroup canvasGroup;

		[HideInInspector]public int index;

		public void OnPointerClick (PointerEventData eventData)
		{
            if(data != null)
			SendMessageUpwards("CellClick",this);
		}  
		
		// Update is called once per frame
		void Update () {
		
		}

		public virtual bool SetData(object _data){

			data = _data;

			return false;
		}

		public virtual void SetSelected(bool _value){

			selected = _value;

			//由于Rect2DMask有bug  所以添加这2行2B代码
		}

		public bool GetSelected()
		{
			return selected;
		}

		private int tweenID;

		public void AlphaIn(Action _callBack){

			Action dele = delegate() {

				tweenID = -1;

				_callBack();
			};

			tweenID = SuperTween.Instance.To(0,1,0.1f,AlphaInDel,dele);
		}

		public void StopAlphaIn(){

			if(tweenID != -1){

				SuperTween.Instance.Remove(tweenID);
			}
		}

		private void AlphaInDel(float _value){

			canvasGroup.alpha = _value;
		}
	}
}
