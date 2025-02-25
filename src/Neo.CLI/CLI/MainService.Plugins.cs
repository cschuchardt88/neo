// Copyright (C) 2015-2025 The Neo Project.
//
// MainService.Plugins.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Akka.Util.Internal;
using Microsoft.Extensions.Configuration;
using Neo.CLI.Tools;
using Neo.ConsoleService;
using Neo.Plugins;
using System;
using System.IO;
using System.Linq;

namespace Neo.CLI
{
    partial class MainService
    {
        private NeoNuGetPackageDownloader? _pluginDownloader;

        /// <summary>
        /// Process "install" command
        /// </summary>
        /// <param name="pluginName">Plugin name</param>
        /// <param name="downloadUrl">Custom plugins download url, this is optional.</param>
        [ConsoleCommand("install", Category = "Plugin Commands")]
        private void OnInstallCommand(string pluginName, string? downloadUrl = null)
        {
            if (_pluginDownloader is null)
            {
                ConsoleHelper.Error($"Plugin downloader is 'null'.");
                return;
            }

            if (PluginExists(pluginName))
            {
                ConsoleHelper.Warning($"Plugin already exist.");
                return;
            }

            try
            {
                var packageId = NeoNuGetPackageDownloader.PluginPrefixName + pluginName;

                _pluginDownloader.ExtractPackage(packageId, pluginName).GetAwaiter().GetResult();
                ConsoleHelper.Info("", $"Install successful, please restart node.");
            }
            catch (Exception ex)
            {
                ConsoleHelper.Error(ex.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Force to install a plugin again. This will overwrite
        /// existing plugin files, in case of any file missing or
        /// damage to the old version.
        /// </summary>
        /// <param name="pluginName">name of the plugin</param>
        [ConsoleCommand("reinstall", Category = "Plugin Commands", Description = "Overwrite existing plugin by force.")]
        private void OnReinstallCommand(string pluginName)
        {
            ConsoleHelper.Info("", $"Reinstall successful, please restart node.");
        }

        /// <summary>
        /// Check that the plugin has all necessary files
        /// </summary>
        /// <param name="pluginName"> Name of the plugin</param>
        /// <returns></returns>
        private static bool PluginExists(string pluginName)
        {
            return Plugin.Plugins.Any(p => p.Name.Equals(pluginName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Process "uninstall" command
        /// </summary>
        /// <param name="pluginName">Plugin name</param>
        [ConsoleCommand("uninstall", Category = "Plugin Commands")]
        private void OnUnInstallCommand(string pluginName)
        {
            if (!PluginExists(pluginName))
            {
                ConsoleHelper.Error("Plugin not found");
                return;
            }

            foreach (var p in Plugin.Plugins)
            {
                try
                {
                    using var reader = File.OpenRead($"Plugins/{p.Name}/config.json");
                    if (new ConfigurationBuilder()
                        .AddJsonStream(reader)
                        .Build()
                        .GetSection("Dependency")
                        .GetChildren()
                        .Select(s => s.Get<string>())
                        .Any(a => a is not null && a.Equals(pluginName, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        ConsoleHelper.Error($"{pluginName} is required by other plugins.");
                        ConsoleHelper.Info("Info: ", $"If plugin is damaged try to reinstall.");
                        return;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            try
            {
                Directory.Delete($"Plugins/{pluginName}", true);
            }
            catch (IOException) { }
            ConsoleHelper.Info("", "Uninstall successful, please restart neo-cli.");
        }

        /// <summary>
        /// Process "plugins" command
        /// </summary>
        [ConsoleCommand("plugins", Category = "Plugin Commands")]
        private void OnPluginsCommand()
        {
            try
            {
                var pluginPrefix = NeoNuGetPackageDownloader.PluginPrefixName;
                var plugins = _pluginDownloader?.SearchPackage(pluginPrefix, Settings.Default.Plugins.Prerelease).GetAwaiter().GetResult() ?? [];
                var installedPlugins = Plugin.Plugins.OrderBy(o => o.Name);

                var maxPluginNameLength = Math.Max(installedPlugins.Max(m => m.Name.Length), plugins.Max(m => m.Identity.Id.Replace(pluginPrefix, string.Empty).Length));

                foreach (var plugin in plugins.OrderBy(o => o.Identity.Id))
                {
                    var pluginName = plugin.Identity.Id.Replace(pluginPrefix, string.Empty);
                    var isInstalled = installedPlugins.Any(p => plugin.Identity.Id.EndsWith(p.Name, StringComparison.InvariantCultureIgnoreCase));

                    var spaces = new string(' ', maxPluginNameLength - pluginName.Length);

                    if (isInstalled == false)
                        ConsoleHelper.Info(
                            "[Not Installed] ",
                            $" {pluginName}{spaces}",
                            " @",
                            $"{plugin.Identity.Version.Version.ToString(3)}  {plugin.Summary}"
                        );
                    else
                    {
                        ConsoleHelper.Info(
                            "[Installed] ",
                            $"     {pluginName}{spaces}",
                            " @",
                            $"{plugin.Identity.Version.Version.ToString(3)}  {plugin.Summary}"
                        );
                    }
                }

                foreach (var plugin in installedPlugins)
                {
                    if (plugins.Any(a => a.Identity.Id.EndsWith(plugin.Name, StringComparison.InvariantCultureIgnoreCase))) continue;

                    var spaces = new string(' ', maxPluginNameLength - plugin.Name.Length);

                    ConsoleHelper.Info(
                        "[Installed] ",
                        $"     {plugin.Name}{spaces}",
                        " @",
                        $"{plugin.Version.ToString(3)}  {plugin.Description}"
                    );
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.Error(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
