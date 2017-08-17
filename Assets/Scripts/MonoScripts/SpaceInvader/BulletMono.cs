using UnityEngine;
using System.Collections;

public class BulletMono : MonoBehaviour {

	[SerializeField]private float m_Rate=1f;
	private float m_ExistTimer;
	void OnTriggerEnter(Collider other)
	{
		if (m_Rate > 0)
			Hero.HasBullet = false;
		else
			SpaceInvaderMono.HasBullet = false;
		if (other.tag == "Boss") 
		{
			
		} 
		else if (other.tag == "Hero") 
		{
			
		}
		if(other.tag!="Border")
			Destroy (other.gameObject);
		Destroy (gameObject);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.up * m_Rate*Time.deltaTime;
		m_ExistTimer += Time.deltaTime;

	}
}
