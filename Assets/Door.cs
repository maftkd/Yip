using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public Vector3 _minBox;
	public Vector3 _maxBox;
	Transform _player;
	bool _playerInZone;
	bool _playerPrevInZone;
	Transform _door;
	Quaternion _closeRot;
	Quaternion _openRot;
	Collider _col;

	public AudioClip _doorOpen;
	public AudioClip _doorClose;
	public float _vol;
	public Vector2 _doorRange;

	void Awake(){
		_player=Camera.main.transform;
		_door=transform.GetChild(0);
		_closeRot=_door.rotation;
		_door.Rotate(Vector3.up*-90f);
		_openRot=_door.rotation;
		_door.rotation=_closeRot;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		_playerInZone=false;
		if(_player.position.x>transform.position.x+_minBox.x&&_player.position.x<transform.position.x+_maxBox.x){
			if(_player.position.z>transform.position.z+_minBox.z&&_player.position.z<transform.position.z+_maxBox.z){
				if(_player.position.y>transform.position.y+_minBox.y&&_player.position.y<transform.position.y+_maxBox.y){
					_playerInZone=true;;
				}
			}
		}
		_door.rotation=_playerInZone?_openRot : _closeRot;

		if(_playerInZone!=_playerPrevInZone){
			Sfx.PlayOneShot3D(_playerInZone ? _doorOpen : _doorClose, transform.position,Random.Range(_doorRange.x,_doorRange.y),_vol);
		}

		_playerPrevInZone=_playerInZone;
    }

	void OnDrawGizmos(){
		Vector3 center = Vector3.Lerp(_minBox,_maxBox,0.5f);
		Vector3 extents=_maxBox-_minBox;
		Gizmos.DrawWireCube(transform.position+center,extents);
	}
}
