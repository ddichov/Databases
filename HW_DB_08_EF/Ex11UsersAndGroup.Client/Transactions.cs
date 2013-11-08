using System;
using System.Linq;
using System.Transactions; ////!
using UsersAndGroups.Data;

namespace Ex11UsersAndGroup.Client
{
    // 11. Create a database holding users and groups. Create a transactional EF 
    // based method that creates an user and puts it in a group "Admins". 
    // In case the group "Admins" do not exist, create the group in the same transaction. 
    // If some of the operations fail (e.g. the username already exist), 
    // cancel the entire transaction.
    public class Transactions
    {
        static void Main(string[] args)
        {
            User newUser = new User();
            newUser.username = "Ivan4";

            AddUser(newUser, "Gosho");
        }

        static void AddUser(User newUser, string userGroup)
        {
            using (UsersAndGroupsEntities db = new UsersAndGroupsEntities())
            {
                using (TransactionScope tr = new TransactionScope())
                {
                    try
                    {
                        Group newGroup = new Group();
                        var gr = from g in db.Groups where g.groupName == userGroup select g;

                        if (gr.Count() == 0)
                        {
                            newGroup.groupName = userGroup;
                            db.Groups.Add(newGroup);
                            db.SaveChanges();
                        }
                        else
                        {
                            newGroup = db.Groups.FirstOrDefault(x => x.groupName == userGroup);
                        }

                        newUser.Groups.Add(newGroup);
                        db.Users.Add(newUser);
                        db.SaveChanges();
                        tr.Complete();
                    }
                    catch (System.Data.UpdateException ex)
                    {
                        Console.WriteLine("The update failed! {0}", ex.Message);
                    }
                }
            }
        }
    }
}