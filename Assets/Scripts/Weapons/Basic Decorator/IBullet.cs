
public interface IBullet 
{
    public void SetParent(IBullet parent);
    public IBullet GetParent();
    public int GetDamage();
    public float GetSpeed();
}
