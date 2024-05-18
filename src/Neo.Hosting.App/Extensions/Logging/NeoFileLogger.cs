// Copyright (C) 2015-2024 The Neo Project.
//
// NeoFileLogger.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.Extensions.Logging;
using Neo.Hosting.App.Configuration.Logging;
using System;
using System.IO;

namespace Neo.Hosting.App.Extensions.Logging
{
    internal sealed class NeoFileLogger(
        string categoryName,
        Func<NeoFileLoggerOptions> getCurrentConfig) : ILogger
    {
        private const string DefaultFileNameDateFormat = "{0:MM.dd.yyyy}";

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
            default!;

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) =>
            logLevel >= getCurrentConfig().LogLevel;

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel) == false)
                return;

            var config = getCurrentConfig();
            var dateTime = config.UseUtcTimestamp ? DateTime.UtcNow : DateTime.Now;

            var fileNameDateString = string.Format(DefaultFileNameDateFormat, dateTime);
            var dateTimeString = dateTime.ToString(config.TimestampFormat);

            var output = string.Format("[{0}] {1}: {2}[{3}] {4}{5}",
                dateTimeString, eventId.Name, categoryName, eventId.Id,
                formatter(state, exception), Environment.NewLine);

            var fileName = Path.Combine(config.OutputDirectory, $"{fileNameDateString}{config.OutputFileExtension}");

            File.AppendAllText(fileName, output);
        }
    }
}
