﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Menu;
using MenuManager;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeaponPaints
{
	internal static class Utility
	{
		internal static WeaponPaintsConfig? Config { get; set; }

		internal static async Task CheckDatabaseTables()
		{
			
		}

		internal static bool IsPlayerValid(CCSPlayerController? player)
		{
			if (player is null || WeaponPaints.WeaponSync is null) return false;

			return player is { IsValid: true, IsBot: false, IsHLTV: false, UserId: not null };
		}

		internal static void LoadSkinsFromFile(string filePath, ILogger logger)
		{
			var json = File.ReadAllText(filePath);
			try
			{
				var deserializedSkins = JsonConvert.DeserializeObject<List<JObject>>(json);
				WeaponPaints.SkinsList = deserializedSkins ?? [];
			}
			catch (FileNotFoundException)
			{
				logger?.LogError("Not found \"skins.json\" file");
			}
		}
		
		internal static void LoadPinsFromFile(string filePath, ILogger logger)
		{
			var json = File.ReadAllText(filePath);
			try
			{
				var deserializedPins = JsonConvert.DeserializeObject<List<JObject>>(json);
				WeaponPaints.PinsList = deserializedPins ?? [];
			}
			catch (FileNotFoundException)
			{
				logger?.LogError("Not found \"pins.json\" file");
			}
		}

		internal static void LoadGlovesFromFile(string filePath, ILogger logger)
		{
			try
			{
				var json = File.ReadAllText(filePath);
				var deserializedSkins = JsonConvert.DeserializeObject<List<JObject>>(json);
				WeaponPaints.GlovesList = deserializedSkins ?? [];
			}
			catch (FileNotFoundException)
			{
				logger?.LogError("Not found \"gloves.json\" file");
			}
		}

		internal static void LoadAgentsFromFile(string filePath, ILogger logger)
		{
			try
			{
				var json = File.ReadAllText(filePath);
				var deserializedSkins = JsonConvert.DeserializeObject<List<JObject>>(json);
				WeaponPaints.AgentsList = deserializedSkins ?? [];
			}
			catch (FileNotFoundException)
			{
				logger?.LogError("Not found \"agents.json\" file");
			}
		}

		internal static void LoadMusicFromFile(string filePath, ILogger logger)
		{
			try
			{
				var json = File.ReadAllText(filePath);
				var deserializedSkins = JsonConvert.DeserializeObject<List<JObject>>(json);
				WeaponPaints.MusicList = deserializedSkins ?? [];
			}
			catch (FileNotFoundException)
			{
				logger?.LogError("Not found \"music.json\" file");
			}
		}

		internal static void Log(string message)
		{
			Console.BackgroundColor = ConsoleColor.DarkGray;
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("[WeaponPaints] " + message);
			Console.ResetColor();
		}
		
		internal static IMenu? CreateMenu(string title)
		{
			var menuType = WeaponPaints.Instance.Config.MenuType.ToLower();
        
			var menu = menuType switch
			{
				_ when menuType.Equals("selectable", StringComparison.CurrentCultureIgnoreCase) =>
					WeaponPaints.MenuApi?.NewMenu(title),

				_ when menuType.Equals("dynamic", StringComparison.CurrentCultureIgnoreCase) =>
					WeaponPaints.MenuApi?.NewMenuForcetype(title, MenuType.ButtonMenu),

				_ when menuType.Equals("center", StringComparison.CurrentCultureIgnoreCase) =>
					WeaponPaints.MenuApi?.NewMenuForcetype(title, MenuType.CenterMenu),

				_ when menuType.Equals("chat", StringComparison.CurrentCultureIgnoreCase) =>
					WeaponPaints.MenuApi?.NewMenuForcetype(title, MenuType.ChatMenu),

				_ when menuType.Equals("console", StringComparison.CurrentCultureIgnoreCase) =>
					WeaponPaints.MenuApi?.NewMenuForcetype(title, MenuType.ConsoleMenu),

				_ => WeaponPaints.MenuApi?.NewMenu(title)
			};

			return menu;
		}

		internal static async Task CheckVersion(string version, ILogger logger)
		{
			using HttpClient client = new();

			try
			{
				var response = await client.GetAsync("https://raw.githubusercontent.com/Nereziel/cs2-WeaponPaints/main/VERSION").ConfigureAwait(false);

				if (response.IsSuccessStatusCode)
				{
					var remoteVersion = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					remoteVersion = remoteVersion.Trim();

					var comparisonResult = string.CompareOrdinal(version, remoteVersion);

					switch (comparisonResult)
					{
						case < 0:
							logger.LogWarning("Plugin is outdated! Check https://github.com/Nereziel/cs2-WeaponPaints");
							break;
						case > 0:
							logger.LogInformation("Probably dev version detected");
							break;
						default:
							logger.LogInformation("Plugin is up to date");
							break;
					}
				}
				else
				{
					logger.LogWarning("Failed to check version");
				}
			}
			catch (HttpRequestException ex)
			{
				logger.LogError(ex, "Failed to connect to the version server.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occurred while checking version.");
			}
		}

		internal static void ShowAd(string moduleVersion)
		{
			Console.WriteLine(" ");
			Console.WriteLine(" _     _  _______  _______  _______  _______  __    _  _______  _______  ___   __    _  _______  _______ ");
			Console.WriteLine("| | _ | ||       ||   _   ||       ||       ||  |  | ||       ||   _   ||   | |  |  | ||       ||       |");
			Console.WriteLine("| || || ||    ___||  |_|  ||    _  ||   _   ||   |_| ||    _  ||  |_|  ||   | |   |_| ||_     _||  _____|");
			Console.WriteLine("|       ||   |___ |       ||   |_| ||  | |  ||       ||   |_| ||       ||   | |       |  |   |  | |_____ ");
			Console.WriteLine("|       ||    ___||       ||    ___||  |_|  ||  _    ||    ___||       ||   | |  _    |  |   |  |_____  |");
			Console.WriteLine("|   _   ||   |___ |   _   ||   |    |       || | |   ||   |    |   _   ||   | | | |   |  |   |   _____| |");
			Console.WriteLine("|__| |__||_______||__| |__||___|    |_______||_|  |__||___|    |__| |__||___| |_|  |__|  |___|  |_______|");
			Console.WriteLine("						>> Version: " + moduleVersion);
			Console.WriteLine("			>> GitHub: https://github.com/Nereziel/cs2-WeaponPaints");
			Console.WriteLine(" ");
		}
	}
}