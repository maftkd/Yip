using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MButton : MonoBehaviour
{
	public UnityEvent _buttonDown;
	public Material _lightMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnMouseDown(){
		Debug.Log("ooh that tickles");
		_buttonDown.Invoke();
		if(_lightMat!=null)
			_lightMat.SetColor("_EmissionColor",Color.white);
	}

	void OnDestroy(){
		if(_lightMat!=null)
			_lightMat.SetColor("_EmissionColor",Color.black);

	}
}
