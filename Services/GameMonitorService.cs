using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace alternator_analyser.Services;

public class GameMonitorService(TimingService timingService)
{
    private ClientWebSocket? _webSocket;
    private string? _currentBeatmapPath;
    
    public async Task CheckForBeatmapChange(CancellationToken token)
    {
        var path = await CurrentBeatmapPath(token);
        if (path == null)
            return;
        if (path != _currentBeatmapPath)
        {
            _currentBeatmapPath = path;
            BeatmapChanged(_currentBeatmapPath);
        }
    }

    private void BeatmapChanged(string beatmapPath)
    {
        timingService.OnBeatmapChanged(beatmapPath);
    }

    private async Task<string?> CurrentBeatmapPath(CancellationToken token)
    {
        await AttemptConnect(token);
        var buffer = new byte[1024 * 1000];
        if (_webSocket?.State != WebSocketState.Open)
        {
            _webSocket?.Abort();
            _webSocket = null;
            return null;
        }
        var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), token);
        if (result.MessageType == WebSocketMessageType.Close)
        {
            return null;
        }

        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var data = JsonDocument.Parse(json);
        var root = data.RootElement;
        if (!root.TryGetProperty("settings", out var settings))
            return null;
        if (!settings.TryGetProperty("folders", out var folders))
            return null;
        if (!folders.TryGetProperty("songs", out var songs))
            return null;
        if (!root.TryGetProperty("menu", out var menu))
            return null;
        if (!menu.TryGetProperty("bm", out var bm))
            return null;
        if (!bm.TryGetProperty("path", out var path))
            return null;
        if (!path.TryGetProperty("folder", out var folder))
            return null;
        if (!path.TryGetProperty("file", out var file))
            return null;
        var fullPath = Path.Combine(songs.ToString(), folder.ToString(), file.ToString());
        return fullPath;
    }
    
    private async Task AttemptConnect(CancellationToken token)
    {
        if (_webSocket != null)
            return;
        _webSocket = new ClientWebSocket();
        await _webSocket.ConnectAsync(new Uri("ws://localhost:24050/ws"), token);
    }
}