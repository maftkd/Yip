using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoHelper : MonoBehaviour
{
	public RenderTexture _renderTexture;

	void Awake(){
		ResetRT();
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ResetRT(){
		RenderTexture rt = RenderTexture.active;
		RenderTexture.active = _renderTexture;
		GL.Clear(true, true, Color.black);
		RenderTexture.active = rt;

	}
}
