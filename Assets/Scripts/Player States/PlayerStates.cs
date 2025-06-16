using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BasePlayerState
{
    float xDirection;
    float speed = 10f;
    public MoveState(PlayerController p_controller) : base(p_controller)
    {
    }
    public override void OnStart()
    {
        base.OnStart();
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("Move State");
        xDirection = Input.GetAxisRaw("Vertical");

        pController.rb.velocity =  xDirection * speed * pController.body.transform.forward;
    }
    public override void OnExit()
    {
        base.OnExit();
    }
   
}

public class InspectState : BasePlayerState
{
    public InspectState(PlayerController p_controller) : base(p_controller)
    {
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void Update()
    {
        base.Update();
    }
}