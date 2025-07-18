using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    StateMachine playerStateMachine;

    [Header("Conditions")]
    [SerializeField] private bool inspect;

    [Header("Minimap Event Listener")]
    [SerializeField] private EventListener miniMapEventListener;

    [Header("Game State Listener")]
    [SerializeField] private EventListener gameStateListener;

    [Header("Body")]
    public Transform body;

    [HideInInspector] public Rigidbody rb;

    [Header("Stair Detection Adjustments")]
    [SerializeField] private Transform bottom;
    [SerializeField] private Transform top;
    [SerializeField] private float bottomDistance;
    [SerializeField] private float topDistance;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float climbPower;

    List<Vector3> directions;
    void Start()
    {
        
        directions = new List<Vector3>() {transform.TransformDirection(0,0,1),
            transform.TransformDirection(-0.7f,0,0.7f),transform.TransformDirection(0.7f,0,0.7f),transform.TransformDirection(-1,0,0),
            transform.TransformDirection(-0.7f,0,-0.7f),transform.TransformDirection(0,0,-1),transform.TransformDirection(0.7f,0,-0.7f),
            transform.TransformDirection(1,0,0)};

        rb = GetComponent<Rigidbody>();

        InitStateMachine();

        miniMapEventListener.AddEvent(HandleInspect);
        gameStateListener.AddEvent(HandleInspect);

        var moveState = new MoveState(this);
        var inspectState = new InspectState(this);

        Add(moveState, inspectState, new FuncPredicate(() => inspect));
        Add(inspectState, moveState, new FuncPredicate(() => !inspect));

        playerStateMachine.ChangeState(moveState);

    }
    void Update()
    {
        playerStateMachine.Update();
        
    }

    private void FixedUpdate()
    {
       
        if (rb.velocity != Vector3.zero)
            ClimbIfPossible();
    }

    public void HandleInspect() => inspect = !inspect;

    private void Add(IState from, IState to, IPredicate predicate)
    {
        playerStateMachine.AddTransition(from, to, predicate);
    }
 
    private void InitStateMachine()
    {
        playerStateMachine = new StateMachine();
    }

    private void ClimbIfPossible()
    {
        foreach(var direction in directions)
        {

            VisualizeDirection(bottom.transform.position, direction,bottomDistance);
            VisualizeDirection(top.transform.position, direction,topDistance);

            Ray rayBottom = new Ray(bottom.transform.position,direction);
            Ray rayTop = new Ray(top.transform.position, direction);

            if (Physics.Raycast(rayBottom,bottomDistance,layer,QueryTriggerInteraction.Ignore))
            {
                Debug.Log($"Direction {direction} hit.");
                if(!Physics.Raycast(rayTop, topDistance, layer, QueryTriggerInteraction.Ignore))
                {
                    ClimbObstacle();
                    Debug.Log("Climbing");
                    break;
                }
            }
        }
       
    }
    private void VisualizeDirection(Vector3 origin, Vector3 direction,float distance) => Debug.DrawRay(origin, direction * distance);

    private void ClimbObstacle()
    {
        rb.MovePosition(rb.position + new Vector3(0, climbPower * Time.fixedDeltaTime, 0));
        
    }
       
}
