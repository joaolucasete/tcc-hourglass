using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hourglass.Api.Service;

namespace Hourglass.Api.User {
	public class UserModel {
		public Guid Id { get; set; }
		public string Cpf { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Street { get; set; }
		public string Number { get; set; }
		public string Complement { get; set; }
		public string Neighborhood { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }

		public List<ServiceSummary> Services { get; set; }
		public Guid? PictureUploadId { get; set; }


		//public List<ConsumedService> ConsumedServices { get; set; }
	}
}
