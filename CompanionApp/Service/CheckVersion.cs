using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CompanionApp.Service
{
    public static class CheckVersion
    {

        static string repoOwner = "jgraph";
        static string repoName = "drawio-desktop";
        static string apiUrl = $"https://api.github.com/repos/{repoOwner}/{repoName}/releases/latest";


        public static async Task<string> IsUpToDate(string currentVersion)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("request");
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseBody);
                    string latestVersion = json["tag_name"].ToString().ToLower();


                    if (latestVersion != currentVersion)
                    {
                        return latestVersion;
                    }
                    else
                    {
                        return "uptodate";
                    }

                }
            }
            catch (Exception)
            {

            }
            return null;

        }


        public static async Task<string> GetLatestReleaseUrl()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("request");
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseBody);
                string latestReleaseUrl = json["html_url"].ToString();

                return latestReleaseUrl;
            }
        }

        public static async void OpenNewVersion()
        {

            string latestReleaseUrl = await GetLatestReleaseUrl();
            Process.Start(new ProcessStartInfo
            {
                FileName = latestReleaseUrl,
                UseShellExecute = true
            });

        }
    }
     
}
