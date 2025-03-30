using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace EnemyEditorWpf
{
    public class CEnemyTemplateList
    {
        private List<CEnemyTemplate> enemies;

        public CEnemyTemplateList()
        {
            enemies = new List<CEnemyTemplate>();
        }
        public List<CEnemyTemplate> Enemies => enemies;

        public void AddEnemy(CEnemyTemplate enemy)
        {
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }

        public bool RemoveEnemyByName(string enemyName)
        {
            var enemyToRemove = enemies.FirstOrDefault(e => e.Name == enemyName);
            if (enemyToRemove != null)
            {
                enemies.Remove(enemyToRemove);
                return true;
            }
            return false;
        }
        public void SaveToJson(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(enemies, options);
            File.WriteAllText(filePath, jsonString);
        }

        public void LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            string jsonString = File.ReadAllText(filePath);
            var loadedList = JsonSerializer.Deserialize<List<CEnemyTemplate>>(jsonString);

            if (loadedList != null)
            {
                enemies = loadedList;
            }
        }
    }
}
