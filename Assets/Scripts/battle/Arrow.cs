using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer sr;

	public void SetColor(Color _color){

		sr.color = _color;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
