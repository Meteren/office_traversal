
using UnityEngine;

public class MoveState : BasePlayerState
{
    float zDirection;
    float xDirection;
    Transform bodyTransform;

    float speed = 10f;
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
        zDirection = Input.GetAxisRaw("Vertical");
        xDirection = Input.GetAxisRaw("Horizontal");   

        pController.rb.velocity =  (zDirection * bodyTransform.forward + 
            xDirection * bodyTransform.right).normalized * speed;
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

    public override void OnStart()
    {
        base.OnStart();
        
    }
    public override void Update()
    {
        base.Update();
        pController.rb.velocity = Vector3.zero;
    }
    public override void OnExit()
    {
        base.OnExit();
    }

}