using System;
using UnityEngine;
using xy3d.tstd.lib.gameObjectFactory;
using xy3d.tstd.lib.superTween;
using xy3d.tstd.lib.textureFactory;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleSkillIcon
    {
        public const float ASSET_WIDTH = 512f / 128f;
        public const float ASSET_HEIGHT = 256f / 128f;

        public const float FONT_WIDTH = 52f / 128f;
        public const float FONT_HEIGHT = 52f / 128f;
		
		public const int unitNum = 40;
		
		private BattleSkillIconUnit[] unitVec;

        private Material mat;

        private MeshRenderer mr;

        private GameObject skillIconGO;
        private GameObject container;

        private static BattleSkillIcon _Instance;

        public static BattleSkillIcon Instance
        {

            get
            {

                if (_Instance == null)
                {

                    _Instance = new BattleSkillIcon();
                }

                return _Instance;
            }
        }
		
		public  BattleSkillIcon(){
		}

		public void Init(GameObject con, Action call){

            container = con;

            if (skillIconGO)
            {
                skillIconGO.SetActive(true);
                if (call != null)
                {
                    call();
                }
                return;
            }
			
			unitVec = new BattleSkillIconUnit[unitNum];
			

			for(int i = 0 ; i < unitNum ; i++){
				
                unitVec[i] = new BattleSkillIconUnit(i);
			}

            Action<GameObject> loadGameObject = delegate(GameObject _go)
            {
                skillIconGO = _go;
                skillIconGO.transform.SetParent(container.transform, false);
                mr = _go.GetComponent<MeshRenderer>();
                mat = mr.material;
                if (call != null)
                {
                    call();
                }
            };

            GameObjectFactory.Instance.GetGameObject("Assets/Arts/battle/BattleTool/SkillIcon.prefab", loadGameObject, true);       
		}

        public void Update()
        {
			if(mr != null){

	            for (int i = 0; i < unitNum; i++)
	            {
	                BattleSkillIconUnit unit = unitVec[i];

	                if (unit.State == 1 || unit.IsChange)
	                {
	                    if (unit.IsChange){

							unit.IsChange = false;
						}

						unit.GetPositionsVec(mr.material);

						unit.GetFixVec(mr.material);

						unit.GetScaleFix(mr.material);

//						mr.material.SetVector("positions" + i.ToString(), unit.GetPositionsVec());
//
//						mr.material.SetVector("fix" + i.ToString(), unit.GetFixVec());
//
//                        mr.material.SetVector("scaleFix" + i.ToString(), unit.GetScaleFix());
	                }
	            }
			}
        }
		
		public BattleSkillIconUnit GetSkillIcon(string _name, float _time, float _height, GameObject _go, Action<BattleSkillIconUnit,Action> endBack, Action _callBack){

            for(int i = 0; i < unitNum; i++)
            {
                if(unitVec[i].State == 0)
                {
                    BattleSkillIconUnit unit = unitVec[i];
                    unit.Init(_height, _go);

                    unit.alpha = 1;
                    unit.State = 1;

                    Action<Sprite> callBack = delegate(Sprite _texture)
                    {
                        mat.mainTexture = _texture.texture;

                        unit.endBack = endBack;
                        unit.callBack = _callBack;

                        unit.uFix = _texture.textureRect.x / 512;
                        unit.vFix = _texture.textureRect.y / 256;
                        unit.alpha = 1;

                        Action delayCall = delegate()
                        {
                            readyToDelSkillIcon(unit);
                        };

                        int id = SuperTween.Instance.DelayCall(_time, delayCall);
                        SuperTween.Instance.SetTag(id, "battle_tag");
                    };

                    TextureFactory.Instance.GetTexture("Assets/Arts/ui/skillIcon/" + _name + ".png", callBack, true);

                    return unit;
                }
                
				
			}

			throw new Exception("SkillIcon is out of use!!!");
		}
		
		private void readyToDelSkillIcon(BattleSkillIconUnit _unit){

            Action<float> toCall = delegate(float value)
            {
                _unit.alpha = value;
            };

            Action endCall = delegate()
            {
                DelSkillIcon(_unit);
            };

            int id = SuperTween.Instance.To(_unit.alpha, 0, 1, toCall, endCall);
            SuperTween.Instance.SetTag(id, "battle_tag");
		}
		
		public void DelSkillIcon(BattleSkillIconUnit _unit){

            _unit.alpha = 0;
            _unit.State = 0;
            _unit.IsChange = true;

            if (_unit.endBack != null)
            {

                Action<BattleSkillIconUnit, Action> endBack = _unit.endBack;
                endBack(_unit, _unit.callBack);
            }
		}

        public void ClearAll()
        {
			
			foreach(BattleSkillIconUnit unit in unitVec){

                DelSkillIcon(unit);
			}
		}
		
		public void Dispose()
        {
            skillIconGO.SetActive(false);
		}
    }
}
