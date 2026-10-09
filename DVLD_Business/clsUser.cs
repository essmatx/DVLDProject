using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_Business
{
    /// <summary>
    /// Core security domain entity that manages user accounts, credential validation, system authentication, and person
    /// identity linkage.
    /// </summary>
    /// <remarks>Inherits from ClsPerson to combine personal identity with account credentials. Persists user
    /// information via clsUserData and exposes lookup and retrieval APIs (FindUserByID, FindUserByUserName,
    /// FindUserByUserNameAndPassowrd, GetAllUsers). Provides lifecycle operations (Save, DeleteUser, DoesUserExists)
    /// where Save coordinates add versus update using an internal Mode and delegates person persistence to the base
    /// type. Loging returns an authenticated, active user instance when credentials match.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Core security domain entity managing user accounts, credential validation, system authentication, and person identity linkage.", Module = "User Management", Version = "1.0")]
    public class ClsUserBuiness : ClsPerson
    {
        #region Enums & Modes
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        /// <remarks>Used to identify the user within the application or persistent storage.</remarks>
        public int UserID { set; get; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// Password used for authentication.
        /// </summary>
        /// <remarks>Treat as sensitive information; avoid logging or storing in plaintext. Prefer secure
        /// alternatives (for example, SecureString or a char[] that can be cleared) and clear the value from memory as
        /// soon as possible.</remarks>
        public string Password { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether the instance is active.
        /// </summary>
        public bool IsActive { set; get; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of ClsUserBuiness and sets default property values: UserID = -1, UserName and
        /// Password to empty strings, IsActive to true, and Mode to enMode._AddNew.
        /// </summary>
        public ClsUserBuiness()
        {
            this.UserID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = true;

            this.Mode = enMode._AddNew;
        }

        /// <summary>
        /// Initializes a ClsUserBuiness instance populated with person and user data for updating an existing user.
        /// </summary>
        /// <remarks>Passes person-related values to the base constructor and sets Mode to
        /// enMode._Update.</remarks>
        /// <param name="UserID">User's unique identifier.</param>
        /// <param name="PersonID">Person record identifier.</param>
        /// <param name="NationalNo">National identification number.</param>
        /// <param name="FirstName">First given name.</param>
        /// <param name="SecondName">Second given name.</param>
        /// <param name="ThirdName">Third given name.</param>
        /// <param name="LastName">Family name.</param>
        /// <param name="DateOfBirth">Date of birth.</param>
        /// <param name="Gendor">Gender code.</param>
        /// <param name="Address">Postal or residential address.</param>
        /// <param name="Phone">Phone number.</param>
        /// <param name="Email">Email address.</param>
        /// <param name="NationalityCountryID">Identifier of the nationality country.</param>
        /// <param name="ImagePath">Path to the user's image file.</param>
        /// <param name="UserName">Login username.</param>
        /// <param name="Password">Login password.</param>
        /// <param name="IsActive">True if the account is active; otherwise false.</param>
        private ClsUserBuiness(int UserID, int PersonID, string NationalNo, string FirstName,
                        string SecondName, string ThirdName, string LastName,
                        DateTime DateOfBirth, short Gendor, string Address,
                        string Phone, string Email, int NationalityCountryID,
                        string ImagePath, string UserName, string Password, bool IsActive)

     : base(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
            DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath)
        {
            this.UserID = UserID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            this.Mode = enMode._Update;
        }
        #endregion

        #region Private CRUD Helpers

        /// <summary>
        /// Inserts a new user record, assigns the generated UserID to the instance, and returns true if the operation
        /// succeeds.
        /// </summary>
        /// <remarks>Sets the instance UserID by calling the data access layer to insert the user using
        /// the instance PersonID, UserName, Password, and IsActive values.</remarks>
        /// <returns>True if a new user was created and UserID is greater than zero; otherwise false.</returns>
        private bool _AddNewUser()
        {
            this.UserID = clsUserData.InsertNewUser(this.PersonID, this.UserName, this.Password, this.IsActive);

            return (this.UserID > 0);
        }

        /// <summary>
        /// Updates the user record identified by UserID using the current PersonID, UserName, Password, and IsActive
        /// values.
        /// </summary>
        /// <remarks>Delegates to clsUserData.UpdateUser and relies on its validation and persistence
        /// behavior.</remarks>
        /// <returns>true if the update succeeded; otherwise, false.</returns>
        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive);
        }
        #endregion

        #region Factory & Search Methods
        /// <summary>
        /// Finds and returns a populated ClsUserBuiness for the specified UserID, or null if no matching user or linked
        /// person is found.
        /// </summary>
        /// <remarks>Performs a data-layer lookup and hydrates the linked ClsPerson details into the
        /// returned object; returns null if the user or linked person record is not found.</remarks>
        /// <param name="UserID">User primary key to locate.</param>
        /// <returns>A ClsUserBuiness populated with user and linked person data if found; otherwise null.</returns>
        [DocInfo("Finds and hydrates a user record and linked person entity by primary key UserID.")]
        public static ClsUserBuiness FindUserByID(int UserID)
        {
            int PersonID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;

            if (clsUserData.FindUserByID(ref UserID, ref PersonID, ref UserName, ref Password, ref IsActive))
            {
                // Get the full Person details using the PersonID we just found
                ClsPerson PersonInfo = ClsPerson.FindPersonByID(PersonID);

                if (PersonInfo != null)
                {
                    return new ClsUserBuiness(
                        UserID,
                        PersonID,
                        PersonInfo.NationalNo,
                        PersonInfo.FirstName,
                        PersonInfo.SecondName,
                        PersonInfo.ThirdName,
                        PersonInfo.LastName,
                        PersonInfo.DateOfBirth,
                        PersonInfo.Gendor,
                        PersonInfo.Address,
                        PersonInfo.Phone,
                        PersonInfo.Email,
                        PersonInfo.NationalityCountryID,
                        PersonInfo.ImagePath,
                        UserName,
                        Password,
                        IsActive
                    );
                }
            }

            return null;

        }

        /// <summary>
        /// Finds and hydrates a user record by username and returns a populated ClsUserBuiness instance.
        /// </summary>
        /// <remarks>Calls the data-access layer which may modify the provided username and hydrates the
        /// returned object with related person information.</remarks>
        /// <param name="UserName">Username to locate the user record.</param>
        /// <returns>A populated ClsUserBuiness for the specified username, or null if no matching user or associated person
        /// record is found.</returns>
        [DocInfo("Finds and hydrates a user record by username string.")]
        public static ClsUserBuiness FindUserByUserName(string UserName)
        {
            int UserID = -1;
            int PersonID = -1;
            string Password = "";
            bool IsActive = false;

            if (clsUserData.FindUserByUserName(ref UserID, ref PersonID, ref UserName, ref Password, ref IsActive))
            {
                ClsPerson PersonInfo = ClsPerson.FindPersonByID(PersonID);

                if (PersonInfo != null)
                {
                    return new ClsUserBuiness
                        (
                        UserID,
                        PersonID,
                        PersonInfo.NationalNo,
                        PersonInfo.FirstName,
                        PersonInfo.SecondName,
                        PersonInfo.ThirdName,
                        PersonInfo.LastName,
                        PersonInfo.DateOfBirth,
                        PersonInfo.Gendor,
                        PersonInfo.Address,
                        PersonInfo.Phone,
                        PersonInfo.Email,
                        PersonInfo.NationalityCountryID,
                        PersonInfo.ImagePath,
                        UserName,
                        Password,
                        IsActive
                        );
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds a user matching the specified username and password and returns a populated ClsUserBuiness instance
        /// when credentials match and the associated person exists.
        /// </summary>
        /// <remarks>Calls the data layer to verify credentials and retrieves person details by ID. The
        /// data-layer call may modify the provided username and password values.</remarks>
        /// <param name="UserName">The username to match.</param>
        /// <param name="Password">The password to match.</param>
        /// <returns>A ClsUserBuiness for the matched user, or null if no matching user or associated person is found.</returns>
        [DocInfo("Finds user entity matching username and password combination.")]
        public static ClsUserBuiness FindUserByUserNameAndPassowrd(string UserName, string Password)
        {
            int UserID = -1;
            int PersonID = -1;
            bool IsActive = false;

            if (clsUserData.FindUserByUserNameAndPassowrd(ref UserID, ref PersonID, ref UserName, ref Password, ref IsActive))
            {
                ClsPerson PersonInfo = ClsPerson.FindPersonByID(PersonID);

                if (PersonInfo != null)
                {
                    return new ClsUserBuiness
                        (
                        UserID,
                        PersonID,
                        PersonInfo.NationalNo,
                        PersonInfo.FirstName,
                        PersonInfo.SecondName,
                        PersonInfo.ThirdName,
                        PersonInfo.LastName,
                        PersonInfo.DateOfBirth,
                        PersonInfo.Gendor,
                        PersonInfo.Address,
                        PersonInfo.Phone,
                        PersonInfo.Email,
                        PersonInfo.NationalityCountryID,
                        PersonInfo.ImagePath,
                        UserName,
                        Password,
                        IsActive
                        );
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves a DataTable containing all registered system users for administration grids.
        /// </summary>
        /// <remarks>The DataTable is a disconnected snapshot of the user store; modifying it does not
        /// alter the underlying data source.</remarks>
        /// <returns>A DataTable containing all registered system users; empty if no users are found.</returns>
        [DocInfo("Retrieves complete list of registered system users for administration grids.")]
        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }
        #endregion

        #region Validation & Authentication

        /// <summary>
        /// Determines whether a user with the specified username exists in the system user storage.
        /// </summary>
        /// <remarks>The check is performed against the system user storage via the underlying user data
        /// provider.</remarks>
        /// <param name="Username">Username to check for presence in the system user storage.</param>
        /// <returns>true if a user with the specified username exists; otherwise, false.</returns>
        [DocInfo("Validates username uniqueness against system user storage.")]
        public static bool DoesUserExists(string Username)
        {
            return clsUserData.IsUserExists(Username);
        }

        /// <summary>
        /// Validates the provided credentials and active account status, returning the matching user when
        /// authentication succeeds.
        /// </summary>
        /// <remarks>Performs a plaintext password comparison; store and compare hashed passwords in
        /// production.</remarks>
        /// <param name="UserName">Account username to authenticate.</param>
        /// <param name="Password">Account password to authenticate.</param>
        /// <returns>Authenticated ClsUserBuiness when credentials match and the account is active; otherwise null.</returns>

        [DocInfo("Validates credentials and account status for system login operations.")]
        public static ClsUserBuiness Loging(string UserName, string Password)
        {
            ClsUserBuiness Userinfo = ClsUserBuiness.FindUserByUserNameAndPassowrd(UserName, Password);

            if (Userinfo != null && Userinfo.Password == Password && Userinfo.IsActive)
            {
                return Userinfo;
            }

            return null;
        }

        /// <summary>
        /// Deletes the user record with the specified primary key UserID.
        /// </summary>
        /// <param name="UserID">Primary key of the user to delete.</param>
        /// <returns>True if the user was deleted; otherwise, false.</returns>
        [DocInfo("Deletes user record by primary key UserID.")]
        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        /// <summary>
        /// Persist the user and base person hierarchy to storage according to the entity state, creating the base
        /// person when needed and transitioning Mode to enMode._Update on success.
        /// </summary>
        /// <remarks>When Mode is enMode._AddNew, attempts to add a new user. If PersonID equals -1, saves
        /// the base person first and assigns PersonID from base.PersonID before adding the user. When Mode is
        /// enMode._Update, updates the existing user. All other modes result in no operation and return
        /// false.</remarks>
        /// <returns>true when the entity and related person are persisted and Mode is transitioned to enMode._Update as
        /// appropriate; otherwise false.</returns>
        [DocInfo("Persists user and base person hierarchy to database storage based on entity state mode.")]
        public bool Save()
        {
            switch (Mode)
            {
                case enMode._AddNew:
                    if (this.PersonID != -1)
                    {
                        if (_AddNewUser())
                        {
                            Mode = enMode._Update;
                            return true;
                        }

                        return false;
                    }

                    if (!base.Save())
                    {
                        return false;
                    }

                    this.PersonID = base.PersonID;

                    if (_AddNewUser())
                    {
                        Mode = enMode._Update;

                        return true;
                    }
                    return false;

                case enMode._Update:

                    return _UpdateUser();
            }

            return false;
        }
        #endregion

    }
}
