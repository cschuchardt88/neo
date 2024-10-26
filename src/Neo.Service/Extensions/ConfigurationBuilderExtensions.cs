// Copyright (C) 2015-2024 The Neo Project.
//
// ConfigurationBuilderExtensions.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Neo.Service.Extensions
{
    internal static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Add Neo configuration settings to the configuration builder.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddNeoConfiguration(this IConfigurationBuilder builder)
        {
            builder.AddInMemoryCollection(
                [
                    new(HostDefaults.ContentRootKey, Environment.CurrentDirectory),
                ]);

            return builder;
        }

        /// <summary>
        /// Add Neo default configuration files to the configuration builder.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddNeoDefaultFiles(this IConfigurationBuilder builder)
        {
            try
            {
                // Set location: %ApplicationPath%
                builder.SetBasePath(AppContext.BaseDirectory);
                // "config.json" settings **FILE MUST EXIST**
                builder.AddJsonFile("config.json", optional: false);
            }
            catch (FileNotFoundException)
            {
                throw;
            }

            return builder;
        }
    }
}