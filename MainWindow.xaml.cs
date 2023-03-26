using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace PlayerMyFrameworkUltraMegaGigaChad
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {
        public bool repeat = true;
        public DirectoryInfo dir;
        public bool isPlay = false;
        private bool IsRandTrek = false;
        public List<string> list = new List<string>();
        public List<string> list2 = new List<string>();

        public Int64 start_time = 0;
        public string path;
        public MainWindow()
        {
            InitializeComponent();
            Start.IsEnabled = false;
            Let_trek.IsEnabled = false;
            Right_trek.IsEnabled = false;
            Start.Content = "⏵︎⏸︎";
            CheckMusicPlayingPeriodically();
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            IsRandTrek = false;
            AllTrek.Items.Clear();
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            var result = dialog.ShowDialog();
            dialog.RestoreDirectory = true;
            if (result == CommonFileDialogResult.Ok)
            {
                string selectedFileName = dialog.FileName;
                path = dialog.FileName + @"\";
                var FolderWithMusic = Directory.GetFiles(dialog.FileName, "*.mp3");

                DirectoryInfo dir = new DirectoryInfo(dialog.FileName);
                var filesM = dir.GetFiles("*.mp3");
                foreach (var music in filesM)
                {
                    
                    AllTrek.Items.Add(music.Name);
                }
                foreach (var item in AllTrek.Items)
                {
                    list.Add(item.ToString());
                    list2.Add(item.ToString());
                }

                AllTrek.SelectedIndex = 0;
                media.Play();
            }
        }

        private void AllTrek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllTrek.SelectedItem != null)
            {
                slider.Minimum = 0;
                string sourse_treak = path + (AllTrek.Items[AllTrek.SelectedIndex].ToString());
                media.Source = new Uri(sourse_treak);
                Start.IsEnabled = true;
                Let_trek.IsEnabled = true;
                Right_trek.IsEnabled = true;
            }
            else
            {
                Start.IsEnabled = false;
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (AllTrek.SelectedItem != null)
            {
                if (isPlay)
                {
                    Start.Content = "⏵︎⏸︎";
                    media.Play();
                    
                    isPlay = false;
                }
                else
                {
                    Start.Content = "⏸︎";
                    media.Pause();
                    isPlay = true;
                }
                Start.IsEnabled = true;
            }
            else
            {
                Start.IsEnabled = false;
            }
        }
        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            slider.Minimum = 0;
            slider.Maximum = media.NaturalDuration.TimeSpan.TotalSeconds;
            slider.ValueChanged += slider_ValueChanged; 
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
        }
        private async void CheckMusicPlayingPeriodically()
        {
            while (true)
            {
                
                if (slider.Value != slider.Maximum)
                {
                    slider.Value = media.Position.TotalSeconds;
                }
                else
                {
                    check_boundaries();
                }
                TimeSpan position = media.Position;
                string Time = string.Format("{0:00}:{1:00}:{2:00}", (int)position.TotalHours, position.Minutes, position.Seconds);

                TimeSpan totalDuration = TimeSpan.Zero;
                if (!media.NaturalDuration.HasTimeSpan || media.NaturalDuration.TimeSpan == TimeSpan.Zero)
                {

                }
                else
                {
                    totalDuration = media.NaturalDuration.TimeSpan;
                    TimeSpan currentPosition = media.Position;

                    TimeSpan Time_until_the_end = totalDuration - currentPosition;
                    string Time2 = string.Format("{0:00}:{1:00}:{2:00}", (int)Time_until_the_end.TotalHours, Time_until_the_end.Minutes, Time_until_the_end.Seconds);
                    Stoptime.Content = Time2;
                    Starttime.Content = Time;

                }

                
                await Task.Delay(1000);
            }
        }
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = TimeSpan.FromSeconds(slider.Value);
        }
        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Volume = volumeSlider.Value;
        }
        private void Right_trek_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = AllTrek.SelectedIndex;


                if ((index + 1) == AllTrek.Items.Count && repeat)
                {
                    AllTrek.SelectedIndex = 0;
                }
                else
                {
                    AllTrek.SelectedIndex++;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        private void check_boundaries()
        {
            int index = AllTrek.SelectedIndex;
            if (AllTrek.SelectedIndex == -1)
            {
                AllTrek.SelectedIndex = AllTrek.Items.Count - 1;
            }
            
            if ((index + 1) == AllTrek.Items.Count && repeat)
            {
                AllTrek.SelectedIndex = 0;
            }
            else
            {
                AllTrek.SelectedIndex++;
            }
        }
        private void Let_trek_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AllTrek.SelectedIndex--;
                if (AllTrek.SelectedIndex == -1)
                {
                    AllTrek.SelectedIndex = AllTrek.Items.Count - 1;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


        private void repeat_but_Click(object sender, RoutedEventArgs e)
        {
            
            if (repeat)
            {
                repeat_but.Content = "(no loop)";
               
                repeat = false;
            }
            else
            {
                repeat_but.Content = "loop";
                repeat = true;
            }
            
        }

        private void rand_trek_Click(object sender, RoutedEventArgs e)
        {
            AllTrek.Items.Clear();
            if (!IsRandTrek)
            {
                rendomListBox();
                IsRandTrek = true;
                foreach (var item in list2)
                {
                    AllTrek.Items.Add(item);
                }
            }
            else
            {
                foreach (var item in list)
                {
                    AllTrek.Items.Add(item);
                }
                IsRandTrek = false;
            }

        }
        private void rendomListBox()
        {
            Random rng = new Random();
            int n = list2.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = list2[k];
                list2[k] = list2[n];
                list2[n] = value;
            }

            
        }
    }
}
