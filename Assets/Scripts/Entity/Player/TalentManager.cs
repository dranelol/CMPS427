using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TalentManager : MonoBehaviour
{
    public const int talentPointIncrement = 1; //Number of talent points given
    public const int depthMultiplier = 1; //Mulitple of points needed to advance to the next tier of talents

    private int totalTalentPoints; //Count of all talent points earned.

    private int talentPointPool; //Count of unused talent points
    public int TalentPointPool
    {
        get { return talentPointPool; }
    }

    private int mightTreePoints;
    public int MightTreePoints
    {
        get { return mightTreePoints; }
    }

    private int magicTreePoints;
    public int MagicTreePoints
    {
        get { return magicTreePoints; }
    }

    private HashSet<Talent> mightTree;
    public HashSet<Talent> MightTree
    {
        get { return mightTree; }
    }

    private HashSet<Talent> magicTree;
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

        totalTalentPoints = 5;
        talentPointPool = 5;

        mightTree = new HashSet<Talent>();
        magicTree = new HashSet<Talent>();

        
        

        mightTreePoints = 0;
        magicTreePoints = 0;
    }
    
    
    // Use this for initialization
	void Start () 
    {
        mightTree.Add(new Talent("Blink", 1, GameManager.Abilities["blink"], 0));
        mightTree.Add(new Talent("Blink Strike", 1, GameManager.Abilities["blinkstrike"], 1));
        mightTree.Add(new Talent("Death Grip", 1, GameManager.Abilities["deathgrip"], 1));
        mightTree.Add(new Talent("Fus Ro Dah", 1, GameManager.Abilities["fusrodah"], 2));

        magicTree.Add(new Talent("Shadowbolt", 1, GameManager.Abilities["shadowbolt"], 0));
        magicTree.Add(new Talent("Blink Strike", 1, GameManager.Abilities["blinkstrike"], 1));
        magicTree.Add(new Talent("Death Grip", 1, GameManager.Abilities["deathgrip"], 1));
        magicTree.Add(new Talent("Fus Ro Dah", 1, GameManager.Abilities["fusrodah"], 2));
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

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

    public void RemovePoint(Talent talent)
    {
        
        if (talent.CurrentPoints > 0)
        {

            if (mightTree.Contains(talent) == true)
            {

                //Checks to see that there are no talents with points in them at a lower depth in the tree than the current talent.
                foreach (Talent t in mightTree)
                {
                    if (t.Depth > talent.Depth && t.CurrentPoints > 0)
                    {
                        return;
                    }
                }

                mightTreePoints--;
            }
            else if (magicTree.Contains(talent) == true)
            {

                //Checks to see that there are no talents with points in them at a lower depth in the tree than the current talent.
                foreach (Talent t in magicTree)
                {
                    if (t.Depth > talent.Depth && t.CurrentPoints > 0)
                    {
                        return;
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

    public void Respec()
    {
        talentPointPool = totalTalentPoints;

        foreach (Talent t in mightTree)
        {
            t.CurrentPoints = 0;
            RemoveAbility(t);
        }

        foreach (Talent t in magicTree)
        {
            t.CurrentPoints = 0;
            RemoveAbility(t);
        }
    }

    private void AddAbility(Talent talent)
    {
        if (playerController.SpellBook.Contains(talent.TalentAbility) == false)
        {
            playerController.SpellBook.Add(talent.TalentAbility);
        }


       
        
    }

    private void RemoveAbility(Talent talent)
    {
        if (playerController.SpellBook.Contains(talent.TalentAbility) == true)
        {
            playerController.SpellBook.Remove(talent.TalentAbility);
        }
    }

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

    public void GiveTalentPoints()
    {
        totalTalentPoints += talentPointIncrement;
        talentPointPool += talentPointIncrement;
    }
}
