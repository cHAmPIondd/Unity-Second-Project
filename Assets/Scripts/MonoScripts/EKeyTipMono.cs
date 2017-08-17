using UnityEngine;
using System.Collections;

public class EKeyTipMono : MonoBehaviour {
	[SerializeField]private string m_TipString;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void DisplayTip()
	{
		GameProgressManager.instance.TipAnimation (m_TipString,true);
	}
}
