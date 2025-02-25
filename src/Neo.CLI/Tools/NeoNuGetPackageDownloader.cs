// Copyright (C) 2015-2025 The Neo Project.
//
// NeoNuGetPackageDownloader.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.ConsoleService;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Neo.CLI.Tools
{
    internal class NeoNuGetPackageDownloader
    {
        public const string PluginPrefixName = "Neo.Plugins.";
        public const string PluginRootPath = "Plugins";
        public const string SourceFallbackUrl = "https://api.nuget.org/v3/index.json";

        public static readonly string[] DefaultExcludedPackageIds =
        [
            "Neo",
            "Neo.ConsoleService",
            "Neo.Cryptography.BLS12_381",
            "Neo.Extensions",
            "Neo.IO",
            "Neo.Json",
            "Neo.VM",
            .. Directory.EnumerateFiles(AppContext.BaseDirectory, "*.dll", SearchOption.TopDirectoryOnly)
                .Select(static s => Path.GetFileNameWithoutExtension(s)),
        ];

        private const int CancelRequestTimeout = 5000;

        private readonly FindPackageByIdResource _resource;
        private readonly FindPackageByIdResource _nugetResource;
        private readonly SourceCacheContext _cache = new();
        private readonly SourceRepository _repository;
        private readonly SourceRepository _nugetRepository;
        private readonly NuGetVersion _version;
        private readonly string _repoUrl;

        public NeoNuGetPackageDownloader(
            string repoUrl,
            Version? version = null)
        {
            _repoUrl = repoUrl;
            _repository = Repository.Factory.GetCoreV3(_repoUrl);
            _nugetRepository = Repository.Factory.GetCoreV3(SourceFallbackUrl);
            _resource = _repository.GetResource<FindPackageByIdResource>();
            _nugetResource = _nugetRepository.GetResource<FindPackageByIdResource>();

            if (version is not null)
                _version = new(version);
            else
                _version = new(Assembly.GetExecutingAssembly().GetVersion());
        }

        public async Task<IEnumerable<IPackageSearchMetadata>> SearchPackage(string packageId, bool preRelease = false)
        {
            var searchType = preRelease ? SearchFilterType.IsAbsoluteLatestVersion : SearchFilterType.IsLatestVersion;
            var searchFilter = new SearchFilter(preRelease, searchType);
            var resource = _repository.GetResource<PackageSearchResource>();
            var cancelTokenSource = new CancellationTokenSource(CancelRequestTimeout);
            var packages = await resource.SearchAsync(
                packageId,
                searchFilter,
                skip: 0,
                take: 20,
                NullLogger.Instance,
                cancelTokenSource.Token);

            return packages.Where(w => w.Identity.Version == _version);
        }

        public async Task ExtractPackage(string packageId, string directoryName, string? version = null)
        {
            using var packageStream = new MemoryStream();
            var packageVersion = string.IsNullOrEmpty(version) ? _version : new(version);

            await _resource.CopyNupkgToStreamAsync(
                packageId,
                packageVersion,
                packageStream,
                _cache,
                NullLogger.Instance,
                default);

            using var package = new PackageArchiveReader(packageStream);
            var dependPackageInfo = await _resource.GetDependencyInfoAsync(
                packageId,
                packageVersion,
                _cache,
                NullLogger.Instance,
                default);

            var files = package.GetReferenceItems().SelectMany(static sm => sm.Items);
            var contents = package.GetContentItems().SelectMany(static sm => sm.Items);

            var dependPackages = dependPackageInfo.DependencyGroups
                .SelectMany(static sm => sm.Packages)
                .Where(static w => !DefaultExcludedPackageIds
                    .Any(a => w.Id.Equals(a, StringComparison.OrdinalIgnoreCase)));

            var pluginPath = Path.Combine(AppContext.BaseDirectory, PluginRootPath, directoryName);

            ExtractFiles(package, pluginPath, files);
            ExtractFiles(package, pluginPath, contents);

            foreach (var dependPackage in dependPackages)
            {
                try { await ExtractPackage(dependPackage.Id, directoryName, dependPackage.VersionRange.ToShortString()); }
                catch { await ExtractNuGetPackage(dependPackage.Id, directoryName, dependPackage.VersionRange.ToShortString()); }
            }
        }

        private async Task ExtractNuGetPackage(string packageId, string directoryName, string? version = null, bool getAll = true)
        {
            using var packageStream = new MemoryStream();
            var packageVersion = string.IsNullOrEmpty(version) ? _version : new(version);

            await _nugetResource.CopyNupkgToStreamAsync(
                packageId,
                packageVersion,
                packageStream,
                _cache,
                NullLogger.Instance,
                default);

            using var package = new PackageArchiveReader(packageStream);
            var dependPackageInfo = await _nugetResource.GetDependencyInfoAsync(
                packageId,
                packageVersion,
                _cache,
                NullLogger.Instance,
                default);

            var files = package.GetReferenceItems().SelectMany(static sm => sm.Items);
            var contents = package.GetContentItems().SelectMany(static sm => sm.Items);

            var dependPackages = dependPackageInfo.DependencyGroups
                .SelectMany(static sm => sm.Packages)
                .Where(static w => !DefaultExcludedPackageIds
                    .Any(a => w.Id.Equals(a, StringComparison.OrdinalIgnoreCase)));

            var pluginPath = Path.Combine(AppContext.BaseDirectory, PluginRootPath, directoryName);

            ExtractFiles(package, pluginPath, files);
            ExtractFiles(package, pluginPath, contents);

            foreach (var dependPackage in dependPackages)
            {
                if (getAll == false) continue;
                try { await ExtractNuGetPackage(dependPackage.Id, directoryName, dependPackage.VersionRange.ToShortString(), false); }
                catch { }
            }
        }

        private static void ExtractFiles(
            PackageArchiveReader packageReader,
            string directoryName,
            IEnumerable<string> files,
            bool overwrite = true)
        {
            foreach (var file in files)
            {
                var filename = Path.Combine(directoryName, Path.GetFileName(file));

                if (File.Exists(filename)) continue;
                if (overwrite == false) continue;
                ConsoleHelper.Info("", $"Extracting {Path.GetFileNameWithoutExtension(filename)}...");

                _ = packageReader.ExtractFile(file, filename, NullLogger.Instance);
            }
        }
    }
}
