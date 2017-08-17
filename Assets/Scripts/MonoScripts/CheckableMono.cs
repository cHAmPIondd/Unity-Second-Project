using UnityEngine;
using System.Collections;

public class CheckableMono : MonoBehaviour {

	[SerializeField]protected float m_Distance;
	public float distance{ get{return m_Distance;}}
	public bool isAnimationOver{ get; set;}
	public virtual void CheckFinished()
	{
		
	}
}
