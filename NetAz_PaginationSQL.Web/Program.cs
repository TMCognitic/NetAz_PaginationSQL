using NetAz_PaginationSQL.Web.Models.Queries;
using System.Data.SqlClient;
using Tools;

namespace NetAz_PaginationSQL.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Services Configuration
            ConfigureServices(builder.Services, builder.Configuration);

            //App Configuration
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddSingleton(new Connection(SqlClientFactory.Instance, configuration.GetConnectionString("TestPagination")));
            services.AddScoped<GetDataQueryHandler>();
        }
    }
}