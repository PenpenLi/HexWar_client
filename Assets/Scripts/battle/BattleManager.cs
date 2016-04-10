using UnityEngine;
using System.Collections;
using HexWar;
using System.IO;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

	private float unitWidth = 0.6f;
	private static readonly float sqrt3 = Mathf.Sqrt (3);

	private Battle battle;

	private List<MapUnit> mapUnitList = new List<MapUnit> ();

	private bool isMine;

	// Use this for initialization
	void Start () {

		ConfigDictionary.Instance.LoadLocalConfig(Application.streamingAssetsPath + "/local.xml");
		
		StaticData.path = ConfigDictionary.Instance.table_path;
		
		StaticData.Load<MapSDS>("map");
		
		Map.Init();
		
		StaticData.Load<HeroTypeSDS>("heroType");
		
		StaticData.Load<HeroSDS>("hero");
		
		Dictionary<int, HeroSDS> dic = StaticData.GetDic<HeroSDS>();
		
		Dictionary<int, IHeroSDS> newDic = new Dictionary<int, IHeroSDS>();
		
		foreach(KeyValuePair<int,HeroSDS> pair in dic)
		{
			newDic.Add(pair.Key, pair.Value);
		}
		
		Battle.Init(newDic,Map.mapDataDic);
		
		battle = new Battle ();
		
		Connection.Instance.Init ("127.0.0.1", 1983, ReceiveData);
	}
	
	private void ReceiveData(byte[] _bytes){

		using (MemoryStream ms = new MemoryStream(_bytes)) {

			using(BinaryReader br = new BinaryReader(ms)){

				isMine = br.ReadBoolean();

				battle.ClientRefreshData(br,isMine);

				RefreshData();
			}
		}
	}

	private void RefreshData(){

		ClearAll ();

		CreateMapPanel ();
	}

	private void ClearAll(){

		for (int i = 0; i < mapUnitList.Count; i++) {

			GameObject.Destroy(mapUnitList[i].gameObject);
		}

		mapUnitList.Clear ();
	}

	private void CreateMapPanel(){
		
		int index = 0;
		
		for (int i = 0; i < battle.mapData.mapHeight; i++) {
			
			for(int m = 0 ; m < battle.mapData.mapWidth ; m++){
				
				if(i % 2 == 1 && m == battle.mapData.mapWidth - 1){
					
					continue;
				}

				if(!battle.mapData.dic.ContainsKey(index)){

					index++;

					continue;
				}
				
				GameObject go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("MapUnit"));
				
				go.transform.SetParent(transform,false);
				
				go.transform.localPosition = new Vector3(m * unitWidth * sqrt3 * 2 + ((i % 2 == 1) ? unitWidth * Mathf.Sqrt(3) : 0),-i * unitWidth * 3,0);
				
				MapUnit unit = go.GetComponent<MapUnit>();

				mapUnitList.Add(unit);
				
				unit.index = index;
				
				unit.SetOffVisible(false);

				if(battle.mapData.dic[index] == isMine){

					unit.SetMainColor(Color.green);

					if(battle.mapBelongDic.ContainsKey(index)){
						
						unit.SetOffColor(Color.red);

					}else{

						unit.SetOffColor(Color.green);
					}
					
				}else{
					
					unit.SetMainColor(Color.red);

					if(battle.mapBelongDic.ContainsKey(index)){
						
						unit.SetOffColor(Color.green);
						
					}else{
						
						unit.SetOffColor(Color.red);
					}
				}
					
				index++;
			}
		}
		
		transform.localPosition = new Vector3 (-0.5f * (battle.mapData.mapWidth * unitWidth * sqrt3 * 2) + unitWidth * sqrt3, 0.5f * (battle.mapData.mapHeight * unitWidth * 3 + unitWidth) - unitWidth * 2, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
