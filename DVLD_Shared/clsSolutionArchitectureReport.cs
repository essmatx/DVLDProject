using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_Shared
{
    /// <summary>
    /// Generates a unified XML architecture report across all solution projects.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="clsDocGenerator"/> which reports on a single assembly,
    /// this class discovers all DVLD project assemblies in the output directory and produces
    /// a consolidated report containing per-project class metadata, a dependency graph,
    /// and a layer breakdown summary. Assemblies that cannot be loaded (e.g., incompatible
    /// target frameworks) are recorded as SkippedAssembly entries rather than causing failures.
    /// </remarks>
    /// 
    [ArchitectureLayer(enArchLayer.DVLD_Shared)]
    [DocInfo("Documentation Generator", Version = "1.0")]
    public static class clsSolutionArchitectureReport
    {
        // Assembly name prefixes used to identify DVLD solution project assemblies.
        private static readonly string[] _solutionAssemblyPrefixes =
        {
            "DVLD_",
            "DVLDManagePeople-"
        };

        /// <summary>
        /// Generates a unified XML architecture report for all DVLD solution assemblies
        /// found in the specified base directory.
        /// </summary>
        /// <remarks>
        /// The report scans <paramref name="baseDirectory"/> (or <see cref="AppDomain.CurrentDomain.BaseDirectory"/>
        /// when null) for DLL and EXE files whose names start with a known DVLD prefix. Each loadable assembly
        /// becomes a Project element containing its decorated types, and a DependencyGraph section captures
        /// inter-project references. A SolutionSummary provides aggregate class counts per architecture layer.
        /// </remarks>
        /// <param name="baseDirectory">
        /// Directory to scan for assemblies. When null, defaults to the current AppDomain base directory.
        /// </param>
        /// <returns>
        /// An XML document string with root element DVLD_SolutionArchitectureReport containing
        /// SolutionSummary, DependencyGraph, and Projects sections.
        /// </returns>
        public static string GenerateUnifiedReport(string baseDirectory = null)
        {
            if (baseDirectory == null)
                baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // --- Phase 1: Discover and load assemblies ---
            var loadedAssemblies = new List<Assembly>();
            var skippedAssemblies = new List<KeyValuePair<string, string>>(); // name -> reason

            IEnumerable<string> assemblyFiles = _FindSolutionAssemblyFiles(baseDirectory);

            foreach (string filePath in assemblyFiles)
            {
                try
                {
                    Assembly asm = Assembly.LoadFrom(filePath);
                    loadedAssemblies.Add(asm);
                }
                catch (Exception ex)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    skippedAssemblies.Add(new KeyValuePair<string, string>(fileName, ex.Message));
                }
            }

            // Build a set of loaded assembly names for dependency graph filtering.
            var loadedNames = new HashSet<string>(
                loadedAssemblies.Select(a => a.GetName().Name),
                StringComparer.OrdinalIgnoreCase);

            // --- Phase 2: Extract per-project metadata ---
            var projectElements = new List<XElement>();
            int totalDocumentedClasses = 0;

            // Layer -> aggregate class count across all projects
            var layerCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (Assembly assembly in loadedAssemblies)
            {
                string assemblyName = assembly.GetName().Name;

                // Discover decorated types in this assembly.
                List<Type> decoratedTypes;
                try
                {
                    decoratedTypes = assembly.GetTypes()
                        .Where(t => t.GetCustomAttribute<DocInfoAttribute>() != null ||
                                    t.GetCustomAttribute<ArchitectureLayerAttribute>() != null)
                        .ToList();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    // Some types failed to load; work with whatever loaded successfully.
                    decoratedTypes = ex.Types
                        .Where(t => t != null &&
                                    (t.GetCustomAttribute<DocInfoAttribute>() != null ||
                                     t.GetCustomAttribute<ArchitectureLayerAttribute>() != null))
                        .ToList();
                }

                // Determine the project-level layer label.
                string projectLayer = _InferProjectLayer(assemblyName, decoratedTypes);

                // Build ClassMetadata elements.
                var classElements = new List<XElement>();
                foreach (Type type in decoratedTypes)
                {
                    var docAttr = type.GetCustomAttribute<DocInfoAttribute>();
                    var archAttr = type.GetCustomAttribute<ArchitectureLayerAttribute>();

                    string classLayer = archAttr != null
                        ? archAttr.Layer.ToString()
                        : projectLayer;

                    classElements.Add(new XElement("ClassMetadata",
                        new XAttribute("ClassName", type.Name),
                        new XAttribute("Namespace", type.Namespace ?? string.Empty),
                        archAttr != null ? new XElement("Layer", archAttr.Layer.ToString()) : null,
                        docAttr != null ? new XElement("Module", docAttr.Module) : null,
                        docAttr != null ? new XElement("Version", docAttr.Version) : null
                    ));

                    // Accumulate layer counts.
                    if (layerCounts.ContainsKey(classLayer))
                        layerCounts[classLayer]++;
                    else
                        layerCounts[classLayer] = 1;
                }

                totalDocumentedClasses += classElements.Count;

                projectElements.Add(new XElement("Project",
                    new XAttribute("AssemblyName", assemblyName),
                    new XAttribute("Layer", projectLayer),
                    new XAttribute("DocumentedClasses", classElements.Count),
                    new XElement("Classes", classElements)
                ));
            }

            // --- Phase 3: Build dependency graph ---
            var dependencyElements = new List<XElement>();
            foreach (Assembly assembly in loadedAssemblies)
            {
                string assemblyName = assembly.GetName().Name;

                List<string> dependsOn = assembly.GetReferencedAssemblies()
                    .Select(r => r.Name)
                    .Where(n => loadedNames.Contains(n) &&
                                !string.Equals(n, assemblyName, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (dependsOn.Count > 0)
                {
                    dependencyElements.Add(new XElement("ProjectDependency",
                        new XAttribute("Project", assemblyName),
                        new XAttribute("DependsOn", string.Join(", ", dependsOn))
                    ));
                }
            }

            // --- Phase 4: Build solution summary ---
            var layerBreakdownElements = layerCounts
                .OrderByDescending(kv => kv.Value)
                .Select(kv => new XElement("Layer",
                    new XAttribute("Name", kv.Key),
                    new XAttribute("ClassCount", kv.Value)))
                .ToList();

            // --- Phase 5: Compose the final document ---
            XElement root = new XElement("DVLD_SolutionArchitectureReport",
                new XAttribute("GeneratedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new XAttribute("TotalProjects", loadedAssemblies.Count),
                new XAttribute("TotalDocumentedClasses", totalDocumentedClasses),

                new XElement("SolutionSummary",
                    new XElement("LayerBreakdown", layerBreakdownElements)
                ),

                new XElement("DependencyGraph", dependencyElements),

                skippedAssemblies.Count > 0
                    ? new XElement("SkippedAssemblies",
                        skippedAssemblies.Select(kv => new XElement("SkippedAssembly",
                            new XAttribute("Name", kv.Key),
                            new XAttribute("Reason", kv.Value))))
                    : null,

                new XElement("Projects", projectElements)
            );

            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root).ToString();
        }

        /// <summary>
        /// Generates the unified architecture report and saves it to the specified file path.
        /// </summary>
        /// <remarks>
        /// The file is created (or overwritten) with UTF-8 encoding. The XML content is produced
        /// by <see cref="GenerateUnifiedReport"/>.
        /// </remarks>
        /// <param name="filePath">The file path to write the XML report to.</param>
        /// <param name="baseDirectory">
        /// Directory to scan for assemblies. When null, defaults to the current AppDomain base directory.
        /// </param>
        public static void SaveUnifiedReportToFile(string filePath, string baseDirectory = null)
        {
            string xmlContent = GenerateUnifiedReport(baseDirectory);
            File.WriteAllText(filePath, xmlContent, Encoding.UTF8);
        }

        /// <summary>
        /// Discovers assembly files (DLL and EXE) in the given directory whose file names
        /// match known DVLD solution project prefixes.
        /// </summary>
        /// <param name="directory">The directory to scan.</param>
        /// <returns>Full paths of matching assembly files.</returns>
        private static IEnumerable<string> _FindSolutionAssemblyFiles(string directory)
        {
            if (!Directory.Exists(directory))
                return Enumerable.Empty<string>();

            var matchingFiles = new List<string>();

            foreach (string file in Directory.GetFiles(directory, "*.*"))
            {
                string extension = Path.GetExtension(file);
                if (!string.Equals(extension, ".dll", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(extension, ".exe", StringComparison.OrdinalIgnoreCase))
                    continue;

                string fileName = Path.GetFileNameWithoutExtension(file);

                foreach (string prefix in _solutionAssemblyPrefixes)
                {
                    if (fileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        matchingFiles.Add(file);
                        break;
                    }
                }
            }

            return matchingFiles;
        }

        /// <summary>
        /// Infers the architecture layer for a project based on its assembly name and
        /// the layer attributes found on its types.
        /// </summary>
        /// <remarks>
        /// If any type in <paramref name="decoratedTypes"/> carries an <see cref="ArchitectureLayerAttribute"/>,
        /// the most common layer value is used. Otherwise the assembly name itself serves as the layer label.
        /// </remarks>
        /// <param name="assemblyName">The simple name of the assembly.</param>
        /// <param name="decoratedTypes">Types in the assembly that carry documentation attributes.</param>
        /// <returns>A string representing the inferred architecture layer.</returns>
        private static string _InferProjectLayer(string assemblyName, List<Type> decoratedTypes)
        {
            // Try to determine layer from the most common ArchitectureLayerAttribute value.
            var layerGroups = decoratedTypes
                .Select(t => t.GetCustomAttribute<ArchitectureLayerAttribute>())
                .Where(a => a != null)
                .GroupBy(a => a.Layer)
                .OrderByDescending(g => g.Count())
                .ToList();

            if (layerGroups.Count > 0)
                return layerGroups.First().Key.ToString();

            // Fallback: derive layer from the assembly name.
            return assemblyName;
        }
    }
}
