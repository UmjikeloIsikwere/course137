using System;

namespace Laba3
{
    public class Enemy
    {
        private string _name;
        private string _iconPath;
        private BigNumber _life;
        private BigNumber _goldReward;

        public string Name => _name;
        public string IconPath => _iconPath;

        public BigNumber Life
        {
            get => _life;
            private set => _life = value;
        }

        public BigNumber GoldReward => _goldReward;

        public Enemy(EnemyTemplate template, int playerLevel)
        {
            _name = template.Name;
            _iconPath = template.IconName;

            int totalLife = template.BaseLife + (template.LifeModifier * playerLevel);
            if (totalLife < 1) totalLife = 1;

            int totalGold = template.BaseGold + (template.GoldModifier * playerLevel);
            if (totalGold < 1) totalGold = 1;

            _life = new BigNumber(totalLife);
            _goldReward = new BigNumber(totalGold);
        }

        public void TakeDamage(BigNumber damage)
        {
            if (damage >= Life)
            {
                Life = new BigNumber(0);
            }
            else
            {
                Life = Life - damage;
            }
        }

        public bool IsDead()
        {
            return Life <= new BigNumber(0);
        }
    }
}
