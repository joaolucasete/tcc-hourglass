using System.Text.RegularExpressions;

namespace Hourglass.Api {
	public static class Validations {
		public static bool IsValidCpf(string cpf, bool ignorePunctuation = true) {
			if (string.IsNullOrWhiteSpace(cpf)) {
				return true;
			}

			cpf = cpf.Trim();
			List<int> digits;
			if (ignorePunctuation) {
				cpf = cpf.RemovePunctuation();
				if (!Regex.IsMatch(cpf, "^\\d{11}$")) {
					return false;
				}

				digits = (from c in cpf.ToCharArray()
						  select int.Parse(c.ToString())).ToList();
			} else if (Regex.IsMatch(cpf, "^\\d{11}$")) {
				digits = (from c in cpf.ToCharArray()
						  select int.Parse(c.ToString())).ToList();
			} else {
				if (!Regex.IsMatch(cpf, "^\\d{3}\\.\\d{3}\\.\\d{3}-\\d{2}$")) {
					return false;
				}

				digits = (from c in cpf.Replace(".", "").Replace("-", "").ToCharArray()
						  select int.Parse(c.ToString())).ToList();
			}

			var num = (from i in Enumerable.Range(0, 9)
					   select digits[i] * (10 - i)).Sum();
			var num2 = 11 - num % 11;
			if (num2 >= 10) {
				num2 = 0;
			}

			if (digits[9] != num2) {
				return false;
			}

			var num3 = (from i in Enumerable.Range(0, 10)
						select digits[i] * (11 - i)).Sum();
			var num4 = 11 - num3 % 11;
			if (num4 >= 10) {
				num4 = 0;
			}

			if (digits[10] != num4) {
				return false;
			}

			return true;
		}

		public static string RemovePunctuation(this string s) {
			return new string((from c in s.ToCharArray()
							   where !char.IsPunctuation(c)
							   select c).ToArray());
		}
	}
}
