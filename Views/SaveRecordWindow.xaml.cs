using PacManWpfApp.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace PacManWpfApp.Views
{
    public partial class SaveRecordWindow : Window
    {
        public string PlayerName { get; private set; }
        public bool IsRecordSaved { get; private set; }
        private readonly string _recordsFilePath = "game_records.json";
        private int _palletsNumber { get; set; }
        private TimeSpan _gameDuration;


        private ObservableCollection<GameRecord> _records { get; set; }

        public SaveRecordWindow(ObservableCollection<GameRecord> records, int pelletsCollected, TimeSpan gameDuration)

        {
            try
            {
                InitializeComponent();
                _records = records;
                _palletsNumber = pelletsCollected;
                _gameDuration = gameDuration;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveRecordButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerName = PlayerNameTextBox.Text;
            IsRecordSaved = true;

            var record = new GameRecord
            {
                PlayerName = PlayerName,
                Date = DateTime.Now,
                Duration = _gameDuration,
                PelletsCollected = _palletsNumber
            };
            _records.Add(record);
            SaveRecordsToFile(_records);
            this.Close();

        }

        private void SaveRecordsToFile(ObservableCollection<GameRecord> records)
        {
            var json = JsonSerializer.Serialize(records);
            File.WriteAllText(_recordsFilePath, json);
        }
    }
}
