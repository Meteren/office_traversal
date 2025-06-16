using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BasePlayerState
{
    float zDirection;
    float xDirection;

    float speed = 3f;
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
        zDirection = Input.GetAxisRaw("Vertical");
        xDirection = Input.GetAxisRaw("Horizontal");

        pController.rb.velocity =  zDirection * speed * pController.body.transform.forward + 
            xDirection * speed * pController.body.transform.right;
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