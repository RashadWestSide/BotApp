using BotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BotApp.Providers
{
    public class BotAppRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string login)
        {
            string[] roles = new string[] { };
            using (BotAppContext _db = new BotAppContext())
            {
                User user = _db.Users.FirstOrDefault(u => u.Login == login);
                if (user != null)
                {
                    Role userRole = _db.Roles.Find(user.RoleId);

                    if (userRole != null)
                    {
                        roles = new string[] { userRole.Name };
                    }
                }
            }
            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string login, string roleName)
        {
            bool outputRes = false;

            using (BotAppContext _db = new BotAppContext())
            {
                User user = _db.Users.FirstOrDefault(u => u.Login == login);
                if (user != null)
                {
                    Role userRole = _db.Roles.Find(user.RoleId);

                    if (userRole != null && userRole.Name == roleName)
                    {
                        outputRes = true;
                    }
                }
            }
            return outputRes;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}