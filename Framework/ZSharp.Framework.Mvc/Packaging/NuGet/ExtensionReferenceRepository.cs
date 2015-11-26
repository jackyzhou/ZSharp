﻿using System.Collections.Generic;
using System.Linq;
using NuGet;
using ZSharp.Framework.Utils;
using ZSharp.Framework.Mvc.Plugins;

namespace ZSharp.Framework.Mvc.Packaging
{
    internal abstract class ExtensionReferenceRepository : PackageRepositoryBase
    {

        public ExtensionReferenceRepository(IProjectSystem project, IPackageRepository sourceRepository)
        {
            GuardHelper.ArgumentNotNull(() => project);
            GuardHelper.ArgumentNotNull(() => sourceRepository);

            Project = project;
            SourceRepository = sourceRepository;
        }

        public IProjectSystem Project
        {
            get;
            set;
        }

        public IPackageRepository SourceRepository
        {
            get;
            set;
        }

        public override void AddPackage(IPackage package) { }

        public override void RemovePackage(IPackage package) { }


        public override string Source
        {
            get { return Project.Root; }
        }

        public override bool SupportsPrereleasePackages
        {
            get { return true; }
        }
    }

    /// <summary>
    /// This repository implementation informs about what plugin packages are already installed.
    /// </summary>
    internal class PluginReferenceRepository : ExtensionReferenceRepository
    {
        private readonly IPluginFinder _pluginFinder;
        private readonly IList<PluginDescriptor> _descriptors;

        public PluginReferenceRepository(IProjectSystem project, IPackageRepository sourceRepository, IPluginFinder pluginFinder)
            : base(project, sourceRepository)
        {
            _pluginFinder = pluginFinder;
            _descriptors = _pluginFinder.GetPluginDescriptors().ToList();
        }

        public override IQueryable<IPackage> GetPackages()
        {
            IEnumerable<IPackage> repositoryPackages = SourceRepository.GetPackages().ToList();
            IEnumerable<IPackage> packages = from plugin in _descriptors
                                             let id = PackagingUtils.BuildPackageId(plugin.SystemName, "Plugin")
                                             let version = plugin.Version != null ? new SemanticVersion(plugin.Version) : null
                                             let package = repositoryPackages.FirstOrDefault(p => p.Id == id && (version == null || p.Version == version))
                                             where package != null
                                             select package;

            return packages.AsQueryable();
        }

    }
}