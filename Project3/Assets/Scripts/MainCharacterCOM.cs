using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterCOM : MonoBehaviour {

    [System.Serializable]

    public struct namedMOI{
        public string name, moi, hsqr, Mhsqr;
        public namedMOI(string n, string m, string h, string M)
        {
            name = n;
            moi = m;
            hsqr = h;
            Mhsqr = M;
        }
    }

    private float GetFloat(string stringValue, float defaultValue)
    {
        float result = defaultValue;
        float.TryParse(stringValue, out result);
        return result;
    }

    public Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 posCenterOfMass = new Vector3(0.0f, 0.0f, 0.0f);


    public Vector3 posPilot = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 posGun = new Vector3(0.0f, 0.0f, 0.0f);



    public GameObject hull;
    public GameObject gun;
    public GameObject pilot;
    public GameObject centerOfMass;

    public int massHull = 500;
    public int massGun = 100;
    public int massPilot = 60;    
    public int massTotal = 0;

    public namedMOI[] MomentOfInertia;
    public string TotalMomentOfInertia;
    
    private Dictionary<string, string> d_MomentOfInertia = new Dictionary<string, string>();
    private Dictionary<string, string> d_hsqr = new Dictionary<string, string>();
    private Dictionary<string, string> d_Mhsqr = new Dictionary<string, string>();


    private Rigidbody MainCharacterRigidbody;

    // Use this for initialization
    void Start () {

        //assign all the masses to the gameOjbects
        LocalMass l_mass = hull.GetComponent<LocalMass>();
        l_mass.mass = massHull;
        l_mass = gun.GetComponent<LocalMass>();
        l_mass.mass = massGun;
        l_mass = pilot.GetComponent<LocalMass>();
        l_mass.mass = massPilot;

        MainCharacterRigidbody = GetComponent<Rigidbody>();
        massTotal = massHull + massPilot + massGun;

        //calculate the position of the center of mass then locate GameObject there
        posPilot = pilot.transform.position;
        posGun = gun.transform.position;
        float comX = (massGun * posGun.x + massPilot * posPilot.x) / massTotal;
        float comZ = (massGun * posGun.z + massPilot * posPilot.z) / massTotal;
        posCenterOfMass.x = comX;
        posCenterOfMass.z = comZ;
        centerOfMass.transform.position = posCenterOfMass;

        //calculate the total Moment of intertia
        CalculateMomentOfInertia(hull);
        CalculateMomentOfInertia(gun);
        CalculateMomentOfInertia(pilot);
        Calculatehsquared(hull);
        Calculatehsquared(gun);
        Calculatehsquared(pilot);
        CalculateMhsquared(hull);
        CalculateMhsquared(gun);
        CalculateMhsquared(pilot);
        //MomentOfInertia = new string[] { d_MomentOfInertia["Hull"], d_MomentOfInertia["Gun"], d_MomentOfInertia["Pilot"] };
        MomentOfInertia = new namedMOI[] { new namedMOI("Hull", d_MomentOfInertia["Hull"],d_hsqr["Hull"],d_Mhsqr["Hull"]),
                                           new namedMOI("Gun", d_MomentOfInertia["Gun"],d_hsqr["Gun"],d_Mhsqr["Gun"]),
                                           new namedMOI("Pilot", d_MomentOfInertia["Pilot"],d_hsqr["Pilot"],d_Mhsqr["Pilot"]) };
        CalculateTotalMOI();
        
    }
    // calculate the moment of inertia of the gameobject and record the value to the Dictionary d_MomentOfInertia
    void CalculateMomentOfInertia(GameObject gameobject)
    {
        float xScale = MainCharacterRigidbody.transform.localScale.x;
        float zScale = MainCharacterRigidbody.transform.localScale.z;
        float xDim = gameobject.transform.localScale.x * xScale;
        float zDim = gameobject.transform.localScale.z * zScale;
        LocalMass l_mass = gameobject.GetComponent<LocalMass>();
        float moi = l_mass.mass * (xDim * xDim + zDim * zDim) / 12;
        string result = moi.ToString(" 0.000e+0");
        d_MomentOfInertia[gameobject.name] = result;
        
        //Debug.LogWarning("for gameobject " + gameobject.name);
       // Debug.LogWarning("the mass is " + l_mass.mass.ToString() + ", xDim=" + xDim.ToString() + ", zDim=" + zDim.ToString() + ", the moi is " + result);
       
    }

    // calculate the h^2 of the gameobject and record the value to the Dictionary d_hsqr
    void Calculatehsquared(GameObject gameobject)
    {
        Vector3 pos_com = centerOfMass.transform.position;
        Vector3 pos_gameobject = gameobject.transform.position;
        Vector3 h = pos_gameobject - pos_com;
        float hsqr = h.sqrMagnitude;
        string result = hsqr.ToString(" 0.000e+0");
        d_hsqr[gameobject.name] = result;

        //Debug.LogWarning("for gameobject " + gameobject.name);
        // Debug.LogWarning("the mass is " + l_mass.mass.ToString() + ", xDim=" + xDim.ToString() + ", zDim=" + zDim.ToString() + ", the moi is " + result);

    }

    // calculate the the h^2 of the gameobject and record the value to the Dictionary d_Mhsqr
    void CalculateMhsquared(GameObject gameobject)
    {
        
        LocalMass l_mass = gameobject.GetComponent<LocalMass>();
        float hsqr = GetFloat(d_hsqr[gameobject.name], 0.0F);
        float mhsqr = l_mass.mass * hsqr;        
        string result = mhsqr.ToString(" 0.000e+0");
        d_Mhsqr[gameobject.name] = result;

        //Debug.LogWarning("for gameobject " + gameobject.name);
        // Debug.LogWarning("the mass is " + l_mass.mass.ToString() + ", xDim=" + xDim.ToString() + ", zDim=" + zDim.ToString() + ", the moi is " + result);

    }
    // Calculate the total Moment of Inertia
    void CalculateTotalMOI()
    {
        float h = GetFloat(d_MomentOfInertia["Hull"], 0.0F) + GetFloat(d_Mhsqr["Hull"], 0.0F);
        float g = GetFloat(d_MomentOfInertia["Gun"], 0.0F) + GetFloat(d_Mhsqr["Gun"], 0.0F);
        float p = GetFloat(d_MomentOfInertia["Pilot"], 0.0F) + GetFloat(d_Mhsqr["Pilot"], 0.0F);
        float t = h + g + p;
        TotalMomentOfInertia = t.ToString(" 0.00e+0");

    }


    // Update is called once per frame
    void FixedUpdate () {
        //track the position of the main character
        position = MainCharacterRigidbody.position;
        massTotal = massHull + massPilot + massGun;
    }
}
