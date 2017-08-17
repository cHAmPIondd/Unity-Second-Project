using UnityEngine;
using System.Collections;

public class UIRectAwakeMono : MonoBehaviour {

    [SerializeField]
    private Rect m_Rect;
	// Use this for initialization
	void Start () {	
		RectTransform transform=GetComponent<RectTransform> ();
		transform.position=new Vector2(m_Rect.x*Screen.width,m_Rect.y*Screen.height)+new Vector2 (m_Rect.width * Screen.width, m_Rect.height * Screen.height)/2;
		transform.sizeDelta = new Vector2 (m_Rect.width * Screen.width, m_Rect.height * Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
