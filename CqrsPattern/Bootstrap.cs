using CQRS.Domain.Commands.CreatePerson;
using CQRS.Domain.Contracts;
using CQRS.Domain.Queries.GetPerson;
using CQRS.Domain.Queries.ListPerson;
using CQRS.Repository;
using CQRS.Repository.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using MongoDB.Driver;

namespace CqrsPattern.API
{
    public static class Bootstrap
    {
        public static void AddInjections(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services, configuration);
            AddComands(services);
            AddQueries(services);
            AddMappers(services);
            AddValidators(services);
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<CreatePersonCommand>, CreatePersonCommandValidator>(); 
        }

        private static void AddComands(this IServiceCollection services)
        {
            services.AddTransient<CreatePersonCommandHandler>();
            services.AddTransient<GetPersonQueryHandler>();
        }

        private static void AddQueries(IServiceCollection services)
        {
            services.AddTransient<ListPersonQueryHandler>();
        }

        private static void AddMappers(this IServiceCollection services) => services.AddAutoMapper(typeof(ListPersonQueryProfile),
            typeof(CreatePersonCommandProfile));       

        private static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection(nameof(MongoRespositorySettings));
            var clientSettings = MongoClientSettings.FromConnectionString(mongoSettings.Get<MongoRespositorySettings>().ConnectionString);

            services.Configure<MongoRespositorySettings>(mongoSettings);            //injeção de configurações
            services.AddSingleton<IMongoClient>(new MongoClient(clientSettings));   // injeção do client
            services.AddSingleton<IPersonRepository, PersonRepository>();           // injeção dp répository
        }
    }
}
