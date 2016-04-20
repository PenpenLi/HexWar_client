using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.gameObjectFactory;
using xy3d.tstd.lib.publicTools;
using System;
using xy3d.tstd.lib.battleHeroTools;
using System.Collections.Generic;
using UnityEngine.Rendering;
using xy3d.tstd.lib.superFunction;
using xy3d.tstd.lib.animatorFactoty;

namespace xy3d.tstd.lib.heroFactory{

	//这个组件是绑在英雄身上的
	public class HeroController : MonoBehaviour
	{
		private Animator[] animators;

		public GameObject body;
		private Material bodyMaterial;

		public GameObject horse;
		private Material horseMaterial;

		public GameObject wing;
		private Material wingMaterial;

		public GameObject mainHandWeaponContainer;
		public GameObject offHandWeaponContainer;

		public GameObject[] particles;

		public GameObject mainHandWeapon;
		private Material mainHandWeaponMaterial;
		public GameObject[] mainHandWeaponParticles;

		public GameObject offHandWeapon;
		private Material offHandWeaponMaterial;
		public GameObject[] offHandWeaponParticles;

        private GameObject shadow;
        private Material shadowMat;

        public GameObject Shadow
        {
            get { return shadow; }
            set { 
                shadow = value;

                if (shadow != null)
                {
                    shadowMat = shadow.GetComponent<Renderer>().material;
                }
                else
                {
                    shadowMat = null;
                }

            }
        }

        private bool isRandomHead = true;

		public string state = "wait";

        public string playOverState = "wait";

        private float curSpeed;

        public List<string> stateList = new List<string>();

		private bool notPlayWaitWhenPoseEnd;

		private float m_Alpha = 1;

		private Color m_Color = Color.white;

		public float Alpha
		{
			get {

				return m_Alpha;
			}

			set { 

				SetMaterialAlpha(bodyMaterial,value);

				if(horseMaterial != null){

					SetMaterialAlpha(horseMaterial,value);
				}

				if(wingMaterial != null){
					
					SetMaterialAlpha(wingMaterial,value);
				}

				if(mainHandWeaponMaterial != null){
					
					SetMaterialAlpha(mainHandWeaponMaterial,value);
				}

				if(offHandWeaponMaterial != null){
					
					SetMaterialAlpha(offHandWeaponMaterial,value);
				}

                if (shadowMat != null)
                {
                    shadowMat.SetFloat("_Alpha", value);
                }

				SetAlpha(value);

				m_Alpha = value;
			}
		}

		public Color Color
		{
			get {
				
				return m_Color;
			}
			
			set { 
				
				SetMaterialColor(bodyMaterial,value);
				
				if(horseMaterial != null){
					
					SetMaterialColor(horseMaterial,value);
				}
				
				if(wingMaterial != null){
					
					SetMaterialColor(wingMaterial,value);
				}
				
				if(mainHandWeaponMaterial != null){
					
					SetMaterialColor(mainHandWeaponMaterial,value);
				}
				
				if(offHandWeaponMaterial != null){
					
					SetMaterialColor(offHandWeaponMaterial,value);
				}

				m_Color = value;
			}
		}

		private void SetMaterialAlpha(Material _material,float _alpha){

			_material.SetFloat("_AlphaXXX", _alpha);
			
			if (_alpha < 1 && Alpha == 1)
			{
				_material.renderQueue = 3000;
				_material.SetInt("_ZWrite", 0);
				
				_material.SetInt("_SrcAlpha", (int)BlendMode.SrcAlpha);
				_material.SetInt("_TarAlpha", (int)BlendMode.OneMinusSrcAlpha);
			}
			else if(_alpha == 1 && Alpha < 1)
			{
				_material.renderQueue = 2000;
				_material.SetInt("_ZWrite", 1);
				
				_material.SetInt("_SrcAlpha", (int)BlendMode.One);
				_material.SetInt("_TarAlpha", (int)BlendMode.Zero);
			}
		}

		private void SetMaterialColor(Material _material,Color _color){

			_material.SetColor("_Color",_color);
		}

		protected virtual void SetAlpha(float _alpha){


		}
		
		public void Init(){
			
			animators = gameObject.GetComponentsInChildren<Animator>();
			
			bodyMaterial = body.GetComponent<Renderer>().material;
			
			if(horse != null){
				
				horseMaterial = horse.GetComponent<Renderer>().material;
			}
			
			if(wing != null){
				
				wingMaterial = wing.GetComponent<Renderer>().material;
			}
			
			if(mainHandWeapon != null){
				
				mainHandWeaponMaterial = mainHandWeapon.GetComponent<Renderer>().material;
			}
			
			if(offHandWeapon != null){
				
				offHandWeaponMaterial = offHandWeapon.GetComponent<Renderer>().material;
			}
			
			SetPartIndex(2);
			
			SetWeaponVisible(true);

			AddShadow();
		}
		
		public void AddShadow()
		{
			GameObjectFactory.Instance.GetGameObject("Assets/Arts/battle/BattleTool/ShadowOne.prefab", ShadowLoadOK, true); 
		}
		
		private void ShadowLoadOK(GameObject _shadow){
			
			Shadow = _shadow;
			Shadow.transform.eulerAngles = new Vector3(90, 0, 0);
			Shadow.transform.SetParent(gameObject.transform, false);
			//Shadow.layer = LayerMask.NameToLayer("UI");
		}

		private void SetPartIndex(int _index){

			bodyMaterial.SetInt("_PartIndex",_index);
		}

        public void ChangeHead(int _index)
        {
            isRandomHead = false;
            SetPartIndex(_index);
        }

        private void ChangeHeadRandom()
        {
            if (isRandomHead)
            {
                float ram = UnityEngine.Random.value;

                if (ram < 0.85f)
                {
                    SetPartIndex(2);
                    ResetTigger("zhucheng_wait01");
                    PlayAnim("zhucheng_wait", true);
                }
                else
                {
                    SetPartIndex(3);
                    ResetTigger("zhucheng_wait");
                    PlayAnim("zhucheng_wait01", true);
                }
            }
        }

		public void SetWeaponVisible(bool _visible){

			if(HeroFactoryTools.MERGE_WEAPON){

				bodyMaterial.SetInt("_WeaponIndex",_visible ? -1 : 0);
			}

			if(mainHandWeaponContainer != null){

				mainHandWeaponContainer.SetActive(_visible);
			}

			if(offHandWeaponContainer != null){

				offHandWeaponContainer.SetActive(_visible);
			}
		}

        public void ResetTigger(string name)
        {
            foreach (Animator animator in animators)
            {
                animator.ResetTrigger(name);
            }
        }

        public void ResetAllTrigger()
        {
            foreach (Animator animator in animators)
            {
                foreach (string _oldState in stateList)
                {
                    animator.ResetTrigger(_oldState);
                }
            }

            stateList.Clear();
        }

		public void PlayAnim(string _state,bool _notPlayWaitWhenPoseEnd)
	    {
			state = _state;

            if (!stateList.Contains(state))
            {
                stateList.Add(state);
            }

			foreach (Animator animator in animators) {

				animator.SetTrigger(state.ToString());
			}

			notPlayWaitWhenPoseEnd = _notPlayWaitWhenPoseEnd;
		}

		private void PlayAnimOver(){

            PlayAnim(playOverState, true);
		}

	    public void SetSpeed(float speed)
	    {
            curSpeed = speed;

			foreach (Animator animator in animators) {

                animator.speed = speed;
			}
	    }

        public void IsPlay(bool b)
        {
            foreach (Animator animator in animators)
            {
                if (b)
                {
                    if (curSpeed <= 0)
                    {
                        animator.speed = 1;
                    }
                    else
                    {
                        animator.speed = curSpeed;
                    }
                }
                else
                {
                    animator.speed = 0;
                }
            }
        }

		public void DispatchAnimationEventHandler(string[] _strs)
	    {
			string eventName = _strs[1];

			switch(eventName){

			case "poseStop":

				if(!notPlayWaitWhenPoseEnd){

					PlayAnimOver();
				}

				DispatchAnimationEventUpwards(eventName,_strs);

				break;

			case "SetPartIndex":

				SetPartIndex(int.Parse(_strs[2]));

				break;

			case "ChangeHeadRandom":

				ChangeHeadRandom();

				break;

			default:

				DispatchAnimationEventUpwards(eventName,_strs);

				break;
			}
	    }

		public void DispatchAnimationEventUpwards(string _eventName,string[] _strs){

			SuperEvent superEvent = new SuperEvent(_eventName);
			
			if(_strs.Length > 2){
				
				object[] tempData = new object[_strs.Length - 2];
				
				for (int i = 2; i < _strs.Length; i++)
				{
					tempData[i - 2] = _strs[i];
				}
				
				superEvent.data = tempData;
			}
			
			SuperFunction.Instance.DispatchEvent (gameObject, superEvent);
		}

		void OnDestroy(){
			
			for (int i = 0; i < animators.Length; i++) {

				if(animators[i].runtimeAnimatorController != null){

					AnimatorFactory.Instance.DelUseNum(animators[i].runtimeAnimatorController);
				}
			}
		}
	}
}