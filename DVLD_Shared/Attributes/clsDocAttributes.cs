using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DVLD_Shared.Attributes
{
    public static class clsDocAttributes
    {
        public enum enArchLayer
        {
            DVLD_PresentationLayer,
            DVLD_Business,
            DVLD_DataAccess,
            DatabaseObject,
            DVLD_UintTest,
            DVLD_Shared,

           DVLD_BusinessWorkflows
        }

        // Class & Interface Layer Tag
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface,Inherited = false)]
        public class ArchitectureLayerAttribute : Attribute
        {
            public enArchLayer Layer { get; }

            public ArchitectureLayerAttribute(enArchLayer layer)
            {
                Layer = layer; 
            }
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method |AttributeTargets.Property,AllowMultiple = false )]
        public class DocInfoAttribute : Attribute
        {
            public string Summary { get; }

            public string Module { get; set; } = "DVLD Core";
            public string Author { get; set; } = "System";
            public string Version { get; set; } = "1.0";
            public DocInfoAttribute(string summary)
            {
                Summary = summary; 
            }
        }


        [AttributeUsage(AttributeTargets.Method,Inherited = false)]
        public class StoredProcedureAttribute : Attribute
        {
            public string ProcedureName { get; }

            public StoredProcedureAttribute(string spName)
            {
                ProcedureName = spName; 
            }
        }

    }
}
