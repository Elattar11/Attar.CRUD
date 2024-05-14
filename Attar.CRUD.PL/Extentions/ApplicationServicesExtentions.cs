using Attar.CRUD.BLL.Interfaces;
using Attar.CRUD.BLL.Repositories;
using Attar.CRUD.PL.Helper;
using Attar.CRUD.PL.Services.EmailService;
using Attar.CRUD.PL.Services.MailKitService;
using Microsoft.Extensions.DependencyInjection;

namespace Attar.CRUD.PL.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            services.AddTransient<IEmailSender, EmailSender>(); //.NET

            services.AddTransient<IMailSettings, MailSettings>(); //MailKit 

            return services;
        }
    }
}
