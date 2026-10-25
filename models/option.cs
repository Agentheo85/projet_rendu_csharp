using Spectre.Console;
// Cette classe va etre utile pour le menu 
public class Option
{
    public string Name {get;set;}
    public string Descriptions {get;set;}
    public string[] Args {get;set;}
    public Action<string[]>? CustomFunction { get; set; } 

    public void Start()
    {
        List<string> args = [];
        if(Args.Length != 0)
        {
            foreach(string arg in Args)
            {
                // https://spectreconsole.net/console/prompts/text-prompt/
                 var inputArg = AnsiConsole.Ask($"{arg}", "N/A");
                 args.Add(inputArg);
            }

              
        }
        if(CustomFunction is not null) 
        CustomFunction(args.ToArray());
    
     
    }

}