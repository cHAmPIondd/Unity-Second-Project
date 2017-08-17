using UnityEngine;
using System.Collections;
using HighlightingSystem;

public class lateAddComponent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.AddComponent<HighlightingRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
