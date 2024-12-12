using Autofac;
using MoveDb.Services.Services;
using MoveDb.Services.Services.IServices;

namespace MoveDb {
    public class ContainerModule : Module {

        private readonly IConfiguration _configuration;

        public ContainerModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assembly = System.Reflection.Assembly.Load("MoveDb.Services");

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
