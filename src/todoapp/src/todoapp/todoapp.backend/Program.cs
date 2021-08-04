using Couchbase.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using todoapp.backend.Models;

namespace todoapp.backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // Todo: Refactor to a database seeder
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var todoRepo = services.GetRequiredService<ITodoRepository>();
                if (todoRepo.GetAll().Count is 0)
                {
                    todoRepo.AddNew(new TodoItem { Id = Guid.NewGuid().ToString(), Completed = false, Title = "Let's learn docker and kuberenetes!", Order = 3 });
                    todoRepo.AddNew(new TodoItem { Id = Guid.NewGuid().ToString(), Completed = true, Title = "Check the new DotNet5!", Order = 1 });
                    todoRepo.AddNew(new TodoItem { Id = Guid.NewGuid().ToString(), Completed = false, Title = "Learn another No-Sql database - Couchbase!", Order = 2 });
                }

            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Program>();
                });
        }


        public Program(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(o => o.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddCouchbase(x =>
            {
                // Change it to http://localhost:8091 to run locally
                var DB_SERVER = Environment.GetEnvironmentVariable("DB_SERVER");
                Console.WriteLine("DB SERVER is - " + DB_SERVER);
                x.Servers = new List<Uri> { new Uri($"http://{DB_SERVER}:8091") };
                x.Username = "Administrator";
                x.Password = "password";
            });
            services.AddTransient<ITodoRepository, TodoRepository>();

            // Register Swagger generator
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo App V1");
            });
            app.UseRouting();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
