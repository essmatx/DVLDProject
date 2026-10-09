using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_Shared
{
    /// <summary>
    /// Provides methods to generate and persist an XML architecture report for types in an assembly that are annotated
    /// with DocInfoAttribute or ArchitectureLayerAttribute.
    /// </summary>
    /// <remarks>GenerateXmlReport returns an XML string representing a DVLD_ArchitectureReport with
    /// attributes for GeneratedDate and TotalDocumentedClasses and a Modules collection of ClassMetadata entries. Each
    /// ClassMetadata includes ClassName and Namespace and may contain Layer, Module, and Version elements when the
    /// corresponding attributes are present. If no assembly is supplied, the calling assembly is used.
    /// SaveXmlReportToFile writes the generated XML string to the specified file path using UTF-8 encoding.</remarks>
    /// 
    [ArchitectureLayer(enArchLayer.DVLD_Shared)]
    [DocInfo("Documentation Generator", Version = "1.0")]
    public static class clsDocGenerator
    {
        /// <summary>
        /// Generates an XML architecture report for types decorated with DocInfoAttribute or ArchitectureLayerAttribute
        /// in the specified assembly.
        /// </summary>
        /// <remarks>Each ClassMetadata element contains ClassName and Namespace and includes optional
        /// Layer, Module, and Version child elements when the corresponding attributes are present.</remarks>
        /// <param name="targetAssembly">Assembly to scan for decorated types; if null, the calling assembly is used.</param>
        /// <returns>An XML document as a string containing the report with root element 'DVLD_ArchitectureReport', generation
        /// timestamp, total documented classes, and a Modules collection of ClassMetadata elements.</returns>
        public static string GenerateXmlReport(Assembly targetAssembly = null)
        {
            if (targetAssembly == null) targetAssembly = Assembly.GetCallingAssembly();

            var decoratedTypes = targetAssembly.GetTypes()
                .Where(t => t.GetCustomAttribute<DocInfoAttribute>() != null ||
                            t.GetCustomAttribute<ArchitectureLayerAttribute>() != null)
                .ToList();

            // Note: Ensure project language version supports the '??=' operator (C# 8.0+). If not, replace with:
            // if (targetAssembly == null) targetAssembly = Assembly.GetCallingAssembly();

            XElement root = new XElement("DVLD_ArchitectureReport",
                new XAttribute("GeneratedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new XAttribute("TotalDocumentedClasses", decoratedTypes.Count),

                new XElement("Modules",
                    decoratedTypes.Select(type =>
                    {
                        var docAttr = type.GetCustomAttribute<DocInfoAttribute>();
                        var archAttr = type.GetCustomAttribute<ArchitectureLayerAttribute>();

                        return new XElement("ClassMetadata",
                            new XAttribute("ClassName", type.Name),
                            new XAttribute("Namespace", type.Namespace ?? string.Empty),

                            archAttr != null ? new XElement("Layer", archAttr.Layer.ToString()) : null,
                            docAttr != null ? new XElement("Module", docAttr.Module) : null,
                            docAttr != null ? new XElement("Version", docAttr.Version) : null
                        );
                    })
                )
            );

            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root).ToString();
        }

        /// <summary>
        /// Saves an XML report to the specified file path.
        /// </summary>
        /// <remarks>The XML content is produced by GenerateXmlReport and written to the file. The file is
        /// overwritten if it exists and is written using UTF-8 encoding.</remarks>
        /// <param name="filePath">The path of the file to create or overwrite with the XML report.</param>
        /// <param name="targetAssembly">The assembly to generate the report for; if null, the executing assembly is used.</param>
        public static void SaveXmlReportToFile(string filePath, Assembly targetAssembly = null)
        {
            string xmlContent = GenerateXmlReport(targetAssembly);
            File.WriteAllText(filePath, xmlContent);
        }
    }
}

