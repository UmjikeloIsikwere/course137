namespace Laba3
{
    public class EnemyTemplate
    {
        public string Name { get; set; }
        public string IconName { get; set; }

        public int BaseLife { get; set; }
        public int LifeModifier { get; set; }

        public int BaseGold { get; set; }
        public int GoldModifier { get; set; }
        public int SpawnChance { get; set; }
    }
}
