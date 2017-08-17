using UnityEngine;
using System.Collections;

public class MovableMono : MonoBehaviour {

	[SerializeField]
	private Vector3 m_Direction;
	[SerializeField]
	private float m_MaxDistance=5;
	[SerializeField]
	private float m_MinDistance=-5;
	[SerializeField]
	private float m_DefaultDistance;
	[SerializeField]
	private Transform m_FeedbackCameraTarget;

	public Vector3 direction{ get{ return m_Direction;}}
	public float maxDistance{ get{ return m_MaxDistance;}}
	public float minDistance{ get{ return m_MinDistance;}}
	public float defaultDistance{ get{ return m_DefaultDistance;}}
	public Transform feedbackCameraTarget{ get{ return m_FeedbackCameraTarget;}}

	public Transform computerCameraTran{ get; private set;}

	public Vector3 originalPosition{ get; private set;}

	void Start () {
		m_Direction = Vector3.Normalize (m_Direction);

		GameObject tempGO;
		if (GameObjectManager.instance.cameraDict.TryGetValue ("ComputerCamera", out tempGO))
			computerCameraTran = tempGO.transform;
		else
			Debug.Log ("找不到电脑相机");

		originalPosition = transform.position;
	}
}
