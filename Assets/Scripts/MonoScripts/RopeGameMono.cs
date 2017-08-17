using UnityEngine;
using System.Collections;

public class RopeGameMono : ControlMono {
	
	[SerializeField] private Camera m_Camera;
	[SerializeField] private SpriteRenderer m_ReadCode;
	[SerializeField] private GameObject m_RopePrefab;
	[SerializeField] private Transform[] m_PointTransform0;
	[SerializeField] private Transform[] m_PointTransform1;
	[SerializeField] private Transform[] m_PointTransform2;
	[SerializeField] private Transform[] m_PointTransform3;


	private bool m_IsDrawing;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)&&!m_IsDrawing) {
			RaycastHit hitObject;
			if (Physics.Raycast (m_Camera.ScreenPointToRay(Input.mousePosition),out hitObject,1000,1<<10)) {
				StartCoroutine( DrawRope(hitObject.transform.GetComponent<IDMono> ().ID));
				m_IsDrawing = true;
			}
		}
	}
	void OnEnable()
	{
		Cursor.visible = true;
		m_CenterSight.SetActive (false);
	}
	void OnDisable()
	{
		Cursor.visible = false;
		m_CenterSight.SetActive (true);
	}

	private IEnumerator DrawRope(int num)
	{
		//判断哪条线
		Transform[] tempPoint;
		switch (num) {
		case 0:
			tempPoint = m_PointTransform0;
			break;
		case 1:
			tempPoint = m_PointTransform1;
			break;
		case 2:
			tempPoint = m_PointTransform2;
			break;
		case 3:
			tempPoint = m_PointTransform3;
			break;
		default:
			Debug.Log ("BUG");
			tempPoint = m_PointTransform0;
			break;
		}

		//划线
		GameObject tempGO=new GameObject();
		m_RopePrefab.SetActive (true);
		m_RopePrefab.transform.position = tempPoint[0].position;
		foreach (Transform tempTran in tempPoint) 
		{
			while (true) 
			{
				if (Vector3.SqrMagnitude (tempTran.position - m_RopePrefab.transform.position) < 0.001)
					break;
				Instantiate (m_RopePrefab, tempGO.transform);
				m_RopePrefab.transform.position += (tempTran.position - m_RopePrefab.transform.position).normalized * Time.deltaTime;
				if (Vector3.SqrMagnitude (tempTran.position - m_RopePrefab.transform.position) < 0.001)
					break;
				Instantiate (m_RopePrefab, tempGO.transform);
				m_RopePrefab.transform.position += (tempTran.position - m_RopePrefab.transform.position).normalized * Time.deltaTime;
				yield return 0;
			}
		}
		yield return new WaitForSeconds (0.5f);
		m_RopePrefab.SetActive (false);
		Destroy (tempGO);
		if (num == 0) 
		{
			SpriteRenderer tempSprite =GetComponent<SpriteRenderer> ();
			for (float i = 1f; i >= 0; i -= Time.deltaTime) 
			{
				tempSprite.color -=new Color (0, 0, 0, 1)*Time.deltaTime;
				m_ReadCode.color += new Color (0, 0, 0, 1)*Time.deltaTime;
				yield return 0;
			}	
			m_ReadCode.GetComponent<Collider> ().enabled = true;
			GameProgressManager.instance.AddAnimation (new GameProgressManager.animation (PlayerControl.instance.ExitAnimation));
			GameProgressManager.instance.AddAnimation (new GameProgressManager.animation (DestorySelf));
		}
		m_IsDrawing = false;
	}
	public IEnumerator DestorySelf()
	{
		Destroy (this.gameObject);
		yield return 0;
	}
}
