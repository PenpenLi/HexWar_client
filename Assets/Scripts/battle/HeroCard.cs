using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeroCard : MonoBehaviour,IPointerClickHandler {

	[SerializeField]
	private Image frame;

	[SerializeField]
	private Image body;

	[SerializeField]
	private Text heroType;

	[SerializeField]
	private Text power;

	[SerializeField]
	private Text damage;

	[SerializeField]
	private Text hp;

	public int uid;
	public HeroSDS sds;

	public void Init(int _uid,int _id){

		uid = _uid;

		sds = StaticData.GetData<HeroSDS> (_id);

		heroType.text = sds.heroTypeSDS.name;

		power.text = sds.power.ToString ();

		damage.text = sds.damage.ToString ();

		hp.text = sds.hp.ToString ();
	}

	public void Init(int _uid,int _id,int _hp){

		Init (_uid, _id);

		hp.text = _hp.ToString ();
	}

	public void SetMouseEnable(bool _b){

		body.raycastTarget = _b;
	}

	public void SetFrameVisible(bool _visible){

		frame.gameObject.SetActive (_visible);
	}

	public void OnPointerClick(PointerEventData _data){

		SendMessageUpwards ("HeroClick", this, SendMessageOptions.DontRequireReceiver);
	}
}
