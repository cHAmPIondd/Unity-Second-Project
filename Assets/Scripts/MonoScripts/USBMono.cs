using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class USBMono : MonoBehaviour {

	[HideInInspector]public List<GameObject> StayList;
	[SerializeField]private Transform[] USBTransform; 
	[SerializeField]private ComputerMono m_ComputerMono;
	void Start()
	{
		StayList = new List<GameObject> ();
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "USB") {
			StayList.Add (other.gameObject);
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "USB") {
			StayList.Remove(other.gameObject);
		}
	}

	public IEnumerator InsertAnimation(GameObject insertGO)
	{
		insertGO.GetComponent<Rigidbody> ().isKinematic = true;
		for (float i = 0; i < 0.5f; i += Time.deltaTime) 
		{
			insertGO.transform.position = Vector3.Lerp (insertGO.transform.position, USBTransform [m_ComputerMono.USBUsedList.Count].position-USBTransform [m_ComputerMono.USBUsedList.Count].right*0.7f, 5 * Time.deltaTime);
			insertGO.transform.rotation = Quaternion.Lerp (insertGO.transform.rotation, USBTransform [m_ComputerMono.USBUsedList.Count].rotation, 5 * Time.deltaTime);
			yield return 0;	
		}
		for (float i = 0; i < 1f; i += Time.deltaTime) 
		{
			insertGO.transform.position = Vector3.Lerp (insertGO.transform.position, USBTransform [m_ComputerMono.USBUsedList.Count].position, 5 * Time.deltaTime);
			insertGO.transform.rotation = Quaternion.Lerp (insertGO.transform.rotation, USBTransform [m_ComputerMono.USBUsedList.Count].rotation, 5 * Time.deltaTime);
			yield return 0;	
		}
		if (insertGO.name == "UDisk") 
		{
			GameObject.Find ("Computer/Desktop/UDisk_Icon").SetActive (true);
			if(m_ComputerMono.UDiskApp!=null)
				m_ComputerMono.USBAppList.Add (m_ComputerMono.UDiskApp);
		}

		m_ComputerMono.USBUsedList.Add (insertGO);
	}
}
