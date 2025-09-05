using System.Text;
using System.Text.Json;

namespace WeaponPaints
{
	/* 
		set of patches for xpro.pw project
		since we don't use mysql
	 */


    public class X_PlayerSkin
    {
        internal int? weapon_defindex;
        internal int? weapon_paint_id;
        internal float? weapon_wear;
        internal int? weapon_seed;
        internal string? weapon_nametag;
        internal bool? weapon_stattrak;
        internal int? weapon_stattrak_count;
        internal string[]? weapon_keychain;
    }

    public class X_Response
	{
		public string? knife;
		public int? weapon_defindex_glove;
		public List<X_PlayerSkin>? playerSkins;
	}

	public static class X_Hook
	{
		private static readonly HttpClient _httpClient = new();

		public static async Task<X_Response?> FetchSkins(int steamid)
	    {
			try
			{
				var url = "https://xpro.pw/api/ws/plugin";
		        var json = JsonSerializer.Serialize(new
		        {
		            steamid
		        });

		        var content = new StringContent(json, Encoding.UTF8, "application/json");
		        HttpResponseMessage response = await _httpClient.PostAsync(url, content);
		        response.EnsureSuccessStatusCode();

		        string responseBody = await response.Content.ReadAsStringAsync();
		        return JsonSerializer.Deserialize<X_Response>(responseBody);
			}
			catch (Exception e)
			{
				return default;
			}
	    }
	}

	public class Database {};
}