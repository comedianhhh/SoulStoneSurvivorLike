/// <summary>
/// Command to remove a specified amount of soulstones from the player's inventory.
/// </summary>
public class RemoveSoulstoneCommand : ICommand
{
    private Player player;
    private StonesDataSO soulstone;
    private int amount;

    /// <summary>
    /// Initializes a new instance of the RemoveSoulstoneCommand class.
    /// </summary>
    /// <param name="player">The player from whom soulstones will be removed.</param>
    /// <param name="soulstone">The type of soulstone to be removed.</param>
    /// <param name="amount">The amount of soulstones to be removed.</param>
    public RemoveSoulstoneCommand(Player player, StonesDataSO soulstone, int amount)
    {
        this.player = player;
        this.soulstone = soulstone;
        this.amount = amount;
    }

    /// <summary>
    /// Executes the command to remove soulstones from the player's inventory.
    /// </summary>
    public void Execute()
    {
        player.UpdateCurrencyAmount(soulstone, -amount);
    }

    /// <summary>
    /// Undoes the removal of soulstones, effectively adding them back to the player's inventory.
    /// </summary>
    public void Undo()
    {
        player.UpdateCurrencyAmount(soulstone, amount);
    }
}
