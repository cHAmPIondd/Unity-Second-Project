 using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (AudioSource))]
public class PlayerControl : MonoBehaviour {


	[SerializeField] private float m_MoveSpeed=7;
	[SerializeField] private float m_CamereRotateXSpeed=2;
	[SerializeField] private float m_CamereRotateYSpeed=2;
	[SerializeField] private AudioClip m_MoveSound;   
	[SerializeField] private AudioClip m_LandSound;

	public static PlayerControl instance;

	private Camera m_Camera;
	private Rigidbody m_Rigidbody;
	private AudioSource m_AudioSource;

	[SerializeField]private Transform m_ObjectCameraTransform;

	private EntryableMono m_CurrentEntryableMono;

	private Ladder m_CurrentLadder;
	private CheckableMono m_CurrentCheckableMono;
	private Rigidbody m_CurrentOperateObject;
	private float m_OperateDistance;
	[SerializeField] private float m_OperateForce=10f;

	private bool m_IsOnGround;

	private OutLineControl m_LastOutLineControl;

	[SerializeField] private USBMono m_USBMono;

	private bool m_HasPhone;
	private bool m_IsOnPhone;
	[SerializeField] private PhoneMono m_PhoneMono;
	[SerializeField] private Transform[] m_PhoneAniPointArray;
	[SerializeField] private GameObject m_PhoneModel;

	// Use this for initialization
	void Start () 
	{
		instance = this;
		m_Camera = GetComponentInChildren<Camera>();
		m_Rigidbody = GetComponent<Rigidbody> ();
		m_AudioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		KeyDown_Esc ();
		if(!m_CurrentOperateObject)
			Selection ();
		if(!m_CurrentEntryableMono&&!m_CurrentCheckableMono&&!m_IsOnPhone)
		{
			CameraRotate ();
			Move ();
			if (!m_CurrentOperateObject) 
			{
				KeyDown_E ();
				KeyDown_Q ();
			}
			KeyDown_Mouse0 ();
		}
	}
	void FixedUpdate()
	{
		IsOnGround ();
		if(m_IsOnPhone)
			m_Rigidbody.velocity = new Vector3 (0,m_Rigidbody.velocity.y,0);			
	}
	private void CameraRotate()
	{	//鼠标移动镜头
		float rotationY = Input.GetAxis("Mouse X") * m_CamereRotateXSpeed;
		float rotationX = Input.GetAxis("Mouse Y") * m_CamereRotateYSpeed;
		transform.localRotation *= Quaternion.Euler(0f, rotationY, 0f);
		m_Camera.transform.localRotation *= Quaternion.Euler(-rotationX, 0f, 0f);
		m_Camera.transform.localRotation = ClampRotationAroundXAxis(m_Camera.transform.localRotation);
	}
	private Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		//上下移动鼠标，镜头的角度限制
		q.x /= q.w;
		q.w = 1.0f;
		float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
		angleY = Mathf.Clamp(angleY, -90, 90);
		q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);
		return q;
	}
	private void Move()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		Vector3 moveDir;
		if (!m_CurrentLadder) {
			moveDir = Vector3.Normalize ((transform.right * horizontal) + (transform.forward * vertical));
			m_Rigidbody.velocity = moveDir * m_MoveSpeed + Vector3.up * m_Rigidbody.velocity.y;
		} else {
			moveDir = (transform.right * horizontal) + (transform.forward * vertical);
			moveDir = m_CurrentLadder.transform.InverseTransformDirection (moveDir);//转换到梯子局部坐标
			if (!m_IsOnGround || moveDir.z > 0) {
				moveDir = new Vector3 (moveDir.x, moveDir.z, 0);//把z轴移动改为y轴
			}
			moveDir = Vector3.Normalize (m_CurrentLadder.transform.TransformDirection (moveDir));//转换为世界坐标并单位化

			m_Rigidbody.velocity = moveDir * m_MoveSpeed;
		}

	}

	public void InLadder(Ladder ladder)
	{
		m_CurrentLadder = ladder;
		m_Rigidbody.useGravity = false;
	}
	public void OutLadder(Ladder ladder)
	{
		if (m_CurrentLadder == ladder) {
			m_CurrentLadder = null;
			m_Rigidbody.useGravity = true;
		}
	}
	private void IsOnGround()
	{
		if (m_Rigidbody.velocity.y > 0&&m_CurrentLadder==null)
			m_Rigidbody.velocity = new Vector3 (m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z);
		if (Physics.Raycast (transform.position+new Vector3(0,0.01f,0), Vector3.down, 0.1f))
		{
			if (!m_IsOnGround) 
			{
				m_IsOnGround = true;
				//播放着地音效
			}
		} 
		else 
		{
			m_IsOnGround = false;
		}	
	}

	private void KeyDown_E()
	{
		RaycastHit hitObject;
		if (Physics.Raycast (m_Camera.ScreenPointToRay (new Vector2 (Screen.width / 2, Screen.height / 2)), out hitObject, 10f)) 
		{ 
			//判断有无可E的组件
			EntryableMono tempEnt= hitObject.transform.GetComponent<EntryableMono> ();
			CheckableMono tempChe= hitObject.transform.GetComponent<CheckableMono> ();
			GetableMono tempGet = hitObject.transform.GetComponent<GetableMono> ();
			EKeyTipMono tempTip= hitObject.transform.GetComponent<EKeyTipMono> ();
			//判断组件是否可用
			if (tempEnt!=null&&!tempEnt.enabled)
				tempEnt = null;
			if (tempChe!=null&&!tempChe.enabled)
				tempChe = null;
			if (tempGet!=null&&!tempGet.enabled)
				tempGet = null;
			if (tempTip!=null&&!tempTip.enabled)
				tempTip = null;
			//提示可E互动
			if ((tempEnt != null || tempChe != null || tempGet != null|| tempTip != null)) {
				GameProgressManager.instance.TipAnimation ("按E互动", false);
			}
			if (Input.GetKeyDown (KeyCode.E)) {
				if (tempEnt != null) {
					m_CurrentEntryableMono = tempEnt;
					StartCoroutine ("EntryAnimation");
				}

				if (tempChe != null) {
					m_CurrentCheckableMono = tempChe;
					StartCoroutine ("CheckAnimation");
				}
				if (tempGet != null) {
						tempGet.BeGet ();
				}
				if (tempTip != null) {
					tempTip.DisplayTip ();
				}
			}
		}
	}
	private void KeyDown_Mouse0()
	{
		if (m_CurrentOperateObject == null) {
			RaycastHit hitObject;
			if (Physics.Raycast (m_Camera.ScreenPointToRay (new Vector2 (Screen.width / 2, Screen.height / 2)), out hitObject, 4f, 1 << 11)) { //1<<11 可操作物体
				GameProgressManager.instance.TipAnimation ("鼠标左键拖动", false);
				if (Input.GetKey (KeyCode.Mouse0)) {
					m_CurrentOperateObject = hitObject.transform.GetComponent<Rigidbody> ();
					if (!m_CurrentOperateObject.isKinematic) {
						m_CurrentOperateObject.useGravity = false;
						m_OperateDistance = Vector3.Magnitude (hitObject.transform.position - m_Camera.transform.position);

						if (m_LastOutLineControl != null)
							m_LastOutLineControl.OutLineOff ();
						m_LastOutLineControl = hitObject.transform.GetComponent<OutLineControl> ();
						m_LastOutLineControl.OutLineOn ();
					} else {
						m_CurrentOperateObject = null;
					}
				}
			}
		} 
		else
		{
			if (!Input.GetKey (KeyCode.Mouse0)) 
			{
			
				if (m_USBMono.StayList.Contains (m_CurrentOperateObject.gameObject)) {
					StartCoroutine (m_USBMono.InsertAnimation (m_CurrentOperateObject.gameObject));
				}
				m_CurrentOperateObject.velocity = Vector3.zero;
				m_CurrentOperateObject.useGravity = true;
				m_CurrentOperateObject = null;
			} 
			else 
			{
				m_CurrentOperateObject.velocity = Vector3.zero;
				Vector3 tempV3 = (m_Camera.transform.position + m_Camera.transform.forward * m_OperateDistance - m_CurrentOperateObject.transform.position);
				if (Vector3.SqrMagnitude (tempV3) > 25) {
					m_CurrentOperateObject.velocity = Vector3.zero;
					m_CurrentOperateObject.useGravity = true;
					m_CurrentOperateObject = null;
				} else if (!Physics.Raycast (m_CurrentOperateObject.transform.position, tempV3, 0.1f))
					m_CurrentOperateObject.AddForce (tempV3 * m_OperateForce / Time.fixedDeltaTime);
			}
		}
	}	
	private void KeyDown_Q()
	{
		if (Input.GetKeyDown (KeyCode.Q)) 
		{
			if (m_HasPhone) 
			{
				if (m_PhoneModel != null)
					Destroy (m_PhoneModel);
				m_IsOnPhone = true;
				StartCoroutine ("EntryPhoneAnimation");
			}
		}
	}
	private void KeyDown_Esc()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (m_CurrentEntryableMono && m_CurrentEntryableMono.controlMono.enabled && !m_CurrentEntryableMono.controlMono.IsCanEsc ())
				StartCoroutine ("ExitAnimation");
			else if (m_CurrentCheckableMono) {
				if (m_CurrentCheckableMono.isAnimationOver) {
					m_CurrentCheckableMono.CheckFinished ();
					Destroy (m_CurrentCheckableMono.gameObject);
				}
			} 
			else if (m_IsOnPhone&&m_PhoneMono.enabled) 
			{
				StartCoroutine ("ExitPhoneAnimation");
			}
		}
	}
	public IEnumerator EntryAnimation()  
	{  
		m_Rigidbody.velocity = new Vector3 (0,m_Rigidbody.velocity.y,0);
		m_ObjectCameraTransform.gameObject.SetActive (true);
		m_Camera.gameObject.SetActive (false);
		m_ObjectCameraTransform.parent = null;
		for (float timer = 0.8f; timer >= 0; timer -= Time.deltaTime) 
		{
			m_ObjectCameraTransform.position=Vector3.Lerp(m_ObjectCameraTransform.position,m_CurrentEntryableMono.cameraTargetTransform.position,5*Time.deltaTime);
			m_ObjectCameraTransform.rotation=Quaternion.Lerp(m_ObjectCameraTransform.rotation,m_CurrentEntryableMono.cameraTargetTransform.rotation,5*Time.deltaTime);
			yield return 0;  
		}
		m_ObjectCameraTransform.position = m_CurrentEntryableMono.cameraTargetTransform.position;
		m_ObjectCameraTransform.rotation = m_CurrentEntryableMono.cameraTargetTransform.rotation;
		m_CurrentEntryableMono.controlMono.enabled =  true;
	}  
	public IEnumerator ExitAnimation()
	{
		m_CurrentEntryableMono.controlMono.enabled = false;
		for (float timer = 1; timer >= 0; timer -= Time.deltaTime)
		{
			m_ObjectCameraTransform.position = Vector3.Lerp (m_ObjectCameraTransform.position, m_Camera.transform.position, 5 * Time.deltaTime);
			m_ObjectCameraTransform.rotation = Quaternion.Lerp (m_ObjectCameraTransform.rotation, m_Camera.transform.rotation, 5 * Time.deltaTime);
			yield return 0;  
		}
		m_Rigidbody.isKinematic = false;
		m_ObjectCameraTransform.position = m_Camera.transform.position;
		m_ObjectCameraTransform.rotation = m_Camera.transform.rotation;
		m_ObjectCameraTransform.gameObject.SetActive (false);
		m_Camera.gameObject.SetActive (true);
		m_ObjectCameraTransform.parent =m_Camera.transform;
		m_CurrentEntryableMono = null;
	}
	public IEnumerator EntryPhoneAnimation()  
	{  
		if(m_LastOutLineControl!=null)
			m_LastOutLineControl.OutLineOff ();
		m_Rigidbody.velocity = new Vector3 (0,m_Rigidbody.velocity.y,0);
		m_ObjectCameraTransform.gameObject.SetActive (true);
		m_Camera.gameObject.SetActive (false);
		m_ObjectCameraTransform.parent = transform;
//		for(int i=0;i<m_PhoneAniPointArray.Length;i++)
//		{
//			while(true)
//			{
//				if (Vector3.SqrMagnitude (m_PhoneTransform.position - m_PhoneAniPointArray[i].position) < 0.01
//					&& Quaternion.Angle (m_PhoneTransform.rotation, m_PhoneAniPointArray[i].rotation) < 5)
//					break;
//				m_PhoneTransform.position = Vector3.Lerp (m_PhoneTransform.position, m_PhoneAniPointArray[i].position, 5 * Time.deltaTime);
//				m_PhoneTransform.rotation = Quaternion.Lerp (m_PhoneTransform.rotation, m_PhoneAniPointArray[i].rotation, 5 * Time.deltaTime);
//				yield return 0;
//			}
//		}
		while(true)
		{
			if (Vector3.SqrMagnitude (m_PhoneMono.transform.position - m_PhoneAniPointArray[m_PhoneAniPointArray.Length-1].position) < 0.001
				&& Quaternion.Angle (m_PhoneMono.transform.rotation, m_PhoneAniPointArray[m_PhoneAniPointArray.Length-1].rotation) < 1)
				break;
			m_PhoneMono.transform.position = Vector3.Lerp (m_PhoneMono.transform.position, m_PhoneAniPointArray[m_PhoneAniPointArray.Length-1].position, 5 * Time.deltaTime);
			m_PhoneMono.transform.rotation = Quaternion.Lerp (m_PhoneMono.transform.rotation, m_PhoneAniPointArray[m_PhoneAniPointArray.Length-1].rotation, 5 * Time.deltaTime);
			yield return 0;
		}
		m_PhoneMono.transform.position = m_PhoneAniPointArray [m_PhoneAniPointArray.Length - 1].position;
		m_PhoneMono.transform.rotation = m_PhoneAniPointArray [m_PhoneAniPointArray.Length - 1].rotation;
		m_PhoneMono.enabled =  true;
	}
	public IEnumerator ExitPhoneAnimation()  
	{  
		m_PhoneMono.GetComponent<PhoneMono>().enabled =  false;
//		for(int i=m_PhoneAniPointArray.Length-1;i>=0;i--)
//		{
//			while(true)
//			{
//				if (Vector3.SqrMagnitude (m_PhoneTransform.position - m_PhoneAniPointArray[i].position) < 0.01
//					&& Quaternion.Angle (m_PhoneTransform.rotation, m_PhoneAniPointArray[i].rotation) < 5)
//					break;
//				m_PhoneTransform.position = Vector3.Lerp (m_PhoneTransform.position, m_PhoneAniPointArray[i].position, 5 * Time.deltaTime);
//				m_PhoneTransform.rotation = Quaternion.Lerp (m_PhoneTransform.rotation, m_PhoneAniPointArray[i].rotation, 5 * Time.deltaTime);
//				yield return 0;
//			}
//		}
		while(true)
		{
			if (Vector3.SqrMagnitude (m_PhoneMono.transform.position - m_PhoneAniPointArray[0].position) < 0.001
				&& Quaternion.Angle (m_PhoneMono.transform.rotation, m_PhoneAniPointArray[0].rotation) < 1)
				break;
			m_PhoneMono.transform.position = Vector3.Lerp (m_PhoneMono.transform.position, m_PhoneAniPointArray[0].position, 5* Time.deltaTime);
			m_PhoneMono.transform.rotation = Quaternion.Lerp (m_PhoneMono.transform.rotation, m_PhoneAniPointArray[0].rotation, 5 * Time.deltaTime);
			yield return 0;
		}
		m_PhoneMono.transform.position=m_PhoneAniPointArray[0].position;
		m_PhoneMono.transform.rotation=m_PhoneAniPointArray[0].rotation;
		m_ObjectCameraTransform.gameObject.SetActive (false);
		m_Camera.gameObject.SetActive (true);
		m_ObjectCameraTransform.parent = m_Camera.transform;
		m_IsOnPhone = false;
		if(m_LastOutLineControl!=null)
			m_LastOutLineControl.OutLineOn ();
	}
	public IEnumerator ReturnAnimation()
	{
		for (float timer = 0.8f; timer >= 0; timer -= Time.deltaTime) 
		{
			m_ObjectCameraTransform.position=Vector3.Lerp(m_ObjectCameraTransform.position,m_CurrentEntryableMono.cameraTargetTransform.position,5*Time.deltaTime);
			m_ObjectCameraTransform.rotation=Quaternion.Lerp(m_ObjectCameraTransform.rotation,m_CurrentEntryableMono.cameraTargetTransform.rotation,5*Time.deltaTime);
			yield return 0;  
		}
		m_ObjectCameraTransform.position = m_CurrentEntryableMono.cameraTargetTransform.position;
		m_ObjectCameraTransform.rotation = m_CurrentEntryableMono.cameraTargetTransform.rotation;
		m_CurrentEntryableMono.controlMono.enabled =  true;
	}
	public void LeavePhoneAnimation()
	{
		m_PhoneMono.gameObject.SetActive (false);
	}
	public IEnumerator ReturnPhoneAnimation()
	{
		for (float timer = 0.8f; timer >= 0; timer -= Time.deltaTime) 
		{
			m_ObjectCameraTransform.position=Vector3.Lerp(m_ObjectCameraTransform.position,m_Camera.transform.position,5*Time.deltaTime);
			m_ObjectCameraTransform.rotation=Quaternion.Lerp(m_ObjectCameraTransform.rotation,m_Camera.transform.rotation,5*Time.deltaTime);
			yield return 0;  
		}
		m_ObjectCameraTransform.position = m_Camera.transform.position;
		m_ObjectCameraTransform.rotation = m_Camera.transform.rotation;
		m_PhoneMono.gameObject.SetActive (true);
		m_PhoneMono.enabled =  true;
	}

	private IEnumerator CheckAnimation()
	{
		m_Rigidbody.velocity = Vector3.zero;
		Vector3 targetPosition = m_Camera.transform.position + m_CurrentCheckableMono.distance * m_Camera.transform.forward;
		Quaternion targetRotation =Quaternion.LookRotation(m_Camera.transform.forward);
		for (float i = 0; i < 0.8f; i += Time.deltaTime) 
		{
			m_CurrentCheckableMono.transform.position = Vector3.Lerp (m_CurrentCheckableMono.transform.position,targetPosition,10*Time.deltaTime);
			m_CurrentCheckableMono.transform.rotation = Quaternion.Lerp (m_CurrentCheckableMono.transform.rotation, targetRotation, 10 * Time.deltaTime);
			yield return 0;
		}
		m_CurrentCheckableMono.isAnimationOver=true;
	}
	private void Selection()
	{
		RaycastHit hitObject;
		if (Physics.Raycast (m_Camera.ScreenPointToRay (new Vector2 (Screen.width / 2, Screen.height / 2)), out hitObject,100f,~(1<<2|1<<9)) && !m_CurrentCheckableMono && !m_CurrentEntryableMono) {
			OutLineControl tempControl = hitObject.transform.GetComponent<OutLineControl> ();
			if (tempControl != m_LastOutLineControl) {
				if (m_LastOutLineControl != null)
					m_LastOutLineControl.OutLineOff ();
				m_LastOutLineControl = tempControl;
				if (m_LastOutLineControl != null)
					m_LastOutLineControl.OutLineOn ();
			}
		}
		else
		{
			if(m_LastOutLineControl!=null)
				m_LastOutLineControl.OutLineOff ();
		}
	}

	public void GetPhone()
	{
		m_HasPhone = true;
	}

}
