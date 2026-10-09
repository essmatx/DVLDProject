using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLDManagePeople_PresentationLayer.Global_Classes
{
    /// <summary>
    /// Provides static helper methods for GUID generation, file system operations, and copying images into the project
    /// image folder.
    /// </summary>
    /// <remarks>All members are static. CreateFolderIfNotExist creates the target directory and shows an
    /// error MessageBox on failure. ReplaceFileNameWithGUID returns a filename composed of a new GUID plus the original
    /// file extension. CopyImageToProjectImageFolder copies a file to a fixed folder (C:\DVLD-People-Images\), updates
    /// the ref parameter to the destination path, and reports IO errors via MessageBox. Callers should validate paths,
    /// handle permissions and IO errors, and be aware the class performs UI interactions and is intended for
    /// presentation-layer use.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_PresentationLayer)]
    [DocInfo("Utility class providing system file IO operations, GUID generation, and image media persistence helpers.", Module = "Global Utilities", Version = "1.0")]
    public class clsUtil
    {
        #region Public Utility

        /// <summary>
        /// Generates a new GUID and returns its string representation in the standard hyphen-separated (D) format.
        /// </summary>
        /// <remarks>Uses Guid.NewGuid() to create the value and Guid.ToString() for formatting.</remarks>
        /// <returns>A string containing the GUID in the standard hyphen-separated (D) format.</returns>
        [DocInfo("Generates a standard string representation of a new GUID.")]
        public static string GenerateGUID()
        {
            Guid newGuid = Guid.NewGuid();

            return newGuid.ToString();
        }

        /// <summary>
        /// Verifies whether the target directory exists and creates it if it does not.
        /// </summary>
        /// <remarks>Creates any necessary parent directories. Exceptions thrown during creation are
        /// caught and reported via a message box; no exception is propagated to the caller.</remarks>
        /// <param name="FolderPath">The path of the directory to verify or create.</param>
        /// <returns>True if the directory exists or was created successfully; otherwise, false.</returns>

        [DocInfo("Verifies existence of target directory and creates it if not already present.")]
        public static bool CreateFolderIfNotExist(string FolderPath)
        {
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Creating folder: " + ex.Message);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Generates a filename using a new GUID while preserving the original file's extension.
        /// </summary>
        /// <remarks>If sorceFile has no extension, the returned filename contains only the
        /// GUID.</remarks>
        /// <param name="sorceFile">Source file path from which the extension is extracted.</param>
        /// <returns>A filename consisting of a generated GUID followed by the original file extension (including the leading
        /// '.').</returns>

        [DocInfo("Extracts extension from source file path and appends it to a newly generated GUID.")]
        public static string ReplaceFileNameWithGUID(string sorceFile)
        {
            string fileName = sorceFile;

            FileInfo fi = new FileInfo(fileName);
            string extn = fi.Extension;
            return GenerateGUID() + extn;
        }

        /// <summary>
        /// Ensures the project's image folder exists, generates a GUID-based filename for the source file, and copies
        /// the file into the project's image folder.
        /// </summary>
        /// <remarks>Uses the fixed destination folder "C:\DVLD-People-Images\". Calls
        /// CreateFolderIfNotExist and ReplaceFileNameWithGUID. Performs File.Copy with overwrite enabled. On
        /// IOException, displays a MessageBox with the error message and returns true. After a successful copy,
        /// sourceFile is updated to the new destination path.</remarks>
        /// <param name="sourceFile">Path of the source file; passed by reference and replaced with the destination file path after a successful
        /// copy.</param>
        /// <returns>True if the file was copied to the project's image folder or if an I/O error occurred during copying; false
        /// if the target directory could not be created.</returns>
        [DocInfo("Ensures target media directory exists, generates unique GUID filename, and copies file to project storage.")]
        public static bool CopyImageToProjectImageFolder(ref string sourceFile)
        {
            string DestinationFolder = @"C:\DVLD-People-Images\";

            if (!CreateFolderIfNotExist(DestinationFolder))
            {
                return false;
            }

            string destinationFile = DestinationFolder + ReplaceFileNameWithGUID(sourceFile);

            try
            {
                File.Copy(sourceFile, destinationFile, true);
            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            sourceFile = destinationFile;

            return true;
        }

        /// <summary>
        /// Computes a hex-encoded SHA-256 hash for the specified UTF-8 plain-text string.
        /// </summary>
        /// <remarks>Uses SHA-256 via SHA256.Create() and UTF-8 encoding; produces contiguous lowercase
        /// hex digits. Not suitable for password storage—use a salted key-derivation function (for example PBKDF2 or
        /// Argon2) for that purpose.</remarks>
        /// <param name="input">Plain-text string to hash.</param>
        /// <returns>Lowercase hexadecimal representation of the SHA-256 hash, or an empty string if input is null or empty.</returns>
        [DocInfo("Computes a hex-encoded SHA-256 cryptographic hash representation for plain-text strings.")]
        public static string ComputeHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        #endregion
    }
}