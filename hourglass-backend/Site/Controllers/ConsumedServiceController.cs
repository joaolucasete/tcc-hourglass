using Hourglass.Site.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Hourglass.Site.Classes.Dtos;

namespace Hourglass.Site.Controllers {
	[Route("api/v1/consumed-services")]
	[ApiController]
	public class ConsumedServiceController : ControllerBase {
		private readonly AppDbContext dbContext;

		public ConsumedServiceController(
			AppDbContext dbContext
		) {
			this.dbContext = dbContext;
		}

		// GET: api/v1/consumed-services
		// Get all the consumed services
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ConsumedServiceDto>>> GetConsumedServices() {
			// Use LINQ to project the entities to DTOs
			var consumedServices = await dbContext.ConsumedServices
				.Select(cs => new ConsumedServiceDto {
					UserId = cs.UserId,
					ServiceId = cs.ServiceId,
					Date = cs.Date,
					StartTime = cs.StartTime,
					EndTime = cs.EndTime,
					Observations = cs.Observations,
					Status = cs.Status
				})
				.ToListAsync();

			return Ok(consumedServices);
		}

		// GET: api/v1/consumed-services/{userId}/{serviceId}/{date}
		// Get a specific consumed service by composite key
		[HttpGet("{userId}/{serviceId}/{date}")]
		public async Task<ActionResult<ConsumedServiceDto>> GetConsumedService(Guid userId, Guid serviceId, DateTime date) {
			// Find the consumed service entity by composite key
			var consumedService = await dbContext.ConsumedServices.FindAsync(userId, serviceId, date);

			// Return 404 if not found
			if (consumedService == null) {
				return NotFound();
			}

			// Map the entity to a DTO
			var consumedServiceDto = new ConsumedServiceDto {
				UserId = consumedService.UserId,
				ServiceId = consumedService.ServiceId,
				Date = consumedService.Date,
				StartTime = consumedService.StartTime,
				EndTime = consumedService.EndTime,
				Observations = consumedService.Observations,
				Status = consumedService.Status
			};

			return Ok(consumedServiceDto);
		}

		// POST: api/v1/consumed-services
		// Create a new consumed service
		[HttpPost]
		public async Task<ActionResult<ConsumedServiceDto>> CreateConsumedService(ConsumedServiceDto consumedServiceDto) {
			// Validate the input DTO
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			// Map the DTO to an entity
			var consumedService = new ConsumedService {
				UserId = consumedServiceDto.UserId,
				ServiceId = consumedServiceDto.ServiceId,
				Date = consumedServiceDto.Date,
				StartTime = consumedServiceDto.StartTime,
				EndTime = consumedServiceDto.EndTime,
				Observations = consumedServiceDto.Observations,
				Status = consumedServiceDto.Status
			};

			// Add the entity to the database context
			dbContext.ConsumedServices.Add(consumedService);

			// Save the changes to the database
			await dbContext.SaveChangesAsync();

			// Return the created DTO with 201 status code and location header
			return CreatedAtAction(nameof(GetConsumedService), new { userId = consumedService.UserId, serviceId = consumedService.ServiceId, date = consumedService.Date }, consumedServiceDto);
		}

		// PUT: api/v1/consumed-services/{userId}/{serviceId}/{date}
		// Update an existing consumed service by composite key
		[HttpPut("{userId}/{serviceId}/{date}")]
		public async Task<IActionResult> UpdateConsumedService(Guid userId, Guid serviceId, DateTime date, ConsumedServiceDto consumedServiceDto) {
			// Validate the input DTO
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			// Find the consumed service entity by composite key
			var consumedService = await dbContext.ConsumedServices.FindAsync(userId, serviceId, date);

			// Return 404 if not found
			if (consumedService == null) {
				return NotFound();
			}

			// Map the DTO to the entity
			consumedService.StartTime = consumedServiceDto.StartTime;
			consumedService.EndTime = consumedServiceDto.EndTime;
			consumedService.Observations = consumedServiceDto.Observations;
			consumedService.Status = consumedServiceDto.Status;

			// Save the changes to the database
			await dbContext.SaveChangesAsync();

			// Return 204 status code to indicate success
			return NoContent();
		}

		// DELETE: api/v1/consumed-services/{userId}/{serviceId}/{date}
		// Delete an existing consumed service by composite key
		[HttpDelete("{userId}/{serviceId}/{date}")]
		public async Task<IActionResult> DeleteConsumedService(Guid userId, Guid serviceId, DateTime date) {
			// Find the consumed service entity by composite key
			var consumedService = await dbContext.ConsumedServices.FindAsync(userId, serviceId, date);

			// Return 404 if not found
			if (consumedService == null) {
				return NotFound();
			}

			// Remove the entity from the database context
			dbContext.ConsumedServices.Remove(consumedService);

			// Save the changes to the database
			await dbContext.SaveChangesAsync();

			// Return 204 status code to indicate success
			return NoContent();
		}

	}
}
