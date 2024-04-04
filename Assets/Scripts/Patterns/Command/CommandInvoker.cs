using System.Collections.Generic;

/// <summary>
/// Manages the execution and undoing of commands.
/// </summary>
public class CommandInvoker
{
    private Stack<ICommand> commandHistory = new Stack<ICommand>();

    /// <summary>
    /// Executes a command and stores it in the history for potential undoing.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandHistory.Push(command);
    }

    /// <summary>
    /// Undoes the last command executed, if there is any command to undo.
    /// </summary>
    public void Undo()
    {
        if (commandHistory.Count > 0)
        {
            var command = commandHistory.Pop();
            command.Undo();
        }
    }
}
