public class BasicBullet : IBullet
{
    private int damage = 0;
    private float speed = 0f;
    private IBullet parent = null;


    public int GetDamage()
    {
        return damage;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetParent(IBullet parent)
    {
        this.parent = parent;
    }

    public IBullet GetParent()
    {
        return parent;
    }
}
