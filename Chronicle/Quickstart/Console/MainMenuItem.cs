namespace Quickstart;

public record MainMenuItem(MainMenuCommand Command, string Text)
{
    public override string ToString() => Text;
}
