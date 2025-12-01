using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using OsuParsers.Beatmaps;
using OsuParsers.Decoders;

namespace alternator_analyser.Services;

public class GameMonitorService
{
    private ClientWebSocket? webSocket = null;
    
    public async Task<Beatmap?> CurrentBeatmap()
    {
        String? path = await CurrentBeatmapPath();

        return (path != null) ? BeatmapDecoder.Decode(path) : null;
    }

    public async Task<String?> CurrentBeatmapPath()
    {
        await AttemptConnect();
        var buffer = new byte[1024 * 1000];

        if (webSocket?.State != WebSocketState.Open)
        {
            webSocket?.Abort();
            webSocket = null;
            return null;
        }
        
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        if (result.MessageType == WebSocketMessageType.Close)
        {
            return null;
        }

        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var data = JsonDocument.Parse(json);
        var root = data.RootElement;
        
        if (root.TryGetProperty("settings", out var settings) &&
            settings.TryGetProperty("folders", out var folders) &&
            folders.TryGetProperty("songs", out var songs) &&
            root.TryGetProperty("menu", out var menu) &&
            menu.TryGetProperty("bm", out var bm) &&
            bm.TryGetProperty("path", out var path) &&
            path.TryGetProperty("folder", out var folder) &&
            path.TryGetProperty("file", out var file))
        {
            String fullPath = Path.Combine(songs.ToString(), folder.ToString(), file.ToString());
            Console.WriteLine(fullPath);
            return fullPath;
        }

        return null;
    }
    
    public async Task AttemptConnect()
    {
        if (webSocket != null)
        {
            return;
        }
        
        webSocket = new ClientWebSocket();
        await webSocket.ConnectAsync(new Uri("ws://localhost:24050/ws"), CancellationToken.None);
    }
}