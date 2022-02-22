using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
	public int _numWalls;
	public float _radius;
	public float _wallHeight;
	public Transform _wallPrefab;

	[ContextMenu("Generate")]
	public void Generate(){
		Clear();
		float angleR=360f/_numWalls;
		angleR*=0.5f*Mathf.Deg2Rad;
		float rToVertex=_radius/Mathf.Cos(angleR);
		float width = 2f*rToVertex*Mathf.Sin(180*Mathf.Deg2Rad/_numWalls);
		for(int i=0; i<_numWalls; i++){
			if(i==0)
				continue;
			float theta=Mathf.PI*2f*(i/(float)_numWalls);
			Vector3 pos = transform.position+new Vector3(Mathf.Cos(theta),0,Mathf.Sin(theta))*_radius;
			pos+=_wallHeight*0.5f*Vector3.up;
			Transform wall = Instantiate(_wallPrefab,pos,Quaternion.identity,transform);
			wall.eulerAngles=-Vector3.up*(theta*Mathf.Rad2Deg+90);
			Vector3 scale = wall.localScale;
			scale.y=_wallHeight;
			scale.x=width;
			wall.localScale=scale;
		}
	}

	[ContextMenu("Clear")]
	public void Clear(){
		int children=transform.childCount;
		for(int i=children-1;i>=0;i--){
			DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
