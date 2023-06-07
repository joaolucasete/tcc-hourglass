using Hourglass.Site.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hourglass.Site.Classes;
public class Util {
	public static string ConvertToHexString(byte[] content)
		=> string.Join("", content.Select(b => b.ToString("x2")));

	public static byte[] DecodeHexString(string hex)
		=> Enumerable.Range(0, hex.Length)
						 .Where(x => x % 2 == 0)
						 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
						 .ToArray();

	public static Guid GetDefaultServiceCategoryGuid()
		=> Guid.Parse("a2f8291f-85c6-4e1c-a70f-27763eaeeda3");
}

