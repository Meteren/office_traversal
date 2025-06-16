using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BasePlayerState
{
    float zDirection;
    float xDirection;
    Transform bodyTransform;

    float speed = 5f;
    public MoveState(PlayerController p_controller) : base(p_controller)
    {
    }
    public override void OnStart()
    {
        base.OnStart();
        bodyTransform = pController.body.transform;
    }

    public override void Update()
    {
        base.Update();
        Debug.Log("Move State");
        zDirection = Input.GetAxisRaw("Vertical");
        xDirection = Input.GetAxisRaw("Horizontal");   

        pController.rb.velocity =  zDirection * speed * bodyTransform.forward + 
            xDirection * speed * bodyTransform.right;
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