namespace Items.Weapons
{
    public class Glock : Weapon
    {
        public Glock()
        {
            weaponName = "Glock";
        }

        protected override void Shoot()
        {
            base.Shoot();
            CreateRay(_firePoint.right);
        }
    }
}