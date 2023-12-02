using AppSemTemplate.Services;

namespace AppSemTemplate.Configuration
{
    public static class AddDependencyInjectionConfig
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddTransient<IOperacaoTransient, Operacao>(); // instancia gerada cada vez que eu pedir
            builder.Services.AddScoped<IOperacaoScoped, Operacao>(); // instancia gerada uma vez dentro do request
            builder.Services.AddSingleton<IOperacaoSingleton, Operacao>(); //instancia gerada uma unica vez
            builder.Services.AddSingleton<IOperacaoSingletonInstance>(new Operacao(Guid.Empty)); //instancia gerada uma unica vez mas eu decido qual objeto usar
            builder.Services.AddTransient<OperacaoService>();

            return builder;

        }

    }
}
