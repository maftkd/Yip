using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSimple : MonoBehaviour
{
	Light _light;
	Material _mat;
	public float _animDur;
	AudioSource _audio;
	public float _targetThresh;
	public float _targetLight;

	void Awake(){
		_light=transform.GetChild(0).GetComponent<Light>();
		_mat=GetComponent<MeshRenderer>().material;
		_audio=GetComponent<AudioSource>();
	}

    // Start is called before the first frame update
    void Start()
    {
		//Animate();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Animate(){
		StartCoroutine(AnimateR());
	}

	IEnumerator AnimateR(){
		float startThresh=1f;
		float endThresh=_targetThresh;
		float startLight=0f;
		float endLight=_targetLight;

		float timer=0;
		float dur = _animDur;

		while(timer<dur){
			timer+=Time.deltaTime;
			float frac=timer/dur;
			_mat.SetFloat("_GlowThresh",Mathf.Lerp(startThresh,endThresh,frac));
			_light.intensity=Mathf.Lerp(startLight,endLight,frac);
			yield return null;
		}
	}
}
