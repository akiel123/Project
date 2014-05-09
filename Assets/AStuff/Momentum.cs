using UnityEngine;
using System.Collections;

public class Momentum : MonoBehaviour {

	private double	mx, //Force of translational momentum on the x axis.
					my,
					mz,
					rx, //Rotational momentum on the x axis
					ry,
					rz;
	private WorldObject obj;

	public Momentum(double mx, double my, double mz, double rx, double ry, double rz, WorldObject relatedObject){
		this.mx = mx;
		this.my = my;
		this.mz = mz;
		this.rx = rx;
		this.ry = ry;
		this.rz = rz;
		obj 	= relatedObject;
		}

	public void accelerate(double timeInSeconds, double workingEnergymx, double workingEnergymy, double workingEnergymz, double workingEnergyrx, double workingEnergyry, double workingEnergyrz){
		double reduction = obj.weight / timeInSeconds;
		mx += workingEnergymx / reduction;
		my += workingEnergymy / reduction;
		mz += workingEnergymz / reduction;
		rx += workingEnergyrx / reduction;
		ry += workingEnergyry / reduction;
		rz += workingEnergyrz / reduction;
	}

	public Location getSpeedTranslational(){ //Calculates the rotational speed in meters/s
		double mass = obj.weight;
		return new Location(WorldObject.sqrt(mx / (0.5 * mass)), WorldObject.sqrt(my / (0.5 * mass)), WorldObject.sqrt(mz / (0.5 * mass)));
	}

	public Location getSpeedRotational(){ //Calculates the rotational speed in radians
		Location inerti = obj.inertimomentum;
		return new Location(WorldObject.sqrt(rx / (0.5 * inerti.x)), WorldObject.sqrt(ry / (0.5 * inerti.y)), WorldObject.sqrt(rz / (0.5 * inerti.z)));
	}






	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
