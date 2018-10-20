using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace steam_utility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Start.Opacity = 0.0;
            Advertise.Opacity = 0.0;
            Validate.Opacity = 0.0;
            Uninstall.Opacity = 0.0;
            AppNews.Opacity = 0;
            Defrag.Opacity = 0;
            UpdateNews.Opacity = 0;
            CheckSysReq.Opacity = 0;

            Height = 371;
        }

        public static string[] all_files;
        public static string steamapps;
        ItemCollection _backup;

        public void Populate()
        {
            foreach (string file in all_files)
            {
                string _appID = "";
                string _name = "";
                string _item = "";

                foreach (string line in File.ReadAllLines(file))
                {
                    if (line.Contains("\"appid"))
                    {
                        _appID = line.Split('"')[3];
                    }

                    if (line.Contains("name"))
                    {
                        _name = line.Split('"')[3];
                    }
                }

                if (_appID.Length != 6)
                {
                    int amt = 6 - _appID.Length;
                    for (var qqqq = 0; qqqq < amt; qqqq++)
                    {
                        _appID = " " + _appID;
                    }
                }

                _item = _appID + " | " + _name;
                if (_appID != "      ") TheMainList.Items.Add(_item);
            }
        }

        private void StartedUp(object sender, RoutedEventArgs e)
        {
            steamapps = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Valve\\Steam", "InstallPath", "").ToString() + "\\steamapps";
            all_files = Directory.GetFiles(steamapps, "*.acf");

            Populate();
            _backup = TheMainList.Items;
        }

        private void TheMainList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TheMainList.SelectedItem != null)
            {
                NameOnly.Content = TheMainList.SelectedItem.ToString().Split('|')[1].Trim(' ');
                IDOnly.Content = TheMainList.SelectedItem.ToString().Split('|')[0].TrimStart(' ');
                FileLoc.Content = "Selection from: " + all_files[TheMainList.SelectedIndex];

                Start.Opacity = 100.0;
                Advertise.Opacity = 100.0;
                Validate.Opacity = 100.0;
                Uninstall.Opacity = 100.0;
                AppNews.Opacity = 100;
                Defrag.Opacity = 100;
                UpdateNews.Opacity = 100;
                CheckSysReq.Opacity = 100;
            }
        }


        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://launch/"+ IDOnly.Content);
            if (CloseOnLaunch.IsChecked == true) Environment.Exit(0);
        }

        private void Advertise_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://advertise/" + IDOnly.Content);
        }

        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://validate/" + IDOnly.Content);
        }

        private void Uninstall_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://uninstall/" + IDOnly.Content);
        }

        private void AppNews_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://appnews/" + IDOnly.Content);
        }

        private void Defrag_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://defrag/" + IDOnly.Content);
        }

        private void UpdateNews_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://updatenews/" + IDOnly.Content);
        }

        /// <summary>
        /// MISCELLANOUS
        /// </summary>
    
        private void CheckSysReq_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://checksysreqs/" + IDOnly.Content);
        }


        private void Backup_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://backup/");
        }

        private void PaypalCancel_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://paypal/cancel");
        }

        private void GuestPasses_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://guestpasses");
        }

        private void FlushConfig_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://flushconfig");
        }

        private void BrowseMeda_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("steam://browsemedia");
        }

        /// <summary>
        /// DEBUG
        /// </summary>

        private void DebugBox_Click(object sender, RoutedEventArgs e)
        {
            if (DebugBox.IsChecked == true)
            {
                Height = 611;
            } else
            {
                Height = 371;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe",steamapps);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TheMainList.Items.Clear();
            Populate();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            List<object> queried_items;
            if (Sensitivity.IsChecked == false)
            {
                queried_items = TheMainList.Items.OfType<object>().ToList().FindAll(x => x.ToString().ToLower().Contains(SearchBox.Text.ToLower()));
            } else
            {
                queried_items = TheMainList.Items.OfType<object>().ToList().FindAll(x => x.ToString().Contains(SearchBox.Text));
            }

            TheMainList.Items.Clear();
            
            foreach (var foundItem in queried_items)
            {
                TheMainList.Items.Add(foundItem);
            }
        }

        private void RestoreFromSearch_Click(object sender, RoutedEventArgs e)
        {
            TheMainList.Items.Clear();
            
            foreach (var x in _backup)
            {
                TheMainList.Items.Add(x);
            }
        }

        Random r = new Random();

        private void RandomSel_Click(object sender, RoutedEventArgs e)
        {
            TheMainList.SelectedIndex = r.Next(TheMainList.Items.Count);
        }

        private void RandomLau_Click(object sender, RoutedEventArgs e)
        {
            TheMainList.SelectedIndex = r.Next(TheMainList.Items.Count);
            Start_Click(sender, e);
        }
    }
}
