using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	bool _goingUp;
	public float _height;
	Vector3 _topPos;
	Vector3 _bottomPos;
	public float _moveSpeed;
	CameraController _player;
	AudioSource _audioSource;
	public float _targetVolume;
	public float _fadeRate;

	void Awake(){
		_goingUp=true;
		_topPos=transform.position;
		_bottomPos=_topPos+Vector3.down*_height;
		_player=FindObjectOfType<CameraController>();
		ActivateWalls(false);
		_audioSource=GetComponent<AudioSource>();
		_audioSource.Play();
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(_goingUp){
			if(transform.position.y<_topPos.y){
				if(_audioSource.volume<_targetVolume)
				{
					_audioSource.volume+=_fadeRate*Time.deltaTime;
				}

				transform.position+=Vector3.up*Time.deltaTime*_moveSpeed;
				if(transform.position.y>_topPos.y){
					transform.position=_topPos;
					ActivateWalls(false);
				}
			}
			else{
				if(_audioSource.volume>0)
				{
					_audioSource.volume-=_fadeRate*Time.deltaTime;
				}
			}
		}
		else{
			if(transform.position.y>_bottomPos.y){
				if(_audioSource.volume<_targetVolume)
				{
					_audioSource.volume+=_fadeRate*Time.deltaTime;
				}
				transform.position+=Vector3.down*Time.deltaTime*_moveSpeed;
				if(transform.position.y<_bottomPos.y){
					transform.position=_bottomPos;
					ActivateWalls(false);
				}
			}
			else{
				if(_audioSource.volume>0)
				{
					_audioSource.volume-=_fadeRate*Time.deltaTime;
				}
			}

		}
        
    }

	public void ToggleDir(){
		//make sure player is standing on elevator pad first
		RaycastHit hit;
		if(Physics.Raycast(_player.transform.position,Vector3.down,out hit, 3f, 1)){
			if(hit.transform!=transform)
				return;
		}

		ActivateWalls(true);
		
		_goingUp=!_goingUp;
	}

	public void ActivateWalls(bool active){
		foreach(Transform t in transform.GetChild(1))
			t.gameObject.SetActive(active);
		_player.transform.SetParent(active?transform:null);
	}

}
