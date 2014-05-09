using UnityEngine;
using System.Collections;

public class Muscle : MonoBehaviour {

	public double tFib; //Amount of twitch    fibers
	public double eFib; //Amount of endurance fibers

	protected double basePowTFib  = 0.1	; //Power of the work 1 twitch	  fiber can do in Newton, on full power
	protected double basePowEFib  = 0.04; //Power of the work 1 endurance fiber can do in Newton, on full power
	protected double cPowTFib = 0.1	; //Current power of the work 1 twitch    fiber can do in Newton
	protected double cPowEFib =	0.04; //Current power of the work 1 endurance fiber can do in Newton
	protected double halfTFib = 5	; //Time it takes for the twitch    fibers to go to half max power on full power
	protected double halfEFib = 60	; //Time it takes for the endurance fibers to go to half max power on full power
	protected double wTFib = 5; //weight of one twitch    fiber in grams
	protected double wEFib = 7; //weight of one endurance fiber in grams
	protected double sTFib = 10; //size   of one twitch    fiber in grams
	protected double sEFib = 12; //size   of one endurance fiber in grams



	public double maxSpeed(){ //returns the maximum speed in meters per second, m/s
		double m1 = 24120; 	//mass of leg
		double m2 = 80123; 	//mass of upper body
		double d1 = 1.2f;	//distance leg has to travel
		double d2 = 35210f;	//distance body has to travel
		double N  = 100412;	//Force applied to motion
		
		double a2 = (N - m1 * (d1 / d2)) / m2;
		double a1 = d1/d2 * a2;
	
		double t1 = Mathf.Sqrt((float)((2*d1)/a1));
		double t2 = Mathf.Sqrt((float)((2*d2)/a2));

		return d1 / t1;
	}

	void Update(){
		print ("Hey this is it  " + maxSpeed());
		/*print (basePowTFib);
		print (cPowEFib);
		print (cPowTFib);
		print (halfEFib);
		print (halfTFib);
		print (wEFib);
		print (wTFib);
		print (sTFib);
		print (sEFib);*/
	}
}
