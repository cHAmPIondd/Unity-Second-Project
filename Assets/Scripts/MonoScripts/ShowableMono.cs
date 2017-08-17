using UnityEngine;
using System.Collections;

public class ShowableMono : MonoBehaviour {
	[SerializeField]
	protected Transform m_FeedbackCameraTarget;
	public Transform feedbackCameraTarget{ get{ return m_FeedbackCameraTarget;}}

	public Transform computerCameraTran{ get; private set;}

	void Start () {

		GameObject tempGO;
		if (GameObjectManager.instance.cameraDict.TryGetValue ("ComputerCamera", out tempGO))
			computerCameraTran = tempGO.transform;
		else
			Debug.Log ("找不到电脑相机");
	}	
	public virtual void Show()
	{

	}
}
