using ChannelMirrorer.Util;
using Newtonsoft.Json;

namespace ChannelMirrorer.Core;

static class Program
{
    private static Config? _config;
    
    static void Main()
    {
        if (!Directory.Exists("files"))
        {
            Directory.CreateDirectory("files");
            Log("'files' directory not found. Directory created.", "&e"); 
        }
        if (!File.Exists("files/config.json"))
        {
            File.WriteAllText("files/config.json", JsonConvert.SerializeObject(new Config(), Formatting.Indented));
            Log("'files/config.json' file not found. File created.", "&e");
        }
        
        _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("files/config.json"))!;
        if (_config.Token == "")
        {
            Log("Error: You need to fill out the 'token' field in 'files/config.json' before starting.\n\n&7Press any key to continue...", "&c");
            Console.Read();
            return;
        }
        
        MainAsync().ConfigureAwait(false).GetAwaiter().GetResult(); 
    }
    
    static async Task MainAsync()
    {
        Log("Starting MainAsync...", "&2");

        if (_config != null)
        {
            var bot = new Bot(_config);
        }

        await Task.Delay(-1);
    }

    public static void Log(string message, string color = "&7")
    {
        FConsole.WriteLine($"{color}[{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")}]%0&f {message}");
    }
}