﻿namespace Octgn.Library.Plugin
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.IO.Abstractions;
    using System.Management.Instrumentation;
    using System.Reflection;

    internal static class PluginManager
    {
        internal static IFileSystem FS { get; set; }

        static PluginManager()
        {
            if(FS == null)
                FS = new FileSystem();
        }

        internal static IQueryable<T> GetPlugins<T>()
        {
            if (!FS.Directory.Exists(Paths.PluginPath)) FS.Directory.CreateDirectory(Paths.PluginPath);

            var folder = FS.DirectoryInfo.FromDirectoryName(Paths.PluginPath);

            var ret = new List<T>();
            foreach (var f in folder.GetDirectories().SelectMany(dir => dir.GetFiles("*.dll", SearchOption.TopDirectoryOnly)))
            {
                try
                {
                    ret.Add(LoadExtension<T>(f.FullName));
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
            }
            // Load all plugins built into OCTGN
            foreach( var e in Assembly.GetEntryAssembly().GetTypes().Where(t => t.GetInterfaces().Any(i=>i == typeof(T))))
                ret.Add((T)Activator.CreateInstance(e));
            return ret.AsQueryable();
        }

        internal static T LoadExtension<T>(string path)
        {
            foreach (var t in Assembly.LoadFile(path).GetTypes().Where(t => t.GetInterfaces().Any(i => i == typeof(T))))
                return (T)Activator.CreateInstance(t);
            throw new InstanceNotFoundException(String.Format("Instance of the plugin type {0} was not found in the file {1}",typeof(T).Name,path));
        }
    }
}
