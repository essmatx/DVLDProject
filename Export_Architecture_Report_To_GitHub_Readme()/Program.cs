using DVLD_Shared;
using DVLDManagePeople_PresentationLayer.Global_Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Export_Architecture_Report_To_GitHub_Readme__
{
   
    internal class Program
    {
        static void Main(string[] args)
        {
            //string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DVLD_SolutionArchitectureReport.xml");
            //string readmePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "README.md");

            //// 1. Generate XML Report
            //clsSolutionArchitectureReport.SaveUnifiedReportToFile(xmlPath);

            //// 2. Convert XML to Markdown
            //clsMarkdownDocExporter.ConvertXmlToMarkdownFile(xmlPath, readmePath);

            //Console.WriteLine($"README.md generated successfully at: {readmePath}");

            //// Auto-open directory
            //if (File.Exists(readmePath))
            //{
            //    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{readmePath}\"");
            //}

            string Username = "user4";
            string Password = "1234";
            clsGlobal.SaveUserNameAndPasswordinReg(Username, Password);
        }
    }
}
