using Microsoft.AspNetCore.Mvc;
using Hourglass.Site.Entities;
using Hourglass.Site.Services;
using Hourglass.Api.Service;

namespace Hourglass.Site.Controllers;

[Route("api/v1/services")]
[ApiController]
public class ServiceController : ControllerBase {
	private readonly ServiceService serviceService;
	private readonly ModelConverter mc;

	public ServiceController(
		ServiceService serviceService,
		ModelConverter mc
	) {
		this.serviceService = serviceService;
		this.mc = mc;
	}

	// GET: api/v1/services
	[HttpGet]
	public async Task<List<ServiceSummary>> GetServices() {
		var services = await serviceService.GetServices();
		return services.Select(mc.ToSummary).ToList();
	}

	// GET: api/v1/services/:id
	[HttpGet("{id}")]
	public async Task<ActionResult<Service>> GetService(Guid id) {
		var service = await serviceService.GetService(id);
		if (service == null) {
			return NotFound();
		}
		return Ok(service);
	}

	// POST: api/v1/services
	[HttpPost]
	public async Task<ActionResult<Service>> CreateService(Service service) {
		var createdService = await serviceService.CreateService(service);
		return CreatedAtAction(nameof(GetService), new { id = createdService.Id }, createdService);
	}

	// PUT: api/v1/services/:id
	[HttpPut("{id}")]
	public async Task<ActionResult<Service>> UpdateService(Guid id, Service service) {
		try {
			var updatedService = await serviceService.UpdateService(id, service);
			return Ok(updatedService);
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

