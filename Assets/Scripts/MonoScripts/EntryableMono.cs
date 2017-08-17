using UnityEngine;
using System.Collections;

public class EntryableMono : MonoBehaviour {
	[SerializeField]private Transform m_CameraTargetTransform;
	[SerializeField]private ControlMono m_ControlMono;
	public ControlMono controlMono
	{
		get{return m_ControlMono;}
	}
	public Transform cameraTargetTransform
	{
		get{return m_CameraTargetTransform;}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
