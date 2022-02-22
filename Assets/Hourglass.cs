using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hourglass : MonoBehaviour
{
	public Color _sandLight;
	public Color _sandDark;
	public ParticleSystem _sandParts;
	public float _sandFallTime;
	public float _flipDur;
	public AnimationCurve _flipCurve;
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(DrainSand());
    }

	IEnumerator DrainSand(){
		MeshRenderer bot=transform.GetChild(0).GetComponent<MeshRenderer>();
		MeshRenderer top=transform.GetChild(1).GetComponent<MeshRenderer>();
		Material botMat=bot.material;
		Material topMat=top.material;
		botMat.SetColor("_Color",_sandLight);
		topMat.SetColor("_Color",_sandDark);
		botMat.SetFloat("_FillAmount",0);
		botMat.SetFloat("_Flip",0);
		topMat.SetFloat("_FillAmount",0);
		topMat.SetFloat("_Flip",1);

		var col = _sandParts.colorOverLifetime;
		Gradient grad = new Gradient();
		grad.SetKeys(new GradientColorKey[] { new GradientColorKey(_sandDark, 0.0f), new GradientColorKey(_sandLight, 0.5f) }, 
				new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );
		col.color=grad;

		//sand fall 1
		float timer=0;
		while(timer<_sandFallTime){
			timer+=Time.deltaTime;
			float t= timer/_sandFallTime;
			botMat.SetFloat("_FillAmount",t);
			topMat.SetFloat("_FillAmount",t);
			yield return null;
		}
		_sandParts.Stop();

		Quaternion curRot=transform.rotation;
		transform.Rotate(-Vector3.forward*180f);
		Quaternion endRot=transform.rotation;

		//sand flip 1
		timer=0;
		while(timer<_flipDur){
			timer+=Time.deltaTime;
			float t = _flipCurve.Evaluate(timer/_flipDur);
			transform.rotation=Quaternion.Slerp(curRot,endRot,t);
			botMat.SetColor("_Color",Color.Lerp(_sandLight,_sandDark,t));
			topMat.SetColor("_Color",Color.Lerp(_sandDark,_sandLight,t));
			yield return null;
		}

		timer=0;
		_sandParts.transform.Rotate(Vector3.up*180);
		_sandParts.Play();
		botMat.SetFloat("_Flip",1);
		botMat.SetFloat("_FillAmount",0);
		topMat.SetFloat("_Flip",0);
		topMat.SetFloat("_FillAmount",0);
		//sand fall 2
		while(timer<_sandFallTime){
			timer+=Time.deltaTime;
			botMat.SetFloat("_FillAmount",timer/_sandFallTime);
			topMat.SetFloat("_FillAmount",timer/_sandFallTime);
			yield return null;
		}

		_sandParts.Stop();
		curRot=transform.rotation;
		transform.Rotate(-Vector3.forward*180f);
		endRot=transform.rotation;
		//sand flip 2
		timer=0;
		while(timer<_flipDur){
			timer+=Time.deltaTime;
			float t = _flipCurve.Evaluate(timer/_flipDur);
			transform.rotation=Quaternion.Slerp(curRot,endRot,t);
			botMat.SetColor("_Color",Color.Lerp(_sandLight,_sandDark,1-t));
			topMat.SetColor("_Color",Color.Lerp(_sandDark,_sandLight,1-t));
			yield return null;
		}

		_sandParts.transform.Rotate(Vector3.up*180);
		_sandParts.Play();
		StartCoroutine(DrainSand());
	}
}
