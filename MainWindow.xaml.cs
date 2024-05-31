using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace CountryInfoApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void FetchCountryInfoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string countryName = CountryTextBox.Text;
                if (string.IsNullOrWhiteSpace(countryName))
                {
                    MessageBox.Show("Please enter a country name.");
                    return;
                }

                string apiUrl = $"https://restcountries.com/v3.1/name/{Uri.EscapeDataString(countryName)}";
                var countryInfo = await FetchDataFromApi(apiUrl);
                DisplayCountryInfo(countryInfo);
            }
            catch (Exception ex)
            {
                CountryInfoTextBlock.Text = $"Error: {ex.Message}";
            }
        }

        private async Task<string> FetchDataFromApi(string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }

        private void DisplayCountryInfo(string jsonData)
        {
            var json = JArray.Parse(jsonData);
            if (json != null && json.Count > 0)
            {
                var country = json[0];
                string name = country["name"]["common"]?.ToString() ?? "N/A";
                string capital = country["capital"]?.First?.ToString() ?? "N/A";
                string region = country["region"]?.ToString() ?? "N/A";
                string population = country["population"]?.ToString() ?? "N/A";

                CountryInfoTextBlock.Text = $"Kraj: {name}\nStoilca: {capital}\nKontynent: {region}\nPopulacja: {population}";
            }
            else
            {
                CountryInfoTextBlock.Text = "No results found.";
            }
        }
    }
}
