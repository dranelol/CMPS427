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
    }
    
    
    // Use this for initialization
	void Start () 
    {
        mightTree.Add(new Talent("Fus Ro Dah", 1, GameManager.Abilities["fusrodah"], 0));
        mightTree.Add(new Talent("Hadouken", 1, GameManager.Abilities["hadouken"], 1));
        mightTree.Add(new Talent("Fireball Turret", 1, GameManager.Abilities["fireballturret"], 1));
        mightTree.Add(new Talent("Blink Strike", 1, GameManager.Abilities["blinkstrike"], 2));

        magicTree.Add(new Talent("Blink", 1, GameManager.Abilities["blink"], 0));
        magicTree.Add(new Talent("Frozen orb", 1, GameManager.Abilities["frozenorb"], 1));
        magicTree.Add(new Talent("Death Grip", 1, GameManager.Abilities["deathgrip"], 1));
        magicTree.Add(new Talent("Chaos Barrage", 1, GameManager.Abilities["chaosbarrage"], 2));
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

            AddAbility(talent);
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
                RemoveAbility(talent);
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
                RemoveAbility(t);
            }

            talentPointPool += mightTreePoints;
            mightTreePoints = 0;
        }
        else if (tree == "magic")
        {
            foreach (Talent t in magicTree)
            {
                t.CurrentPoints = 0;
                RemoveAbility(t);
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
}
