namespace Banapuchin.Main
{
    public class VersionChecker
    {
        public static string GetLatestVersion()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var responseMessage = httpClient
                    .GetAsync("https://raw.githubusercontent.com/HanSolo1000Falcon/Banapuchin/MelonLoader/VERSION.txt").Result;
                responseMessage.EnsureSuccessStatusCode();
                
                using (Stream stream = responseMessage.Content.ReadAsStreamAsync().Result)
                using (StreamReader reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }
    }
}