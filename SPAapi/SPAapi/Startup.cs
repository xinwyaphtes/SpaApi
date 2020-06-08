using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SPAService;
using NLog.Extensions.Logging;

namespace SPAapi
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    [Obsolete]
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddLogging();
      services.AddControllers();
      services.AddTransient<IService1, Service1>();
      services.AddMvc(options =>
      {
        options.Filters.Add<ValidateModelAttribute>(1);
        options.Filters.Add<ApiResultFilterAttribute>(2);
        options.Filters.Add<CustomExceptionAttribute>(3);
      });
      //services.AddCors(options =>
      //{
      //  options.AddPolicy("CustomCorsPolicy", policy =>
      //  {
      //    // �趨����������Դ���ж��������','����
      //    policy.WithOrigins("https://localhost:54991")//ֻ����https://localhost:54991��Դ�������
      //    .AllowAnyHeader()
      //    .AllowAnyMethod()
      //    .AllowCredentials();
      //  });
      //});
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthorization();
      //app.UseCors("CustomCorsPolicy");
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
