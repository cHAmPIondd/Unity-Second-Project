using UnityEngine;
using System.Collections;

public class ControlMono : MonoBehaviour {

	[SerializeField] protected GameObject m_CenterSight;
	public virtual bool IsCanEsc()
	{
		return false;
	}
}
