using AutoMapper;
using MedShopWebAPi.Data;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using MedShop.Hubs;
using MedShop.Models;
using MedShop.Models.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            
            
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
      
        public void ConfigureServices(IServiceCollection services)
        {
            //konfigurisanje email servisa
            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);


            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder.WithOrigins("https://localhost:44322")
                .AllowAnyMethod()
                .WithHeaders(".AspNetCore.Identity.Application").AllowCredentials();
                
               ;  
            }));

            services.AddControllersWithViews().AddXmlSerializerFormatters().AddNewtonsoftJson(s => {

                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            }); ;

       
            services.AddDbContextPool<CoreDBContext>(opt=>opt.UseSqlServer(Configuration.GetConnectionString("CoreDBContext")));
         
            services.AddIdentity<ApplicationUser, IdentityRole>(
                options=> {
                    //set this property if you required email to be confirmed during the login action!

                    options.SignIn.RequireConfirmedEmail = true;
                
                
                }).AddEntityFrameworkStores<CoreDBContext>()
                  .AddDefaultTokenProviders();

            services.AddAuthentication().AddGoogle(
                options =>
                {
                    options.ClientId = "180759108418-pe775u65fq177mlm2l162hkgg7gpiqpp.apps.googleusercontent.com";
                    options.ClientSecret = "QSF-1WLSQEAWIfHolSZHMWyu";
                }).AddFacebook(
                options =>
                {
                    options.AppId = "208794137515212";
                    options.AppSecret = "e2daf6389bfcaa629f7b253296a24a68";
                });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role"));

                //zato sto su roles claims i to tipa Roles
                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin"));

                options.AddPolicy("EditRolePolicy",
                   policy => policy.RequireClaim("Edit Role"));
            });


            services.AddSignalR();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOpeningHoursRepository, OpeningHoursRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IShippingRepository, ShippingRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IMedShopRepo, SqlMedShopRepo>();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {

                options.IdleTimeout = TimeSpan.FromMinutes(20);
                //options.Cookie.Name = ".TokenAuthCookies";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
  
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Commander API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
          
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

            }

            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Commander API V1");
            });

            app.UseStaticFiles();
            
            app.UseAuthentication();

            app.UseCors("Cors");

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MyHub>("/myHubs");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
