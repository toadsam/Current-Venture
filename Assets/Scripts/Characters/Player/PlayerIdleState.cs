using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
       // float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "I1");
       // Debug.Log(normalizedTime);
        base.Update();
        if (stateMachine.MovementInput != Vector2.zero)
        {
            OnMove();
            return;
        }
    }
}