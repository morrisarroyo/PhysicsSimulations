using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Values : MonoBehaviour
{
    public Position position;
    public Mass mass;
    public MomentOfInertiaZ momentOfInertiaZ;
    public _h2 _h2;
    public Mh2 mh2;
    public TotalMoementOfInertia totalMomentOfInertia;

    private GameObject _boat;
    private GameObject _gun;
    private GameObject _pilot;
    private GameObject _com;

    // Use this for initialization
    void Start()
    {
        _boat = GameObject.Find("Boat");
        _gun = GameObject.Find("Gun");
        _pilot = GameObject.Find("Pilot");
        _com = GameObject.Find("Com");

        position = new Position
        {
            hull = _boat.transform.position,
            pilot = _pilot.transform.position,
            gun = _gun.transform.position,
            com = new Vector3(0, 0, 0)
        };

        mass = new Mass
        {
            hull = _boat.GetComponent<Rigidbody>().mass,
            pilot = _pilot.GetComponent<Rigidbody>().mass,
            gun = _gun.GetComponent<Rigidbody>().mass,
            total = (float)Math.Ceiling(_boat.GetComponent<Rigidbody>().mass
                + _pilot.GetComponent<Rigidbody>().mass
                + _gun.GetComponent<Rigidbody>().mass)
        };

        position.com = new Vector3(
            CenterOfMass3(mass.hull, position.hull.x, mass.pilot, position.pilot.x, mass.gun, position.gun.x)
            , 0
            , CenterOfMass3(mass.hull, position.hull.z, mass.pilot, position.pilot.z, mass.gun, position.gun.z));

        _com.transform.position = new Vector3(position.com.x, .5f, position.com.z);
        momentOfInertiaZ = new MomentOfInertiaZ
        {
            hull = MomentOfInertiaCenter(mass.hull, _boat.transform.localScale.x, _boat.transform.localScale.z)
            ,
            pilot = MomentOfInertiaCenter(mass.pilot, _pilot.transform.localScale.x, _pilot.transform.localScale.z)
            ,
            gun = MomentOfInertiaCenter(mass.gun, _gun.transform.localScale.x, _gun.transform.localScale.z)
            ,
            total = 0
        };

        momentOfInertiaZ.total = momentOfInertiaZ.hull + momentOfInertiaZ.pilot + momentOfInertiaZ.gun;


        _h2 = new _h2
        {
            hull = position.com.z * position.com.z + position.com.x * position.com.x
            ,
            pilot = (position.pilot.z - position.com.z) * (position.pilot.z - position.com.z) + (position.pilot.x - position.com.x) * (position.pilot.x - position.com.x)
            ,
            gun = (position.gun.z - position.com.z) * (position.gun.z - position.com.z) + (position.gun.x - position.com.x) * (position.gun.x - position.com.x)
        };

        mh2 = new Mh2
        {
            hull = _h2.hull * mass.hull
                    ,
            pilot = _h2.pilot * mass.pilot
                    ,
            gun = _h2.gun * mass.gun
                    ,
            total = 0
        };

        mh2.total = mh2.hull + mh2.pilot + mh2.gun;

        totalMomentOfInertia = new TotalMomentOfInertia
        {
            hull = momentOfInertiaZ.hull + mh2.hull
            ,
            pilot = momentOfInertiaZ.pilot + mh2.pilot
            ,
            gun = momentOfInertiaZ.gun + mh2.gun
            ,
            total = momentOfInertiaZ.total + mh2.total
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    float CenterOfMass3(float mass1, float length1, float mass2, float length2, float mass3, float length3)
    {
        float com = 0;
        com = (mass1 * length1 + mass2 * length2 + mass3 * length3) / (mass1 + mass2 + mass3);
        return com;
    }

    float MomentOfInertiaCenter(float mass, float length, float width)
    {
        float moi = 0;
        moi = mass * (length * length + width * width) / 12;
        return moi;
    }
}

[Serializable]
public class Position
{
    public Vector3 hull;
    public Vector3 pilot;
    public Vector3 gun;
    public Vector3 com;
}

[Serializable]
public class Mass
{
    public float hull;
    public float pilot;
    public float gun;
    public float total;
}

[Serializable]
public class MomentOfInertiaZ
{
    public float hull;
    public float pilot;
    public float gun;
    public float total;
}

[Serializable]
public class _h2
{
    public float hull;
    public float pilot;
    public float gun;
}

[Serializable]
public class Mh2
{
    public float hull;
    public float pilot;
    public float gun;
    public float total;
}

[Serializable]
public class TotalMomentOfInertia
{
    public float hull;
    public float pilot;
    public float gun;
    public float total;
}
