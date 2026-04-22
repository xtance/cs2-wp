using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace Storage
{
  public class Storage<T>
  {

    private readonly string Id;
    private readonly Dictionary<int, T?> Dictionary = [];
    internal static Log Log = new("Storage_WP");

    public Storage(string id, EConfigFlag[] config)
    {
      Id = id;
      Manager.AddCallback((UserId, flag) =>
      {
        if (!config.Contains(flag)) return;
        var data = GetData(UserId);
        var removed = RemoveData(UserId);
        //Log.D($"[PS/Cleanup] Key: {id}, Slot: {slot}, had data: {Util2.Value.IsTruthy(data)} ({data?.ToString()}), removed: {removed}");
      });
    }

    public List<CCSPlayerController> FindPlayers()
    {
      return [.. Manager.GetValidPlayers().Where(HasData)];
    }

    public CCSPlayerController? FindPlayer()
    {
      return FindPlayers().FirstOrDefault();
    }

    public T? SetData(CCSPlayerController player, T? value)
    {
      if (player.UserId == null) Log.D($"Null userid at player: {player.PlayerName} (Slot {player.Slot})");
      else Dictionary[(int)player.UserId] = value;
      return value;
    }

    public T? SetData(int? UserId, T? value)
    {
      if (UserId == null) Log.D($"Null userid at UserId: {UserId})");
      else Dictionary[(int)UserId] = value;
      return value;
    }

    public T? GetData(CCSPlayerController player)
    {
      if (player == null) return default;
      if (player.UserId == null) return default;
      return Dictionary.TryGetValue((int)player.UserId, out T? value) ? value : default;
    }

    public T? GetData(int? UserId)
    {
      if (UserId == null || UserId <= 0) return default;
      return Dictionary.TryGetValue((int)UserId, out T? value) ? value : default;
    }

    public bool HasData(CCSPlayerController player)
    {
      if (player.UserId == null) return false;
      return Dictionary.ContainsKey((int)player.UserId);
    }

    public bool TryGetData(CCSPlayerController? player, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T? value)
    {
      if (player == null || player.UserId == null)
      {
        value = default;
        return false;
      }
      else
      {
        var result = Dictionary.TryGetValue((int)player.UserId, out T? tempValue);
        value = tempValue;
        return tempValue != null && result;
      }
    }

    public bool RemoveData(CCSPlayerController player)
    {
      if (player.UserId == null) return false;
      return Dictionary.Remove((int)player.UserId);
    }

    public bool RemoveData(int? UserId)
    {
      if (UserId == null) return false;
      return Dictionary.Remove((int)UserId);
    }

    public void Clear()
    {
      Dictionary.Clear();
    }

    public int Count
    {
      get { return Dictionary.Count; }
    }

    public Dictionary<int, T?>.ValueCollection Values()
    {
      return Dictionary.Values;
    }

    public List<KeyValuePair<int, T>> Entries()
    {
      return [.. Dictionary];
    }
  }
}

namespace Storage
{
  public class Manager
  {
    internal static Log Log = new Log("Storage/Manager");
    private static readonly List<Action<int?, EConfigFlag>> Callbacks = [];

    public static void AddCallback(Action<int?, EConfigFlag> callback)
    {
      Callbacks.Add(callback);
    }

    public static void Initialize(BasePlugin plugin)
    {

      // ClearOnNewRound
      plugin.RegisterEventHandler<EventRoundStart>((@event, info) =>
      {
        GetValidPlayers().ForEach(player =>
        {
          var userid = player.UserId;
          Log.D($"[PS/RoundStart] {userid} | {player.PlayerName} | {player.SteamID} | {player.UserId} | {player.NetworkIDString}");
          Callbacks.ForEach(cb => cb(userid, EConfigFlag.ClearOnNewRound));
        });
        return HookResult.Continue;
      });

      // ClearOnRespawn
      plugin.RegisterEventHandler<EventPlayerSpawn>((@event, info) =>
      {
        var player = @event.Userid;
        if (player == null)
        {
          Log.D($"[PS/Respawn] Player is null.");
        }
        else
        {
          var userid = player.UserId;
          Log.D($"[PS/Respawn] {userid} | {player.PlayerName} | {player.SteamID} | {player.UserId} | {player.NetworkIDString}");
          Server.NextFrame(() => Callbacks.ForEach(cb => cb(userid, EConfigFlag.ClearOnRespawn)));
        }
        return HookResult.Continue;
      });

      // ClearOnQuit
      plugin.RegisterEventHandler<EventPlayerDisconnect>((@event, info) =>
      {
        var player = @event.Userid;
        if (player == null)
        {
          Log.D($"[PS/Disconnect] Player {@event.Playerid} is null.");
        }
        else
        {
          var userid = player.UserId;
          Log.D($"[PS/Disconnect] #{@event.Playerid} | {userid} | {player.PlayerName} | {player.SteamID} | {player.UserId} | {player.NetworkIDString}");
          Server.NextFrame(() => Callbacks.ForEach(cb => cb(userid, EConfigFlag.ClearOnQuit)));
        }
        return HookResult.Continue;
      });
    }

    public static List<CCSPlayerController> GetValidPlayers(int? excludeSlot = null)
    {
      List<CCSPlayerController> players = [];
      for (int i = 0; i < Server.MaxPlayers; i++)
      {
        if (excludeSlot != null && i == excludeSlot) continue;
        var player = Utilities.GetPlayerFromSlot(i);
        if (!IsValid(player)) continue;
        players.Add(player);
      }
      return players;
    }

    public static bool IsValid([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] CCSPlayerController? player)
    {
      if (player is null) return false;
      if (!player.IsValid || player.Connected != PlayerConnectedState.PlayerConnected || player.IsBot || player.IsHLTV || player.UserId == 65535) return false;
      // no check for uid
      return true;
    }

    public static CCSPlayerController? GetPlayerBySteamID64(ulong steamId64)
    {
      for (int i = 0; i < Server.MaxPlayers; i++)
      {
        var player = Utilities.GetPlayerFromSlot(i);
        if (!IsValid(player)) continue;
        if (player.SteamID == steamId64) return player;
      }
      return null;
    }
  }
}

namespace Storage
{
  public enum EConfigFlag
  {
    ClearOnNewRound,
    ClearOnRespawn,
    ClearOnQuit,
  }
}

public class Log
{
  // public static void D(string text)
  // {
  // 	Console.WriteLine($"[xPRO] {text}");
  // }

  // public static void d(string text)
  // {
  // 	Console.WriteLine($"[xPRO] {text}");
  // }

  private readonly string Tag;

  public Log(string tag)
  {
    Tag = tag;
  }

  public void D(string text)
  {
    var time = DateTime.Now.ToString("h:mm:ss");
    Console.WriteLine($"[{time}] [{Tag}] {text}");
  }

  public void d(string text)
  {
    D(text);
  }
}