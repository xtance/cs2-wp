using System.Text;
using System.Text.Json;

namespace WeaponPaints
{
	/* 
		set of patches for xpro.pw project
		since we don't use mysql
	 */

public class X_StickerSchema
{
	public required int? sticker_id;
    public required float? x;
    public required float? y;
    public required float? rotation;
    public required float? negativeRotation;
    public required float? wear;
    public required int? slot;
    public required float? scale;
}

    public class X_PlayerSkin
    {
        public required int? weapon_defindex;
        public required int? weapon_paint_id;
        public required float? weapon_wear;
        public required int? weapon_seed;
        public required string? weapon_nametag;
        public required bool? weapon_stattrak;
        public required int? weapon_stattrak_count;
        public required string? weapon_keychain;
		public required List<X_StickerSchema>? weapon_stickers;
    }

    public class X_Response
	{
		public required string? knife;
		public required int? weapon_defindex_glove;
		public required List<X_PlayerSkin>? playerSkins;
	}

	public class X_RawResponse
	{
		public required string text;
		public required X_Response data; 
	}

	public static class X_Hook
	{
		private static readonly HttpClient _httpClient = new();

		public static async Task<X_Response?> FetchSkins(int steamid)
	    {
			Console.WriteLine($"[wp] FetchSkins => {steamid}");
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
				//Console.WriteLine($"[wp]  {steamid} responseBody => {responseBody}");
		        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<X_RawResponse>(responseBody);

				//Console.WriteLine($"[wp]  {steamid} text = {obj?.text}, back = {Newtonsoft.Json.JsonConvert.SerializeObject(obj)}");
				return obj?.data ?? null;
			}
			catch (Exception e)
			{
				Console.WriteLine($"[wp] exception at {e}");
				return null;
			}
	    }
	}

	public class Database {};
}