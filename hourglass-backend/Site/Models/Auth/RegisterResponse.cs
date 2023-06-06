using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hourglass.Api.Auth {
	public class RegisterResponse {
		public bool IsSuccessfulRegistration { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}
