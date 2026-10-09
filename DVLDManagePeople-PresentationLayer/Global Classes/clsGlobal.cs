using DVLD_Business;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLDManagePeople_PresentationLayer.Global_Classes
{
    /// <summary>
    /// Provides global utilities for managing the current logged-in user session and persisting remember-me credentials
    /// to a local file.
    /// </summary>
    /// <remarks>Static, non-thread-safe helper that exposes CurrentUser and methods to save and retrieve
    /// credentials. Credentials are written to and read from "data.txt" in the application's current working directory;
    /// calling RememberUsernameAndPassword with an empty username deletes the file. Methods perform file I/O and show a
    /// MessageBox on exceptions. Storing passwords in plain text is insecure; callers should protect sensitive data and
    /// consider secure credential storage mechanisms.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Global utility class managing current logged-in user session state and local file storage for remember-me credentials.", Module = "Global Utilities", Version = "1.0")]
    public static class clsGlobal
    {
        #region Private Fields

        /// <summary>
        /// Stores the current ClsUserBuiness instance.
        /// </summary>
        /// <remarks>Static field shared across all instances. Access is not synchronized; ensure
        /// thread-safe access if used concurrently.</remarks>
        private static ClsUserBuiness _currentUser = null;
        #endregion

        #region Public Session Properties

        /// <summary>
        /// Gets or sets the current user for the application session.
        /// </summary>
        /// <remarks>May be null when no user is authenticated. Access is not synchronized; provide
        /// external synchronization in multithreaded scenarios if required.</remarks>
        public static ClsUserBuiness CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
        #endregion

        #region Public Credential Persistence 

        /// <summary>
      /// Persist user credentials to a local data file or remove the file when the username is empty.
      /// </summary>
      /// <remarks>Writes to 'data.txt' in the application's current working directory using the format
      /// 'Username#//#Password'. If Username is empty and the file exists, the file is deleted. Exceptions are shown
      /// via a message box and cause the method to return false. Storing credentials in plain text is insecure; prefer
      /// secure storage APIs such as the Windows Credential Manager or ProtectedData for sensitive data.</remarks>
      /// <param name="Username">Username to store; when empty, deletes the credential file if it exists.</param>
      /// <param name="Password">Password to store alongside the username in the credential file.</param>
      /// <returns>True if the credentials were saved or the file was removed; false if an error occurred.</returns>
        [DocInfo("Persists user credentials to a local data file or removes the file if credentials are empty.")]
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            try
            {
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                string filePath = currentDirectory + "\\data.txt"; 

                if(Username == "" && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true; 

                }

                string dataToSave = Username + "#//#" + Password; 

                using(StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(dataToSave);

                    return true; 
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }


        public static void SaveUserNameAndPasswordinReg(string Username, string Password)
        {
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\MyUsernameAndPassword";

          

           

            try
            {
                Registry.SetValue(keyPath, Username, Password, RegistryValueKind.String);

                Console.WriteLine($"User Name {Username} has written successfully in Registry"); 
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error " + ex.Message); 
            }
        }

        /// <summary>
        /// Reads credentials from a local data.txt in the current working directory, parses a '#//#' delimited line,
        /// and assigns the parsed username and password to the provided ref parameters.
        /// </summary>
        /// <remarks>Locates data.txt using Directory.GetCurrentDirectory(). Reads the file line by line
        /// and uses the last parsed line if multiple exist. Expects at least two tokens separated by '#//#'. Exceptions
        /// display a message box and cause the method to return false.</remarks>
        /// <param name="Username">Set to the first token parsed from the file's '#//#' delimited line.</param>
        /// <param name="Password">Set to the second token parsed from the file's '#//#' delimited line.</param>
        /// <returns>True if the file was found and credentials were successfully parsed and assigned; false if the file does not
        /// exist or an error occurs.</returns>
        [DocInfo("Reads and parses local credentials file using '#/#' delimiter into output parameter references.")]
        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            try
            {
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                string filePath = currentDirectory + "\\data.txt"; 

                if(File.Exists(filePath))
                {
                    using(StreamReader reader =  new StreamReader(filePath))
                    {
                        string line; 

                        while((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line);

                            string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);


                            Username = result[0];
                            Password = result[1]; 
                        }

                        return true; 

                    }
                }
                else
                {
                    return false; 
                }
            }catch(Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
