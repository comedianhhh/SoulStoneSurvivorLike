/// <summary>
/// Defines a command within the Command Pattern framework.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Executes the command. This is where the command's main logic should be implemented.
    /// </summary>
    void Execute();

    /// <summary>
    /// Reverses the execution of the command, effectively undoing its effects.
    /// </summary>
    void Undo();
}
