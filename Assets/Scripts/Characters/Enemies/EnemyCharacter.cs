using System;

namespace Characters.Enemies
{
    public class EnemyCharacter : Character
    {
        private void Start()
        {
            _healthComponent.OnDeath += Death;
        }

        private void Death()
        {
            gameObject.SetActive(false);
        }
    }
}