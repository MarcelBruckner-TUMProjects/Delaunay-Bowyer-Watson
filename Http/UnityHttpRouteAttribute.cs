using System;

[AttributeUsage(AttributeTargets.Method)]
public class UnityHttpRouteAttribute : Attribute {

    public string Route;
    public string Verb;

    public UnityHttpRouteAttribute(string route, string verb = "GET") {
        Route = route.ToUpper();
        Verb = verb.ToUpper();
    }
}

