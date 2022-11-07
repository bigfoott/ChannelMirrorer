namespace ChannelMirrorer.Util;

[Serializable]
public class Config
{
    public string Token;
    public List<ulong> Channels;
    public Dictionary<ulong, ulong> ChannelWebhooks; //key is channel id, value is webhook id
    
    public Config()
    {
        Token = "";
        Channels = new List<ulong>();
        ChannelWebhooks = new Dictionary<ulong, ulong>();
    }
}
