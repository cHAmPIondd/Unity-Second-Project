using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	[SerializeField]private GameObject m_HeroBulletPrefab;
	[SerializeField]private float m_Rate=1f;

	public static bool HasBullet;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.D))
			transform.position+=(transform.right*m_Rate);
		if(Input.GetKey(KeyCode.A))
			transform.position+=(-transform.right*m_Rate);
		if (transform.localPosition.x < -4.6)
			transform.localPosition = new Vector3 (-4.6f,transform .localPosition.y,transform .localPosition.z);
		if(transform.localPosition.x >2)
			transform.localPosition = new Vector3 (2f,transform .localPosition.y,transform .localPosition.z);
		if (!HasBullet&&Input.GetKeyDown(KeyCode.Space)) 
		{
			HasBullet = true;
			GameObject tempGO=(GameObject)Instantiate (m_HeroBulletPrefab,transform);
			tempGO.transform.localPosition = new Vector2 (0, 0.15f);
			tempGO.transform.parent= transform.parent;
		}
	}
}
