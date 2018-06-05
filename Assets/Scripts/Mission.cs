using System.Collections.Generic;
using Newtonsoft.Json;

public class Mission
{
    public virtual void Start()
    {
        HasStarted = true;
    }
    
    public Dictionary<Language, string> Title;
    public Dictionary<Language, string> Text;
    public int ObjectiveAmmount;
    
    [JsonIgnore] public bool IsCompleted => CurrentAmmount >= ObjectiveAmmount;
    [JsonIgnore] public bool HasStarted;
    [JsonIgnore] public int CurrentAmmount;
}