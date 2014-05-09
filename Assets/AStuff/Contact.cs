using UnityEngine;
using System.Collections;

public class Contact : MonoBehaviour {



	public Location pos;
	public WorldObject obj1;
	public WorldObject obj2;
	
	double nForce;
	
	public Contact(Location position, WorldObject object1, WorldObject object2){
		pos = position;
		obj1 = object1;
		obj2 = object2;
	}
	
	public void checkValidity(){ //!!!! might not work as intended, containing logical error!!!!
		bool valid = true;
		//posx1 the relation between the position of the contact and the center of mass of obj1
		double	posx1 	= pos.x - obj1.massC.x,
				posy1 	= pos.y - obj1.massC.y,
				posz1 	= pos.z - obj1.massC.z,
				posx2 	= pos.x - obj2.massC.x,
				posy2 	= pos.y - obj2.massC.y,
				posz2 	= pos.z - obj2.massC.z;
		//dis2 the distance from te object to the contact
		double 	dis1 = WorldObject.pyth (WorldObject.pyth (posx1, posy1), posz1),
				dis2 = WorldObject.pyth (WorldObject.pyth (posx2, posy2), posz2);
		//spdx1 is the speed of the point on obj1 moving along the x-axis.
		double spdx1 = 0;
	}




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
