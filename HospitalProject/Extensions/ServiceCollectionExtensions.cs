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
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();

            services.AddScoped<INurseRepository, NurseRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();

            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientBillDetailRepository, PatientBillDetailRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();


            return services;
        }
    }
}
