using Spectre.Console;

public class Option
{
    public string Name {get;set;}
    public string Descriptions {get;set;}
    public string[] Args {get;set;}

    public void Start()
    {
        var name = AnsiConsole.Ask("What's your [green]name[/]?", "Anonymous");
        AnsiConsole.MarkupLine($"Hello, [blue]{name}[/]!");
    }

}