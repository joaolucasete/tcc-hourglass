using Hourglass.Site.Classes;
using Hourglass.Site.Entities;
using Hourglass.Site.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace Hourglass.Site.Services;

public interface IServiceService {
	Task<List<Service>> GetServicesAsync();
	Task<Service> GetServiceAsync(Guid id);
	Task<Service> CreateServiceAsync(ServiceCreateRequest request);
	Task<Service> UpdateService(Guid id, Service service);
	Task<bool> DeleteService(Guid id);
}

public class ServiceService : IServiceService {
	private readonly AppDbContext context;

	public ServiceService(AppDbContext context) {
		this.context = context;
	}

	public async Task<List<Service>> GetServicesAsync() {
		return await context.Services
			.Include(s => s.ServiceCategory)
			.Include(s => s.User)
			.ToListAsync();
	}

	public async Task<Service> GetServiceAsync(Guid id) {
		return await context.Services
			.Include(s => s.ServiceCategory)
			.Include(s => s.User)
			.FirstOrDefaultAsync(s => s.Id == id);
	}

	public async Task<Service> CreateServiceAsync(ServiceCreateRequest request) {
		if (request.ServiceCategoryId == null) {
			request.ServiceCategoryId = Util.GetDefaultServiceCategoryGuid();
		}
		var service = new Service {
			Id = Guid.NewGuid(),
			Name = request.Name,
			Description = request.Description,
			Price = request.Price,
			ContactLink = request.ContactLink,
			ServiceCategoryId = request.ServiceCategoryId.Value,
			UserId = request.UserId,
		};

		context.Services.Add(service);
		await context.SaveChangesAsync();
		return service;
	}

	public async Task<Service> UpdateService(Guid id, Service service) {
		context.Entry(service).State = EntityState.Modified;
		
		return service;
	}

	public async Task<bool> DeleteService(Guid id) {
		var service = await context.Services.FindAsync(id);
		if (service == null) {
			return false;
		}

		context.Set<Service>().Remove(service);
		await context.SaveChangesAsync();
		return true;
	}
}

