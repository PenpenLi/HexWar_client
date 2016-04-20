using UnityEngine;
using System;
using xy3d.tstd.lib.gameObjectFactory;
using System.Collections.Generic;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleHeroShadow
    {
        public BattleHeroShadowScript battleHeroShadowScript;
        private Dictionary<GameObject, Action<BattleHeroShadowUnit>> callList = new Dictionary<GameObject, Action<BattleHeroShadowUnit>>();
        private GameObject con;
        private static BattleHeroShadow _Instance;

        public static BattleHeroShadow Instance
        {

            get
            {

                if (_Instance == null)
                {

                    _Instance = new BattleHeroShadow();
                }

                return _Instance;
            }
        }

        public BattleHeroShadow()
        {
            Action<GameObject> loadGameObject = delegate(GameObject _go)
            {
                battleHeroShadowScript = _go.GetComponent<BattleHeroShadowScript>();
                if (callList.Count > 0)
                {
                    foreach (GameObject hero in callList.Keys)
                    {
                        Action<BattleHeroShadowUnit> callBack = callList[hero];
                        BattleHeroShadowUnit unit = battleHeroShadowScript.GetShadow(hero);
                        callBack(unit);
                    }
                }
            };

            GameObjectFactory.Instance.GetGameObject("Assets/Arts/battle/BattleTool/Shadow.prefab", loadGameObject, true);            
        }

        public void SetParent(GameObject _con)
        {
            con = _con;
            if (battleHeroShadowScript != null)
            {
                battleHeroShadowScript.gameObject.transform.SetParent(_con.transform, false);
            }
        }

        public void GetShadow(GameObject _go, Action<BattleHeroShadowUnit> callBack)
        {
            if (battleHeroShadowScript != null)
            {
                BattleHeroShadowUnit unit = battleHeroShadowScript.GetShadow(_go);
                callBack(unit);
            }
            else
            {
                callList.Add(_go, callBack);
            }

        }

        public void DelShadow(BattleHeroShadowUnit _unit)
        {

            battleHeroShadowScript.DelShadow(_unit);
        }

		public void Dispose()
        {
            battleHeroShadowScript.Dispose();
		}

    }
}
