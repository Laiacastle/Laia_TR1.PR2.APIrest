using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Client_Laia_T1.PR2.APIrest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //hacemos que espera para que se ejecute primero la api
            await Task.Delay(3000);
            // Add services to the container.
            builder.Services.AddRazorPages();

            //damos accesa a la configuración de appsettings
            string urlApi = builder.Configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("Api base URL not found");

            //añadimos la connexion HttpClient
            builder.Services.AddHttpClient("ApiLaia", client =>
            {
                client.BaseAddress = new Uri(urlApi);

            });

            builder.Services.AddHttpClient("AuthorizedClient", client =>
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

            });
            //
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/AccesDenied";
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient<AuthenticationService>(client =>
            {
                client.BaseAddress = new Uri(urlApi);
            });

            builder.Services.AddSession();

            builder.Services.AddSession();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //Activamos la session
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();
            app.MapGet("/", context =>
            {
                context.Response.Redirect("/ViewGames");
                return Task.CompletedTask;
            });

            app.Run();
        }
    }
}
