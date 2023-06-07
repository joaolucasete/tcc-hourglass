using Hourglass.Site.Entities;
using Hourglass.Site.Models.ServiceCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Hourglass.Site.Classes.Dtos;

namespace Hourglass.Site.Controllers {
	[Route("api/v1/service-categories")]
	[ApiController]
	public class ServiceCategoryController : ControllerBase {
		private readonly AppDbContext dbContext;

		public ServiceCategoryController(
				AppDbContext dbContext
			) {
			this.dbContext = dbContext;
		}

		// GET: api/v1/service-categories
		// Get all the service categories
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ServiceCategoryDto>>> GetServiceCategories() {
			// Use LINQ to project the entities to DTOs
			var serviceCategories = await dbContext.ServiceCategories
				.Select(sc => new ServiceCategoryDto {
					Id = sc.Id,
					Name = sc.Name,
					CreatedAt = sc.CreatedAt,
					UpdatedAt = sc.UpdatedAt
				})
				.ToListAsync();

			return Ok(serviceCategories);
		}

		// GET: api/v1/service-categories/{id}
		// Get a specific service category by id
		[HttpGet("{id}")]
		public async Task<ActionResult<ServiceCategoryDto>> GetServiceCategory(Guid id) {
			// Find the service category entity by id
			var serviceCategory = await dbContext.ServiceCategories.FindAsync(id);

			// Return 404 if not found
			if (serviceCategory == null) {
				return NotFound();
			}

			// Map the entity to a DTO
			var serviceCategoryDto = new ServiceCategoryDto {
				Id = serviceCategory.Id,
				Name = serviceCategory.Name,
				CreatedAt = serviceCategory.CreatedAt,
				UpdatedAt = serviceCategory.UpdatedAt
			};

			return Ok(serviceCategoryDto);
		}

		// POST: api/v1/service-categories
		// Create a new service category
		[HttpPost]
		public async Task<ActionResult> CreateServiceCategory(ServiceCategoryCreateRequest request) {

			// Map the DTO to an entity
			var serviceCategory = new ServiceCategory {
				Id = Guid.NewGuid(),
				Name = request.Name,
			};

			// Add the entity to the database context
			dbContext.ServiceCategories.Add(serviceCategory);

			// Save the changes to the database
			await dbContext.SaveChangesAsync();

			// Return the created DTO with 201 status code and location header
			return CreatedAtAction(nameof(GetServiceCategory), new { id = serviceCategory.Id });
		}

		// PUT: api/v1/service-categories/{id}
		// Update an existing service category by id
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateServiceCategory(Guid id, ServiceCategoryUpdateRequest request) {
			// Find the service category entity by id
			var serviceCategory = await dbContext.ServiceCategories.FindAsync(id);

			// Return 404 if not found
			if (serviceCategory == null) {
				return NotFound();
			}

			if (!string.IsNullOrEmpty(request.Name)) {
				serviceCategory.Name = request.Name;
			}

			// Update the timestamp
			serviceCategory.UpdatedAt = DateTimeOffset.UtcNow;

			// Save the changes to the database
			await dbContext.SaveChangesAsync();

			// Return 204 status code to indicate success
			return NoContent();
		}

		// DELETE: api/v1/service-categories/{id}
		// Delete an existing service category by id
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteServiceCategory(Guid id) {
			// Find the service category entity by id
			var serviceCategory = await dbContext.ServiceCategories.FindAsync(id);

			// Return 404 if not found
			if (serviceCategory == null) {
				return NotFound();
			}

			// Remove the entity from the database context
			dbContext.ServiceCategories.Remove(serviceCategory);

			// Save the changes to the database
			await dbContext.SaveChangesAsync();

			// Return 204 status code to indicate success
			return NoContent();
		}
	}
}
