﻿using System.Collections.Concurrent;
using CounterStrikeSharp.API.Modules.Utils;
using System.Globalization;

namespace WeaponPaints;

internal class WeaponSynchronization
{
	private readonly WeaponPaintsConfig _config;
	private readonly Database _database;

	internal WeaponSynchronization(Database database, WeaponPaintsConfig config)
	{
		_database = database;
		_config = config;
	}

	internal async Task GetPlayerData(PlayerInfo? player)
	{
		try
		{
			if (player?.SteamId == null)
			{
				Console.WriteLine($"[WP] Player steamid is null for {player?.Name}");
				return;
			}
			;

			var connection = await X_Hook.FetchSkins(player.SteamId ?? 0);
			if (connection == null)
			{
				Console.WriteLine($"[WP] X_Hook.FetchSkins is null for {player.SteamId}");
				return;
			}
			Console.WriteLine($"[WP] X_Hook.FetchSkins is NOT null for {player.SteamId} !!!");

			if (_config.Additional.KnifeEnabled)
				GetKnifeFromDatabase(player, connection);
			if (_config.Additional.GloveEnabled)
				GetGloveFromDatabase(player, connection);
			if (_config.Additional.AgentEnabled)
				GetAgentFromDatabase(player, connection);
			if (_config.Additional.MusicEnabled)
				GetMusicFromDatabase(player, connection);
			if (_config.Additional.SkinEnabled)
				GetWeaponPaintsFromDatabase(player, connection);
			if (_config.Additional.PinsEnabled)
				GetPinsFromDatabase(player, connection);
		}
		catch (Exception ex)
		{
			// Log the exception or handle it appropriately
			Console.WriteLine($"An error occurred: {ex.Message}");
		}
	}

	private void GetKnifeFromDatabase(PlayerInfo? player, X_Response row)
	{
		try
		{
			if (!_config.Additional.KnifeEnabled || player?.SteamId == null)
				return;

			if (row.knife == null) return;

			// Get or create entries for the player’s slot
			var playerKnives = WeaponPaints.GPlayersKnife.GetOrAdd(player.Slot, _ => new ConcurrentDictionary<CsTeam, string>());


			// Assign knife to both teams if weaponTeam is None
			playerKnives[CsTeam.Terrorist] = row.knife;
			playerKnives[CsTeam.CounterTerrorist] = row.knife;


		}
		catch (Exception ex)
		{
			Utility.Log($"An error occurred in GetKnifeFromDatabase: {ex.Message}");
		}
	}

	private void GetGloveFromDatabase(PlayerInfo? player, X_Response row)
	{
		try
		{
			if (!_config.Additional.GloveEnabled || player?.SteamId == null)
			{
				Console.WriteLine($"{player?.Name} glove 0");
				return;
			}

			if (row.weapon_defindex_glove == null) 
			{
				Console.WriteLine($"{player?.Name} glove 1");
				return;
			}

			var playerGloves = WeaponPaints.GPlayersGlove.GetOrAdd(player.Slot, _ => new ConcurrentDictionary<CsTeam, ushort>());

			Console.WriteLine($"{player?.Name} glove 2 ({(ushort)row.weapon_defindex_glove})");
			

			// Assign glove ID to both teams if weaponTeam is None
			playerGloves[CsTeam.Terrorist] = (ushort)row.weapon_defindex_glove;
			playerGloves[CsTeam.CounterTerrorist] = (ushort)row.weapon_defindex_glove;
		}
		catch (Exception ex)
		{
			Utility.Log($"An error occurred in GetGlovesFromDatabase: {ex.Message}");
			Console.WriteLine($"{player?.Name} glove fuck");
		}
	}

	private void GetAgentFromDatabase(PlayerInfo? player, X_Response row)
	{
		try
		{
			if (!_config.Additional.AgentEnabled || player?.SteamId == null)
				return;

			// const string query = "SELECT `agent_ct`, `agent_t` FROM `wp_player_agents` WHERE `steamid` = @steamid";
			// var agentData = connection.QueryFirstOrDefault<(string, string)>(query, new { steamid = player.SteamId });

			// if (agentData == default) return;
			// var agentCT = agentData.Item1;
			// var agentT = agentData.Item2;

			// if (!string.IsNullOrEmpty(agentCT) || !string.IsNullOrEmpty(agentT))
			// {
			// 	WeaponPaints.GPlayersAgent[player.Slot] = (
			// 		agentCT,
			// 		agentT
			// 	);
			// }
		}
		catch (Exception ex)
		{
			Utility.Log($"An error occurred in GetAgentFromDatabase: {ex.Message}");
		}
	}

	private void GetWeaponPaintsFromDatabase(PlayerInfo? player, X_Response response)
	{
		try
		{
			if (!_config.Additional.SkinEnabled || player == null || player?.SteamId == null)
				return;

			Console.WriteLine($"[wp] GetWeaponPaintsFromDatabase | {response.playerSkins?.Count}");
			if (response.playerSkins == null || response.playerSkins.Count <= 0) return;

			var playerWeapons = WeaponPaints.GPlayerWeaponsInfo.GetOrAdd(player.Slot,
				_ => new ConcurrentDictionary<CsTeam, ConcurrentDictionary<int, WeaponInfo>>());

			// var weaponInfos = new ConcurrentDictionary<int, WeaponInfo>();
			
			foreach (var row in response.playerSkins)
			{
				int weaponDefIndex = row.weapon_defindex ?? 0;
				int weaponPaintId = row.weapon_paint_id ?? 0;
				float weaponWear = row.weapon_wear ?? 0f;
				int weaponSeed = row.weapon_seed ?? 0;
				string weaponNameTag = row.weapon_nametag ?? "";
				bool weaponStatTrak = row.weapon_stattrak ?? false;
				int weaponStatTrakCount = row.weapon_stattrak_count ?? 0;

				CsTeam weaponTeam = CsTeam.None;

				string[]? keyChainParts = row.weapon_keychain?.ToString()?.Split(';');

				KeyChainInfo keyChainInfo = new KeyChainInfo();

				if (keyChainParts?.Length == 5 &&
					uint.TryParse(keyChainParts[0], out uint keyChainId) &&
					float.TryParse(keyChainParts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float keyChainOffsetX) &&
					float.TryParse(keyChainParts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float keyChainOffsetY) &&
					float.TryParse(keyChainParts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out float keyChainOffsetZ) &&
					uint.TryParse(keyChainParts[4], out uint keyChainSeed))
				{
					// Successfully parsed the values
					keyChainInfo.Id = keyChainId;
					keyChainInfo.OffsetX = keyChainOffsetX;
					keyChainInfo.OffsetY = keyChainOffsetY;
					keyChainInfo.OffsetZ = keyChainOffsetZ;
					keyChainInfo.Seed = keyChainSeed;
				}
				else
				{
					// Failed to parse the values, default to 0
					keyChainInfo.Id = 0;
					keyChainInfo.OffsetX = 0f;
					keyChainInfo.OffsetY = 0f;
					keyChainInfo.OffsetZ = 0f;
					keyChainInfo.Seed = 0;
				}

				// Create the WeaponInfo object
				WeaponInfo weaponInfo = new WeaponInfo
				{
					Paint = weaponPaintId,
					Seed = weaponSeed,
					Wear = weaponWear,
					Nametag = weaponNameTag,
					KeyChain = keyChainInfo,
					StatTrak = weaponStatTrak,
					StatTrakCount = weaponStatTrakCount,
				};

				// Retrieve and parse sticker data (up to 5 slots) 

				if (row.weapon_stickers != null)
				{
					foreach (var sticker in row.weapon_stickers)
					{
						// Access the sticker data dynamically using reflection
						//string stickerColumn = $"weapon_sticker_{i}";
						//var stickerData = ((IDictionary<string, object>)row!)[stickerColumn]; // Safely cast row to a dictionary;

						var stickerId = sticker.sticker_id ?? 0;
						var stickerSchema = 0;
						var stickerOffsetX = sticker.x ?? 0;
						var stickerOffsetY = sticker.y ?? 0;
						var stickerWear = sticker.wear ?? 0;
						var stickerScale = sticker.scale ?? 0;
						var stickerRotation = sticker.rotation ?? 0;


						StickerInfo stickerInfo = new StickerInfo
						{
							Id = (uint)stickerId,
							Schema = (uint)stickerSchema,
							OffsetX = stickerOffsetX,
							OffsetY = stickerOffsetY,
							Wear = stickerWear,
							Scale = stickerScale,
							Rotation = stickerRotation
						};

						weaponInfo.Stickers.Add(stickerInfo);
					}
				}
				if (weaponTeam == CsTeam.None)
				{
					// Get or create entries for both teams
					var terroristWeapons = playerWeapons.GetOrAdd(CsTeam.Terrorist, _ => new ConcurrentDictionary<int, WeaponInfo>());
					var counterTerroristWeapons = playerWeapons.GetOrAdd(CsTeam.CounterTerrorist, _ => new ConcurrentDictionary<int, WeaponInfo>());

					// Add weaponInfo to both team weapon dictionaries
					terroristWeapons[weaponDefIndex] = weaponInfo;
					counterTerroristWeapons[weaponDefIndex] = weaponInfo;
				}
				else
				{
					// Add to the specific team
					var teamWeapons = playerWeapons.GetOrAdd(weaponTeam, _ => new ConcurrentDictionary<int, WeaponInfo>());
					teamWeapons[weaponDefIndex] = weaponInfo;
				}

				// Console.WriteLine($"[wp] skin was added = {weaponInfo.Paint}, weaponDefIndex = {weaponDefIndex}");
				// weaponInfos[weaponDefIndex] = weaponInfo;
			}

			// WeaponPaints.GPlayerWeaponsInfo[player.Slot][weaponTeam] = weaponInfos;
		}
		catch (Exception ex)
		{
			Utility.Log($"An error occurred in GetWeaponPaintsFromDatabase: {ex.Message}, {ex.StackTrace}, {ex.InnerException}");
		}
	}

	private void GetMusicFromDatabase(PlayerInfo? player, X_Response row)
	{
		// try
		// {
		// 	if (!_config.Additional.MusicEnabled || player?.SteamId == null)
		// 		return;

		// 	const string query = "SELECT `music_id`, `weapon_team` FROM `wp_player_music` WHERE `steamid` = @steamid ORDER BY `weapon_team` ASC";
		// 	var rows = connection.Query<dynamic>(query, new { steamid = player.SteamId }); // Retrieve all records for the player

		// 	foreach (var row in rows)
		// 	{
		// 		// Check if music_id is null
		// 		if (row.music_id == null) continue;

		// 		// Determine the weapon team based on the query result
		// 		CsTeam weaponTeam = (int)row.weapon_team switch
		// 		{
		// 			2 => CsTeam.Terrorist,
		// 			3 => CsTeam.CounterTerrorist,
		// 			_ => CsTeam.None,
		// 		};

		// 		// Get or create entries for the player’s slot
		// 		var playerMusic = WeaponPaints.GPlayersMusic.GetOrAdd(player.Slot, _ => new ConcurrentDictionary<CsTeam, ushort>());

		// 		if (weaponTeam == CsTeam.None)
		// 		{
		// 			// Assign music ID to both teams if weaponTeam is None
		// 			playerMusic[CsTeam.Terrorist] = (ushort)row.music_id;
		// 			playerMusic[CsTeam.CounterTerrorist] = (ushort)row.music_id;
		// 		}
		// 		else
		// 		{
		// 			// Assign music ID to the specific team
		// 			playerMusic[weaponTeam] = (ushort)row.music_id;
		// 		}
		// 	}
		// }
		// catch (Exception ex)
		// {
		// 	Utility.Log($"An error occurred in GetMusicFromDatabase: {ex.Message}");
		// }
	}

	private void GetPinsFromDatabase(PlayerInfo? player, X_Response row)
	{
		// try
		// {
		// 	if (player?.SteamId == null)
		// 		return;

		// 	const string query = "SELECT `id`, `weapon_team` FROM `wp_player_pins` WHERE `steamid` = @steamid ORDER BY `weapon_team` ASC";
		// 	var rows = connection.Query<dynamic>(query, new { steamid = player.SteamId }); // Retrieve all records for the player

		// 	foreach (var row in rows)
		// 	{
		// 		// Check if id is null
		// 		if (row.id == null) continue;

		// 		// Determine the weapon team based on the query result
		// 		CsTeam weaponTeam = (int)row.weapon_team switch
		// 		{
		// 			2 => CsTeam.Terrorist,
		// 			3 => CsTeam.CounterTerrorist,
		// 			_ => CsTeam.None,
		// 		};

		// 		// Get or create entries for the player’s slot
		// 		var playerPins = WeaponPaints.GPlayersPin.GetOrAdd(player.Slot, _ => new ConcurrentDictionary<CsTeam, ushort>());

		// 		if (weaponTeam == CsTeam.None)
		// 		{
		// 			// Assign pin ID to both teams if weaponTeam is None
		// 			playerPins[CsTeam.Terrorist] = (ushort)row.id;
		// 			playerPins[CsTeam.CounterTerrorist] = (ushort)row.id;
		// 		}
		// 		else
		// 		{
		// 			// Assign pin ID to the specific team
		// 			playerPins[weaponTeam] = (ushort)row.id;
		// 		}
		// 	}
		// }
		// catch (Exception ex)
		// {
		// 	Utility.Log($"An error occurred in GetPinsFromDatabase: {ex.Message}");
		// }
	}

	internal async Task SyncKnifeToDatabase(PlayerInfo player, string knife, CsTeam[] teams)
	{
		//
	}

	internal async Task SyncGloveToDatabase(PlayerInfo player, ushort gloveDefIndex, CsTeam[] teams)
	{
		//
	}

	internal async Task SyncAgentToDatabase(PlayerInfo player)
	{
		//
	}

	internal async Task SyncWeaponPaintsToDatabase(PlayerInfo player)
	{
		//
	}

	internal async Task SyncMusicToDatabase(PlayerInfo player, ushort music, CsTeam[] teams)
	{
		//
	}

	internal async Task SyncPinToDatabase(PlayerInfo player, ushort pin, CsTeam[] teams)
	{
		//
	}

	internal async Task SyncStatTrakToDatabase(PlayerInfo player)
	{
		//
	}
}