using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Bll.Services;

public class TelegramLogger
{
    private readonly string _botToken;
    private readonly string _chatId;
    private readonly HttpClient _httpClient = new();

    public TelegramLogger(string botToken, string chatId)
    {
        _botToken = botToken;
        _chatId = chatId;
    }

    public async Task LogAsync(string message)
    {
        var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
        var data = new Dictionary<string, string>
        {
            ["chat_id"] = _chatId,
            ["text"] = message
        };

        await _httpClient.PostAsync(url, new FormUrlEncodedContent(data));
    }
}
