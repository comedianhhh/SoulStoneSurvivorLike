public abstract class BulletDecorator : IBullet
{
    private int damage = 0;
    private float speed = 0f;
    private IBullet bullet;
    private IBullet parent;

    public BulletDecorator(IBullet bullet, int damage, float speed)
    {
        this.bullet = bullet;
        this.damage = damage;
        this.speed = speed;
        bullet.SetParent(this);
    }

    public void SetParent(IBullet bullet)
    {
        this.parent = bullet;
    }

    public int GetDamage()
    {
        return damage + bullet.GetDamage();
    }

    public float GetSpeed()
    {
        return speed + bullet.GetSpeed();   
    }

    public IBullet GetParent()
    {
        return parent;
    }

    public void Transfer(IBullet bullet) {
        this.bullet.SetParent(parent); 
    }
}
