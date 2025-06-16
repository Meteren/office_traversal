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
        throw new System.NotImplementedException();
    }

    public virtual void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {

    }
  
}


