using Microsoft.AspNetCore.Mvc;
using Hourglass.Site.Entities;
using Hourglass.Site.Services;
using Hourglass.Api.Service;
using Hourglass.Site.Models.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hourglass.Site.Controllers;

[Route("api/v1/services")]
[ApiController]
public class ServiceController : ControllerBase {
	private readonly ServiceService serviceService;
	private readonly ModelConverter mc;
	private readonly AppDbContext appDbContext;

	public ServiceController(
		ServiceService serviceService,
		ModelConverter mc,
		AppDbContext appDbContext
	) {
		this.serviceService = serviceService;
		this.mc = mc;
		this.appDbContext = appDbContext;
	}

	// GET: api/v1/services
	[HttpGet]
	public async Task<List<ServiceSummary>> GetServices() {
		var services = await serviceService.GetServicesAsync();
		return services.Select(mc.ToSummary).ToList();
	}

	// GET: api/v1/services/:id
	[HttpGet("{id}")]
	public async Task<ActionResult<ServiceSummary>> GetService(Guid id) {
		var service = await serviceService.GetServiceAsync(id);
		if (service == null) {
			return NotFound();
		}
		return mc.ToSummary(service);
	}

	// POST: api/v1/services
	[HttpPost]
	public async Task<ActionResult> CreateService(ServiceCreateRequest request) {
		var createdService = await serviceService.CreateServiceAsync(request);
		return NoContent();
	}

	// PUT: api/v1/services/:id
	[HttpPut("{id}")]
	public async Task<ActionResult> UpdateService(Guid id, ServiceUpdateRequest request) {
		try {
			var service = await appDbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
			if (service == null) {
				return NotFound();
			}

			if (!string.IsNullOrEmpty(request.Name)) {
				service.Name = request.Name;
			}
			if (request.Price.HasValue) {
				service.Price = request.Price.Value;
			}
			if (!string.IsNullOrEmpty(request.ContactLink)) {
				service.ContactLink = request.ContactLink;
			}

			service.UpdatedAt = DateTimeOffset.UtcNow;

			await appDbContext.SaveChangesAsync();

			return NoContent();
		} catch (ArgumentException ex) {
			return BadRequest(ex.Message);
		}
	}

	// DELETE: api/v1/services/:id
	[HttpDelete("{id}")]
	public async Task<ActionResult<bool>> DeleteService(Guid id) {
		var result = await serviceService.DeleteService(id);
		if (!result) {
			return NotFound();
		}
		return Ok(result);
	}
}

