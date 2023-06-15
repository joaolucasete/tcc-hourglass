using Hourglass.Site.Classes;
using Hourglass.Site.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hourglass.Site.Controllers {
	[Route("api/v1/uploads")]
	[ApiController]
	public class UploadController : ControllerBase {
		private readonly AppDbContext context;

		public UploadController(
			AppDbContext context
		) {
			this.context = context;
		}

		[Authorize]
		[DisableFormValueModelBinding]
		[HttpPost]
		public async Task<IActionResult> Upload(IFormFile fileTest) {
			var a = fileTest;
			var picture = Request.Form.Files.FirstOrDefault();

			var upload = new Upload {
				Id = Guid.NewGuid(),
			};

			if (picture?.Length > 0) {
				using var memoryStream = new MemoryStream();
				await picture.CopyToAsync(memoryStream);
				if (memoryStream.Length < 2_097_152) {
					upload.Value = memoryStream.ToArray();
				} else {
					return BadRequest("Picture file is too large");
				}
			}

			context.Uploads.Add(upload);
			await context.SaveChangesAsync();

			return Ok(new {
				uploadId = upload.Id
			});
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id) {
			var upload = context.Uploads.FirstOrDefault(u => u.Id == id);
			if (upload == null) {
				return NotFound();
			}
			return File(upload.Value, "image/png");
		}
	}
}
