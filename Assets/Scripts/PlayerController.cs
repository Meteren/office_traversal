using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    StateMachine playerStateMachine;

    [Header("Conditions")]
    [SerializeField] private bool inspect;

    [Header("Minimap Event Listener")]
    [SerializeField] private EventListener miniMapEventListener;

    [Header("Body")]
    public Transform body;

    [HideInInspector] public Rigidbody rb;
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        InitStateMachine();

        miniMapEventListener.AddEvent(HandleInspect);

        var moveState = new MoveState(this);
        var inspectState = new InspectState(this);

        Add(moveState, inspectState, new FuncPredicate(() => inspect));
        Add(inspectState, moveState, new FuncPredicate(() => !inspect));

        playerStateMachine.ChangeState(moveState);

    }

    public void HandleInspect() => inspect = !inspect;


    private void Add(IState from, IState to, IPredicate predicate)
    {
        playerStateMachine.AddTransition(from, to, predicate);  
    }

    // Update is called once per frame
    void Update()
    {
        playerStateMachine.Update();    
    }
    private void InitStateMachine()
    {
        playerStateMachine = new StateMachine();
    }
}
