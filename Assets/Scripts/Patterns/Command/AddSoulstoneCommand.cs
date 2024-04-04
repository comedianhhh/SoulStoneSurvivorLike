/// <summary>
/// Command to add a specified amount of soulstones to the player's inventory.
/// </summary>
public class AddSoulstoneCommand : ICommand
{
    private Player player;
    private StonesDataSO soulstone;
    private int amount;

    /// <summary>
    /// Initializes a new instance of the AddSoulstoneCommand class.
    /// </summary>
    /// <param name="player">The player to whom soulstones will be added.</param>
    /// <param name="soulstone">The type of soulstone to be added.</param>
    /// <param name="amount">The amount of soulstones to be added.</param>
    public AddSoulstoneCommand(Player player, StonesDataSO soulstone, int amount)
    {
        this.player = player;
        this.soulstone = soulstone;
        this.amount = amount;
    }

    /// <summary>
    /// Executes the command to add soulstones to the player's inventory.
    /// </summary>
    public void Execute()
    {
        player.UpdateCurrencyAmount(soulstone, amount);
    }

    /// <summary>
    /// Undoes the addition of soulstones, effectively removing them from the player's inventory.
    /// </summary>
    public void Undo()
    {
        player.UpdateCurrencyAmount(soulstone, -amount);
    }
}
