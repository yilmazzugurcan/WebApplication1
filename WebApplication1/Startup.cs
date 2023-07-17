using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // ConfigureServices metodu, hizmetlerin yapılandırması için kullanılır.
        public void ConfigureServices(IServiceCollection services)
        {
            // Gerekli hizmetleri ekleyin.
            services.AddControllers();

            // Burada başka hizmetleri de ekleyebilirsiniz.
        }

        // Configure metodu, HTTP taleplerini işlemek için kullanılır.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Üretim ortamında hata sayfalarını yönetmek için gerekli middleware'leri ekleyin.
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // HTTP taleplerinin yönlendirilmesi ve işlenmesi için gerekli middleware'leri ekleyin.
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // API controller'larını eşleştirin.
                endpoints.MapControllers();
            });
        }
    }
}
