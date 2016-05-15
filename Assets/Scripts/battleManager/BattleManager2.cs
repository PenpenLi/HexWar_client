using UnityEngine;
using System.Collections;
using HexWar;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using xy3d.tstd.lib.superTween;
using System;

public class BattleManager2 : MonoBehaviour {
	
	private static readonly Color summonColor = new Color (0.2f, 0.8f, 0.8f);
	
	private const float arrowZFix = -5;
	
	private const float mapUnitWidth = 30;
	private const float mapUnitScale = 50;
	private const float heroScale = 0.5f;
	private const float mapContainerYFix = 60;
	private static readonly float sqrt3 = Mathf.Sqrt (3);
	
	[SerializeField]
	private GraphicRaycaster graphicRayCaster;
	
	[SerializeField]
	private RectTransform battleContainer;
	
	[SerializeField]
	private RectTransform cardContainer;
	
	[SerializeField]
	private RectTransform mapContainer;
	
	[SerializeField]
	private RectTransform heroContainer;
	
	[SerializeField]
	private RectTransform arrowContainer;
	
	[SerializeField]
	private Text myMoneyTf;

	[SerializeField]
	private Text oppMoneyTf;

	[SerializeField]
	private Text oppCardNumTf;

	[SerializeField]
	private GameObject isSkipGo;
	
	[SerializeField]
	private GameObject actionBt;
	
	[SerializeField]
	private HeroDetail heroDetail;
	
	[SerializeField]
	private RoundNum roundNum;
	
	private Battle2 battle;
	
	private Dictionary<int,MapUnit> mapUnitDic = new Dictionary<int, MapUnit> ();
	
	private Dictionary<int,HeroCard> cardDic = new Dictionary<int, HeroCard>();
	
	private Dictionary<int,HeroCard> heroDic = new Dictionary<int, HeroCard>();

	private HeroCard summonHero;

	private Arrow arrow;

	private bool isTweening = false;

	private List<byte[]> packagePool = new List<byte[]>();

//	private HeroCard m_nowChooseCard;
//	
//	private HeroCard nowChooseCard{
//		
//		get{
//			
//			return m_nowChooseCard;
//		}
//		
//		set{
//			
//			if(value == null){
//				
//				heroDetail.Hide();
//				
//			}else{
//				
//				heroDetail.Init(value);
//			}
//			
//			m_nowChooseCard = value;
//		}
//	}
	
	private HeroCard m_nowChooseHero;
	
	private HeroCard nowChooseHero{
		
		get{
			
			return m_nowChooseHero;
		}
		
		set{
			
			if(value == null){
				
				heroDetail.Hide();
				
			}else{
				
				heroDetail.Init(value);
			}
			
			m_nowChooseHero = value;
		}
	}
	
	private int movingHeroPos = -1;

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
		
		Dictionary<int, HeroSDS> heroDic = StaticData.GetDic<HeroSDS>();
		
		Dictionary<int, IHeroSDS> newHeroDic = new Dictionary<int, IHeroSDS>();
		
		foreach(KeyValuePair<int,HeroSDS> pair in heroDic)
		{
			newHeroDic.Add(pair.Key, pair.Value);
		}

		StaticData.Load<SkillSDS> ("skill");

		Dictionary<int, SkillSDS> skillDic = StaticData.GetDic<SkillSDS>();
		
		Dictionary<int, ISkillSDS> newSkillDic = new Dictionary<int, ISkillSDS>();
		
		foreach(KeyValuePair<int,SkillSDS> pair in skillDic)
		{
			newSkillDic.Add(pair.Key, pair.Value);
		}
		
		Battle2.Init(newHeroDic,Map.mapDataDic,newSkillDic);
		
		battle = new Battle2 ();
		
		battle.ClientSetCallBack (SendData, ClientRefreshData, DoSummon, DoMove, DoAction);
		
		Connection.Instance.Init ("127.0.0.1", 1983, ReceiveData);
	}
	
	private void ReceiveData(byte[] _bytes){

		if (isTweening) {

			packagePool.Add (_bytes);

		} else {
		
			battle.ClientGetPackage (_bytes);
		}
	}
	
	private void SendData(MemoryStream _ms){
		
		Connection.Instance.Send (_ms);
	}

	private void ClientRefreshData(){

		RefreshData ();

		SetIsTweening (false);
	}
	
	private void RefreshData(){
		
		heroDetail.Hide();
		
		ClearMapUnits ();
		
		ClearCards ();
		
		ClearSummonHeros ();
		
		ClearHeros ();
		
		ClearMoves ();
		
		CreateMapPanel ();
		
		CreateCards ();
		
		CreateSummonHeros ();
		
		CreateHeros ();
		
		CreateMoves ();
		
		CreateMoneyTf ();
		
		RefreshTouchable ();
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
	
	private void ClearSummonHeros(){

		if (summonHero != null) {

			GameObject.Destroy(summonHero.gameObject);

			summonHero = null;
		}
	}
	
	private void ClearHeros(){
		
		Dictionary<int,HeroCard>.ValueCollection.Enumerator enumerator2 = heroDic.Values.GetEnumerator ();
		
		while (enumerator2.MoveNext()) {
			
			GameObject.Destroy(enumerator2.Current.gameObject);
		}
		
		heroDic.Clear ();
	}
	
	private void ClearMoves(){

		if (arrow != null) {

			GameObject.Destroy(arrow.gameObject);

			arrow = null;
		}
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
				
				if(battle.mapDic[index] == battle.clientIsMine){
					
					unit.SetMainColor(Color.green);
					
					unit.SetOffColor(Color.green);
					
				}else{
					
					unit.SetMainColor(Color.red);
					
					unit.SetOffColor(Color.red);
				}
				
				index++;
			}
		}
		
		battleContainer.localPosition = new Vector3 (-0.5f * (battle.mapData.mapWidth * mapUnitWidth * sqrt3 * 2) + mapUnitWidth * sqrt3,mapContainerYFix + 0.5f * (battle.mapData.mapHeight * mapUnitWidth * 3 + mapUnitWidth) - mapUnitWidth * 2, 0);
	}
	
	private void CreateCards(){
		
		cardDic.Clear ();
		
		Dictionary<int,int> tmpCardDic = battle.clientIsMine ? battle.mHandCards : battle.oHandCards;
		
		Dictionary<int,int>.Enumerator enumerator = tmpCardDic.GetEnumerator ();
		
		int index = 0;
		
		while (enumerator.MoveNext()) {

			if(battle.clientActionType == ActionType.SUMMON && battle.clientActionData0 == enumerator.Current.Key){

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
	
	private void CreateSummonHeros(){

		if (battle.clientActionType == ActionType.SUMMON) {

			AddCardToMap(battle.clientActionData0,battle.clientActionData1);
		}
	}
	
	private void CreateHeros(){
		
		Dictionary<int,Hero2>.ValueCollection.Enumerator enumerator = battle.heroMapDic.Values.GetEnumerator ();
		
		while (enumerator.MoveNext()) {
			
			AddHeroToMap(enumerator.Current);
		}
	}
	
	private void CreateMoneyTf(){
		
		myMoneyTf.text = GetMoney().ToString ();

		oppMoneyTf.text = (battle.clientIsMine ? battle.oMoney : battle.mMoney).ToString();

		oppCardNumTf.text = battle.clientOppHandCardsNum.ToString ();

		isSkipGo.SetActive (battle.isSkip);
	}
	
	private void CreateMoves(){

		if(battle.clientActionType == ActionType.MOVE){
			
			int pos = battle.clientActionData0;
			
			int direction = battle.clientActionData1;
			
			GameObject go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Arrow"));
			
			arrow = go.GetComponent<Arrow>();
			
			go.transform.SetParent(arrowContainer,false);
			
			go.transform.localPosition = new Vector3( mapUnitDic[pos].transform.localPosition.x, mapUnitDic[pos].transform.localPosition.y,arrowZFix);
			
			go.transform.localScale = new Vector3(mapUnitScale,mapUnitScale,mapUnitScale);
			
			go.transform.eulerAngles = new Vector3(0,0,60 - direction * 60);
		}
	}
	
	public void MapUnitDown(MapUnit _mapUnit){
		
		if (battle.mapDic[_mapUnit.index] == battle.clientIsMine && heroDic.ContainsKey (_mapUnit.index)) {
			
			HeroCard heroCard = heroDic[_mapUnit.index];
			
			if(nowChooseHero == heroCard){
				
				Hero2 hero = battle.heroMapDic[_mapUnit.index];
				
				if(!hero.isSummon && hero.nowPower > 0){

					if(battle.clientActionType == ActionType.MOVE && battle.clientActionData0 == hero.pos){

						movingHeroPos = hero.pos;

						battle.ClientRequestUndoAction();

						ClearMoves();

					}else if(battle.clientActionType == ActionType.NONE){

						movingHeroPos = hero.pos;
					}
				}
			}
		}
	}
	
	public void MapUnitEnter(MapUnit _mapUnit){
		
		if (movingHeroPos != -1) {

			if(!battle.heroMapDic.ContainsKey(_mapUnit.index)){
				
				Hero2 hero = battle.heroMapDic[movingHeroPos];
				
				int dis = _mapUnit.index - hero.pos;
				
				int direction = -1;
				
				if(dis == 1){
					
					direction = 1;
					
				}else if(dis == battle.mapData.mapWidth){
					
					direction = 2;
					
				}else if(dis == battle.mapData.mapWidth - 1){
					
					direction = 3;
					
				}else if(dis == -1){
					
					direction = 4;
					
				}else if(dis == -battle.mapData.mapWidth){
					
					direction = 5;
					
				}else if(dis == -battle.mapData.mapWidth + 1){
					
					direction = 0;
				}
				
				if(direction != -1){
					
					battle.ClientRequestMove(movingHeroPos,direction);
					
					ClearMoves();
					
					CreateMoves();
				}
			}
		}
	}
	
	public void MapUnitExit(MapUnit _mapUnit){
		
		if (movingHeroPos != -1) {
			
			if(battle.clientActionType == ActionType.MOVE && battle.clientActionData0 == movingHeroPos){

				battle.ClientRequestUndoAction();
				
				ClearMoves();
			}
		}
	}
	
	public void MapUnitUp(MapUnit _mapUnit){
		
		if (movingHeroPos != -1) {
			
			movingHeroPos = -1;
		}
	}
	
	public void MapUnitUpAsButton(MapUnit _mapUnit){

		if (battle.clientActionType == ActionType.SUMMON && battle.clientActionData1 == _mapUnit.index) {
			
			if (nowChooseHero == null) {
				
				nowChooseHero = summonHero;
				
				nowChooseHero.SetFrameVisible (true);
				
			} else {
				
				if (nowChooseHero == summonHero) {
					
					nowChooseHero = null;
					
					UnsummonHero (summonHero.cardUid);
					
				} else {
					
					ClearNowChooseHero ();
					
					nowChooseHero = summonHero;
					
					nowChooseHero.SetFrameVisible (true);
				}
			}
			
		}else if (battle.heroMapDic.ContainsKey (_mapUnit.index)) {

			HeroCard nowHero = heroDic [_mapUnit.index];

			if (nowChooseHero == null) {
					
				nowChooseHero = nowHero;
				
				nowChooseHero.SetFrameVisible (true);
				
			} else {
				
				if (nowChooseHero != nowHero) {
					
					ClearNowChooseHero();
					
					nowChooseHero = nowHero;
					
					nowChooseHero.SetFrameVisible (true);
				}
			}

		} else if(summonHero == null && nowChooseHero != null && nowChooseHero.cardUid != -1 && battle.mapDic [_mapUnit.index] == battle.clientIsMine && nowChooseHero.sds.cost <= GetMoney ()) {

			SummonHero (nowChooseHero.cardUid, _mapUnit.index);
			
		} else {
			
			ClearNowChooseHero();
		}
	}
	
	public void HeroClick(HeroCard _hero){
		
		ClearNowChooseHero();
		
		if (nowChooseHero != _hero) {
			
			nowChooseHero = _hero;
			
			nowChooseHero.SetFrameVisible(true);
		}
	}
	
//	private void ClearNowChooseCard(){
//		
//		if (nowChooseCard != null) {
//			
//			nowChooseCard.SetFrameVisible(false);
//			
//			nowChooseCard = null;
//		}
//	}
	
	private void ClearNowChooseHero(){
		
		if (nowChooseHero != null) {
			
			nowChooseHero.SetFrameVisible(false);
			
			nowChooseHero = null;
		}
	}
	
	private void SummonHero(int _cardUid,int _pos){
		
		battle.ClientRequestSummon (_cardUid, _pos);
		
		ClearCards ();
		
		CreateCards ();
		
		CreateSummonHeros ();

		CreateMoneyTf ();
	}
	
	private void UnsummonHero(int _cardUid){
		
		battle.ClientRequestUndoAction ();
		
		ClearCards ();
		
		CreateCards ();
		
		ClearSummonHeros ();
		
		CreateMoneyTf ();
	}
	
	private void AddHeroToMap(Hero2 _hero){
		
		GameObject go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Hero"));
		
		HeroCard hero = go.GetComponent<HeroCard>();
		
		if (_hero.isSummon) {

			hero.body.color = summonColor;
		}
		
		heroDic.Add (_hero.pos, hero);
		
		hero.Init (_hero.id, _hero.nowHp, _hero.nowPower);
		
		AddHeroToMapReal (hero, _hero.pos);
	}
	
	private void AddCardToMap(int _cardUid,int _pos){
		
		int cardID = (battle.clientIsMine ? battle.mHandCards : battle.oHandCards) [_cardUid];
		
		GameObject go = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Hero"));
		
		summonHero = go.GetComponent<HeroCard>();
		
		summonHero.body.color = Color.magenta;
		
		summonHero.Init(_cardUid,cardID);
		
		AddHeroToMapReal (summonHero, _pos);
	}
	
	private void AddHeroToMapReal(HeroCard _heroCard,int _pos){
		
		MapUnit mapUnit = mapUnitDic [_pos];
		
		_heroCard.SetFrameVisible(false);
		
		_heroCard.transform.SetParent (heroContainer, false);
		
		_heroCard.transform.localPosition = mapUnit.transform.localPosition;
		
		//		_heroCard.transform.localPosition = Vector3.zero;
		
		_heroCard.transform.localScale = new Vector3 (heroScale, heroScale, heroScale);
		
		_heroCard.SetMouseEnable (false);
	}
	
	private int GetMoney(){
		
		int money = battle.clientIsMine ? battle.mMoney : battle.oMoney;
		
		if (summonHero != null) {

			money -= summonHero.sds.GetCost();
		}

		return money;
	}
	
	public void ActionBtClick(){

		ClearNowChooseHero ();
		
		battle.ClientRequestDoAction ();
		
		RefreshTouchable ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyUp(KeyCode.F5)){
			
			battle.ClientRequestRefreshData();
		}
	}

	private void SetIsTweening(bool _b){

		isTweening = _b;

		RefreshTouchable ();

		if (!_b) {

			if(packagePool.Count > 0){

				byte[] package = packagePool[0];

				packagePool.RemoveAt(0);

				battle.ClientGetPackage(package);
			}
		}
	}
	
	private void RefreshTouchable(){
		
		bool tmpTouchable = battle.clientIsMine == battle.isMineAction && !isTweening;
		
		graphicRayCaster.enabled = tmpTouchable;
		MapUnit.touchable = tmpTouchable;
		actionBt.SetActive (tmpTouchable);
	}

	private void DoSummon(int _cardID,int _pos){

		SetIsTweening (true);

		RefreshData ();

		HeroCard hero = heroDic [_pos];

		Action<float> del = delegate(float obj) {

			hero.transform.localScale = new Vector3 (obj, obj, obj);
		};

		Action over = delegate() {

			SetIsTweening (false);
		};

		SuperTween.Instance.To (5, hero.transform.localScale.x, 0.5f, del, over);

		hero.transform.localScale = new Vector3 (5, 5, 5);
	}

	private void DoMove(int _pos,int _direction){

		SetIsTweening (true);

		ClearMoves ();

		int newPos = battle.mapData.neighbourPosMap [_pos] [_direction];
			
		HeroCard hero = heroDic [_pos];
			
		Action<float> delX = delegate(float obj) {
			
			hero.transform.localPosition = new Vector3 (obj, hero.transform.localPosition.y, hero.transform.localPosition.z);
		};
		
		Action<float> delY = delegate(float obj) {
			
			hero.transform.localPosition = new Vector3 (hero.transform.localPosition.x, obj, hero.transform.localPosition.z);
		};

		Action over = delegate() {

			RefreshData();
			
			SetIsTweening (false);
		};
		
		SuperTween.Instance.To (hero.transform.localPosition.x, mapUnitDic [newPos].transform.localPosition.x, 1, delX, over);
			
		SuperTween.Instance.To (hero.transform.localPosition.y, mapUnitDic [newPos].transform.localPosition.y, 1, delY, null);
	}
	
	private void DoAction(BinaryReader _br){

		SetIsTweening (true);

		DoAttack(_br);
	}

	private void DoAttack(BinaryReader _br){
		
		IEnumerator enumerator = battle.ClientDoAttack (_br);
		
		DoAttackReal (enumerator, _br);
	}
	
	private void DoAttackReal(IEnumerator _enumerator,BinaryReader _br){
		
		if (_enumerator.MoveNext ()) {
			
			if(_enumerator.Current is KeyValuePair<int,int>){
				
				DoRush(_enumerator,_br);
				
			}else if(_enumerator.Current is KeyValuePair<int, KeyValuePair<int, int>[]>){
				
				DoDamage(_enumerator,_br);
				
			}else if(_enumerator.Current is int){
				
				Action del = delegate() {
					
					DoAttackReal(_enumerator,_br);
				};
				
				RefreshData();
				
				roundNum.Move("Round " + _enumerator.Current,del);
			}
			
		} else {
			
			battle.ClientDoRecover (_br);
			
			_br.Close();

			SetIsTweening (false);
		}
	}
	
	private void DoRush(IEnumerator _enumerator,BinaryReader _br){
		
		KeyValuePair<int,int> pair = (KeyValuePair<int,int>)_enumerator.Current;
		
		GameObject go = GameObject.Instantiate<GameObject> (Resources.Load<GameObject> ("Attack"));
		
		go.GetComponent<SpriteRenderer> ().color = Color.magenta;
		
		go.transform.SetParent (arrowContainer, false);
		
		go.transform.localPosition = new Vector3(mapUnitDic [pair.Key].transform.localPosition.x,mapUnitDic [pair.Key].transform.localPosition.y,arrowZFix);
		
		go.transform.localScale = new Vector3 (mapUnitScale, mapUnitScale, mapUnitScale);
		
		Action doNext = delegate() {
			
			DoAttackReal(_enumerator,_br);
		};
		
		Action over = delegate() {
			
			GameObject.Destroy(go);
			
			heroDic[pair.Value].SetPower(battle.heroMapDic[pair.Value].nowPower);
			
			SuperTween.Instance.DelayCall(1,doNext);
		};
		
		Action<float> delX = delegate(float obj) {
			
			go.transform.localPosition = new Vector3(obj,go.transform.localPosition.y,go.transform.localPosition.z);
		};
		
		Action<float> delY = delegate(float obj) {
			
			go.transform.localPosition = new Vector3(go.transform.localPosition.x,obj,go.transform.localPosition.z);
		};
		
		Vector3 targetPos = mapUnitDic [pair.Value].transform.localPosition;
		
		go.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2(targetPos.y - go.transform.localPosition.y,targetPos.x - go.transform.localPosition.x) * 180 / Mathf.PI);
		
		SuperTween.Instance.To (go.transform.localPosition.x, targetPos.x, 1, delX, over);
		
		SuperTween.Instance.To (go.transform.localPosition.y, targetPos.y, 1, delY, null);
	}
	
	private void DoDamage(IEnumerator _enumerator,BinaryReader _br){
		
		KeyValuePair<int, KeyValuePair<int, int>[]> pair = (KeyValuePair<int, KeyValuePair<int, int>[]>)_enumerator.Current;
		
		KeyValuePair<int,int>[] targets = pair.Value;
		
		for (int i = 0; i < targets.Length; i++) {
			
			GameObject go = GameObject.Instantiate<GameObject> (Resources.Load<GameObject> ("Attack"));
			
			go.GetComponent<SpriteRenderer> ().color = Color.black;
			
			go.transform.SetParent (arrowContainer, false);
			
			go.transform.localPosition = new Vector3(mapUnitDic [pair.Key].transform.localPosition.x,mapUnitDic [pair.Key].transform.localPosition.y,arrowZFix);
			
			go.transform.localScale = new Vector3 (mapUnitScale, mapUnitScale, mapUnitScale);
			
			Action<float> delX = delegate(float obj) {
				
				go.transform.localPosition = new Vector3 (obj, go.transform.localPosition.y, go.transform.localPosition.z);
			};
			
			Action<float> delY = delegate(float obj) {
				
				go.transform.localPosition = new Vector3 (go.transform.localPosition.x, obj, go.transform.localPosition.z);
			};
			
			Vector3 targetPos = mapUnitDic [targets[i].Key].transform.localPosition;
			
			go.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2(targetPos.y - go.transform.localPosition.y,targetPos.x - go.transform.localPosition.x) * 180 / Mathf.PI);
			
			int heroPos = targets[i].Key;
			
			int damage = targets[i].Value;
			
			Action over;
			
			if(i == 0){
				
				Action doNext = delegate() {
					
					DoAttackReal (_enumerator, _br);
				};
				
				over = delegate() {
					
					GameObject.Destroy (go);
					
					heroDic [heroPos].SetHp (battle.heroMapDic [heroPos].nowHp);
					
					GameObject damageGo = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("DamageNum"));
					
					damageGo.transform.SetParent(arrowContainer,false);
					
					damageGo.transform.localPosition = heroDic [heroPos].transform.localPosition;
					
					DamageNum damageNum = damageGo.GetComponent<DamageNum>();
					
					damageNum.Init(-damage,doNext);
					
					RefreshData();
					
					//					SuperTween.Instance.DelayCall(1,doNext);
				};
				
			}else{
				
				over = delegate() {
					
					GameObject.Destroy (go);
					
					heroDic [heroPos].SetHp (battle.heroMapDic [heroPos].nowHp);
					
					GameObject damageGo = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("DamageNum"));
					
					damageGo.transform.SetParent(arrowContainer,false);
					
					damageGo.transform.localPosition = heroDic [heroPos].transform.localPosition;
					
					DamageNum damageNum = damageGo.GetComponent<DamageNum>();
					
					damageNum.Init(-damage,null);
				};
			}
			
			SuperTween.Instance.To (go.transform.localPosition.x, targetPos.x, 1, delX, over);
			
			SuperTween.Instance.To (go.transform.localPosition.y, targetPos.y, 1, delY, null);
		}
	}
}
