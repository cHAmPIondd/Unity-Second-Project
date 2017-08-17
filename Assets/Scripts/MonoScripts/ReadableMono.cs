using UnityEngine;
using System.Collections;

public class ReadableMono : MonoBehaviour {
	private BookMono m_BookMono;
	public BookMono bookMono {
		get{ return m_BookMono; }
	}

	[SerializeField]private Transform m_FeedbackCameraTarget;
	public Transform feedbackCameraTarget{ get{ return m_FeedbackCameraTarget;}}


	public Transform computerCameraTran{ get; private set;}

	public Vector3 originalPosition{ get; set;}
	public Quaternion originalRotation{get;set;}


	void Start () {
		m_BookMono = transform.parent.GetComponent<BookMono> ();

		GameObject tempGO;
		if (GameObjectManager.instance.cameraDict.TryGetValue ("ComputerCamera", out tempGO))
			computerCameraTran = tempGO.transform;
		else
			Debug.Log ("找不到电脑相机");
	}

}