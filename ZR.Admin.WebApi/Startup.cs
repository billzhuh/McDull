using Hei.Captcha;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using ZR.Admin.WebApi.Extensions;
using ZR.Admin.WebApi.Filters;
using ZR.Admin.WebApi.Framework;
using ZR.Admin.WebApi.Middleware;
using ZR.Common.Cache;

namespace ZR.Admin.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            string corsUrls = Configuration["sysConfig:cors"];

            //���ÿ���
            services.AddCors(c =>
            {
                c.AddPolicy("Policy", policy =>
                {
                    policy.WithOrigins(corsUrls.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    .AllowAnyHeader()//��������ͷ
                    .AllowCredentials()//����cookie
                    .AllowAnyMethod();//�������ⷽ��
                });
            });
            //����Error unprotecting the session cookie����
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DataProtection"));
            //��ͨ��֤��
            services.AddHeiCaptcha();
            services.AddSession();
            services.AddHttpContextAccessor();

            //����������Model��
            services.Configure<OptionsSetting>(Configuration);

            //Cookie ��֤
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie()
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = JwtUtil.ValidParameters();
            });

            InjectServices(services, Configuration);

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalActionMonitor));//ȫ��ע���쳣
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonConverterUtil.DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new JsonConverterUtil.DateTimeNullConverter());
            });

            services.AddSwaggerConfig();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            //ʹ���Զ�ζ�ȥbody����
            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                return next();
            });
            //�������ʾ�̬�ļ�/wwwrootĿ¼�ļ���Ҫ����UseRoutingǰ��
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("Policy");//Ҫ����app.UseEndpointsǰ��

            //app.UseAuthentication������Authentication�м�������м������ݵ�ǰHttp�����е�Cookie��Ϣ������HttpContext.User���ԣ�������õ�����
            //����ֻ����app.UseAuthentication����֮��ע����м�����ܹ���HttpContext.User�ж�ȡ��ֵ��
            //��Ҳ��Ϊʲô����ǿ��app.UseAuthentication����һ��Ҫ���������app.UseMvc����ǰ�棬��Ϊֻ������ASP.NET Core��MVC�м���в��ܶ�ȡ��HttpContext.User��ֵ��
            //1.�ȿ�����֤
            app.UseAuthentication();
            //2.�ٿ�����Ȩ
            app.UseAuthorization();
            app.UseSession();
            app.UseResponseCaching();

            // �ָ�/��������
            app.UseAddTaskSchedulers();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// ע��Services����
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private void InjectServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAppService();

            //�����ƻ�����
            services.AddTaskSchedulers();
            //��ʼ��db
            DbExtension.AddDb(configuration);

            //ע��REDIS ����
            Task.Run(() =>
            {
                //RedisServer.Initalize();
            });
        }
    }
}
