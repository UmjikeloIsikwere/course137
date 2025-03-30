using System;

namespace EnemyEditorWpf
{
    public class CEnemyTemplate
    {
        public string Name { get; set; }
        public string IconName { get; set; }
        public int BaseLife { get; set; }
        public double LifeModifier { get; set; }
        public int BaseGold { get; set; }
        public double GoldModifier { get; set; }
        public double SpawnChance { get; set; }

        public CEnemyTemplate() { }

        public CEnemyTemplate(
            string name,
            string iconName,
            int baseLife,
            double lifeModifier,
            int baseGold,
            double goldModifier,
            double spawnChance)
        {
            Name = name;
            IconName = iconName;
            BaseLife = baseLife;
            LifeModifier = lifeModifier;
            BaseGold = baseGold;
            GoldModifier = goldModifier;
            SpawnChance = spawnChance;
        }
    }
}
