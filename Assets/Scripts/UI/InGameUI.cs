using UnityEngine;
using System.Collections;

public class InGameUI : UIState {

    public InGameUI(int id, UIController controller)
        : base(id, controller) { }

    public override void Enter()
    {
        Debug.Log("Entering InGame state.");
    }

    public override void Exit()
    {
        Debug.Log("Exiting InGame state.");
    }

    public override void Update()
    {
        
    }

    public override void OnGui()
    {
        
    }
}
