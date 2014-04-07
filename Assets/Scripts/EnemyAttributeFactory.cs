using UnityEngine;
using System.Collections;

public class EnemyAttributeFactory {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //OMG WHY DID WE MAKE THESE FLOATS
    public float skellybaseHP = 50f;
    public float skellybasePOW = 0.0f;
    public float skellybaseDEF = 0f;
    public float skellyscalingHP = 10f;
    public float skellyscalingPOW = 10f;
    public float skellyscalingDEF = 10f;
    public int skellydamagerange = 3;
    public float skellyattackspd = 1.0f;


    Attributes MakeEnemyAttributes(int level, string type)
    {
        Attributes tempatts = new Attributes();

        //IF IS SKELLINGTON, ADD THE GODDAMN SKELLINGTON ATTRIBUTES
        if (type == "skeleton")
        {
            tempatts.Health = (skellybaseHP + (skellyscalingHP * level));
            tempatts.Power = skellybasePOW + (skellyscalingPOW * level);
            tempatts.Defense = skellybaseDEF + (skellyscalingDEF * level);
            //THIS'LL INCREASE THE DAMAGE EVERY 4 LEVELS
            tempatts.MinDamage = (float)(((level - 1) / 4) + 1);
            tempatts.MaxDamage = tempatts.MinDamage + (float)skellydamagerange;
            tempatts.AttackSpeed = skellyattackspd;

        }





        return tempatts;

    }


}
