using Hourglass.Api.Review;
using Hourglass.Site.Entities;
using Hourglass.Site.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Hourglass.Site.Classes.Dtos;

namespace Hourglass.Site.Controllers {
	[Route("api/v1/reviews")]
	[ApiController]
	public class ReviewController : ControllerBase {
		private readonly AppDbContext dbContext;
		private readonly ModelConverter mc;

		public ReviewController(
			AppDbContext dbContext,
			ModelConverter mc
		) {
			this.dbContext = dbContext;
			this.mc = mc;
		}

		// GET: api/v1/reviews
		[HttpGet]
		public async Task<ActionResult<List<ReviewSummary>>> GetReviews() {
			var reviews = await dbContext.Reviews.ToListAsync();

			return reviews.Select(r => mc.ToSummary(r)).ToList();
		}

		// GET: api/reviews/{userId}/{consumedServiceId}
		// Get a specific review by composite key
		[HttpGet("{userId}/{consumedServiceId}")]
		public async Task<ActionResult<ReviewDto>> GetReview(Guid userId, Guid consumedServiceId) {
			// Find the review entity by composite key
			var review = await dbContext.Reviews.FindAsync(userId, consumedServiceId);

			// Return 404 if not found
			if (review == null) {
				return NotFound();
			}

			// Map the entity to a DTO
			var reviewDto = new ReviewDto {
				UserId = review.UserId,
				ConsumedServiceId = review.ConsumedServiceId,
				Comment = review.Comment,
				Rating = review.Rating
			};

			return Ok(reviewDto);
		}

		// POST: api/reviews
		// Create a new review
		[HttpPost]
		public async Task<ActionResult<ReviewDto>> CreateReview(ReviewDto reviewDto) {
			// Validate the input DTO
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			// Map the DTO to an entity
			var review = new Review {
				UserId = reviewDto.UserId,
				ConsumedServiceId = reviewDto.ConsumedServiceId,
				Comment = reviewDto.Comment,
				Rating = reviewDto.Rating
			};

			// Add the entity to the database context
			dbContext.Reviews.Add(review);

			// Save the changes to the database
			await dbContext.SaveChangesAsync();

			// Return the created DTO with 201 status code and location header
			return CreatedAtAction(nameof(GetReview), new { userId = review.UserId, consumedServiceId = review.ConsumedServiceId }, reviewDto);
		}

		// PUT: api/reviews/{userId}/{consumedServiceId}
		// Update an existing review by composite key
		[HttpPut("{userId}/{consumedServiceId}")]
		public async Task<IActionResult> UpdateReview(Guid userId, Guid consumedServiceId, ReviewDto reviewDto) {
			// Validate the input DTO
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			// Find the review entity by composite key
			var review = await dbContext.Reviews.FindAsync(userId, consumedServiceId);

			// Return 404 if not found
			if (review == null) {
				return NotFound();
			}

			// Map the DTO to the entity
			review.Comment = reviewDto.Comment;
			review.Rating = reviewDto.Rating;

			// Save the changes to the database
			await dbContext.SaveChangesAsync();

			// Return 204 status code to indicate success
			return NoContent();
		}


		// DELETE: api/reviews/{userId}/{consumedServiceId}
		// Delete an existing review by composite key
		[HttpDelete("{userId}/{consumedServiceId}")]
		public async Task<IActionResult> DeleteReview(Guid userId, Guid consumedServiceId) {
			// Find the review entity by composite key
			var review = await dbContext.Reviews.FindAsync(userId, consumedServiceId);

			// Return 404 if not found
			if (review == null) {
				return NotFound();
			}

			// Remove the entity from the database context
			dbContext.Reviews.Remove(review);

			// Save the changes to the database
			await dbContext.SaveChangesAsync();

			// Return 204 status code to indicate success
			return NoContent();
		}
	}
}
