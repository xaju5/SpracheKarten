using UnityEngine;

public class Verb
{
    public string infiniv {get; private set;}
    public string perfect {get; private set;}
    public string translation {get; private set;}

    public Verb(string[] splittedLine)
    {
        this.infiniv = splittedLine[0];
        this.perfect = splittedLine[1];
        this.translation = splittedLine[2];
    }
}
