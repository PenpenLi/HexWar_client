using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeroDetail : MonoBehaviour {

	[SerializeField]
	private Text cost;

	[SerializeField]
	private Text hp;

	[SerializeField]
	private Text power;

	[SerializeField]
	private Text damage;

	[SerializeField]
	private Text attackTimes;

	public void Init(HeroCard _hero){

		cost.text = _hero.sds.cost.ToString ();

		hp.text = _hero.sds.hp.ToString ();

		power.text = _hero.sds.power.ToString ();

		damage.text = _hero.sds.damage.ToString ();

		attackTimes.text = _hero.sds.attackTimes.ToString ();

		gameObject.SetActive (true);
	}

	public void Hide(){

		gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
