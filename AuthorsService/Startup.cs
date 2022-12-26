using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using AuthorsService.Data;
using AuthorsService.SyncDataServices.Http;
using AuthorsService.AsyncDataServices;
using AuthorsService.SyncDataServices.Grpc;

namespace AuthorsService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        
        

        public void ConfigureServices(IServiceCollection services)
        {   
             if(_env.IsProduction())
             {
                Console.WriteLine("----> Using SqlServer Db");
                services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("AuthorsConn")));
             }
             else 
             {
                 Console.WriteLine("----> Using InMem Db");
                 services.AddDbContext<AppDbContext>(opt => 
                 opt.UseInMemoryDatabase("InMem"));
            }
            
            
            services.AddScoped<IAuthorRepo, AuthorRepo>();

            services.AddHttpClient<IBookDataClient, HttpBookDataClient>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddGrpc();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthorService", Version = "v1" });
            });

            Console.WriteLine($"----> BooksService Endpoint {Configuration["BooksService"]}");

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthorService v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcAuthorService>();

                endpoints.MapGet("/protos/authors.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/authors.proto"));
                });
            });

             PrepDb.PrepPopulation(app, env.IsProduction());

        }
    }
}