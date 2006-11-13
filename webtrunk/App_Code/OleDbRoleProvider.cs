/*
 * This provider was adapted from the example on MSDN at http://msdn2.microsoft.com/en-us/library/tksy7hd7.aspx
 *
 */


using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Globalization;

/*

This provider works with the following schema for the tables of role data.

CREATE TABLE Roles
(
  Rolename Text (255) NOT NULL,
  ApplicationName Text (255) NOT NULL,
    CONSTRAINT PKRoles PRIMARY KEY (Rolename, ApplicationName)
)

CREATE TABLE UsersInRoles
(
  Username Text (255) NOT NULL,
  Rolename Text (255) NOT NULL,
  ApplicationName Text (255) NOT NULL,
    CONSTRAINT PKUsersInRoles PRIMARY KEY (Username, Rolename, ApplicationName)
)

*/


public sealed class OleDbRoleProvider : RoleProvider
{

    //
    // Global connection string, generic exception message, event log info.
    //

    private string eventSource = "OleDbRoleProvider";
    private string eventLog = "Application";
    private string exceptionMessage = "An exception occurred. Please check the Event Log.";

    private ConnectionStringSettings pConnectionStringSettings;
    private string connectionString;


    //
    // If false, exceptions are thrown to the caller. If true,
    // exceptions are written to the event log.
    //

    private bool pWriteExceptionsToEventLog = false;

    public bool WriteExceptionsToEventLog
    {
        get { return pWriteExceptionsToEventLog; }
        set { pWriteExceptionsToEventLog = value; }
    }



    //
    // System.Configuration.Provider.ProviderBase.Initialize Method
    //

    public override void Initialize(string name, NameValueCollection config)
    {

        //
        // Initialize values from web.config.
        //

        if (config == null)
            throw new ArgumentNullException("config");

        if (name == null || name.Length == 0)
            name = "OleDBRoleProvider";

        if (String.IsNullOrEmpty(config["description"]))
        {
            config.Remove("description");
            config.Add("description", "OLE DB Role provider");
        }

        // Initialize the abstract base class.
        base.Initialize(name, config);


        if (config["applicationName"] == null || config["applicationName"].Trim() == "")
        {
            pApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
        }
        else
        {
            pApplicationName = config["applicationName"];
        }


        if (config["writeExceptionsToEventLog"] != null)
        {
            if (config["writeExceptionsToEventLog"].ToUpper() == "TRUE")
            {
                pWriteExceptionsToEventLog = true;
            }
        }


        //
        // Initialize OleDbConnection.
        //

        pConnectionStringSettings = ConfigurationManager.
          ConnectionStrings[config["connectionStringName"]];

        if (pConnectionStringSettings == null || pConnectionStringSettings.ConnectionString.Trim() == "")
        {
            throw new ProviderException("Connection string cannot be blank.");
        }

        connectionString = pConnectionStringSettings.ConnectionString;
    }



    //
    // System.Web.Security.RoleProvider properties.
    //


    private string pApplicationName;


    public override string ApplicationName
    {
        get { return pApplicationName; }
        set { pApplicationName = value; }
    }

    //
    // System.Web.Security.RoleProvider methods.
    //

    //
    // RoleProvider.AddUsersToRoles
    //

    public override void AddUsersToRoles(string[] usernames, string[] rolenames)
    {
        foreach (string rolename in rolenames)
        {
            if (!RoleExists(rolename))
            {
                throw new ProviderException("Role name not found.");
            }
        }

        foreach (string username in usernames)
        {
            if (username.Contains(","))
            {
                throw new ArgumentException("User names cannot contain commas.");
            }

            foreach (string rolename in rolenames)
            {
                if (IsUserInRole(username, rolename))
                {
                    throw new ProviderException("User is already in role.");
                }
            }
        }


        OleDbConnection conn = new OleDbConnection(connectionString);
        OleDbCommand cmd = new OleDbCommand("INSERT INTO UsersInRoles " +
                " (Username, Rolename, ApplicationName) " +
                " Values(?, ?, ?)", conn);

        OleDbParameter userParm = cmd.Parameters.Add("@Username", OleDbType.VarChar, 255);
        OleDbParameter roleParm = cmd.Parameters.Add("@Rolename", OleDbType.VarChar, 255);
        cmd.Parameters.Add("@ApplicationName", OleDbType.VarChar, 255).Value = ApplicationName;

        OleDbTransaction tran = null;

        try
        {
            conn.Open();
            tran = conn.BeginTransaction();
            cmd.Transaction = tran;

            foreach (string username in usernames)
            {
                foreach (string rolename in rolenames)
                {
                    userParm.Value = username;
                    roleParm.Value = rolename;
                    cmd.ExecuteNonQuery();
                }
            }

            tran.Commit();
        }
        catch (OleDbException e)
        {
            try
            {
                tran.Rollback();
            }
            catch { }


            if (WriteExceptionsToEventLog)
            {
                WriteToEventLog(e, "AddUsersToRoles");
            }
            else
            {
                throw e;
            }
        }
        finally
        {
            conn.Close();
        }
    }


    //
    // RoleProvider.CreateRole
    //

    public override void CreateRole(string rolename)
    {
        if (rolename.Contains(","))
        {
            throw new ArgumentException("Role names cannot contain commas.");
        }

        if (RoleExists(rolename))
        {
            throw new ProviderException("Role name already exists.");
        }

        OleDbConnection conn = new OleDbConnection(connectionString);
        OleDbCommand cmd = new OleDbCommand("INSERT INTO Roles " +
                " (Rolename, ApplicationName) " +
                " Values(?, ?)", conn);

        cmd.Parameters.Add("@Rolename", OleDbType.VarChar, 255).Value = rolename;
        cmd.Parameters.Add("@ApplicationName", OleDbType.VarChar, 255).Value = ApplicationName;

        try
        {
            conn.Open();

            cmd.ExecuteNonQuery();
        }
        catch (OleDbException e)
        {
            if (WriteExceptionsToEventLog)
            {
                WriteToEventLog(e, "CreateRole");
            }
            else
            {
                throw e;
            }
        }
        finally
        {
            conn.Close();
        }
    }


    //
    // RoleProvider.DeleteRole
    //

    public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
    {
        if (!RoleExists(rolename))
        {
            throw new ProviderException("Role does not exist.");
        }

        if (throwOnPopulatedRole && GetUsersInRole(rolename).Length > 0)
        {
            throw new ProviderException("Cannot delete a populated role.");
        }

        OleDbConnection conn = new OleDbConnection(connectionString);
        OleDbCommand cmd = new OleDbCommand("DELETE FROM Roles " +
                 " WHERE Rolename = ? AND ApplicationName = ?", conn);

        cmd.Parameters.Add("@Rolename", OleDbType.VarChar, 255).Value = rolename;
        cmd.Parameters.Add("@ApplicationName", OleDbType.VarChar, 255).Value = ApplicationName;


        OleDbCommand cmd2 = new OleDbCommand("DELETE FROM UsersInRoles " +
                 " WHERE Rolename = ? AND ApplicationName = ?", conn);

        cmd2.Parameters.Add("@Rolename", OleDbType.VarChar, 255).Value = rolename;
        cmd2.Parameters.Add("@ApplicationName", OleDbType.VarChar, 255).Value = ApplicationName;

        OleDbTransaction tran = null;

        try
        {
            conn.Open();
            tran = conn.BeginTransaction();
            cmd.Transaction = tran;
            cmd2.Transaction = tran;

            cmd2.ExecuteNonQuery();
            cmd.ExecuteNonQuery();

            tran.Commit();
        }
        catch (OleDbException e)
        {
            try
            {
                tran.Rollback();
            }
            catch { }


            if (WriteExceptionsToEventLog)
            {
                WriteToEventLog(e, "DeleteRole");

                return false;
            }
            else
            {
                throw e;
            }
        }
        finally
        {
            conn.Close();
        }

        return true;
    }


    //
    // RoleProvider.GetAllRoles
    //

    public override string[] GetAllRoles()
    {
        string tmpRoleNames = "";

        OleDbConnection conn = new OleDbConnection(connectionString);
        OleDbCommand cmd = new OleDbCommand("SELECT Rolename FROM Roles " +
                  " WHERE ApplicationName = ?", conn);

        cmd.Parameters.Add("@ApplicationName", OleDbType.VarChar, 255).Value = ApplicationName;

        OleDbDataReader reader = null;

        try
        {
            conn.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tmpRoleNames += reader.GetString(0) + ",";
            }
        }
        catch (OleDbException e)
        {
            if (WriteExceptionsToEventLog)
            {
                WriteToEventLog(e, "GetAllRoles");
            }
            else
            {
                throw e;
            }
        }
        finally
        {
            if (reader != null) { reader.Close(); }
            conn.Close();
        }

        if (tmpRoleNames.Length > 0)
        {
            // Remove trailing comma.
            tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
            return tmpRoleNames.Split(',');
        }

        return new string[0];
    }


    //
    // RoleProvider.GetRolesForUser
    //

    public override string[] GetRolesForUser(string username)
    {
        string tmpRoleNames = "";

        OleDbConnection conn = new OleDbConnection(connectionString);
        OleDbCommand cmd = new OleDbCommand("SELECT Rolename FROM UsersInRoles " +
                " WHERE Username = ? AND ApplicationName = ?", conn);

        cmd.Parameters.Add("@Username", OleDbType.VarChar, 255).Value = username;
        cmd.Parameters.Add("@ApplicationName", OleDbType.VarChar, 255).Value = ApplicationName;

        OleDbDataReader reader = null;

        try
        {
            conn.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tmpRoleNames += reader.GetString(0) + ",";
            }
        }
        catch (OleDbException e)
        {
            if (WriteExceptionsToEventLog)
            {
                WriteToEventLog(e, "GetRolesForUser");
            }
            else
            {
                throw e;
            }
        }
        finally
        {
            if (reader != null) { reader.Close(); }
            conn.Close();
        }

        if (tmpRoleNames.Length > 0)
        {
            // Remove trailing comma.
            tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
            return tmpRoleNames.Split(',');
        }

        return new string[0];
    }


    //
    // RoleProvider.GetUsersInRole
    //

    public override string[] GetUsersInRole(string rolename)
    {
        string tmpUserNames = "";

        OleDbConnection conn = new OleDbConnection(connectionString);
        OleDbCommand cmd = new OleDbCommand("SELECT Username FROM UsersInRoles " +
                  " WHERE Rolename = ? AND ApplicationName = ?", conn);

        cmd.Parameters.Add("@Rolename", OleDbType.VarChar, 255).Value = rolename;
        cmd.Parameters.Add("@ApplicationName", OleDbType.VarChar, 255).Value = ApplicationName;

        OleDbDataReader reader = null;

        try
        {
            conn.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tmpUserNames += reader.GetString(0) + ",";
            }
        }
        catch (OleDbException e)
        {
            if (WriteExceptionsToEventLog)
            {
                WriteToEventLog(e, "GetUsersInRole");
            }
            else
            {
                throw e;
            }
        }
        finally
        {
            if (reader != null) { reader.Close(); }
            conn.Close();
        }

        if (tmpUserNames.Length > 0)
        {
            // Remove trailing comma.
            tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
            return tmpUserNames.Split(',');
        }

        return new string[0];
    }


    //
    // RoleProvider.IsUserInRole
    //

    public override bool IsUserInRole(string username, string rolename)
    {
        bool userIsInRole = false;

        OleDbConnection conn = new OleDbConnection(connectionString);
        OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM UsersInRoles " +
                " WHERE Username = ? AND Rolename = ? AND ApplicationName = ?", conn);

        cmd.Parameters.Add("@Username", OleDbType.VarChar, 255).Value = username;
        cmd.Parameters.Add("@Rolename", OleDbType.VarChar, 255).Value = rolename;
        cmd.Parameters.Add("@ApplicationName", OleDbType.VarChar, 255).Value = ApplicationName;

        try
        {
            conn.Open();

            int numRecs = (int)cmd.ExecuteScalar();

            if (numRecs > 0)
            {
                userIsInRole = true;
            }
        }
        catch (OleDbException e)
        {
            if (WriteExceptionsToEventLog)
            {
                WriteToEventLog(e, "IsUserInRole");
            }
            else
            {
                throw e;
            }
        }
        finally
        {
            conn.Close();
        }

        return userIsInRole;
    }


    //
    // RoleProvider.RemoveUsersFromRoles
    //

    public override void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
    {
        foreach (string rolename in rolenames)
        {
            if (!RoleExists(rolename))
            {
                throw new ProviderException("Role name not found.");
            }
        }

        foreach (string username in usernames)
        {
            foreach (string rolename in rolenames)
            {
                if (!IsUserInRole(username, rolename))
                {
                    throw new ProviderException("User is not in role.");
                }
            }
        }


        OleDbConnection conn = new OleDbConnection(connectionString);
        OleDbCommand cmd = new OleDbCommand("DELETE FROM UsersInRoles " +
                " WHERE Username = ? AND Rolename = ? AND ApplicationName = ?", conn);

        OleDbParameter userParm = cmd.Parameters.Add("@Username", OleDbType.VarChar, 255);
        OleDbParameter roleParm = cmd.Parameters.Add("@Rolename", OleDbType.VarChar, 255);
        cmd.Parameters.Add("@ApplicationName", OleDbType.VarChar, 255).Value = ApplicationName;

        OleDbTransaction tran = null;

        try
        {
            conn.Open();
            tran = conn.BeginTransaction();
            cmd.Transaction = tran;

            foreach (string username in usernames)
            {
                foreach (string rolename in rolenames)
                {
                    userParm.Value = username;
                    roleParm.Value = rolename;
                    cmd.ExecuteNonQuery();
                }
            }

            tran.Commit();
        }
        catch (OleDbException e)
        {
            try
            {
                tran.Rollback();
            }
            catch { }


            if (WriteExceptionsToEventLog)
            {
                WriteToEventLog(e, "RemoveUsersFromRoles");
            }
            else
            {
                throw e;
            }
        }
        finally
        {
            conn.Close();
        }
    }


    //
    // RoleProvider.RoleExists
    //

    public override bool RoleExists(string rolename)
    {
        bool exists = false;

        OleDbConnection conn = new OleDbConnection(connectionString);
        OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM Roles " +
                  " WHERE Rolename = ? AND ApplicationName = ?", conn);

        cmd.Parameters.Add("@Rolename", OleDbType.VarChar, 255).Value = rolename;
        cmd.Parameters.Add("@ApplicationName", OleDbType.VarChar, 255).Value = ApplicationName;

        try
        {
            conn.Open();

            int numRecs = (int)cmd.ExecuteScalar();

            if (numRecs > 0)
            {
                exists = true;
            }
        }
        catch (OleDbException e)
        {
            if (WriteExceptionsToEventLog)
            {
                WriteToEventLog(e, "RoleExists");
            }
            else
            {
                throw e;
            }
        }
        finally
        {
            conn.Close();
        }

        return exists;
    }

    //
    // RoleProvider.FindUsersInRole
    //

    public override string[] FindUsersInRole(string rolename, string usernameToMatch)
    {
        OleDbConnection conn = new OleDbConnection(connectionString);
        OleDbCommand cmd = new OleDbCommand("SELECT Username FROM UsersInRoles  " +
                  "WHERE Username LIKE ? AND RoleName = ? AND ApplicationName = ?", conn);
        cmd.Parameters.Add("@UsernameSearch", OleDbType.VarChar, 255).Value = usernameToMatch;
        cmd.Parameters.Add("@RoleName", OleDbType.VarChar, 255).Value = rolename;
        cmd.Parameters.Add("@ApplicationName", OleDbType.VarChar, 255).Value = pApplicationName;

        string tmpUserNames = "";
        OleDbDataReader reader = null;

        try
        {
            conn.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tmpUserNames += reader.GetString(0) + ",";
            }
        }
        catch (OleDbException e)
        {
            if (WriteExceptionsToEventLog)
            {
                WriteToEventLog(e, "FindUsersInRole");
            }
            else
            {
                throw e;
            }
        }
        finally
        {
            if (reader != null) { reader.Close(); }

            conn.Close();
        }

        if (tmpUserNames.Length > 0)
        {
            // Remove trailing comma.
            tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
            return tmpUserNames.Split(',');
        }

        return new string[0];
    }

    //
    // WriteToEventLog
    //   A helper function that writes exception detail to the event log. Exceptions
    // are written to the event log as a security measure to avoid private database
    // details from being returned to the browser. If a method does not return a status
    // or boolean indicating the action succeeded or failed, a generic exception is also 
    // thrown by the caller.
    //

    private void WriteToEventLog(OleDbException e, string action)
    {
        EventLog log = new EventLog();
        log.Source = eventSource;
        log.Log = eventLog;

        string message = exceptionMessage + "\n\n";
        message += "Action: " + action + "\n\n";
        message += "Exception: " + e.ToString();

        log.WriteEntry(message);
    }

}