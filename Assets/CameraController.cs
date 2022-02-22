using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	Camera _cam;
	public float _mouseLookX;
	public float _mouseLookY;
	int _state;
	public float _moveSpeed;
	Collider [] _colliders;
	public float _wallHitRadius;
	
	//signage
	public Transform _startSign;
	Transform _curSign;
	public float _stepTime;
	public AnimationCurve _stepCurve;

	//step audio
	float _stepTimer;
	public float _stepDur;

	void Awake(){
		_cam=GetComponent<Camera>();
		Camera[] cams = FindObjectsOfType<Camera>();
		foreach(Camera c in cams){
			if(c!=_cam)
				c.enabled=false;
		}

		Cursor.visible=false;
		Cursor.lockState = CursorLockMode.Locked;

		_colliders = new Collider[3];

		//SnapToSign(_startSign);
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		switch(_state){
			case 0:
			default:
				float zIn = Input.GetAxis("Vertical");
				float xIn = Input.GetAxis("Horizontal");
				Vector2 move = new Vector2(xIn,zIn);

				float mouseX= Input.GetAxis("Mouse X");
				float mouseY = Input.GetAxis("Mouse Y");

				Vector3 eulers=transform.eulerAngles;
				eulers.y+=mouseX*_mouseLookX;
				if(eulers.x>180f)
					eulers.x=-(360f-eulers.x);
				else if(eulers.x<-180f)
					eulers.x=(360f-eulers.x);
				eulers.x+=mouseY*_mouseLookY;
				eulers.x=Mathf.Clamp(eulers.x,-60f,60f);
				transform.eulerAngles=eulers;

				Vector3 flatForward=transform.forward;
				flatForward.y=0;
				flatForward.Normalize();
				Vector3 flatRight=-Vector3.Cross(flatForward,Vector3.up);
				if(move.sqrMagnitude>1f)
					move.Normalize();
				move*=_moveSpeed;
				transform.position+=flatForward*Time.deltaTime*move.y;
				transform.position+=flatRight*Time.deltaTime*move.x;

				//step
				if(move.sqrMagnitude>0f){
					if(_stepTimer==0){
						RaycastHit hit;
						if(Physics.Raycast(transform.position,Vector3.down, out hit, 2f, 1)){
							if(hit.transform.GetComponent<Footstep>())
								hit.transform.GetComponent<Footstep>().Sound(hit.point);
						}

					}
					_stepTimer+=Time.deltaTime;
					if(_stepTimer>_stepDur){
						_stepTimer=0f;
					}
				}
				else
				{
					if(_stepTimer!=0){
						/*
						RaycastHit hit;
						if(Physics.Raycast(transform.position,Vector3.down, out hit, 2f, 1)){
							if(hit.transform.GetComponent<Footstep>())
								hit.transform.GetComponent<Footstep>().Sound(hit.point);
						}
						*/
					}
					_stepTimer=0f;
				}

				//collision
				int numColliders= Physics.OverlapSphereNonAlloc(transform.position, 
						_wallHitRadius, _colliders);
				Vector3 closePoint=Vector3.zero;

				int attempts=0;
				while(numColliders>0&&attempts<5){
					for(int i=0; i<numColliders; i++){
						closePoint=_colliders[i].ClosestPoint(transform.position);
						//check if transform is still within collider
						if((closePoint-_colliders[i].transform.position).sqrMagnitude <
								(transform.position-_colliders[i].transform.position).sqrMagnitude)
						{
							//offset player from wall
							transform.position=closePoint+
								(transform.position-closePoint).normalized*_wallHitRadius;
						}
					}

					numColliders= Physics.OverlapSphereNonAlloc(transform.position, 
							_wallHitRadius, _colliders);
					attempts++;
				}

				if(Input.GetKeyDown(KeyCode.Escape))
				{
					Cursor.visible=true;
					Cursor.lockState = CursorLockMode.None;
					_state=1;
				}
				break;
			case 1:
				if(Input.GetKeyDown(KeyCode.Tab)){
					Cursor.visible=false;
					Cursor.lockState = CursorLockMode.Locked;
					_state=0;
				}
				break;
			case 2:
				float zIn2 = Input.GetAxis("Vertical");
				float xIn2 = Input.GetAxis("Horizontal");
				Vector2 move2 = new Vector2(xIn2,zIn2);
				if(move2.sqrMagnitude>=0.25f){
					StepBackFromCurSign();
				}
				break;
		}
    }

	public void SnapToSign(Transform sign){
		Camera snapCam=sign.Find("SignCam").GetComponent<Camera>();
		_state=2;
		transform.position=snapCam.transform.position;
		transform.rotation=snapCam.transform.rotation;
		_cam.fieldOfView=snapCam.fieldOfView;
		_curSign=sign;
	}

	void StepBackFromCurSign(){
		StartCoroutine(StepBack());
	}

	IEnumerator StepBack(){
		Vector3 startPos=transform.position;
		Quaternion startRot=transform.rotation;
		float startFov=_cam.fieldOfView;

		Camera endCam = _curSign.Find("StandCam").GetComponent<Camera>();
		Vector3 endPos = endCam.transform.position;
		Quaternion endRot = endCam.transform.rotation;
		float endFov = endCam.fieldOfView;

		float timer=0;
		float dur = _stepTime;
		while(timer<dur){
			timer+=Time.deltaTime;
			float frac = _stepCurve.Evaluate(timer/dur);
			transform.position=Vector3.Lerp(startPos,endPos,frac);
			transform.rotation=Quaternion.Slerp(startRot,endRot,frac);
			_cam.fieldOfView=Mathf.Lerp(startFov,endFov,frac);
			yield return null;
		}

		transform.position=endPos;
		transform.rotation=endRot;
		_cam.fieldOfView=endFov;

		_state=0;
	}
}
