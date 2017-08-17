using UnityEngine;
using System.Collections;

public class showCode : MonoBehaviour {
	[SerializeField]private ReadableMono ReadableMono;
	[SerializeField]private GameObject Code;
	// Use this for initialization
	void Start () {
		StartCoroutine (update ());
	}
	
	private IEnumerator update()
	{
		while (true) 
		{
			if (Code == null)
				break;
			yield return new WaitForSeconds (1);
			if (ReadableMono.bookMono.lastReadBook==ReadableMono) 
			{
				Code.SetActive (true);
			}
			else
			{
				Code.SetActive (false);
			}
		}
	}
}
