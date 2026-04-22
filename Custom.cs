using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Newtonsoft.Json;

namespace Custom
{
  public static class Weapons
  {
    // Equivalent to `weaponNameToDefIndex`
    public static readonly IReadOnlyDictionary<string, int> WeaponNameToDefIndex =
      new Dictionary<string, int>
      {
        ["weapon_deagle"] = 1,
        ["weapon_elite"] = 2,
        ["weapon_fiveseven"] = 3,
        ["weapon_glock"] = 4,
        ["weapon_ak47"] = 7,
        ["weapon_aug"] = 8,
        ["weapon_awp"] = 9,
        ["weapon_famas"] = 10,
        ["weapon_g3sg1"] = 11,
        ["weapon_galilar"] = 13,
        ["weapon_m249"] = 14,
        ["weapon_m4a1"] = 16,
        ["weapon_mac10"] = 17,
        ["weapon_p90"] = 19,
        ["weapon_mp5sd"] = 23,
        ["weapon_ump45"] = 24,
        ["weapon_xm1014"] = 25,
        ["weapon_bizon"] = 26,
        ["weapon_mag7"] = 27,
        ["weapon_negev"] = 28,
        ["weapon_sawedoff"] = 29,
        ["weapon_tec9"] = 30,
        ["weapon_taser"] = 31,
        ["weapon_hkp2000"] = 32,
        ["weapon_mp7"] = 33,
        ["weapon_mp9"] = 34,
        ["weapon_nova"] = 35,
        ["weapon_p250"] = 36,
        ["weapon_scar20"] = 38,
        ["weapon_sg556"] = 39,
        ["weapon_ssg08"] = 40,

        ["weapon_m4a1_silencer"] = 60,
        ["weapon_usp_silencer"] = 61,
        ["weapon_cz75a"] = 63,
        ["weapon_revolver"] = 64,

        ["weapon_bayonet"] = 500,
        ["weapon_knife_css"] = 503,
        ["weapon_knife_flip"] = 505,
        ["weapon_knife_gut"] = 506,
        ["weapon_knife_karambit"] = 507,
        ["weapon_knife_m9_bayonet"] = 508,
        ["weapon_knife_tactical"] = 509,
        ["weapon_knife_falchion"] = 512,
        ["weapon_knife_survival_bowie"] = 514,
        ["weapon_knife_butterfly"] = 515,
        ["weapon_knife_push"] = 516,
        ["weapon_knife_cord"] = 517,
        ["weapon_knife_canis"] = 518,
        ["weapon_knife_ursus"] = 519,
        ["weapon_knife_gypsy_jackknife"] = 520,
        ["weapon_knife_outdoor"] = 521,
        ["weapon_knife_stiletto"] = 522,
        ["weapon_knife_widowmaker"] = 523,
        ["weapon_knife_skeleton"] = 525,
        ["weapon_knife_kukri"] = 526,
      };
  }

  public sealed class CustomWeapon
  {
    public required int Id { get; init; }
    public required int DefIndex { get; init; }
    public required bool IsKnife { get; init; }
    public string? Model { get; init; }
    public string? Subclass { get; init; }
  }

  public static class CustomWeaponsConfig
  {
    public static readonly List<CustomWeapon> CustomWeapons =
    [
      new()
      {
        Id = 100,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_bayonet"],
        IsKnife = true,
        Model = "weapons/nozb1/knife/blooming_rose/blooming_rose_ag2.vmdl"
      },
      new()
      {
        Id = 101,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_ak47"],
        IsKnife = false,
        Model = "weapons/nozb1/ak4x/weapon_ak4x_ag2.vmdl"
      },
      new()
      {
        Id = 200,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_ak47"],
        IsKnife = false,
        Model = "weapons/nozb1/luna_pack/ak47/ak47_ag2.vmdl"
      },
      new()
      {
        Id = 201,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_bayonet"],
        IsKnife = true,
        Model = "weapons/nozb1/luna_pack/v_knife/v_knife_ag2.vmdl"
      },
      new()
      {
        Id = 202,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_m4a1"],
        IsKnife = false,
        Model = "weapons/nozb1/luna_pack/v_mp5/v_mp5_ag2.vmdl"
      },
      new()
      {
        Id = 203,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_ssg08"],
        IsKnife = false,
        Model = "weapons/nozb1/luna_pack/v_ssg/v_ssg_ag2.vmdl"
      },
      new()
      {
        Id = 204,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_usp_silencer"],
        IsKnife = false,
        Model = "weapons/nozb1/luna_pack/v_usp/v_usp_ag2.vmdl"
      },

      new()
      {
        Id = 300,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_awp"],
        IsKnife = false,
        Model = "weapons/models/moovik/awpwhite/awp_white_ag2.vmdl",
        Subclass = "white"
      },
      new()
      {
        Id = 301,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_awp"],
        IsKnife = false,
        Model = "weapons/models/moovik/awplegenda/awp_legenda_ag2.vmdl",
        Subclass = "legenda"
      },

      new()
      {
        Id = 302,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_ak47"],
        IsKnife = false,
        Model = "weapons/models/moovik/ak47vandal/ak47_vandal_ag2.vmdl",
        Subclass = "vandal"
      },
      new()
      {
        Id = 303,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_knife_karambit"],
        IsKnife = true,
        Model = "weapons/models/moovik/apex_karambit/ka_apex02_ag2.vmdl",
        Subclass = "apex02"
      },
      new()
      {
        Id = 304,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_knife_karambit"],
        IsKnife = true,
        Model = "weapons/models/moovik/apex_karambit/ka_apex04_ag2.vmdl",
        Subclass = "apex04"
      },
      new()
      {
        Id = 305,
        DefIndex = Weapons.WeaponNameToDefIndex["weapon_m4a1"],
        IsKnife = false,
        Model = "weapons/models/moovik/m4a1ruin/m4a1_ruin_ag2.vmdl",
        Subclass = "ruin"
      },



			// new()
			// {
			// 	Id = 301,
			// 	DefIndex = Weapons.WeaponNameToDefIndex["weapon_knife_css"],
			// 	Model = "models/pickaxe/pickaxe.vmdl"
			// },
			// new()
			// {
			// 	Id = 303,
			// 	DefIndex = Weapons.WeaponNameToDefIndex["weapon_bayonet"],
			// 	Model = "weapons/nozb1/knife/baseball_batlow/baseball_batlow.vmdl"
			// },
			// new()
			// {
			// 	Id = 304,
			// 	DefIndex = Weapons.WeaponNameToDefIndex["weapon_knife_m9_bayonet"],
			// 	Model = "weapons/nozb1/knife/blaine_spineedge/blaine_spineedge.vmdl"
			// },
			// new()
			// {
			// 	Id = 305,
			// 	DefIndex = Weapons.WeaponNameToDefIndex["weapon_knife_kukri"],
			// 	Model = "weapons/nozb1/knife/cudgel/cudgel.vmdl"
			// },
			// new()
			// {
			// 	Id = 306,
			// 	DefIndex = Weapons.WeaponNameToDefIndex["weapon_bayonet"],
			// 	Model = "weapons/nozb1/knife/fantasy_dagger/fantasy_dagger.vmdl"
			// },
			// new()
			// {
			// 	Id = 307,
			// 	DefIndex = Weapons.WeaponNameToDefIndex["weapon_knife_kukri"],
			// 	Model = "weapons/nozb1/knife/morrowind/morrowind.vmdl"
			// },
			// new()
			// {
			// 	Id = 308,
			// 	DefIndex = Weapons.WeaponNameToDefIndex["weapon_bayonet"],
			// 	Model = "weapons/nozb1/knife/wooden_sword/wooden_sword.vmdl"
			// },
		];
  }


  public class Manager
  {
    public static readonly Storage.Storage<Dictionary<int, CustomWeapon>> CustomData = new("custom_data", [Storage.EConfigFlag.ClearOnQuit]);

    public static void SetCustomData(int UserId, int[]? custom)
    {
      //int[] custom = { /* 301, 302, 303, 305*/  300, 304 };
      CustomData.RemoveData(UserId);
      //if (json == null || string.IsNullOrEmpty(json)) return;
      //var list = ParseIntList(json);
      if (custom == null) return;
      if (custom.Length == 0) return;

      var dict = new Dictionary<int, CustomWeapon>();
      foreach (var id in custom)
      {
        var customWeapon = CustomWeaponsConfig.CustomWeapons.Find(w => w.Id == id);
        if (customWeapon == null) continue;
        dict[customWeapon.DefIndex] = customWeapon;
        //Console.WriteLine($"Defindex {customWeapon.DefIndex}, {id}");
      }
      CustomData.SetData(UserId, dict);
    }

    public static void Initialize(BasePlugin plugin)
    {
      plugin.RegisterListener<Listeners.OnServerPrecacheResources>((manifest) =>
            {
              var allModels = CustomWeaponsConfig.CustomWeapons.Select(w => w.Model);
              foreach (var path in allModels)
              {
                if (path == null) continue;
                manifest.AddResource(path);
              }
            });
    }

    // Code from xpack, is not needed right now.

    // public static void Initialize(/* BasePlugin plugin */)
    // {
    //   // plugin.RegisterEventHandler<EventItemEquip>((@event) =>
    //   // {

    //   // });

    //   // plugin.AddCommand("wp2", "", (player, info) =>
    //   // {
    //   //   if (!Storage.Manager.IsValid(player)) return;
    //   //   if (player.PlayerPawn?.Value?.WeaponServices == null || player.PlayerPawn.Value.ItemServices == null)
    //   //     return;

    //   //   var weapons = player.PlayerPawn.Value.WeaponServices.MyWeapons;

    //   //   if (weapons.Count == 0)
    //   //     return;
    //   //   if (player.Team is CsTeam.None or CsTeam.Spectator)
    //   //     return;

    //   //   player.PrintToChat($" >> Fix Count: {weapons.Count}");

    //   //   foreach (var weapon in weapons)
    //   //   {
    //   //     if (!weapon.IsValid || weapon.Value == null ||
    //   //       !weapon.Value.IsValid || !weapon.Value.DesignerName.Contains("weapon_"))
    //   //       continue;

    //   //     CCSWeaponBaseGun gun = weapon.Value.As<CCSWeaponBaseGun>();

    //   //     if (weapon.Value.Entity == null) continue;
    //   //     if (!weapon.Value.OwnerEntity.IsValid) continue;
    //   //     if (gun.Entity == null) continue;
    //   //     if (!gun.IsValid) continue;

    //   //     player.PrintToChat($" >> Fix {weapon.Value.DesignerName}");
    //   //     GivePlayerWeaponSkin(player, gun);

    //   //   }
    //   // });


    //   // plugin.AddCommand("xc", "", (player, info) =>
    //   // {
    //   //   if (!Storage.Manager.IsValid(player)) return;
    //   // 	var json = info.ArgString;
    //   // 	if (json == null || string.IsNullOrEmpty(json)) return;
    //   // 	var list = ParseIntList(json);

    //   // 	var dict = new Dictionary<int, CustomWeapon>();
    //   // 	foreach (var id in list ?? [])
    //   // 	{
    //   // 		var customWeapon = CustomWeaponsConfig.CustomWeapons.Find(w => w.Id == id);
    //   // 		if (customWeapon == null) continue;
    //   // 		dict[customWeapon.DefIndex] = customWeapon;
    //   // 		player.PrintToChat($"ID = {id}");
    //   // 	}
    //   // 	CustomData.Clear();
    //   // 	CustomData.SetData(player, dict);

    //   // });

    //   // plugin.RegisterListener<Listeners.OnEntityCreated>((entity) =>
    //   // {
    //   //   if (entity == null || entity.Entity == null || !entity.IsValid || !entity.DesignerName.Contains("weapon_"))
    //   //     return;
    //   //   // make sure the entity pointer is valid
    //   //   if (entity.Entity.Handle == IntPtr.Zero)
    //   //     return;
    //   //   //CBasePlayerWeapon weapon = entity.As<CBasePlayerWeapon>();

    //   //   Server.NextWorldUpdate(() =>
    //   //   {
    //   //     var weapon = new CBasePlayerWeapon(entity.Handle);
    //   //     if (!weapon.IsValid) return;

    //   //     try
    //   //     {
    //   //       SteamID? steamid = null;

    //   //       if (weapon.OriginalOwnerXuidLow > 0)
    //   //         steamid = new SteamID(weapon.OriginalOwnerXuidLow);

    //   //       CCSPlayerController? player;

    //   //       if (steamid != null && steamid.IsValid())
    //   //       {
    //   //         player = Storage.Manager.GetPlayerBySteamID64(steamid.SteamId64);

    //   //         if (player == null)
    //   //           player = Utilities.GetPlayerFromSteamId(weapon.OriginalOwnerXuidLow);
    //   //       }
    //   //       else
    //   //       {
    //   //         CCSWeaponBaseGun gun = weapon.As<CCSWeaponBaseGun>();
    //   //         player = Utilities.GetPlayerFromIndex((int)weapon.OwnerEntity.Index) ?? Utilities.GetPlayerFromIndex((int)gun.OwnerEntity.Value!.Index);
    //   //       }

    //   //       if (string.IsNullOrEmpty(player?.PlayerName)) return;
    //   //       if (!Storage.Manager.IsValid(player)) return;

    //   //       GivePlayerWeaponSkin2(player, weapon);
    //   //     }
    //   //     catch (Exception)
    //   //     {
    //   //     }
    //   //   });
    //   // });
    // }

    public static void GivePlayerWeaponSkin2(CCSPlayerController player, CBasePlayerWeapon weapon)
    {
      try
      {
        var defIndex = weapon.AttributeManager.Item.ItemDefinitionIndex;
        var data = CustomData.GetData(player);
        if (data?.TryGetValue(defIndex, out var customWeapon) == true && customWeapon != null)
        {
          weapon.PrivateVScripts = ".";
          //player.PrintToChat($" >> Setting model for #{customWeapon.Id}");
          if (!string.IsNullOrEmpty(customWeapon.Model))
          {
            //player.PrintToChat($" >> Setting Model for #{customWeapon.Id} = {customWeapon.Model}");
            weapon.SetModel(customWeapon.Model);
          }
          if (!string.IsNullOrEmpty(customWeapon.Subclass))
          {
            //player.PrintToChat($" >> Setting ChangeSubclass for #{customWeapon.Id} = {customWeapon.Subclass}");
            Server.NextFrame(() => weapon.AcceptInput("ChangeSubclass", value: customWeapon.Subclass));
          }


        }
      }
      catch (Exception) { }
    }



    public static List<int>? ParseIntList(string json)
    {
      try
      {
        return JsonConvert.DeserializeObject<List<int>>(json);
      }
      catch
      {
        return null;
      }
    }

  }
}