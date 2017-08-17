using UnityEngine;
using System.Collections;

public class SpaceInvaderMono : MonoBehaviour {

	[SerializeField]private GameObject m_GamePanelPrefab;
	[SerializeField]private GameObject[] m_MonsterPrefabs;
	[SerializeField]private GameObject m_MonsterBulletPrefab;
	[SerializeField]private GameObject m_HeroPrefab;
	[SerializeField]private float m_Spacing=1;

	[SerializeField]private GameObject m_Congratulation;
	[SerializeField]private GameObject m_GameOver;
//	public GameObject code
//	{
	//	get{return m_Code;}
//}

	private App m_AppParent;
	public App appParent{ set{m_AppParent = value;}}
	private GameObject gamePanel;
	private GameObject[,] m_Monster;
	private GameObject m_Boss;
	public GameObject boss
	{
		get{return m_Boss;}
	}
	private GameObject m_Hero;
	public GameObject hero
	{
		get{return m_Hero;}
	}
	public static bool HasBullet;
	void Awake()
	{
        m_Monster = new GameObject[6, 8]; 
	}
	public void Init()
	{
		HasBullet = false;
		Hero.HasBullet = false;	
		gamePanel=(GameObject)Instantiate(m_GamePanelPrefab,transform);
		for (int i = 0; i < 6; i++) 
		{
			for (int j = 0; j < 8; j++) 
			{
				m_Monster [i,j] = (GameObject)Instantiate (m_MonsterPrefabs [i],gamePanel.transform);
				m_Monster [i, j].transform.position += (m_Monster [i, j].transform.right * m_Spacing*j);
				m_Monster [i, j].SetActive (true);
			}
		}
		m_Boss =  (GameObject)Instantiate (m_MonsterPrefabs [6],gamePanel.transform);
		m_Boss.SetActive (true);
		m_Hero=(GameObject)Instantiate(m_HeroPrefab,gamePanel.transform);
		m_Hero.SetActive (true);
		m_Congratulation.SetActive (false);
		m_GameOver.SetActive (false);
	}
	public void GameOver()
	{
		m_GameOver.SetActive (true);
		Destroy (gamePanel);
	}
	public void Victory()
	{
		m_Congratulation.SetActive (true);
		Destroy (gamePanel);
		if(!CodeLibrary.instance.codeList.Contains("SETCLIMBABLE_CODE"))
			CodeLibrary.instance.AddCode ("SetClimbable");
	}
	public void Close()
	{
		StartCoroutine (m_AppParent.Close());
		m_Congratulation.SetActive (false);
		m_GameOver.SetActive (false);
	}
	void Update () 
	{
		if (!HasBullet) 
		{
			for (int i = 0; i < 6; i++) 
			{
				for (int j = 0; j < 8; j++) 
				{
					if (m_Monster [i, j] != null) 
					{
						RaycastHit hitObject;
						if (Physics.Raycast (m_Monster [i, j].transform.position+new Vector3 (0, -0.15f,0), -m_Monster [i, j].transform.up,out hitObject, 10f, 1 << 13)) 
						{
							if(hitObject.transform.tag=="Hero")
							{
								HasBullet = true;
								GameObject tempGO=(GameObject)Instantiate (m_MonsterBulletPrefab,m_Monster [i, j].transform);
								tempGO.transform.localPosition = new Vector2 (0, -0.15f);
								tempGO.transform.parent = gamePanel.transform;
								break;
							}
						}
					}
				}
				if (HasBullet)
					break;
			}
			if (!HasBullet) 
			{
				int num=Random.Range (0, 8);
				for (int i = 0; i < 6; i++) 
				{
					if (m_Monster [i, num] != null) 
					{
						HasBullet = true;
						GameObject tempGO=(GameObject)Instantiate (m_MonsterBulletPrefab,m_Monster [i, num].transform);
						tempGO.transform.localPosition = new Vector2 (0, -0.15f);
						tempGO.transform.parent = gamePanel.transform;
						break;
					}
				}
			}
		}
	}
}
