using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace Hourglass.Api {

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class CpfAttribute : ValidationAttribute {
		protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
			if (string.IsNullOrEmpty(value?.ToString())) {
				return ValidationResult.Success;
			}

			if (!Validations.IsValidCpf(value.ToString())) {
				return new ValidationResult(base.ErrorMessage ?? "Invalid CPF");
			}

			return ValidationResult.Success;
		}
	}
}
