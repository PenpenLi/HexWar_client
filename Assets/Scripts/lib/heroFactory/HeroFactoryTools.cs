using UnityEngine;
using System.Collections;
using System;
using xy3d.tstd.lib.animatorFactoty;
using xy3d.tstd.lib.publicTools;
using xy3d.tstd.lib.gameObjectFactory;
using System.Collections.Generic;

namespace xy3d.tstd.lib.heroFactory{

	public class HeroFactoryTools{

		public const bool MERGE_WEAPON = false;

		private const string WING_DUMMY_NAME = "chibang";
		
		private const string HORSE_MAIN_BONE_NAME = "Bip_horse";
		
		private const string HORSE_CHAIN_BONE_NAME = "Bone100";

        public static void GetHero<T>(string _bodyPath, string _animatorControllerPath, bool _hasWeapon, string _mainHandWeaponPath, string _mainHandWeaponJoint, string[] _mainHandParticles, string _offHandWeaponPath, string _offHandWeaponJoint, string[] _offHandParticles, float _weaponScale, string _horsePath, string _horseAnimatorControllerPath, float _horseScale, string[] _particlePaths, string[] _particleJoints, string _wingPath, string _wingAnimatorControllerPath, Action<GameObject> _callBack, bool _addHeroControl, bool isDownHouse) where T : HeroController
        {
			GameObject hero = null;
			RuntimeAnimatorController animatorController = null;
			GameObject mainHandWeapon = null;
			GameObject offHandWeapon = null;
			GameObject horse = null;
			GameObject[] particles = null;
			GameObject[] mainHandWeaponParticles = null;
			GameObject[] offHandWeaponParticles = null;
			GameObject wing = null;

			int loadNum = 2;//预先加的1  主体皮肤
			
			//加载AnimatorController
			Action<RuntimeAnimatorController> callBack2 = delegate(RuntimeAnimatorController result) {
				
				animatorController = result;

				AnimatorLoadOK(hero,result,_addHeroControl);

//                OneLoadOK<T>(ref loadNum, _mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, hero, animatorController, mainHandWeapon, offHandWeapon, horse, particles, mainHandWeaponParticles, offHandWeaponParticles, wing, _callBack, _addHeroControl, isDownHouse);
			};

			AnimatorFactory.Instance.GetAnimator(_animatorControllerPath, callBack2);
			
			//加载主体
			Action<GameObject> callBack = delegate(GameObject _hero) {
				
				PublicTools.SetGameObjectVisible(_hero,false);
				
				hero = _hero;

                OneLoadOK<T>(ref loadNum, _mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, hero, animatorController, mainHandWeapon, offHandWeapon, horse, particles, mainHandWeaponParticles, offHandWeaponParticles, wing, _callBack, _addHeroControl, isDownHouse);
			};
			
			if(!_addHeroControl){
				
				GameObjectFactory.Instance.GetGameObject (_bodyPath, callBack, true);

                OneLoadOK<T>(ref loadNum, _mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, hero, animatorController, mainHandWeapon, offHandWeapon, horse, particles, mainHandWeaponParticles, offHandWeaponParticles, wing, _callBack, _addHeroControl, isDownHouse);
				
				return;
			}
			
			//加载武器
			if (_hasWeapon) {

				List<string> weaponList = new List<string>();
				List<string> jointList = new List<string>();
				List<float> weaponScaleList = new List<float>();
				
				if(!string.IsNullOrEmpty(_mainHandWeaponPath)){
					
					if(MERGE_WEAPON){
						
						weaponList.Add(_mainHandWeaponPath);
						jointList.Add(_mainHandWeaponJoint);
						weaponScaleList.Add(_weaponScale);
						
					}else{
						
						loadNum++;
						
						Action<GameObject> weaponCallBack = delegate(GameObject _go) {
							
							PublicTools.SetGameObjectVisible(_go,false);
							
							mainHandWeapon = _go;

                            OneLoadOK<T>(ref loadNum, _mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, hero, animatorController, mainHandWeapon, offHandWeapon, horse, particles, mainHandWeaponParticles, offHandWeaponParticles, wing, _callBack, _addHeroControl, isDownHouse);
						};
						
						GameObjectFactory.Instance.GetGameObject (_mainHandWeaponPath, weaponCallBack, true);
					}
					
					mainHandWeaponParticles = new GameObject[_mainHandParticles.Length];
					
					for (int i = 0; i < _mainHandParticles.Length; i++) {
						
						loadNum++;
						
						int particleIndex = i;
						
						Action<GameObject> mainHandWeaponCallBack = delegate(GameObject _go) {
							
							PublicTools.SetGameObjectVisible(_go,false);
							
							mainHandWeaponParticles[particleIndex] = _go;

                            OneLoadOK<T>(ref loadNum, _mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, hero, animatorController, mainHandWeapon, offHandWeapon, horse, particles, mainHandWeaponParticles, offHandWeaponParticles, wing, _callBack, _addHeroControl, isDownHouse);
						};
						
						GameObjectFactory.Instance.GetGameObject(_mainHandParticles[i],mainHandWeaponCallBack,true);
					}
				}
				
				if(!string.IsNullOrEmpty(_offHandWeaponPath)){
					
					if(MERGE_WEAPON){
						
						weaponList.Add(_offHandWeaponPath);
						jointList.Add(_offHandWeaponJoint);
						weaponScaleList.Add(_weaponScale);
						
					}else{
						
						loadNum++;
						
						Action<GameObject> weaponCallBack = delegate(GameObject _go) {
							
							PublicTools.SetGameObjectVisible(_go,false);
							
							offHandWeapon = _go;

                            OneLoadOK<T>(ref loadNum, _mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, hero, animatorController, mainHandWeapon, offHandWeapon, horse, particles, mainHandWeaponParticles, offHandWeaponParticles, wing, _callBack, _addHeroControl, isDownHouse);
						};
						
						GameObjectFactory.Instance.GetGameObject (_offHandWeaponPath, weaponCallBack, true);
					}
					
					offHandWeaponParticles = new GameObject[_offHandParticles.Length];
					
					for (int i = 0; i < _offHandParticles.Length; i++) {
						
						loadNum++;
						
						int particleIndex = i;
						
						Action<GameObject> offHandWeaponCallBack = delegate(GameObject _go) {
							
							PublicTools.SetGameObjectVisible(_go,false);
							
							offHandWeaponParticles[particleIndex] = _go;

                            OneLoadOK<T>(ref loadNum, _mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, hero, animatorController, mainHandWeapon, offHandWeapon, horse, particles, mainHandWeaponParticles, offHandWeaponParticles, wing, _callBack, _addHeroControl, isDownHouse);
						};
						
						GameObjectFactory.Instance.GetGameObject(_offHandParticles[i],offHandWeaponCallBack,true);
					}
				}
				
				if(MERGE_WEAPON){
					
					GameObjectFactory.Instance.GetGameObject (_bodyPath, callBack, weaponList.ToArray(), jointList.ToArray(), weaponScaleList.ToArray(), true, false);
					
				}else{
					
					GameObjectFactory.Instance.GetGameObject (_bodyPath, callBack, true);
				}
				
			} else {
				
				GameObjectFactory.Instance.GetGameObject (_bodyPath, callBack, true);
			}
			
			//加载马
			if (!string.IsNullOrEmpty(_horsePath)) {
				
				loadNum++;
				
				callBack = delegate(GameObject _go) {
					
					PublicTools.SetGameObjectVisible(_go,false);
					
					horse = _go;

                    OneLoadOK<T>(ref loadNum, _mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, hero, animatorController, mainHandWeapon, offHandWeapon, horse, particles, mainHandWeaponParticles, offHandWeaponParticles, wing, _callBack, _addHeroControl, isDownHouse);
				};

                GetHero<T>(_horsePath, _horseAnimatorControllerPath, false, null, null, null, null, null, null, 0, null, null, 0, null, null, null, null, callBack, false, false);
			}
			
			//加载身上的粒子
			if (_particlePaths.Length > 0) {
				
				particles = new GameObject[_particlePaths.Length];
				
				for(int i = 0 ; i < _particlePaths.Length ; i++){
					
					loadNum++;
					
					int tmpIndex = i;
					
					callBack = delegate(GameObject _go) {
						
						PublicTools.SetGameObjectVisible(_go,false);
						
						particles[tmpIndex] = _go;

                        OneLoadOK<T>(ref loadNum, _mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, hero, animatorController, mainHandWeapon, offHandWeapon, horse, particles, mainHandWeaponParticles, offHandWeaponParticles, wing, _callBack, _addHeroControl, isDownHouse);
					};
					
					GameObjectFactory.Instance.GetGameObject(_particlePaths[i],callBack,true);
				}
			}
			
			//加载翅膀
			if(!string.IsNullOrEmpty(_wingPath)){
				
				loadNum++;
				
				callBack = delegate (GameObject _go){
					
					PublicTools.SetGameObjectVisible(_go,false);
					
					wing = _go;

                    OneLoadOK<T>(ref loadNum, _mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, hero, animatorController, mainHandWeapon, offHandWeapon, horse, particles, mainHandWeaponParticles, offHandWeaponParticles, wing, _callBack, _addHeroControl, isDownHouse);
				};

                GetHero<T>(_wingPath, _wingAnimatorControllerPath, false, null, null, null, null, null, null, 0, null, null, 0, null, null, null, null, callBack, false, false);
			}

            OneLoadOK<T>(ref loadNum, _mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, hero, animatorController, mainHandWeapon, offHandWeapon, horse, particles, mainHandWeaponParticles, offHandWeaponParticles, wing, _callBack, _addHeroControl, isDownHouse);
		}

        private static void OneLoadOK<T>(ref int _loadNum, string _mainHandWeaponJoint, string _offHandWeaponJoint, float _weaponScale, float _horseScale, string[] _particleJoints, GameObject _hero, RuntimeAnimatorController _animatorController, GameObject _mainHandWeapon, GameObject _offHandWeapon, GameObject _horse, GameObject[] _particles, GameObject[] _mainHandWeaponParticles, GameObject[] _offHandWeaponParticles, GameObject _wing, Action<GameObject> _callBack, bool _addHeroControl, bool isDownHouse) where T : HeroController
        {
			_loadNum--;
			
			if (_loadNum == 0) {

                AllLoadOK<T>(_mainHandWeaponJoint, _offHandWeaponJoint, _weaponScale, _horseScale, _particleJoints, _hero, _animatorController, _mainHandWeapon, _offHandWeapon, _horse, _particles, _mainHandWeaponParticles, _offHandWeaponParticles, _wing, _callBack, _addHeroControl, isDownHouse);
			}
		}

		private static void AnimatorLoadOK(GameObject _hero,RuntimeAnimatorController _animatorController,bool _addHeroControl){

			if(_hero != null){

				_hero.GetComponent<Animator> ().runtimeAnimatorController = _animatorController;

				AnimatorFactory.Instance.AddUseNum(_animatorController);

				if(_addHeroControl){

					HeroControllerDispatcher heroControllerDispatcher = _hero.GetComponent<HeroControllerDispatcher>();

					if(heroControllerDispatcher == null){

						_hero.AddComponent<HeroControllerDispatcher>();
					}
				}
			}
		}
		
		private static void AllLoadOK<T>(string _mainHandWeaponJoint,string _offHandWeaponJoint, float _weaponScale, float _horseScale,string[] _particleJoints,GameObject _hero,RuntimeAnimatorController _animatorController,GameObject _mainHandWeapon,GameObject _offHandWeapon,GameObject _horse,GameObject[] _particles,GameObject[] _mainHandWeaponParticles,GameObject[] _offHandWeaponParticles,GameObject _wing,Action<GameObject> _callBack,bool _addHeroControl, bool isDownHouse) where T:HeroController{

			Animator animator = _hero.GetComponent<Animator> ();

			if(animator.runtimeAnimatorController == null && _animatorController != null){

				animator.runtimeAnimatorController = _animatorController;

				AnimatorFactory.Instance.AddUseNum(_animatorController);
			}
			
			if(_addHeroControl){
				
				HeroController heroController = null;
				
				GameObject horseContainer = null;
				
				GameObject body = _hero.GetComponentInChildren<Renderer>().gameObject;
				
				HeroControllerDispatcher heroControllerDispatcher = _hero.GetComponent<HeroControllerDispatcher>();
				
				if(heroControllerDispatcher == null){
					
					_hero.AddComponent<HeroControllerDispatcher>();
				}
				
				if (_horse != null) {
					
					horseContainer = new GameObject(_hero.name + "ma");
					
					_horse.transform.SetParent(horseContainer.transform,false);
					
					heroController = horseContainer.AddComponent<T> ();
					
					heroController.horse = _horse.GetComponentInChildren<Renderer>().gameObject;
					
					_horse.transform.localScale = new Vector3(_horseScale,_horseScale,_horseScale);

                    if (isDownHouse)
                    {

                        _hero.transform.localPosition = Vector3.zero;

                        _hero.transform.SetParent(horseContainer.transform, false);
                    }
                    else
                    {
                        GameObject bone = PublicTools.FindChild(_horse, HORSE_CHAIN_BONE_NAME);

                        bone.transform.localScale = new Vector3(1 / _horseScale, 1 / _horseScale, 1 / _horseScale);

                        _hero.transform.SetParent(bone.transform, true);

                        _hero.transform.localPosition = Vector3.zero;
                    }
					
				}else{
					
					heroController = _hero.AddComponent<T> ();
				}
				
				heroController.body = body;
				
				if (_particles != null) {
					
					for(int i = 0 ; i < _particles.Length ; i++){
						
						PublicTools.AddChild(_hero,_particles[i],_particleJoints[i]);
					}
					
					heroController.particles = _particles;
				}

				if(_mainHandWeapon != null){
					
					GameObject mainWeaponContainer = new GameObject();//为了让武器上的粒子配合武器进行缩放  所以添加了这个容器
					
					mainWeaponContainer.name = "mainHandWeapon";
					
					heroController.mainHandWeaponContainer = mainWeaponContainer;
					
					mainWeaponContainer.transform.localScale = new Vector3(_weaponScale,_weaponScale,_weaponScale);
					
					PublicTools.AddChild(_hero,mainWeaponContainer,_mainHandWeaponJoint);
					
					if(!MERGE_WEAPON){
						
						heroController.mainHandWeapon = _mainHandWeapon;
						
						_mainHandWeapon.transform.SetParent(mainWeaponContainer.transform,false);
					}
					
					if (_mainHandWeaponParticles != null) {
						
						heroController.mainHandWeaponParticles = _mainHandWeaponParticles;
						
						for (int i = 0; i < _mainHandWeaponParticles.Length; i++) {
							
							_mainHandWeaponParticles[i].transform.SetParent(mainWeaponContainer.transform,false);
						}
					}
				}
				
				if(_offHandWeapon != null){
					
					GameObject offWeaponContainer = new GameObject();//为了让武器上的粒子配合武器进行缩放  所以添加了这个容器
					
					offWeaponContainer.name = "offHandWeapon";
					
					heroController.offHandWeaponContainer = offWeaponContainer;
					
					offWeaponContainer.transform.localScale = new Vector3(_weaponScale,_weaponScale,_weaponScale);
					
					PublicTools.AddChild(_hero,offWeaponContainer,_offHandWeaponJoint);
					
					if(!MERGE_WEAPON){
						
						heroController.offHandWeapon = _offHandWeapon;
						
						_offHandWeapon.transform.SetParent(offWeaponContainer.transform,false);
					}
					
					if (_offHandWeaponParticles != null) {
						
						heroController.offHandWeaponParticles = _offHandWeaponParticles;
						
						for (int i = 0; i < _offHandWeaponParticles.Length; i++) {
							
							_offHandWeaponParticles[i].transform.SetParent(offWeaponContainer.transform,false);
						}
					}
				}
				
				if(_wing != null){
					
					GameObject heroDummy = PublicTools.FindChild(_hero,WING_DUMMY_NAME);
					GameObject wingDummy = PublicTools.FindChild(_wing,WING_DUMMY_NAME);
					
					_wing.transform.localPosition = (wingDummy.transform.worldToLocalMatrix * wingDummy.transform.parent.localToWorldMatrix).MultiplyPoint(_wing.transform.localPosition);
					
					_wing.transform.SetParent(heroDummy.transform,false);
					
					heroController.wing = _wing.GetComponentInChildren<Renderer>().gameObject;
				}
				
				heroController.Init();
				
				if(_horse != null) {
					
					PublicTools.SetGameObjectVisible(horseContainer,true);
					
					_callBack (horseContainer);
					
				}else{
					
					PublicTools.SetGameObjectVisible(_hero,true);
					
					_callBack (_hero);
				}
				
			}else{
				
				_callBack(_hero);
			}
		}
	}
}