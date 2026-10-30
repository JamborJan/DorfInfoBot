using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.Sqlite;
using DorfInfoBot.API.Contexts;
using DorfInfoBot.API.Services;


namespace DorfInfoBot.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddMvc()
                .AddMvcOptions(o =>
                {
                    o.EnableEndpointRouting = false;
                });

            // In case this is sensitive data, inject via ENV, ENV is always considered last and overwrites this
            // Example ENV: DorfInfoBot.API__DataFilesPath=/data/
            var absolutePath = _configuration["DorfInfoBot.API:DataFilesPath"];
            var connectionString = new SqliteConnectionStringBuilder()
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                DataSource = $"{absolutePath}news.db"
            }.ToString();

            services.AddDbContext<NewsContext>(o =>
            {
                o.UseSqlite(connectionString);
            });

            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IChannelRepository, ChannelRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStatusCodePages();

            app.UseMvc();

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DorfInfoBot.API");
                c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
            });
        }
    }
}
