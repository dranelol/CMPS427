using UnityEngine;
using System.Collections;

public class Talent
{

    private int currentPoints; //Current points spent on the talent

    public int CurrentPoints
    {
        get { return currentPoints; }

        set { currentPoints = value; }
    }

    private int maxPoints; //Max number of points the talent will have.
    public int MaxPoints
    {
        get { return MaxPoints; }
    }

    private int depth;
    public int Depth
    {
        get { return depth; }
    }

    private Ability ability; //The ability spending points on this talent will unlock.
    

    /// <summary>
    /// Talent Constructor
    /// </summary>
    /// <param name="maxPoints">Max number of points the talent will have.</param>
    /// <param name="ability">The ability spending points on this talent will unlock.</param>
    /// <param name="depth">Depth the talent is on the tree.</param>
    public Talent(int maxPoints, Ability ability, int depth)
    {
        currentPoints = 0;
        this.maxPoints = maxPoints;
        this.ability = ability;
        this.depth = depth;
    }

    public void AddAblilty()
    {
        //Add ability to spellbook

    }

    public void RemoveAblilty()
    {
        //Remove ability from spellbook

    }
}
