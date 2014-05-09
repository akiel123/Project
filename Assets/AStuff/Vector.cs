using UnityEngine;
using System.Collections;

public class Vector : MonoBehaviour {

	public readonly double	x,
							y,
							z,
							power;

	public Vector(double x, double y, double z){
		this.x = x; 
		this.y = y;
		this.z = z;
		power = WorldObject.pyth (WorldObject.pyth (x, y), z);
	}






	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
