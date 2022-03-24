﻿using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using System;
using ZR.Service.System.IService;
using ZR.Tasks;

namespace ZR.Admin.WebApi.Extensions
{
    /// <summary>
    /// 定时任务扩展方法
    /// </summary>
    public static class TasksExtension
    {
        public static void AddTaskSchedulers(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //添加Quartz服务
            services.AddSingleton<IJobFactory, JobFactory>();
            //添加我们的服务
            services.AddTransient<Job_SyncTest>();

            services.AddTransient<ITaskSchedulerServer, TaskSchedulerServer>();
        }

        /// <summary>
        /// 程序启动后添加任务计划
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAddTaskSchedulers(this IApplicationBuilder app)
        {
            var _tasksQzService = (ISysTasksQzService)App.GetRequiredService(typeof(ISysTasksQzService));

            //此写法不通过有待研究
            //var _tasksQzService2 = (ISysTasksQzService)services.GetRequiredService(typeof(SysTasksQzService));
            ITaskSchedulerServer _schedulerServer = App.GetRequiredService<ITaskSchedulerServer>();

            var tasks = _tasksQzService.GetList(m => m.IsStart=="0");

            //程序启动后注册所有定时任务
            foreach (var task in tasks)
            {
                _schedulerServer.AddTaskScheduleAsync(task);
            }

            return app;
        }

    }
}
