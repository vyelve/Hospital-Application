//using AutoMapper.Configuration;
using Hospital.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace HospitalProject.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INurseRepository, NurseRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();

            return services;
        }
    }
}
