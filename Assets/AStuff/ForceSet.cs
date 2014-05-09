using UnityEngine;
using System.Collections;

public class ForceSet : MonoBehaviour {

	public double mx,		//Force moving along the x/y/z axis
	my,
	mz,

	rx,				//Force rotating on  the x/y/z axis
	ry,
	rz;

	public Location pos;
	public WorldObject oObject;

	public ForceSet(double mx, double my, double mz, double rx, double ry, double rz, Location position, WorldObject originObject){
		this.mx = mx;
		this.my = my;
		this.mz = mz;
		this.rx = rx;
		this.ry = ry;
		this.rz = rz;
		this.pos = position;
		oObject = originObject;
	}

	public void setForces(double mx, double my, double mz, double rx, double ry, double rz){
		this.mx = mx;
		this.my = my;
		this.mz = mz;
		this.rx = rx;
		this.ry = ry;
		this.rz = rz;
	}


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
