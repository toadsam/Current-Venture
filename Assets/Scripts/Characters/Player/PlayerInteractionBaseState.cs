using UnityEngine;

public class PlayerInteractionBaseState : PlayerBaseState
{
    public PlayerInteractionBaseState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Interaction State");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exited Interaction State");
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("Updating Interaction State");
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Debug.Log("Updating Physics in Interaction State");
    }
}
