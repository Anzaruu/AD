using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
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
using System.Windows.Media;
using System.Timers;
using System.Windows.Threading;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace AUP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MediaPlayer player = new MediaPlayer();
        int PlayOrPause = 0;
        String uri;
        string[] music;
        DispatcherTimer timer = new DispatcherTimer();
        bool rep = false;
        SoundPlayer soundPlayer;
        SystemSound systemSound;

        public MainWindow()
        {
            InitializeComponent();
            if (Moving.Value == 100)
            {
                Listik.SelectedIndex = Listik.SelectedIndex + 1;
                uri = Listik.Items[Listik.SelectedIndex].ToString();
                MusicPlay(uri);
            }
            if (uri!=null)
            {
                double dlina = Convert.ToDouble(player.NaturalDuration);
                Moving.Maximum = (int)dlina;

                double tekPosition = Convert.ToDouble(player.Position);
                Moving.Value = (int)tekPosition;
            }
        }

        private void Choose()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true};
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                music = Directory.GetFiles(dialog.FileName, "*.mp3");
                Listik.ItemsSource= music;
            }
        }

        private void MusicPlay(string url)
        {
            player.Stop();
            uri = url;
            player.Open(new Uri(url, UriKind.Absolute));
            player.Play();
            PlayOrPause = 1; 
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            //Moving.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
        }

    void timer_Tick(object sender, EventArgs e)
    {
            if (player.Source != null)
            {
                Tim.Text = String.Format("{0} / {1}", player.Position.ToString(@"mm\:ss"), player.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                Moving.Value = player.Position.TotalSeconds;
                if (player.Position== player.NaturalDuration.TimeSpan)
                {
                    if (rep == false)
                    {
                        Listik.SelectedIndex = Listik.SelectedIndex + 1;
                        uri = Listik.Items[Listik.SelectedIndex].ToString();
                        MusicPlay(uri);
                    }
                    else
                    {
                        uri = Listik.Items[Listik.SelectedIndex].ToString();
                        MusicPlay(uri);
                    }
                }
            }
            else
                Tim.Text = "No file selected...";
    }

    private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Name == "Search")
            {
                Choose();
            }

            else if ((sender as Button).Name == "Befor")
            {
                Listik.SelectedIndex = Listik.SelectedIndex - 1;
                uri = Listik.Items[Listik.SelectedIndex].ToString();
                MusicPlay(uri);
            }

            else if ((sender as Button).Name == "Paus")
            {
                if (PlayOrPause== 0)
                {
                    player.Play();
                    PlayOrPause = 1;
                }
                else
                {
                    player.Pause();
                    PlayOrPause = 0;
                }
                
            }

            else if ((sender as Button).Name == "Nexte")
            {
                Listik.SelectedIndex = Listik.SelectedIndex + 1;
                uri = Listik.Items[Listik.SelectedIndex].ToString();
                MusicPlay(uri);
            }

            else if ((sender as Button).Name == "Repet")
            {
                if (rep == false)
                {
                    rep= true;
                }
                else { rep = false; }
            }

            else if ((sender as Button).Name == "Mixing")
            {
                Random rand = new Random();
                for (int i = music.Length - 1; i > 0; i--)
                {
                    int j = rand.Next(i);
                    string tmp = music[i];
                    music[i] = music[j];
                    music[j] = tmp;
                }
            }
        }

        private void Listik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = Listik.SelectedItem.ToString();
            MusicPlay(selected);
        }
        private void Moving_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double position = Moving.Value;
            if (position > 0)
            {
                Moving.Value = position;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (slider != null)
            {
                player.Volume = slider.Value;
            }
        }
    }
}
