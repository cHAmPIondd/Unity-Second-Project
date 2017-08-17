using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectManager {

	private static GameObjectManager m_Instance;
	public static GameObjectManager instance
	{
		get
		{
			if (m_Instance == null) {
				m_Instance = new GameObjectManager();
			}
			return m_Instance;
		}
	}
	private Dictionary<string,GameObject> m_RendererDict;
	public Dictionary<string,GameObject> rendererDict
	{
		get{return m_RendererDict;}
	}

	private Dictionary<string,GameObject> m_CameraDict;
	public Dictionary<string,GameObject> cameraDict
	{
		get{return m_CameraDict;}
	}


	private GameObjectManager()
	{
		m_RendererDict = new Dictionary<string,GameObject> ();
		Renderer[] render= GameObject.FindObjectsOfType<Renderer> ();
		foreach (Renderer temp in render) 
		{
			if (temp.name != "???") 
			{
				GameObject a;//Debug
				if (!m_RendererDict.TryGetValue (temp.name.ToUpper(), out a))//Debug
					m_RendererDict.Add (temp.name.ToUpper(), temp.gameObject);
			}

			if (temp.name == "Game_Icon")
				temp.gameObject.SetActive (false);
			if (temp.name == "Game_Icon(0)")
				temp.gameObject.SetActive (false);
		}

		m_CameraDict =new Dictionary<string,GameObject> ();
		Camera[] camera= GameObject.FindObjectsOfType<Camera> ();
		foreach (Camera temp in camera) 
		{
			m_CameraDict.Add (temp.name,temp.gameObject);
			if (temp.name == "ComputerCamera")
				temp.gameObject.SetActive (false);
		}
	}

}
