using UnityEngine;
using System.Collections;

public class HUD_GUI : MonoBehaviour {

	// Use this for initialization
	public Texture HealthOverlay;
	public Texture HealthLiquid;
	public Texture BrightLiquid;
	public Texture currentLiquid;
	public Texture HealthGlare;
	public Texture Ability1;
	public Texture Ability2;
	public Texture Ability3;
	public Texture Ability4;
	public Texture Ability5;
	public Texture AbilityCoolDown;

	public float Ability1CoolDownTime = .5f;
	public float Ability2CoolDownTime = .5f;
	public float Ability3CoolDownTime = .5f;
	public float Ability4CoolDownTime = .5f;
	public float Ability5CoolDownTime = .5f;

	public GUIStyle AbiltyStyle;

	public GUIStyle infoBoxStyle;

	public Rect AbilitiesOuterBox;
	public Rect AbilityOneBox;
	public Rect AbilityTwoBox;
	public Rect AbilityThreeBox;
	public Rect AbilityFourBox;
	public Rect AbilityFiveBox;

	public float slider;
	public GUIStyle Slider; 
	public GUIStyle thumb; 

	public Vector2 HealthGroupPos;
	public Vector2 HealthGroupSize;
	public Vector2 HealthOverlayPos;
	public Vector2 currentLiquidPos;
	public Vector2 HealthOverlaySize;
	public Vector2 currentLiquidSize;


	public float native_width;
	public float native_height;
    public Rect InfoBox1;
    public Rect InfoBox2;
    public Rect InfoBox3;

    public PlayerEntity player;
	bool damageFlag = false;
    

    private Rect CDBox1;
    private Rect CDBox2;
    private Rect CDBox3;
    private Rect CDBox4;
    private Rect CDBox5;

    private Rect TempManaBox;

	private int lastDamageFrame;

	public float health = 0.0f;
	float healthLastFrame = 0.0f;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	void Start () 
    {
		native_width = Screen.width;
		native_height = Screen.height;

		AbilitiesOuterBox = new Rect(Screen.width * .251f,Screen.height * .90269f, Screen.width *.208385f, Screen.height *.09944f);
		AbilityOneBox = new Rect(Screen.width * .01f,Screen.height * .0f, Screen.width *.03695f, Screen.height *.09f);
		AbilityTwoBox = new Rect(Screen.width * .045854f,Screen.height * .00455f, Screen.width *.038f, Screen.height *.08799f);
		AbilityThreeBox = new Rect(Screen.width * .0875776f,Screen.height * .0f, Screen.width *.036f, Screen.height *.09f);
		AbilityFourBox = new Rect(Screen.width * .1264f,Screen.height * .0f, Screen.width *.03776f, Screen.height *.09f);
		AbilityFiveBox = new Rect(Screen.width * .1646f,Screen.height * .0f, Screen.width *.03776f, Screen.height *.09f);

		HealthOverlayPos.x = 0f;
		HealthOverlayPos.y = 0f;
		HealthOverlaySize.x = Screen.width * .491f;// 300f;//.491f
		HealthOverlaySize.y = Screen.height * .21834f;//100f;//.21834f

		currentLiquidPos.x = Screen.width * .019399f;//12f;//.019399f
		currentLiquidPos.y = Screen.height * .21834f;//100f;//.21834f
		currentLiquidSize.x = Screen.width * .17f;//104f;//.17f
		currentLiquidSize.y = Screen.height * .1986999f;//91f;//.1986999f

		HealthGroupPos.x = 0f;//
		HealthGroupPos.y = Screen.height * .78f;//358f;//.78f
		HealthGroupSize.x = Screen.width * .491f;//300f;//.491f
		HealthGroupSize.y = Screen.height * .21834f;//100f;//.21834f

        InfoBox1 = new Rect(Screen.width * .5f, Screen.height * .87f, Screen.width * .45f, Screen.height * .1f);
        InfoBox2 = new Rect(Screen.width * .91f, Screen.height * .90f, Screen.width * .45f, Screen.height * .1f);

        InfoBox3 = new Rect(Screen.width * .02f, Screen.height * .10f, Screen.width * .1f, Screen.height * .5f);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEntity>();
		currentLiquid = HealthLiquid;


        #region Cooldown GUI init

        CDBox1 = new Rect(Screen.width * .65f, Screen.height * .875f, Screen.width * .45f, Screen.height * .1f);
        CDBox2 = new Rect(Screen.width * .65f, Screen.height * .900f, Screen.width * .45f, Screen.height * .1f);
        CDBox3 = new Rect(Screen.width * .65f, Screen.height * .925f, Screen.width * .45f, Screen.height * .1f);
        CDBox4 = new Rect(Screen.width * .65f, Screen.height * .950f, Screen.width * .45f, Screen.height * .1f);
        CDBox5 = new Rect(Screen.width * .65f, Screen.height * .975f, Screen.width * .45f, Screen.height * .1f);
        

        #endregion

        TempManaBox = new Rect(Screen.width * .9f, Screen.height * .80f, Screen.width * .45f, Screen.height * .1f);
	}

	void OnGUI(){

		float rx = Screen.width / native_width;
		float ry = Screen.height / native_height;
		GUI.matrix = Matrix4x4.TRS(new Vector3(0f,0f,0f), Quaternion.identity,new Vector3(rx,ry,1));

		//health = GUI.VerticalSlider(new Rect(10f,10f,20f,50f),health,1f,0f);
        health = (player.CurrentHP / player.currentAtt.Health);
        //health = (player.currentHP / player.maxHP);


		if(health < healthLastFrame){
			lastDamageFrame = Time.frameCount - 1;
		}
		if(Time.frameCount - lastDamageFrame < 60){
			if(Time.frameCount % 20 < 1){
				currentLiquid = BrightLiquid;
			}
			else currentLiquid = HealthLiquid;
		}
		healthLastFrame = health;

		Mathf.Clamp(health, 0.0f, 1.0f);

		GUI.BeginGroup(AbilitiesOuterBox);{//161.9 436, 134.2,48.3
			GUI.DrawTexture(AbilityOneBox,Ability1);//6.44,.03,23.8,43.7

			GUI.DrawTextureWithTexCoords(new Rect(AbilityOneBox.x,//0,
			                                      AbilityOneBox.y + (AbilityOneBox.height * (1f-Ability1CoolDownTime)),//100-(100*health),// this sinks images clipping the bottom
			                                      AbilityOneBox.width,//100f,
			                                      AbilityOneBox.height),//100f),
			                             AbilityCoolDown, // texture
			                             new Rect(0f,
			         Ability1CoolDownTime, // this adjust image to keep stationary
			         1f,
			         1f
			         ));

			GUI.DrawTexture(AbilityTwoBox,Ability2);//29.53,2.2,24.5,42.5

			GUI.DrawTextureWithTexCoords(new Rect(AbilityTwoBox.x,//0,
			                                      AbilityTwoBox.y + (AbilityTwoBox.height * (1f-Ability2CoolDownTime)),//100-(100*health),// this sinks images clipping the bottom
			                                      AbilityTwoBox.width,//100f,
			                                      AbilityTwoBox.height),//100f),
			                             AbilityCoolDown, // texture
			                             new Rect(0f,
			         Ability2CoolDownTime, // this adjust image to keep stationary
			         1f,
			         1f
			         ));

			GUI.DrawTexture(AbilityThreeBox,Ability3);//56.4,0,23.3,43.7

			GUI.DrawTextureWithTexCoords(new Rect(AbilityThreeBox.x,//0,
			                                      AbilityThreeBox.y + (AbilityThreeBox.height * (1f-Ability3CoolDownTime)),//100-(100*health),// this sinks images clipping the bottom
			                                      AbilityThreeBox.width,//100f,
			                                      AbilityThreeBox.height),//100f),
			                             AbilityCoolDown, // texture
			                             new Rect(0f,
			         Ability3CoolDownTime, // this adjust image to keep stationary
			         1f,
			         1f
			         ));


			GUI.DrawTexture(AbilityFourBox,Ability4);//81.4,0,24.32,43.7

			GUI.DrawTextureWithTexCoords(new Rect(AbilityFourBox.x,//0,
			                                      AbilityFourBox.y + (AbilityFourBox.height * (1f-Ability4CoolDownTime)),//100-(100*health),// this sinks images clipping the bottom
			                                      AbilityFourBox.width,//100f,
			                                      AbilityFourBox.height),//100f),
			                             AbilityCoolDown, // texture
			                             new Rect(0f,
			         Ability4CoolDownTime, // this adjust image to keep stationary
			         1f,
			         1f
			         ));


			GUI.DrawTexture(AbilityFiveBox,Ability5);//106,0,26.4,44
			
			GUI.DrawTextureWithTexCoords(new Rect(AbilityFiveBox.x,//0,
			                                      AbilityFiveBox.y + (AbilityFiveBox.height * (1f-Ability5CoolDownTime)),//100-(100*health),// this sinks images clipping the bottom
			                                      AbilityFiveBox.width,//100f,
			                                      AbilityFiveBox.height),//100f),
			                             AbilityCoolDown, // texture
			                             new Rect(0f,
			         Ability5CoolDownTime, // this adjust image to keep stationary
			         1f,
			         1f
			         ));

			
			
		}GUI.EndGroup();



		GUI.BeginGroup(new Rect(HealthGroupPos.x,//100,
		                        HealthGroupPos.y,//100,
		                        HealthGroupSize.x,//100,
		                        HealthGroupSize.y//100
		                        ));{
			GUI.DrawTextureWithTexCoords(new Rect(currentLiquidPos.x,//0,
			                                      currentLiquidPos.y - (currentLiquidSize.y * health),//100-(100*health),// this sinks images clipping the bottom
			                                      currentLiquidSize.x,//100f,
			                                      currentLiquidSize.y),//100f),
			                             		  currentLiquid, // texture
			                            new Rect(0f,
			                                     health, // this adjust image to keep stationary
			                                     1f,
			                                     1f
			         ));

		

			GUI.DrawTexture(new Rect(HealthOverlayPos.x,//0f,
			                         HealthOverlayPos.y,//0f
			                         HealthOverlaySize.x,//200f,
			                         HealthOverlaySize.y),//100f),
			                 		 HealthOverlay);// texture





			GUI.DrawTexture(new Rect(HealthOverlayPos.x,//0f,
			                         HealthOverlayPos.y,//0f
			                         HealthOverlaySize.x,//200f,
			                         HealthOverlaySize.y),//100f),
			                HealthGlare);// texture

		
		
		}GUI.EndGroup();
		GUI.backgroundColor = Color.green;
        //GUI.color = Color.white;
        infoBoxStyle.normal.textColor = Color.white;

        string qname, wname, ename, rname, rClickName;

        if (player.abilityManager.abilities[1] != null)
        {
            rClickName = player.abilityManager.abilities[1].Name;
        }
        else
        {
            rClickName = "none";
        }


        if (player.abilityManager.abilities[2] != null)
        {
            qname = player.abilityManager.abilities[2].Name;
        }
        else
        {
            qname = "none";
        }

        if (player.abilityManager.abilities[3] != null)
        {
            wname = player.abilityManager.abilities[3].Name;
        }
        else
        {
            wname = "none";
        }
        if (player.abilityManager.abilities[4] != null)
        {
            ename = player.abilityManager.abilities[4].Name;
        }
        else
        {
            ename = "none";
        }
        if (player.abilityManager.abilities[5] != null)
        {
            rname = player.abilityManager.abilities[5].Name;
        }
        else
        {
            rname = "none";
        }
        

        string attackList = "RClick = " + rClickName + " \n"
                          + "Q = " + qname + " \n"
                          + "W = " + wname + " \n"
                          + "E = " + ename + " \n"
                          + "R = " + rname + " \n";

        /*
        string attackList = "Q = " + player.abilityManager.abilities[2].Name + " \n"
                          + "W = " + player.abilityManager.abilities[3].Name + " \n"
                          + "E = " + player.abilityManager.abilities[4].Name + " \n"
                          + "R = " + player.abilityManager.abilities[5].Name + " \n";
        */
        string version = "Alpha v0.3.3";

        GUI.Label(InfoBox1, attackList, infoBoxStyle);
        GUI.Label(InfoBox2, version, infoBoxStyle);
        GUI.Label(TempManaBox, player.CurrentResource.ToString() + "/" + player.currentAtt.Resource.ToString() + " Resource", infoBoxStyle);

        string controls =  "Left-click: Movement\n" +
                            "Right-click: Ability 1\n" +
                            "Q: Ability 2\n" +
                            "W: Ability 3\n" +
                            "E: Ability 4\n" +
                            "R: Ability 5\n" +
                            "\n" +
                            "N: Talents\n" +
                            "J: Spellbook\n" +
                            "I: Inventory\n" +
                            "H: Attributes\n" +
                            "\n" +
                            "1: Gain level\n" +
                            "2: Gain 50 talent points\n" +
                            "3: Gain 25 attribute points\n" +
                            "4: Gain Might abilities\n" +
                            "5: Gain Magic abilities\n" +
                            "6: Gain full set of equipment";

        GUI.Label(InfoBox3, controls, infoBoxStyle);


        #region ability cooldown GUI

   
        float timeLeft = 0;

        #region ability 1
        if (player.abilityManager.abilities[1] != null)
        {
            if (player.abilityManager.activeCoolDowns[1] > Time.time)
            {
                timeLeft = player.abilityManager.activeCoolDowns[1] - Time.time;
            }
            else
            {
                timeLeft = 0;
            }

            GUI.Label(CDBox1, player.abilityManager.abilities[1].Name + " CD Remaining: " + timeLeft.ToString("F") + "s", infoBoxStyle);
        }
        #endregion

        #region ability 2
        if (player.abilityManager.abilities[2] != null)
        {
            if (player.abilityManager.activeCoolDowns[2] > Time.time)
            {


                timeLeft = player.abilityManager.activeCoolDowns[2] - Time.time;
            }
            else
            {
                timeLeft = 0;
            }

            GUI.Label(CDBox2, player.abilityManager.abilities[2].Name + " CD Remaining: " + timeLeft.ToString("F") + "s", infoBoxStyle);
        }
        #endregion
        
        #region ability 3
        if (player.abilityManager.abilities[3] != null)
        {
            if (player.abilityManager.activeCoolDowns[3] > Time.time)
            {

                timeLeft = player.abilityManager.activeCoolDowns[3] - Time.time;
            }
            else
            {
                timeLeft = 0;
            }

            GUI.Label(CDBox3, player.abilityManager.abilities[3].Name + " CD Remaining: " + timeLeft.ToString("F") + "s", infoBoxStyle);
        }
        #endregion

        #region ability 4
        if (player.abilityManager.abilities[4] != null)
        {
            if (player.abilityManager.activeCoolDowns[4] > Time.time)
            {
                timeLeft = player.abilityManager.activeCoolDowns[4] - Time.time;
            }
            else
            {
                timeLeft = 0;
            }

            GUI.Label(CDBox4, player.abilityManager.abilities[4].Name + " CD Remaining: " + timeLeft.ToString("F") + "s", infoBoxStyle);
        }
        #endregion

        #region ability 5
        if (player.abilityManager.abilities[5] != null)
        {
            if (player.abilityManager.activeCoolDowns[5] > Time.time)
            {
                timeLeft = player.abilityManager.activeCoolDowns[5] - Time.time;
            }
            else
            {
                timeLeft = 0;
            }


            GUI.Label(CDBox5, player.abilityManager.abilities[5].Name + " CD Remaining: " + timeLeft.ToString("F") + "s", infoBoxStyle);
        }
        #endregion

        #endregion

        #region Level and Exp and Health

        GUI.Label(new Rect(Screen.width * .025f, Screen.height * .75f, Screen.width * .45f, Screen.height * .1f), "Health: " + player.CurrentHP.ToString()+"/"+player.currentAtt.Health, infoBoxStyle);

        GUI.Label(new Rect(Screen.width * .025f, Screen.height * .025f, Screen.width * .45f, Screen.height * .1f),"Level: "+player.Level.ToString(), infoBoxStyle );

        if (player.LevelCap == false)
        {
            GUI.Label(new Rect(Screen.width * .025f, Screen.height * .05f, Screen.width * .45f, Screen.height * .1f), "Experience: " + player.Experience.ToString() + "/" + player.NextLevelExperience, infoBoxStyle);
        }
        #endregion

    }
}
