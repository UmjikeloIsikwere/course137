namespace Laba3
{
    public class Player
    {
        private BigNumber _gold;
        private BigNumber _damage;
        private int _level;

        public BigNumber Gold
        {
            get => _gold;
            private set => _gold = value;
        }

        public BigNumber Damage
        {
            get => _damage;
            private set => _damage = value;
        }

        public int Level
        {
            get => _level;
            private set => _level = value;
        }

        public Player()
        {
            _gold = new BigNumber(0);
            _damage = new BigNumber(1);
            _level = 0;
        }

        public void AddGold(BigNumber amount)
        {
            Gold = Gold + amount;
        }

        public bool SpendGold(BigNumber cost)
        {
            if (Gold >= cost)
            {
                Gold = Gold - cost;
                return true;
            }
            return false;
        }

        public void IncreaseDamage()
        {
            Damage = Damage + new BigNumber(1);
        }

        public void IncreaseLevel()
        {
            Level++;
        }
    }
}
