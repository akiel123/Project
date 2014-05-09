using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	float sForward = 0.1F;
	float sBackward = 0.1F;
	float tLeft = 2F;
	float tRight = 2F;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("w")){
				transform.Translate(0,0,sForward);
			}
		if(Input.GetKey("s")){
				transform.Translate(0,0,-sBackward);
			}
		if(Input.GetKey("d")){
			transform.Rotate(0,tRight,0);
			tRight += 0.05F;
		}
		else{
			tRight = 2F;
		}
		if(Input.GetKey("a")){
			transform.Rotate(0,-tLeft,0);
			tLeft += 0.05F;
		}
		else{
			tLeft = 2F;
		}

	}
}
