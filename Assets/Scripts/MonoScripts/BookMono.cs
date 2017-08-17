using UnityEngine;
using System.Collections;

public class BookMono : ControlMono {

	[SerializeField]private ReadableMono[] m_Book;
	public ReadableMono lastReadBook{ get; set;}
	[SerializeField]private Transform m_OpenBookTransform;
	public Transform openBookTranform
	{
		get{return m_OpenBookTransform;}
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
}
