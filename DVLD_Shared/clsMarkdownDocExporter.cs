using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_Shared
{
    /// <summary>
    /// Converts XML architecture reports into GitHub-flavored Markdown README content, including Mermaid diagrams for
    /// dependency graphs.
    /// </summary>
    /// <remarks>Provides static methods to convert an XDocument to a Markdown string and to write the
    /// Markdown to a UTF-8 file. Throws ArgumentNullException if the provided XDocument or its Root is null and
    /// FileNotFoundException if the input XML file does not exist. Methods are stateless and safe for concurrent use.
    /// Mermaid node identifiers are sanitized by replacing '-' and '.' with '_'.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_Shared)]
    [DocInfo("Converts XML architecture reports into clean GitHub README Markdown with Mermaid diagrams.", Module = "Documentation Engine", Version = "1.0")]
    public static class clsMarkdownDocExporter
    {
        public static string ConvertXmlToMarkdown(XDocument doc)
        {
            if (doc == null || doc.Root == null)
                throw new ArgumentNullException(nameof(doc));

            XElement root = doc.Root;
            StringBuilder sb = new StringBuilder();

            // 1. Header & Badges
            sb.AppendLine("# 🚗 DVLD — Driver & Vehicle Licensing Department System");
            sb.AppendLine();
            sb.AppendLine("[![Language](https://img.shields.io/badge/Language-C%23%20%2F%20.NET-purple.svg)](https://dotnet.microsoft.com/)");
            sb.AppendLine("[![Architecture](https://img.shields.io/badge/Architecture-Layered%20%2F%20N--Tier-blue.svg)]()");
            sb.AppendLine("[![Database](https://img.shields.io/badge/Database-SQL%20Server-red.svg)](https://www.microsoft.com/sql-server/)");
            sb.AppendLine("[![Testing](https://img.shields.io/badge/Testing-NUnit-green.svg)](https://nunit.org/)");
            sb.AppendLine();
            sb.AppendLine("A robust desktop enterprise application built with **C#**, **WinForms**, and **SQL Server**.");
            sb.AppendLine();
            sb.AppendLine("---");
            sb.AppendLine();

            // 2. System Summary Table
            sb.AppendLine("## 🏗️ Architectural Overview & Summary");
            sb.AppendLine();
            sb.AppendLine("### 📊 Metadata Summary");
            sb.AppendLine();
            sb.AppendLine("| Metric | Value |");
            sb.AppendLine("| :--- | :--- |");
            sb.AppendLine($"| **Generated Date** | `{root.Attribute("GeneratedDate")?.Value}` |");
            sb.AppendLine($"| **Total Projects** | `{root.Attribute("TotalProjects")?.Value}` |");
            sb.AppendLine($"| **Total Documented Classes** | `{root.Attribute("TotalDocumentedClasses")?.Value}` |");
            sb.AppendLine();

            // Layer Breakdown Table
            var layerBreakdown = root.Element("SolutionSummary")?.Element("LayerBreakdown")?.Elements("Layer");
            if (layerBreakdown != null && layerBreakdown.Any())
            {
                sb.AppendLine("### 🧱 Layer Breakdown");
                sb.AppendLine();
                sb.AppendLine("| Layer Name | Class Count |");
                sb.AppendLine("| :--- | :---: |");
                foreach (var layer in layerBreakdown)
                {
                    sb.AppendLine($"| **`{layer.Attribute("Name")?.Value}`** | `{layer.Attribute("ClassCount")?.Value}` |");
                }
                sb.AppendLine();
            }

            sb.AppendLine("---");
            sb.AppendLine();

            // 3. Dependency Graph Section (Mermaid Diagram)
            var dependencies = root.Element("DependencyGraph")?.Elements("ProjectDependency").ToList();
            if (dependencies != null && dependencies.Any())
            {
                sb.AppendLine("## 🔄 Project Dependency Graph");
                sb.AppendLine();
                sb.AppendLine("```mermaid");
                sb.AppendLine("graph TD");

                foreach (var dep in dependencies)
                {
                    string proj = dep.Attribute("Project")?.Value;
                    string dependsOnStr = dep.Attribute("DependsOn")?.Value;
                    if (!string.IsNullOrEmpty(dependsOnStr))
                    {
                        string[] targets = dependsOnStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var target in targets)
                        {
                            sb.AppendLine($"    {_SanitizeMermaidId(proj)} --> {_SanitizeMermaidId(target.Trim())}");
                        }
                    }
                }

                sb.AppendLine("```");
                sb.AppendLine();
            }

            // 4. Per-Project Class Metadata
            sb.AppendLine("## 📋 Detailed Component & Class Metadata");
            sb.AppendLine();

            var projects = root.Element("Projects")?.Elements("Project");
            if (projects != null)
            {
                foreach (var proj in projects)
                {
                    string projName = proj.Attribute("AssemblyName")?.Value;
                    string projLayer = proj.Attribute("Layer")?.Value;
                    var classes = proj.Element("Classes")?.Elements("ClassMetadata").ToList();

                    sb.AppendLine($"### 📦 Project: `{projName}` (`{projLayer}`)");
                    sb.AppendLine();

                    if (classes != null && classes.Any())
                    {
                        sb.AppendLine("| Class Name | Namespace | Layer | Module | Version | Description |");
                        sb.AppendLine("| :--- | :--- | :--- | :--- | :---: | :--- |");

                        foreach (var cls in classes)
                        {
                            string className = cls.Attribute("ClassName")?.Value;
                            string ns = cls.Attribute("Namespace")?.Value;
                            string layer = cls.Element("Layer")?.Value ?? "-";
                            string module = cls.Element("Module")?.Value ?? "-";
                            string version = cls.Element("Version")?.Value ?? "-";
                            string desc = cls.Element("Description")?.Value ?? "-";

                            sb.AppendLine($"| `{className}` | `{ns}` | `{layer}` | {module} | `{version}` | {desc} |");
                        }
                    }
                    else
                    {
                        sb.AppendLine("*No classes currently decorated with attributes in this project.*");
                    }

                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        public static void ConvertXmlToMarkdownFile(string xmlFilePath, string markdownOutputPath)
        {
            if (!File.Exists(xmlFilePath))
                throw new FileNotFoundException("XML report file not found.", xmlFilePath);

            XDocument doc = XDocument.Load(xmlFilePath);
            string markdownContent = ConvertXmlToMarkdown(doc);

            File.WriteAllText(markdownOutputPath, markdownContent, Encoding.UTF8);
        }

        private static string _SanitizeMermaidId(string name)
        {
            return name.Replace("-", "_").Replace(".", "_");
        }
    }
}
