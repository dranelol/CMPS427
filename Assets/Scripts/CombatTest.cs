using UnityEngine;
//using System;
using System.Collections;
//using System.Math;

public class CombatTest : MonoBehaviour 
{
    public float tmax = 0;
    public float tmin = 0;
    public float tpower = 0;
    public float tdef = 0;
    public float tmagic = 0;
    public CombatFSM AttackFSM;

	// Use this for initialization
	void Start () 
    {
        AttackFSM = GetComponent<CombatFSM>();

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AttackFSM.Attack(2f);
            DamageCalc(tmax, tmin, tpower, tdef, tmagic);
        }
	}

    //Damage Calculator
    public void DamageCalc(float max, float min, float power, float def, float magicnum)
    {
        float temprange;
        float tempbias;

        temprange = max - min;
        tempbias = power - def;

        double computation;
        computation = (tempbias * tempbias) + magicnum;

        computation = (temprange * tempbias) / (System.Math.Sqrt(computation));

        float ceiling = max + (float)computation;
        float floor = (float)computation + min;

        if (ceiling > max)
        {
            ceiling = max;
        }

        if (floor < min)
        {
            floor = min;
        }

        float random = Random.Range(floor, ceiling);

        int trandom = Mathf.RoundToInt(random);
    }
}


