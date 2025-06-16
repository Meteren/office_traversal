using System;

public class FuncPredicate : IPredicate
{

    Func<bool> func;
    public FuncPredicate(Func<bool> func)
    {
        this.func = func;
    }

    public bool Execute() => func.Invoke();
}





