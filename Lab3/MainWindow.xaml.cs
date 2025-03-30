using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Text.Json;

namespace Laba3
{
    public partial class MainWindow : Window
    {
        private List<EnemyTemplate> _enemyTemplates = new();
        private Random _random = new();
        private Player _player;
        private Enemy _currentEnemy;

        public MainWindow()
        {
            InitializeComponent();
            _player = new Player();

            LoadEnemyTemplatesFromJson();
            SelectNextEnemy();
            UpdateUI();
        }

        private void LoadEnemyTemplatesFromJson()
        {
            try
            {
                string path = "Enemies.json";

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    _enemyTemplates = JsonSerializer.Deserialize<List<EnemyTemplate>>(json);
                }
                else
                {
                    MessageBox.Show("Файл Enemies.json не найден. Будут использованы заглушки.");
                    _enemyTemplates.Add(new EnemyTemplate
                    {
                        Name = "Тестовый враг",
                        IconName = "",
                        BaseLife = 10,
                        LifeModifier = 2,
                        BaseGold = 5,
                        GoldModifier = 1,
                        SpawnChance = 10
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при чтении JSON: " + ex.Message);
            }
        }
        private void SelectNextEnemy()
        {
            if (_enemyTemplates == null || _enemyTemplates.Count == 0)
            {
                MessageBox.Show("Нет шаблонов врагов!");
                return;
            }

            int totalChance = _enemyTemplates.Sum(t => t.SpawnChance);

            int roll = _random.Next(1, totalChance + 1);
            int runningSum = 0;

            EnemyTemplate selected = null;
            foreach (var template in _enemyTemplates)
            {
                runningSum += template.SpawnChance;
                if (roll <= runningSum)
                {
                    selected = template;
                    break;
                }
            }

            if (selected != null)
            {
                _currentEnemy = new Enemy(selected, _player.Level);
            }
        }

        private void UpdateUI()
        {
            if (_currentEnemy != null)
            {
                EnemyLifeTextBlock.Text = $"Здоровье врага: {_currentEnemy.Life}";
                EnemyNameTextBlock.Text = $"Имя: {_currentEnemy.Name}";

                if (!string.IsNullOrEmpty(_currentEnemy.IconPath) && File.Exists(_currentEnemy.IconPath))
                {
                    EnemyImage.Source = new BitmapImage(new Uri(_currentEnemy.IconPath));
                }
                else
                {
                    EnemyImage.Source = null;
                }
            }
            else
            {
                EnemyLifeTextBlock.Text = "Здоровье врага: нет";
                EnemyNameTextBlock.Text = "Имя: нет";
                EnemyImage.Source = null;
            }

            PlayerGoldTextBlock.Text = $"Золото: {_player.Gold}";
            PlayerDamageTextBlock.Text = $"Урон: {_player.Damage}";
        }

        private void EnemyImage_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_currentEnemy == null) return;

            _currentEnemy.TakeDamage(_player.Damage);

            if (_currentEnemy.IsDead())
            {
                _player.AddGold(_currentEnemy.GoldReward);
                _player.IncreaseLevel();
                SelectNextEnemy();
            }

            UpdateUI();
        }

        private void UpgradeDamageButton_Click(object sender, RoutedEventArgs e)
        {
            var cost = new BigNumber(5);
            if (_player.SpendGold(cost))
            {
                _player.IncreaseDamage();
            }
            else
            {
                MessageBox.Show("Недостаточно золота для улучшения урона!");
            }

            UpdateUI();
        }
    }
}
