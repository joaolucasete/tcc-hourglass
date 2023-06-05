using Hourglass.Site.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hourglass.Site.Services;

public interface IServiceService {
	Task<List<Service>> GetServices();
	Task<Service> GetService(Guid id);
	Task<Service> CreateService(Service service);
	Task<Service> UpdateService(Guid id, Service service);
	Task<bool> DeleteService(Guid id);
}

public class ServiceService : IServiceService {
	private readonly AppDbContext context;

	public ServiceService(AppDbContext context) {
		this.context = context;
	}

	public async Task<List<Service>> GetServices() {
		return await context.Services
			.Include(s => s.ServiceCategory)
			.Include(s => s.User)
			.ToListAsync();
	}

	public async Task<Service> GetService(Guid id) {
		return await context.Services
			.Include(s => s.ServiceCategory)
			.FirstOrDefaultAsync(s => s.Id == id);
	}

	public async Task<Service> CreateService(Service service) {
		context.Services.Add(service);
		await context.SaveChangesAsync();
		return service;
	}

	public async Task<Service> UpdateService(Guid id, Service service) {
		if (id != service.Id) {
			throw new ArgumentException("Service Id does not match");
		}

		context.Entry(service).State = EntityState.Modified;
		await context.SaveChangesAsync();
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

