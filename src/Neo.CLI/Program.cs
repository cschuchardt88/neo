// Copyright (C) 2015-2025 The Neo Project.
//
// Program.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.CLI;
using System;
using System.IO;
using System.Reflection;

namespace Neo
{
    static class Program
    {
        static void Main(string[] args)
        {
            LoadPlugins();
            var mainService = new MainService();
            mainService.Run(args);
        }

        static void LoadPlugins()
        {
            var pluginPath = Plugins.Plugin.PluginsDirectory;

            if (!Directory.Exists(pluginPath)) return;
            foreach (var rootPath in Directory.GetDirectories(pluginPath))
            {
                try
                {
                    var pluginName = Path.GetFileNameWithoutExtension(rootPath);
                    var pluginFilename = Path.Combine(rootPath, $"{pluginName}.dll");
                    var pluginContext = new PluginLoadContext(pluginFilename);
                    var assembly = pluginContext.LoadFromAssemblyName(AssemblyName.GetAssemblyName(pluginFilename));

                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(Plugins.Plugin).IsAssignableFrom(type))
                        {
                            var result = Activator.CreateInstance(type) as Plugins.Plugin;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utility.Log(nameof(Plugins.Plugin), LogLevel.Error, ex);
                }
            }
        }
    }
}
