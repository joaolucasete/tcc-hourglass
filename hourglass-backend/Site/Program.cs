using Hangfire;
using Hangfire.PostgreSql;
using Hourglass.Site.Entities;
using Hourglass.Site.Extensions;
using Hourglass.Site.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddIdentity<User, Role>(options => {
	options.Password.RequiredLength = 6;
	options.SignIn.RequireConfirmedEmail = true;
})
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAppConfiguration(builder.Configuration);
builder.Services.AddAppServices(builder.Configuration);
builder.Services.AddHangfire(config =>
	config
	.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
	.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();
builder.Services.Configure<CorsOptions>(corsOptions => {
	corsOptions.AddPolicy("AllowAll", policy => {
		policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
	});
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
		Type = SecuritySchemeType.ApiKey,
		Name = "Authorization",
		In = ParameterLocation.Header,
		Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement()
	{
		{
			new OpenApiSecurityScheme {
				Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header,
			},
			new List<string>()
		}
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// configure cors
app.UseCors(policy => policy
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.InitializeDatabaseAsync();
app.UseHangfireDashboard();

app.Run();

