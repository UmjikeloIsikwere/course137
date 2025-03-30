using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EnemyEditorWpf
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<CEnemyTemplate> _enemies;
        private string _iconsFolderPath = "Icons";

        public MainWindow()
        {
            InitializeComponent();

            _enemies = new ObservableCollection<CEnemyTemplate>();
            EnemiesDataGrid.ItemsSource = _enemies;

            LoadIcons();
        }

        private void LoadIcons()
        {
            if (!Directory.Exists(_iconsFolderPath))
            {
                MessageBox.Show($"Папка '{_iconsFolderPath}' не найдена.");
                return;
            }
            string absoluteFolderPath = Path.GetFullPath(_iconsFolderPath);

            var imageFiles = Directory.GetFiles(absoluteFolderPath, "*.*")
                                      .Where(file => file.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                                                  || file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                                                  || file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                                      .ToList();

            if (imageFiles.Count == 0)
            {
                MessageBox.Show("В папке 'Icons' не найдено изображений.");
                return;
            }

            IconComboBox.ItemsSource = imageFiles;
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = NameTextBox.Text;
                string icon = IconComboBox.SelectedItem as string;

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(icon))
                {
                    MessageBox.Show("Заполните все поля.");
                    return;
                }

                int baseLife = int.Parse(BaseLifeTextBox.Text);
                double lifeModifier = double.Parse(LifeModifierTextBox.Text);
                int baseGold = int.Parse(BaseGoldTextBox.Text);
                double goldModifier = double.Parse(GoldModifierTextBox.Text);
                double spawnChance = double.Parse(SpawnChanceTextBox.Text);

                var newEnemy = new CEnemyTemplate
                {
                    Name = name,
                    IconName = icon,
                    BaseLife = baseLife,
                    LifeModifier = lifeModifier,
                    BaseGold = baseGold,
                    GoldModifier = goldModifier,
                    SpawnChance = spawnChance
                };

                _enemies.Add(newEnemy);

                NameTextBox.Clear();
                IconComboBox.SelectedIndex = -1;
                BaseLifeTextBox.Clear();
                LifeModifierTextBox.Clear();
                BaseGoldTextBox.Clear();
                GoldModifierTextBox.Clear();
                SpawnChanceTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка ввода данных: " + ex.Message);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEnemy = EnemiesDataGrid.SelectedItem as CEnemyTemplate;
            if (selectedEnemy != null)
            {
                _enemies.Remove(selectedEnemy);
            }
            else
            {
                MessageBox.Show("Не выбран противник для удаления.");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var list = new CEnemyTemplateList();
                foreach (var enemy in _enemies)
                {
                    list.AddEnemy(enemy);
                }

                string fileName = "enemies.json";
                list.SaveToJson(fileName);

                MessageBox.Show($"Сохранено {list.Enemies.Count} противников в файл {fileName}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = "enemies.json";
                if (!File.Exists(fileName))
                {
                    MessageBox.Show($"Файл {fileName} не найден.");
                    return;
                }

                var list = new CEnemyTemplateList();
                list.LoadFromJson(fileName);

                _enemies.Clear();
                foreach (var enemy in list.Enemies)
                {
                    _enemies.Add(enemy);
                }

                MessageBox.Show($"Загружено {list.Enemies.Count} противников.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }
    }
}
