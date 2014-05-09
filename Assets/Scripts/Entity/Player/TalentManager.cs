using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TalentManager : MonoBehaviour
{
    public const int talentPointIncrement = 1; //Number of talent points given
    public static int depthMultiplier = 1; //Mulitple of points needed to advance to the next tier of talents

    private int totalTalentPoints; //Count of all talent points earned.

    private int talentPointPool; //Count of unused talent points
    public int TalentPointPool
    {
        get { return talentPointPool; }
        set { talentPointPool = value; }
    }

    private int mightTreePoints; //Points currently spent in the Might Tree
    public int MightTreePoints
    {
        get { return mightTreePoints; }
        set { mightTreePoints = value; }
    }

    private int magicTreePoints; //Points currently spent in the Magic Tree
    public int MagicTreePoints
    {
        get { return magicTreePoints; }
        set { magicTreePoints = value; }
    }

    private HashSet<Talent> mightTree; //Set of talents in the Might Tree.
    public HashSet<Talent> MightTree
    {
        get { return mightTree; }
    }

    private HashSet<Talent> magicTree; //Set of talents in the Magic Tree.
    public HashSet<Talent> MagicTree
    {
        get { return magicTree; }
    }

    private Dictionary<string, bool> bonuses;
    public  Dictionary<string, bool> Bonuses
    {
        get { return bonuses; }
    }

    private Dictionary<string, int> bonusRanks;
    public Dictionary<string, int> BonusRanks
    {
        get { return bonusRanks; }
    }

    private PlayerController playerController;

    GameManager gameManager;

    void Awake()
    {
        playerController = transform.GetComponent<PlayerController>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        totalTalentPoints = 0;
        talentPointPool = 0;

        mightTree = new HashSet<Talent>();
        magicTree = new HashSet<Talent>();

        mightTreePoints = 0;
        magicTreePoints = 0;

        bonuses = new Dictionary<string, bool>();
        bonusRanks = new Dictionary<string, int>();

        bonuses.Add("attackDamage", false);
        bonusRanks.Add("attackDamage", 0);
        bonuses.Add("attackSpeed", false);
        bonusRanks.Add("attackSpeed", 0);
        bonuses.Add("fire", false);
        bonusRanks.Add("fire", 0);
        bonuses.Add("ice", false);
        bonusRanks.Add("ice", 0);
        bonuses.Add("shadow", false);
        bonusRanks.Add("shadow", 0);
        bonuses.Add("onHitDamage", false);
        bonusRanks.Add("onHitDamage", 0);
        bonuses.Add("defense", false);
        bonusRanks.Add("defense", 0);

    }
    
    
    // Use this for initialization
	void Start ()
    {

        #region Might Talents

        //Tier 1
        mightTree.Add(new Talent("Cleave", 1, GameManager.Abilities["cleave"], 0));
        mightTree.Add(new Talent("Axe Throw", 1, GameManager.Abilities["axethrow"], 0));
        
        //Tier 2
        mightTree.Add(new Talent("On Hit Damage +", 1, "onHitDamage", .1f, 1));
        mightTree.Add(new Talent("Fus Ro Dah", 1, GameManager.Abilities["fusrodah"], 1));
        mightTree.Add(new Talent("Blink Strike", 1, GameManager.Abilities["blinkstrike"], 1));

        //Tier 3
        mightTree.Add(new Talent("Attack Damage +", 3, "attackDamage", .1f, 2));
        mightTree.Add(new Talent("Attack Speed +", 2, "attackSpeed", .1f, 2));
        mightTree.Add(new Talent("Defense", 5, "defense", .1f, 2));

        //Tier 4 
        mightTree.Add(new Talent("Whirlwind", 1, GameManager.Abilities["whirlwind"], 3));
        mightTree.Add(new Talent("Hadouken", 1, GameManager.Abilities["hadouken"], 3));

        //Tier 5
        mightTree.Add(new Talent("Boomerang Blade", 1, GameManager.Abilities["boomerangblade"], 4));
        mightTree.Add(new Talent("Death Grip", 1, GameManager.Abilities["GETOVERHERE"], 4));

        //Tier 6
        mightTree.Add(new Talent("Shield Breaker", 1, GameManager.Abilities["shieldbreaker"], 5));
        mightTree.Add(new Talent("Shadow Pull", 1, GameManager.Abilities["deathgrip"], 5));
        mightTree.Add(new Talent("Blade Waltz", 1, GameManager.Abilities["bladewaltz"], 5));

        //Tier 7
        mightTree.Add(new Talent("Dervish", 1, GameManager.Abilities["dervish"], 6));

        #endregion

        #region Magic Talents

        //Tier 1
        magicTree.Add(new Talent("Fireball", 1, GameManager.Abilities["fireball"], 0));
        magicTree.Add(new Talent("Ice Bolt", 1, GameManager.Abilities["icebolt"], 0));
        magicTree.Add(new Talent("Shadow Bolt", 1, GameManager.Abilities["shadowbolt"], 0));

        //Tier 2
        magicTree.Add(new Talent("Flame Strike", 1, GameManager.Abilities["flamestrike"], 1));
        magicTree.Add(new Talent("Frozen orb", 1, GameManager.Abilities["frozenorb"], 1));
        magicTree.Add(new Talent("Shadow Trap", 1, GameManager.Abilities["shadowtrap"], 1));

        //Tier 3
        magicTree.Add(new Talent("Fire Damage +", 1, "fire", .1f, 2));
        magicTree.Add(new Talent("Ice Damage +", 1, "ice", .1f, 2));
        magicTree.Add(new Talent("Shadow Damage +", 1, "shadow", .1f, 2));

        //Tier 4
        magicTree.Add(new Talent("Fire Mine", 1, GameManager.Abilities["firemine"], 3));
        magicTree.Add(new Talent("Blink", 1, GameManager.Abilities["blink"], 3));
        magicTree.Add(new Talent("Improved Shadow Bolt", 1, GameManager.Abilities["improvedshadowbolt"], 3));

        //Tier 5
        magicTree.Add(new Talent("Fire Turret", 1, GameManager.Abilities["fireballturret"], 4));
        magicTree.Add(new Talent("Let It Go", 1, GameManager.Abilities["aoefreeze"], 4));
        magicTree.Add(new Talent("Shadowfury", 1, GameManager.Abilities["shadowfury"], 4));

        //Tier 6
        magicTree.Add(new Talent("Fire Barrage", 1, GameManager.Abilities["fireballbarrage"], 5));
        magicTree.Add(new Talent("Frost Nova", 1, GameManager.Abilities["frostnova"], 5));
        magicTree.Add(new Talent("Death and Decay", 1, GameManager.Abilities["deathanddecay"], 5));

        //Tier 7
        magicTree.Add(new Talent("Chaos Barrage", 1, GameManager.Abilities["chaosbarrage"], 6));

        #endregion



    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}


    /// <summary>
    /// Handles spending a talent point. 
    /// Adds the ability tied to the passed talent, increments the number of points spent on that talent and
    /// the number of points spent in that talent's tree, and decrements the number of unused talent points.
    /// </summary>
    /// <param name="talent">The talent to spend the point on.</param>
    public void SpendPoint(Talent talent)
    {
        
        if (talentPointPool > 0 && talent.CurrentPoints < talent.MaxPoints && IsTalentActive(talent) == true)
        {
            
            talentPointPool--;
            talent.CurrentPoints++;

            if (mightTree.Contains(talent) == true)
            {
                mightTreePoints++;
            }
            else if (magicTree.Contains(talent) == true)
            {
                magicTreePoints++;
            }


            if (talent.TalentAbility != null)
            {
                AddAbility(talent);
            }
            else if(talent.Bonus != "")
            {
                AddPassive(talent);
            }
        }
    }


    /// <summary>
    /// Handles Removing a Talent Point
    /// Removes a point from the passed talent, adds the point back to the unused talent pool, and removes the ability attached to the talent if removing a point will zero out the talent.
    /// If removing a point from this talent does means that talents at lower depths will not have enough points above them to be valid, a point will not be removed from this talent. 
    /// </summary>
    /// <param name="talent">The talent to remove a point from.</param>
    public void RemovePoint(Talent talent)
    {
        
        if (talent.CurrentPoints > 0)
        {

            if (mightTree.Contains(talent) == true)
            {

                
                foreach (Talent t in mightTree)
                {
                    if (t.Depth > talent.Depth && t.CurrentPoints > 0)
                    {
                        if ((mightTreePoints - 1) <= t.Depth * depthMultiplier)
                        {
                            return;
                        }
                    }
                }

                mightTreePoints--;
            }
            else if (magicTree.Contains(talent) == true)
            {

                
                foreach (Talent t in magicTree)
                {
                    if (t.Depth > talent.Depth && t.CurrentPoints > 0)
                    {
                        if ((magicTreePoints - 1) <= t.Depth * depthMultiplier)
                        {
                            return;
                        }
                    }
                }
                
                magicTreePoints--;
            }

            talentPointPool++;
            talent.CurrentPoints--;

            if (talent.CurrentPoints == 0)
            {
                if (talent.TalentAbility != null)
                {
                    RemoveAbility(talent);
                }
                else if (talent.Bonus != "")
                {
                    RemovePassive(talent);
                }
            }
        }
    }


    /// <summary>
    /// Removes all points from a talent tree and refunds them to the unused talent pool.
    /// </summary>
    /// <param name="tree">The tree to reset. Either "might" or "magic".</param>
    public void Respec(string tree)
    {
        

        if (tree == "might")
        {
            foreach (Talent t in mightTree)
            {
                t.CurrentPoints = 0;

                if (t.TalentAbility != null)
                {
                    RemoveAbility(t);
                }
                else if(t.Bonus != "")
                {
                    RemovePassive(t);
                }
                
            }

            talentPointPool += mightTreePoints;
            mightTreePoints = 0;
        }
        else if (tree == "magic")
        {
            foreach (Talent t in magicTree)
            {
                t.CurrentPoints = 0;
                if (t.TalentAbility != null)
                {
                    RemoveAbility(t);
                }
                else if (t.Bonus != "")
                {
                    RemovePassive(t);
                }
            }

            talentPointPool += magicTreePoints;
            magicTreePoints = 0;
        }
        
    }

    
    /// <summary>
    /// Adds a talent's ability to the player's spellbook if it is not already present.
    /// </summary>
    /// <param name="talent">The talent to add.</param>
    private void AddAbility(Talent talent)
    {
        if (playerController.SpellBook.Contains(talent.TalentAbility) == false)
        {
            playerController.SpellBook.Add(talent.TalentAbility);
        } 
    }

    /// <summary>
    /// Removes a talent's ability from the player's spellbook if it is present.
    /// </summary>
    /// <param name="talent"></param>
    private void RemoveAbility(Talent talent)
    {
        if (playerController.SpellBook.Contains(talent.TalentAbility) == true)
        {
            playerController.SpellBook.Remove(talent.TalentAbility);
        }
        else if (playerController.entity.abilityManager.abilities.Contains(talent.TalentAbility))
        {
            playerController.entity.abilityManager.RemoveAbility(playerController.entity.abilityIndexDict[talent.TalentAbility.ID]);
        }

    }

    /// <summary>
    /// Checks to see if a talent is active.
    /// A talent is active if there are enough points in the talent's tree to make its use valid based on the talent's depth.
    /// </summary>
    /// <param name="talent">The talent to determine active/inactive.</param>
    /// <returns></returns>
    public bool IsTalentActive(Talent talent)
    {
        
        if (mightTree.Contains(talent) == true)
        {
            
            if (mightTreePoints >= talent.Depth * depthMultiplier)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (magicTree.Contains(talent) == true)
        {
            if (magicTreePoints >= talent.Depth * depthMultiplier)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Adds the specified number of talent points.
    /// </summary>
    /// <param name="points">The points to add.</param>
    public void GiveTalentPoints(int points)
    {
        totalTalentPoints += points;
        talentPointPool += points;
    }


    public void AddPassive(Talent talent)
    {
        bonuses[talent.Bonus] = true;
        bonusRanks[talent.Bonus] = talent.CurrentPoints;
    }

    public void RemovePassive(Talent talent)
    {
        bonuses[talent.Bonus] = false;
        bonusRanks[talent.Bonus] = 0;
    }
}
