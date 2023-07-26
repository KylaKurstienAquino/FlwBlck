using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace RuleLayer
{
    public class FBRuleLayer
    {
        private SqlData sqlDataAccess;

        public FBRuleLayer(string sqlconnection)
        {
            sqlDataAccess = new SqlData(sqlconnection);
        }

        public void LogIn(string studentNo, string password)
        {
            Accounts account = sqlDataAccess.GetAccountByStudentNo(studentNo);
            if(account == null || account.Password != password)
            {
                Console.WriteLine("Invalid");
                return;
            }

            Console.WriteLine("Welcome " + account.Username);

            Console.WriteLine("Enter username to search: ");
            string username = Console.ReadLine();

            SearchAccount(username);
        }

        public void SearchAccount(string username)
        {
            Accounts account = sqlDataAccess.GetAccountByUsername(username);
            if (account == null)
            {
                Console.WriteLine("Account not found");
                return;
            }

            Console.WriteLine("StudentNo: " + account.StudentNo);
            Console.WriteLine("Username: " + account.Username);
            Console.WriteLine("Course: " + account.Course);
            Console.WriteLine("Section: " + account.Section);

            Console.WriteLine("Enter F to follow or B to Block: ");
            string choice = Console.ReadLine().ToUpper();

            if (choice == "F")
            {
                sqlDataAccess.InsertFollowing(loggedInStudentNo, username);
            }
            else if (choice == "B")
            {
                sqlDataAccess.InsertBlocked(loggedInStudentNo, username);
            }
            else
            {
                Console.WriteLine("Invalid Choice");
            }


        }
    }
}
