using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour {

//No more testing

	public static double decRad = 0.0174532925;
	public WorldObject	parent;
	private ForceSet[] 	forceSource = new ForceSet[1];
	private ForceSet 	currentForce;
	private ForceSet 	resultingForce; //The force to be moved according to.
	public double 		weight;
	public Location 	massC;
	public Contact[] 	contactPoint;
	public Momentum 	momentum;
	public bool			complexObject;
	public bool			hasParent;
	private double		CxEquil,	//Might not be needed
						CyEquil,
						CzEquil;
	WorldObject[] 		components;
	public Location 	inertimomentum;

	public int weightResolution = 10; //The maximum number of sub components to be considered when doing calculations considering weight balance

	public WorldObject(Location centerOfMass, double weight, Location velocity, Momentum momentum, WorldObject[] components){
		this.weight = weight;
		this.massC = centerOfMass;
		this.momentum = momentum;
		complexObject = false;
		applyGravity ();
	}

	public WorldObject(WorldObject[] components){
		this.components = components;
		updateComponents ();
		complexObject = true;
	}




	public void updateComponents(){
		double	baseX = components [0].massC.x,
				baseY = components [0].massC.y,
				baseZ = components [0].massC.x;
		double	baseWeight = components [0].weight;
		for(int i = 1; i < components.Length; i++){
			double dx = components[i].massC.x - baseX;
			double dy = components[i].massC.y - baseY;
			double dz = components[i].massC.z - baseZ;
			//WeightBalance: Determines how big an impact the weight of the component has on shifting the position of massC.
			double weightBalance = components[i].weight / baseWeight;	
			baseX += dx * weightBalance;
			baseY += dy * weightBalance;
			baseZ += dz * weightBalance;
			baseWeight += components[i].weight;
		}
		massC = new Location (baseX, baseY, baseZ);
		weight = baseWeight;
	}
	public void addComponent(WorldObject objectToBeAdded){
		objectToBeAdded.addParent (this);
		this.complexObject = true;
		WorldObject[] temp = new WorldObject[components.Length + 1];
		for (int i = 0; i < components.Length; i++) {
			temp[i] = components[i];
		}
		temp [temp.Length - 1] = objectToBeAdded;
		components = temp;
		updateInertimomentum ();
		applyGravity ();
	}
	public void addParent(WorldObject parent){
		this.hasParent = true;
		this.parent = parent;
	}

	public void moveObject(){
		//Handles movement of the object
		updateForce ();
	}

	private void applyGravity(){
		double gravitationalPull = -9.82 * weight;
		forceSource [0] = new ForceSet (0, gravitationalPull, 0, 0, 0, 0, massC, this);
	}

	public void updateInertimomentum(){
		double	momentumx = 0,
				momentumy = 0,
				momentumz = 0;
		for (int i = 0; i < components.Length; i++) {
			double 	dx = components[i].massC.x - massC.x,
					dy = components[i].massC.y - massC.y,
					dz = components[i].massC.z - massC.z;
			momentumx += components[i].weight * pow(dx, 2);
			momentumy += components[i].weight * pow(dy, 2);
			momentumz += components[i].weight * pow(dz, 2);
		}
		inertimomentum = new Location (momentumx, momentumy, momentumz);
	}

	public void updateForce(){
		double rotationx = 0;
		double rotationy = 0;
		double rotationz = 0;
		double movementx = 0;
		double movementy = 0;
		double movementz = 0;
		for (int i = 0; i < forceSource.Length; i++) {
			//dx = the difference of the x position of the Force and the Center of Mass of the object
			double dx 		= forceSource[i].pos.x - massC.x;
			double dy		= forceSource[i].pos.y - massC.y;
			double dz		= forceSource[i].pos.z - massC.z;
			//powerx: the radius of the circle from the x-axis.
			double powerx	= pyth(dy, dz);
			double powery	= pyth(dx, dz);
			double powerz	= pyth(dx, dy);
			//fRotxy: fraction of force caused by the y power, applied to the rotation on the x axis.
			double fRotxy	= dz / powerx;
			double fRotxz	= dy / powerx;
			double fRotyx	= dz / powery;
			double fRotyz	= dx / powery;
			double fRotzx	= dy / powerz;
			double fRotzy	= dx / powerz;
			//fMovx: fraction of force caused by the x power, applied to the movement on the x axis.
			double fMovx	= 1 - (fRotyx + fRotzx);
			double fMovy	= 1 - (fRotxy + fRotzy);
			double fMovz	= 1 - (fRotxz + fRotyz);
			//rotationx: Rotation force on the x-axis.	 movementx: Movement force along the x-axis.
			rotationx += (forceSource[i].my * fRotzx) + (forceSource[i].mz * fRotxy);
			rotationy += (forceSource[i].mx * fRotzy) + (forceSource[i].mz * fRotyx);
			rotationz += (forceSource[i].mx * fRotyz) + (forceSource[i].my * fRotzx);
			movementx +=  forceSource[i].mx * fMovx;
			movementy +=  forceSource[i].my * fMovy;
			movementz +=  forceSource[i].mz * fMovz;
		}
		for(int j = 0; j < components.Length; j++){
			for (int i = 0; i < forceSource.Length; i++) {
				//dx = the difference of the x position of the Force and the Center of Mass of the object
				double dx 		= components[j].forceSource[i].pos.x - massC.x;
				double dy		= components[j].forceSource[i].pos.y - massC.y;
				double dz		= components[j].forceSource[i].pos.z - massC.z;
				//powerx: the radius of the circle from the x-axis.
				double powerx	= pyth(dy, dz);
				double powery	= pyth(dx, dz);
				double powerz	= pyth(dx, dy);
				//fRotxy: fraction of force caused by the y power, applied to the rotation on the x axis.
				double fRotxy	= dz / powerx;
				double fRotxz	= dy / powerx;
				double fRotyx	= dz / powery;
				double fRotyz	= dx / powery;
				double fRotzx	= dy / powerz;
				double fRotzy	= dx / powerz;
				//fMovx: fraction of force caused by the x power, applied to the movement on the x axis.
				double fMovx	= 1 - (fRotyx + fRotzx);
				double fMovy	= 1 - (fRotxy + fRotzy);
				double fMovz	= 1 - (fRotxz + fRotyz);
				//rotationx: Rotation force on the x-axis.	 movementx: Movement force along the x-axis.
				rotationx += (components[j].forceSource[i].my * fRotxy) + (components[j].forceSource[i].mz * fRotxz);
				rotationy += (components[j].forceSource[i].mx * fRotyx) + (components[j].forceSource[i].mz * fRotyz);
				rotationz += (components[j].forceSource[i].mx * fRotzx) + (components[j].forceSource[i].my * fRotzy);
				movementx +=  components[j].forceSource[i].mx * fMovx;
				movementy +=  components[j].forceSource[i].my * fMovy;
				movementz +=  components[j].forceSource[i].mz * fMovz;
			}
		}
		currentForce.setForces(movementx, movementy, movementz, rotationx, rotationy, rotationz);
	}

	public void calculateRestrictiveFriction(double time){ //time is time in millisiconds, that the movement takes place.
		for (int i = contactPoint.Length - 1; i > 0; i--) {
			contactPoint[i].checkValidity();
				}
		//smxp the stopping force of movement in the a positive direction of x. This is all movmenet applied by rotation on the x axis
		//in a positive direction and all movement caused by moving in a positive direction on the x axis when following relation between
		//dy and dx is true dy * dx < 0. View it as a two dimensional coordinate system. When standing on it, looking along the x-axis
		//the front left corner is positive because any dx and dy values would be dx>0 and dy>0 and if encountering a block in one such position,
		//it would cause the object to rotate positively around the y-axis. Howver, rotation in unity is negated, and thus this will be regarded
		//as a negative rotation value.
		double	smxp = 0,
				smxn = 0,
				smyp = 0,
				smyn = 0,
				smzp = 0,
				smzn = 0,
				srxp = 0,
				srxn = 0,
				sryp = 0,
				sryn = 0,
				srzp = 0,
				srzn = 0;
			
		for(int i = 0; i < contactPoint.Length; i++){
			//Distance between the center of mass and the contact point.
			double 	dx = contactPoint[i].pos.x - massC.x,
					dy = contactPoint[i].pos.y - massC.y,
					dz = contactPoint[i].pos.z - massC.z;
			double	tdx = dx / (2 * Mathf.PI), //Divide with 2Pi so sxt is converted to meters/s.
					tdy = dy / (2 * Mathf.PI),
					tdz = dz / (2 * Mathf.PI);

			//Translational Speed
			Location st = momentum.getSpeedTranslational();
			//Rotational Speed
			Location sr = momentum.getSpeedRotational();
			
			//powerx: the radius of the circle from the x-axis.
			double	powerx	= pyth(dy, dz),
					powery	= pyth(dx, dz),
					powerz	= pyth(dx, dy);
			//fRotxy: fraction of force caused by the y rotational power, applied to the rotation on the x axis.
			double	fRotxy	= dz / powerx,
					fRotxz	= dy / powerx,
					fRotyx	= dz / powery,
					fRotyz	= dx / powery,
					fRotzx	= dy / powerz,
					fRotzy	= dx / powerz;
			//The speed the contact point is moving in a certain direction caused by the rotational motion measured in m/s
			double	sxt = sr.z * fRotyx * tdy + sr.y * fRotzx * tdz,
					syt = sr.z * fRotxy * tdx + sr.x * fRotzy * tdz,
					szt = sr.y * fRotxz * tdx + sr.x * fRotyz * tdy;


			if(dx * dy < 0){
			//	smzp += contactPoint[i].obj2.momentum.smx;
			}
		}
	}

	public static double pyth(double a1, double b1) {
		float a = (float)(a1);
		float b = (float)(b1);
		return Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
	}
	public static double cos (double a1){ //uses degrees
		float a = (float)(a1 / decRad);
		return Mathf.Cos (a);
	}
	public static double sin (double a1){ //uses degrees
		float a = (float)(a1 / decRad);
		return Mathf.Sin (a);
	}
	public static double pow (double a1, double b1){
		float a = (float)(a1);
		float b = (float)(b1);
		return Mathf.Pow (a, b);
	}
	public static double sqrt(double a1){
				float a = (float)(a1);
				return Mathf.Sqrt (a);
	}

	//Determines at what distance from the center there is equally much mass further away from the center and between that point and the center
	/*public double getRadiusOfMassEquilibrium(double x1, double y1, double z1){ 	//Dont think that this is needed any more...
		double standardVariable = x1 + y1 + z1;
		double	angleX = x1 / standardVariable, //probably logistically wrong
				angleY = y1 / standardVariable, //
				angleZ = z1 / standardVariable; //
		double 	x = cos (angleY * 360) * CyEquil + cos (angleZ * 360) * CzEquil,
				y = cos (angleX * 360) * CxEquil + cos (angleZ * 360) * CzEquil,
				z = cos (angleX * 360) * CxEquil + cos (angleY * 360) * CyEquil;

		return pyth (pyth(sin
	}*/











	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
