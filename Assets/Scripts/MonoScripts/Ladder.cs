using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		other.SendMessage ("InLadder", this);
	}
	void OnTriggerExit(Collider other)
	{
		other.SendMessage ("OutLadder", this);
	}
}
