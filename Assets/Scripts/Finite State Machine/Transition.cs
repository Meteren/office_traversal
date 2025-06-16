public class Transition
{
    IPredicate predicate;
    IState to;

    public IState To { get { return to; }}
    public Transition(IState to, IPredicate predicate)
    {
        this.predicate = predicate;
        this.to = to;
    }

    public bool Execute() => predicate.Execute();

}





