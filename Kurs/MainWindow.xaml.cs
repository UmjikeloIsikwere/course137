using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace PhotoAlbum
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Photo> Photos = new ObservableCollection<Photo>();
        ICollectionView PhotosView;

        public MainWindow()
        {
            InitializeComponent();

            PhotosView = CollectionViewSource.GetDefaultView(Photos);
            PhotoListBox.ItemsSource = PhotosView;

            // Заполнение ComboBox для фильтрации
            CategoryComboBox.Items.Add("Все");
            CategoryComboBox.Items.Add("Природа");
            CategoryComboBox.Items.Add("Путешествия");
            CategoryComboBox.Items.Add("Семья");
            CategoryComboBox.SelectedIndex = 0;

            // Установка значения по умолчанию в форме добавления
            AddCategoryComboBox.SelectedIndex = 0;
        }

        // Фильтрация списка по выбранной категории и тексту поиска
        private void RefreshFilter()
        {
            PhotosView.Filter = (obj) =>
            {
                Photo photo = obj as Photo;
                bool matchesCategory = true;
                if (CategoryComboBox.SelectedItem != null && CategoryComboBox.SelectedItem.ToString() != "Все")
                {
                    matchesCategory = photo.Category == CategoryComboBox.SelectedItem.ToString();
                }
                bool matchesSearch = true;
                if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
                {
                    string searchText = SearchTextBox.Text.ToLower();
                    matchesSearch = (photo.Description != null && photo.Description.ToLower().Contains(searchText)) ||
                                    (photo.Tags != null && photo.Tags.ToLower().Contains(searchText));
                }
                return matchesCategory && matchesSearch;
            };
            PhotosView.Refresh();
        }

        private void CategoryComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            RefreshFilter();
        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            RefreshFilter();
        }

        // Отображение выбранной фотографии и её деталей
        private void PhotoListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PhotoListBox.SelectedItem is Photo selectedPhoto)
            {
                TitleTextBlock.Text = selectedPhoto.Title;
                DescriptionTextBlock.Text = selectedPhoto.Description;
                TagsTextBlock.Text = "Теги: " + selectedPhoto.Tags;
                if (File.Exists(selectedPhoto.FilePath))
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(selectedPhoto.FilePath));
                    PhotoPreview.Source = bitmap;
                }
                else
                {
                    PhotoPreview.Source = null;
                }
            }
            else
            {
                TitleTextBlock.Text = "";
                DescriptionTextBlock.Text = "";
                TagsTextBlock.Text = "";
                PhotoPreview.Source = null;
            }
        }

        // Добавление фотографии на основе введённых данных в форме
        private void InlineAddButton_Click(object sender, RoutedEventArgs e)
        {
            Photo newPhoto = new Photo
            {
                Title = AddTitleTextBox.Text,
                FilePath = AddFilePathTextBox.Text,
                Category = (AddCategoryComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString(),
                Description = AddDescriptionTextBox.Text,
                Tags = AddTagsTextBox.Text
            };
            Photos.Add(newPhoto);
            RefreshFilter();

            // Очистка полей формы
            AddTitleTextBox.Clear();
            AddFilePathTextBox.Clear();
            AddDescriptionTextBox.Clear();
            AddTagsTextBox.Clear();
            AddCategoryComboBox.SelectedIndex = 0;
        }

        // Обработка кнопки "Обзор..." для выбора файла изображения
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Все файлы (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                AddFilePathTextBox.Text = dlg.FileName;
            }
        }

        // Удаление выбранной фотографии
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (PhotoListBox.SelectedItem is Photo selectedPhoto)
            {
                Photos.Remove(selectedPhoto);
                RefreshFilter();
            }
        }

        // Сохранение коллекции в JSON-файл с использованием System.Text.Json
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(Photos, options);
                File.WriteAllText(dlg.FileName, json);
            }
        }

        // Загрузка коллекции из JSON-файла
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                string json = File.ReadAllText(dlg.FileName);
                try
                {
                    var loadedPhotos = JsonSerializer.Deserialize<ObservableCollection<Photo>>(json);
                    Photos.Clear();
                    foreach (var photo in loadedPhotos)
                    {
                        Photos.Add(photo);
                    }
                    RefreshFilter();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке: " + ex.Message);
                }
            }
        }
    }
}
