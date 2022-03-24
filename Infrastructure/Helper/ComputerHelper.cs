using Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Infrastructure
{
    public class ComputerHelper
    {
        /// <summary>
        /// 内存使用情况
        /// </summary>
        /// <returns></returns>
        public static MemoryMetrics GetComputerInfo()
        {
            try
            {
                MemoryMetricsClient client = new();
                MemoryMetrics memoryMetrics = IsUnix() ? client.GetUnixMetrics() : client.GetWindowsMetrics();

                memoryMetrics.FreeRam = Math.Round(memoryMetrics.Free / 1024, 2) + "GB";
                memoryMetrics.UsedRam = Math.Round(memoryMetrics.Used / 1024, 2) + "GB";
                memoryMetrics.TotalRAM = Math.Round(memoryMetrics.Total / 1024, 2) + "GB";
                memoryMetrics.RAMRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total).ToString() + "%";
                memoryMetrics.CPURate = Math.Ceiling(GetCPURate().ParseToDouble()) + "%";
                return memoryMetrics;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取内存使用出错，msg=" + ex.Message + "," + ex.StackTrace);
            }
            return new MemoryMetrics();
        }

        /// <summary>
        /// 获取内存大小
        /// </summary>
        /// <returns></returns>
        public static List<DiskInfo> GetDiskInfos()
        {
            List<DiskInfo> diskInfos = new();

            if (IsUnix())
            {
                try
                {
                    string output = ShellHelper.Bash("df -m / | awk '{print $2,$3,$4,$5,$6}'");
                    var arr = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length == 0) return diskInfos;

                    var rootDisk = arr[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                    if (rootDisk == null || rootDisk.Length == 0)
                    {
                        return diskInfos;
                    }
                    DiskInfo diskInfo = new()
                    {
                        DiskName = "/",
                        TotalSize = long.Parse(rootDisk[0]) / 1024,
                        Used = long.Parse(rootDisk[1]) / 1024,
                        AvailableFreeSpace = long.Parse(rootDisk[2]) / 1024,
                        AvailablePercent = decimal.Parse(rootDisk[3].Replace("%", ""))
                    };
                    diskInfos.Add(diskInfo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("获取磁盘信息出错了" + ex.Message);
                }
            }
            else
            {
                var driv = DriveInfo.GetDrives();
                foreach (var item in driv)
                {
                    var obj = new DiskInfo()
                    {
                        DiskName = item.Name,
                        TypeName = item.DriveType.ToString(),
                        TotalSize = item.TotalSize / 1024 / 1024 / 1024,
                        AvailableFreeSpace = item.AvailableFreeSpace / 1024 / 1024 / 1024,
                    };
                    obj.Used = obj.TotalSize - obj.AvailableFreeSpace;
                    obj.AvailablePercent = decimal.Ceiling(obj.Used / (decimal)obj.TotalSize * 100);
                    diskInfos.Add(obj);
                }
            }

            return diskInfos;
        }

        public static bool IsUnix()
        {
            var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            return isUnix;
        }

        public static string GetCPURate()
        {
            string cpuRate;
            if (IsUnix())
            {
                string output = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
                cpuRate = output.Trim();
            }
            else
            {
                string output = ShellHelper.Cmd("wmic", "cpu get LoadPercentage");


                cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();

            }
            return cpuRate;
        }

        /// <summary>
        /// 获取系统运行时间
        /// </summary>
        /// <returns></returns>
        public static string GetRunTime()
        {
            string runTime = string.Empty;
            try
            {
                if (IsUnix())
                {
                    string output = ShellHelper.Bash("uptime -s").Trim();
                    runTime = DateTimeHelper.FormatTime((DateTime.Now - output.ParseToDateTime()).TotalMilliseconds.ToString().Split('.')[0].ParseToLong());
                }
                else
                {
                    string output = ShellHelper.Cmd("wmic", "OS get LastBootUpTime/Value");
                    string[] outputArr = output.Split('=', (char)StringSplitOptions.RemoveEmptyEntries);
                    if (outputArr.Length == 2)
                    {
                        runTime = DateTimeHelper.FormatTime((DateTime.Now - outputArr[1].Split('.')[0].ParseToDateTime()).TotalMilliseconds.ToString().Split('.')[0].ParseToLong());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取runTime出错" + ex.Message);
            }
            return runTime;
        }
    }

    /// <summary>
    /// 内存信息
    /// </summary>
    public class MemoryMetrics
    {
        [JsonIgnore]
        public double Total { get; set; }
        [JsonIgnore]
        public double Used { get; set; }
        [JsonIgnore]
        public double Free { get; set; }

        public string UsedRam { get; set; }
        /// <summary>
        /// CPU使用率%
        /// </summary>
        public string CPURate { get; set; }
        /// <summary>
        /// 总内存 GB
        /// </summary>
        public string TotalRAM { get; set; }
        /// <summary>
        /// 内存使用率 %
        /// </summary>
        public string RAMRate { get; set; }
        /// <summary>
        /// 空闲内存
        /// </summary>
        public string FreeRam { get; set; }
    }

    public class DiskInfo
    {
        /// <summary>
        /// 磁盘名
        /// </summary>
        public string DiskName { get; set; }
        public string TypeName { get; set; }
        public long TotalFree { get; set; }
        public long TotalSize { get; set; }
        /// <summary>
        /// 已使用
        /// </summary>
        public long Used { get; set; }
        /// <summary>
        /// 可使用
        /// </summary>
        public long AvailableFreeSpace { get; set; }
        public decimal AvailablePercent { get; set; }
    }

    public class MemoryMetricsClient
    {
        #region 获取内存信息

        /// <summary>
        /// windows系统获取内存信息
        /// </summary>
        /// <returns></returns>
        public MemoryMetrics GetWindowsMetrics()
        {
            string output = ShellHelper.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
            var metrics = new MemoryMetrics();
            var lines = output.Trim().Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length <= 0) return metrics;

            var freeMemoryParts = lines[0].Split('=', (char)StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split('=', (char)StringSplitOptions.RemoveEmptyEntries);

            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);//m
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }

        /// <summary>
        /// Unix系统获取
        /// </summary>
        /// <returns></returns>
        public MemoryMetrics GetUnixMetrics()
        {
            string output = ShellHelper.Bash("free -m | awk '{print $2,$3,$4,$5,$6}'");
            var metrics = new MemoryMetrics();
            var lines = output.Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length <= 0) return metrics;

            if (lines != null && lines.Length > 0)
            {
                var memory = lines[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                if (memory.Length >= 3)
                {
                    metrics.Total = double.Parse(memory[0]);
                    metrics.Used = double.Parse(memory[1]);
                    metrics.Free = double.Parse(memory[2]);//m
                }
            }
            return metrics;
        }
        #endregion
    }


    public class ServerInfos
    {

        public Cpu cpu { get; set; }
        public List<SysFilesItem> sysFiles { get; set; }
        public Mem mem { get; set; }
        public Jvm jvm { get; set; }
        public Sys sys { get; set; }

        public bool IsUnix { get { return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux); } }

        public Cpu GetCpuInfos()
        {
            Cpu cpu = new Cpu();
            if (IsUnix)
            {
                cpu.sys = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'").Trim().ParseToDouble();

            }
            else
            {

                cpu.cpuNum = int.Parse(ShellHelper.Cmd("wmic", "cpu get NumberOfLogicalProcessors").Replace("NumberOfLogicalProcessors", string.Empty).Trim());
                cpu.sys = double.Parse(ShellHelper.Cmd("wmic", "cpu get LoadPercentage").Replace("LoadPercentage", string.Empty).Trim());

            }
            return cpu;
        }
        public List<SysFilesItem> GetDiskInfos()
        {
            List<SysFilesItem> diskInfos = new List<SysFilesItem>();
            // List<DiskInfo> diskInfos = new();

            if (IsUnix)
            {
                try
                {
                    string output = ShellHelper.Bash("df -m / | awk '{print $2,$3,$4,$5,$6}'");
                    var arr = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length == 0) return diskInfos;

                    var rootDisk = arr[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                    if (rootDisk == null || rootDisk.Length == 0)
                    {
                        return diskInfos;
                    }
                    SysFilesItem diskInfo = new()
                    {
                        dirName = "/",
                        total = (long.Parse(rootDisk[0]) / 1024).ToString(),
                        used = (long.Parse(rootDisk[1]) / 1024).ToString(),
                        free = (long.Parse(rootDisk[2]) / 1024).ToString(),
                        usage = decimal.Parse(rootDisk[3].Replace("%", ""))
                    };
                    diskInfos.Add(diskInfo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("获取磁盘信息出错了" + ex.Message);
                }
            }
            else
            {
                var driv = DriveInfo.GetDrives();
                foreach (var item in driv)
                {
                    var obj = new SysFilesItem()
                    {
                        dirName = item.Name,
                        typeName=item.Name,
                        sysTypeName = item.DriveFormat.ToString(),
                        total = (item.TotalSize / 1024 / 1024 / 1024).ToString(),
                        free = (item.AvailableFreeSpace / 1024 / 1024 / 1024).ToString(),
                    };
                    obj.used = (obj.total.ParseToLong() - obj.free.ParseToLong()).ToString();
                    obj.usage = decimal.Ceiling(obj.used.ParseToDecimal() / obj.total.ParseToDecimal() * 100);
                    diskInfos.Add(obj);
                }
            }

            return diskInfos;


        }
        public Mem GetMem()
        {
            Mem mem = new Mem();
            try
            {
                MemoryMetricsClient client = new();
                MemoryMetrics memoryMetrics = IsUnix ? client.GetUnixMetrics() : client.GetWindowsMetrics();

                mem.total = Math.Round(memoryMetrics.Total / 1024, 2);
                mem.used = Math.Round(memoryMetrics.Used / 1024, 2);
                mem.free = Math.Round(mem.total - mem.used,2); //Math.Round(memoryMetrics.Free / 1024, 2);
                mem.usage = Math.Round(100 * memoryMetrics.Used / memoryMetrics.Total, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取内存使用出错，msg=" + ex.Message + "," + ex.StackTrace);
            }
            return mem;
        }
        public Jvm GetJvm()
        {

            Jvm jvm = new Jvm();
             jvm.name = RuntimeInformation.FrameworkDescription;
            jvm.version= RuntimeInformation.FrameworkDescription;
            jvm.startTime= Process.GetCurrentProcess().StartTime.ToString("yyyy-MM-dd HH:mm:ss");
            jvm.runTime= ComputerHelper.GetRunTime();
       
            return jvm;
        }
        public Sys GetSys(string serverIp,string userDir)
        {
            Sys sys = new Sys();
            sys.computerName = Environment.MachineName;
            sys.osName = RuntimeInformation.OSDescription;
            sys.computerIp = serverIp;//Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + Request.HttpContext.Connection.LocalPort;//获取服务器IP
            sys.osArch= RuntimeInformation.OSArchitecture.ToString();
            sys.userDir = userDir;
            return sys;
        }
    }

    //-----------------------------------------------------------------------




    /// <summary>
    /// CPU信息
    /// </summary>
    public class Cpu
    {
        /// <summary>
        /// cpu核数
        /// </summary>
        public int cpuNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 系统使用率
        /// </summary>
        public double sys { get; set; }
        /// <summary>
        /// 用户使用率
        /// </summary>
        public double used { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int wait { get; set; }
        /// <summary>
        /// 空闲率
        /// </summary>
        public double free { get; set; }
    }
    /// <summary>
    /// 内存信息
    /// </summary>
    public class Mem
    {
        /// <summary>
        /// 
        /// </summary>
        public double total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double used { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double free { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double usage { get; set; }
    }
    /// <summary>
    /// .net core运行时信息
    /// </summary>
    public class Jvm
    {
        /// <summary>
        /// 
        /// </summary>
        public double total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int max { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double free { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string home { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double used { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double usage { get; set; }
        /// <summary>
        /// 0天1小时56分钟
        /// </summary>
        public string runTime { get; set; }
    }
    /// <summary>
    /// PC信息
    /// </summary>
    public class Sys
    {
        /// <summary>
        /// 服务器名
        /// </summary>
        public string computerName { get; set; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string computerIp { get; set; }
        /// <summary>
        /// 系统盘
        /// </summary>
        public string userDir { get; set; }
        /// <summary>
        /// 系统名
        /// </summary>
        public string osName { get; set; }
        /// <summary>
        /// 系统架构
        /// </summary>
        public string osArch { get; set; }
    }
    /// <summary>
    /// 磁盘信息
    /// </summary>
    public class SysFilesItem
    {
        /// <summary>
        /// 盘符
        /// </summary>
        public string dirName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string sysTypeName { get; set; }
        /// <summary>
        /// 磁盘名
        /// </summary>
        public string typeName { get; set; }
        /// <summary>
        /// 总大小
        /// </summary>
        public string total { get; set; }
        /// <summary>
        /// 空闲大小
        /// </summary>
        public string free { get; set; }
        /// <summary>
        /// 已使用大小
        /// </summary>
        public string used { get; set; }
        /// <summary>
        /// 使用占比
        /// </summary>
        public decimal usage { get; set; }
    }





}