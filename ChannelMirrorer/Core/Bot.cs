using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using ChannelMirrorer.Util;
using DSharpPlus.EventArgs;
using Emzi0767.Utilities;
using Newtonsoft.Json;

namespace ChannelMirrorer.Core;

public class Bot
{
    private static DiscordClient? _client;
    private static Config? _config;

    public static DiscordClient? GetClient()
    {
        return _client ?? null;
    }

    public static Config? GetConfig()
    {
        return _config ?? null;
    }

    public Bot(Config cfg)
    {
        Program.Log("Initializing Bot...", "&3");

        _config = cfg;

        var config = new DiscordConfiguration
        {
            Token = _config.Token,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
            MinimumLogLevel = LogLevel.Critical,
            Intents = DiscordIntents.AllUnprivileged + (int)DiscordIntents.MessageContents
        };

        Program.Log("Initializing components...", "&3");

        _client = new DiscordClient(config);

        _client.MessageCreated += async (c, e) =>
        {
            if (!_config.Channels.Contains(e.Channel.Id)) return;
            if (e.Author.IsBot) return;

            var member = (DiscordMember)e.Author;

            var builder = new DiscordWebhookBuilder()
            {
                Username = member.DisplayName,
                AvatarUrl = member.AvatarUrl,
                Content = e.Message.Content
            };

            foreach (var a in e.Message.Attachments)
            {
                builder.Content += " " + a.Url;
            }

            foreach (ulong id in _config.Channels.Where(c => c != e.Channel.Id))
            {
                var channel = await _client.GetChannelAsync(id);
                channel = channel.Guild.GetChannel(channel.Id);

                DiscordWebhook wh = null;

                var webhooks = await channel.GetWebhooksAsync();
                
                if (!_config.ChannelWebhooks.ContainsKey(channel.Id))
                {
                    wh = await channel.CreateWebhookAsync("Channel Mirrorer");
                    _config.ChannelWebhooks.Add(channel.Id, wh.Id);
                    await File.WriteAllTextAsync("files/config.json", JsonConvert.SerializeObject(_config));
                }
                else
                {
                    foreach (var _wh in webhooks)
                    {
                        if (_wh.Id == _config.ChannelWebhooks[channel.Id])
                        {
                            wh = _wh;
                            break;
                        }
                    }
                }

                if (wh == null) return;
                
                await builder.SendAsync(wh);
            }
        };

        _client.Ready += async (_, _) =>
        {
            await _client.UpdateStatusAsync(new DiscordActivity("mindcraf", ActivityType.Watching), UserStatus.Online);
        };

        Program.Log("Bot initialization complete. Connecting...", "&3");

        _client.ConnectAsync();

        Program.Log("Connected.", "&3");
    }
}