using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerState : IState
{
   
    protected PlayerController pController;

    public BasePlayerState(PlayerController pController)
    {
        this.pController = pController;
    }

    public virtual void OnStart()
    {
        return;
    }

    public virtual void OnExit()
    {
        return;
    }

    public virtual void Update()
    {
        return;
    }
  
}


