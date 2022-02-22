using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Footstep : MonoBehaviour
{
	public AudioClip [] _clips;
	[Range(0f,1f)]
	public float _volume;
	float _walkSpeed;
	float _runSpeed;
	public UnityEvent _onPlay;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Sound(Vector3 pos,float volume=-1f){
		if(_clips.Length==0)
			return;
		Sfx.PlayOneShot3DVol(_clips[Random.Range(0,_clips.Length)],pos,volume<0? _volume : volume);
	}
}
