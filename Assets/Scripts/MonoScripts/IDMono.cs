using UnityEngine;
using System.Collections;

public class IDMono : MonoBehaviour {

	[SerializeField]private int m_ID;
	public int ID
	{
		get{return m_ID;}
		set{m_ID = value;}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
