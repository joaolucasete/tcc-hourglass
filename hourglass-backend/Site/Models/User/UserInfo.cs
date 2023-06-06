using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hourglass.Api.User;
public class UserInfo {
	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("claims")]
	public Dictionary<string, string> Claims { get; set; }
}

