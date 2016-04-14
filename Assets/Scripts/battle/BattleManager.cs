using UnityEngine;
using System.Collections;
using HexWar;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

	private const float mapUnitWidth = 30;
	private const float mapUnitScale = 50;
	private const float heroScale = 0.5f;
	private const float mapContainerYFix = 60;
	private static readonly float sqrt3 = Mathf.Sqrt (3);

	[SerializeField]
	private RectTransform cardContainer;

	[SerializeField]
	private RectTransform mapContainer;

	[SerializeField]
	private Text moneyTf;

	[SerializeField]
	private GameObject actionBt;

	private Battle battle;

	private Dictionary<int,MapUnit> mapUnitDic = new Dictionary<int, MapUnit> ();

	private Dictionary<int,HeroCard> cardDic = new Dictionary<int, HeroCard>();

	private Dictionary<int,HeroCard> heroDic = new Dictionary<int, HeroCard>();

	private HeroCard nowChooseCard;

	private HeroCard nowChooseHero;

	private bool isMine;

	private Dictionary<int,int> summonDic{

		get{

			return isMine ? battle.mSummonAction : battle.oSummonAction;
		}
	}

	private Dictionary<int,int> moveDic{

		get{
			
			return isMine ? battle.mMoveAction : battle.oMoveAction;
		}
	}

	private void WriteLog(string _str){

		Debug.Log (_str);
	}

	// Use this for initialization
	void Start () {

		Log.Init (WriteLog);

		ConfigDictionary.Instance.LoadLocalConfig(Application.streamingAssetsPath + "/local.xml");
		
		StaticData.path = ConfigDictionary.Instance.table_path;
		
		StaticData.Load<MapSDS>("map");
		
		Map.Init();
		
		StaticData.Load<HeroTypeClientSDS>("heroType");
		
		StaticData.Load<HeroSDS>("hero");
		
		Dictionary<int, HeroSDS> dic = StaticData.GetDic<HeroSDS>();
		
		Dictionary<int, IHeroSDS> newDic = new Dictionary<int, IHeroSDS>();
		
		foreach(KeyValuePair<int,HeroSDS> pair in dic)
		{
			newDic.Add(pair.Key, pair.Value);
		}
		
		Battle.Init(newDic,Map.mapDataDic);
		
		battle = new Battle ();

		battle.ClientSetCallBack (SendData, RefreshData);
		
		Connection.Instance.Init ("127.0.0.1", 1983, ReceiveData);
	}
	
	private void ReceiveData(byte[] _bytes){

		battle.ClientGetPackage (_bytes);
	}

	private void SendData(MemoryStream _ms){

		Connection.Instance.Send (_ms);
	}

	private void RefreshData(bool _isMine){

		isMine = _isMine;

		ClearMapUnits ();
		
		ClearCards ();

		ClearHeros ();

		CreateMapPanel ();

		CreateCards ();

		CreateHeros ();

		CreateMoneyTf ();
	}

	private void ClearMapUnits(){

		Dictionary<int,MapUnit>.ValueCollection.Enumerator enumerator = mapUnitDic.Values.GetEnumerator ();
		
		while (enumerator.MoveNext()) {
			
			GameObject.Destroy(enumerator.Current.gameObject);
		}
		
		mapUnitDic.Clear ();
	}

	private void ClearCards(){

		Dictionary<int,HeroCard>.ValueCollection.Enumerator enumerator2 = cardDic.Values.GetEnumerator ();
		
		while (enumerator2.MoveNext()) {
			
			GameObject.Destroy(enumerator2.Current.gameObject);
		}
		
		cardDic.Clear ();
	}

	private void ClearHeros(){

		Dictionary<int,HeroCard>.ValueCollection.Enumerator enumerator2 = heroDic.Values.GetEnumerator ();
		
		while (enumerator2.MoveNext()) {
			
			GameObject.Destroy(enumerator2.Current.gameObject);
		}
		
		heroDic.Clear ();
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
				
				go.transform.SetParent(mapContainer,false);
				
				go.transform.localPosition = new Vector3(m * mapUnitWidth * sqrt3 * 2 + ((i % 2 == 1) ? mapUnitWidth * Mathf.Sqrt(3) : 0),-i * mapUnitWidth * 3,0);

				go.transform.localScale = new Vector3(mapUnitScale,mapUnitScale,mapUnitScale);

				MapUnit unit = go.GetComponent<MapUnit>();

				mapUnitDic.Add(index,unit);

				unit.index = index;
				
				unit.SetOffVisible(true);

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
		
		mapContainer.localPosition = new Vector3 (-0.5f * (battle.mapData.mapWidth * mapUnitWidth * sqrt3 * 2) + mapUnitWidth * sqrt3,mapContainerYFix + 0.5f * (battle.mapData.mapHeight * mapUnitWidth * 3 + mapUnitWidth) - mapUnitWidth * 2, 0);
	}

	private void CreateCards(){

		cardDic.Clear ();

		Dictionary<int,int> tmpCardDic = isMine ? battle.mHandCards : battle.oHandCards;

		Dictionary<int,int>.Enumerator enumerator = tmpCardDic.GetEnumerator ();

		int index = 0;

		while (enumerator.MoveNext()) {

			if(summonDic.ContainsKey(enumerator.Current.Key)){

				continue;
			}

			GameObject go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Hero"));

			HeroCard hero = go.GetComponent<HeroCard>();

			hero.SetFrameVisible(false);

			hero.Init(enumerator.Current.Key,enumerator.Current.Value);

			cardDic.Add(enumerator.Current.Key,hero);

			go.transform.SetParent(cardContainer,false);

			float cardWidth = (go.transform as RectTransform).sizeDelta.x;
			float cardHeight = (go.transform as RectTransform).sizeDelta.y;

			(go.transform as RectTransform).anchoredPosition = new Vector2(-0.5f * cardContainer.rect.width + cardWidth * 0.5f + index * cardWidth,-0.5f * cardContainer.rect.height + cardHeight * 0.5f);

			index++;
		}
	}

	private void CreateHeros(){

		Dictionary<int,Hero>.ValueCollection.Enumerator enumerator = battle.heroDic.Values.GetEnumerator ();

		while (enumerator.MoveNext()) {

			AddHeroToMap(enumerator.Current);
		}

		Dictionary<int,int>.Enumerator enumerator2 = summonDic.GetEnumerator ();

		while (enumerator2.MoveNext()) {

			AddCardToMap(enumerator2.Current.Key,enumerator2.Current.Value);
		}
	}

	private void CreateMoneyTf(){

		moneyTf.text = GetMoney().ToString ();
	}

	public void MapUnitDown(MapUnit _mapUnit){

//		mouseDownMapUnit = _mapUnit;
//
//		Debug.Log ("mapDown:" + _mapUnit.index);
	}

	public void MapUnitEnter(MapUnit _mapUnit){

//		mouseEnterMapUnit = _mapUnit;
	}

	public void MapUnitExit(MapUnit _mapUnit){

//		mouseEnterMapUnit = null;
	}

	public void MapUnitUp(MapUnit _mapUnit){

//		if (mouseDownMapUnit == mouseEnterMapUnit) {
//
//			if (nowChooseCard != null) {
//				
//				if (battle.mapDic [_mapUnit.index] == isMine && !battle.mapBelongDic.ContainsKey (_mapUnit.index) && nowChooseCard.sds.cost <= GetMoney() && !summonDic.ContainsValue(_mapUnit.index) && !battle.heroMapDic.ContainsKey(_mapUnit.index)) {
//
//					SummonHero(nowChooseCard.uid,_mapUnit.index);
//				}
//			}
//		}
//
//		ClearNowChooseCard ();
//		
//		mouseDownMapUnit = mouseEnterMapUnit = null;
	}

	public void MapUnitUpAsButton(MapUnit _mapUnit){

		if (summonDic.ContainsValue (_mapUnit.index)) {

			HeroCard summonHero = heroDic [_mapUnit.index];

			if (nowChooseHero == null) {

				nowChooseHero = summonHero;

				nowChooseHero.SetFrameVisible (true);

			} else {

				if (nowChooseHero == summonHero) {

					UnsummonHero (summonHero.uid);

				} else {

					nowChooseHero.SetFrameVisible (false);

					nowChooseHero = summonHero;

					nowChooseHero.SetFrameVisible (true);
				}
			}
			
		} else if (battle.heroMapDic.ContainsKey (_mapUnit.index)) {

			if(battle.mapDic[_mapUnit.index] == isMine){
			
				HeroCard nowHero = heroDic [_mapUnit.index];
				
				if (nowChooseHero == null) {
					
					nowChooseHero = nowHero;
					
					nowChooseHero.SetFrameVisible (true);
					
				} else {
					
					if (nowChooseHero != nowHero) {

						nowChooseHero.SetFrameVisible (false);
						
						nowChooseHero = nowHero;
						
						nowChooseHero.SetFrameVisible (true);
					}
				}

			}else{

				if (nowChooseHero != null) {
					
					nowChooseHero.SetFrameVisible (false);
					
					nowChooseHero = null;
				}
			}

		} else if(nowChooseCard != null) {

			if (battle.mapDic [_mapUnit.index] == isMine && !battle.mapBelongDic.ContainsKey (_mapUnit.index) && nowChooseCard.sds.cost <= GetMoney ()) {
				
				SummonHero (nowChooseCard.uid, _mapUnit.index);
			}

		}else {

			if (nowChooseHero != null) {
				
				nowChooseHero.SetFrameVisible (false);

				nowChooseHero = null;
			}
		}

		ClearNowChooseCard ();
	}

	public void HeroClick(HeroCard _hero){

		if (nowChooseCard != _hero) {

			ClearNowChooseCard();

			nowChooseCard = _hero;

			nowChooseCard.SetFrameVisible(true);
		}
	}

	private void ClearNowChooseCard(){

		if (nowChooseCard != null) {
			
			nowChooseCard.SetFrameVisible(false);

			nowChooseCard = null;
		}
	}

	private void SummonHero(int _uid,int _pos){
		
		battle.ClientRequestSummon (isMine, _uid, _pos);

		CreateMoneyTf ();

		ClearCards ();

		CreateCards ();

		ClearHeros ();

		CreateHeros ();
	}

	private void UnsummonHero(int _uid){

		battle.ClientRequestUnsummon (isMine, _uid);
		
		CreateMoneyTf ();
		
		ClearCards ();
		
		CreateCards ();
		
		ClearHeros ();
		
		CreateHeros ();
	}

	private void AddHeroToMap(Hero _hero){

		GameObject go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Hero"));
		
		HeroCard hero = go.GetComponent<HeroCard>();
		
		hero.Init (_hero.uid, _hero.id, _hero.nowHp);
		
		AddHeroToMapReal (hero, _hero.pos);
	}

	private void AddCardToMap(int _uid,int _pos){

		int cardID = (isMine ? battle.mHandCards : battle.oHandCards) [_uid];

		GameObject go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Hero"));
		
		HeroCard hero = go.GetComponent<HeroCard>();

		hero.SetAlpha (0.6f);
		
		hero.Init(_uid,cardID);

		AddHeroToMapReal (hero, _pos);
	}

	private void AddHeroToMapReal(HeroCard _heroCard,int _pos){

		MapUnit mapUnit = mapUnitDic [_pos];
		
		_heroCard.SetFrameVisible(false);
		
		_heroCard.transform.SetParent (mapUnit.transform, false);
		
		_heroCard.transform.localPosition = Vector3.zero;
		
		float scale = 1 / mapUnitScale * heroScale;
		
		_heroCard.transform.localScale = new Vector3 (scale, scale, scale);
		
		_heroCard.SetMouseEnable (false);

		heroDic.Add (_pos, _heroCard);
	}

	private int GetMoney(){

		int money = isMine ? battle.mMoney : battle.oMoney;

		Dictionary<int,int> cards = isMine ? battle.mHandCards : battle.oHandCards;

		Dictionary<int,int>.KeyCollection.Enumerator enumerator = summonDic.Keys.GetEnumerator ();

		while (enumerator.MoveNext()) {

			int cardID = cards[enumerator.Current];

			HeroSDS heroSDS = StaticData.GetData<HeroSDS>(cardID);

			money -= heroSDS.cost;
		}

		return money;
	}

	public void ActionBtClick(){

		battle.ClientRequestDoAction (isMine);

		summonDic.Clear ();

		moveDic.Clear ();
	}

	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyUp(KeyCode.F5)){

			battle.ClientRequestRefreshData();
		}
	}
}
