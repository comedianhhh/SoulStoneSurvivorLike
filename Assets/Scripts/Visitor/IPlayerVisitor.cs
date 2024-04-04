// The interface that defines the visitor's actions
public interface IPlayerVisitor
{
    void Visit(SoulstoneCollectible soulstone);
    void Visit(PowerUpCollectible powerUp);
}