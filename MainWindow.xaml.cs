using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace Test_API
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            GetAPI().GetAwaiter();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            GetAPI().GetAwaiter();
        }

        private async Task GetAPI()
        {
            //WebResponse response = await request.GetResponseAsync();

            while (true)
            {
                WebRequest request = WebRequest.Create("http://api.open-notify.org/iss-now.json");
                WebResponse response = await request.GetResponseAsync();

                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    string line;
                    if ((line = stream.ReadLine()) != null)
                    {
                        Location location = JsonConvert.DeserializeObject<Location>(line);

                        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(location.timestamp).ToLocalTime();

                        label.Content = dt.ToString("HH:mm:ss  dd-MM-yyyy");
                        label1.Content = location.iss_position.longitude;
                        label2.Content = location.iss_position.latitude;

                        //string url = "https://www.google.ru/maps/@" + location.iss_position.longitude + "001," + location.iss_position.latitude + "001,1z";
                        //wbMaps.Source = new Uri(url);
                        //wbMaps.Source = new Uri("https://www.google.ru/maps/@57.4663505,47.5245677,4.33z");
                    }
                }
                Thread.Sleep(1000);
            }
        }

    }

    public class Location
    {
        public int timestamp { get; set; }
        public string message { get; set; }
        public IssPosition iss_position { get; set; }
    }
    public class IssPosition
    {
        public string longitude { get; set; }
        public string latitude { get; set; }
    }

}
