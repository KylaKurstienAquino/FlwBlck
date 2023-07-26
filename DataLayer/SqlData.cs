using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ModelLayer;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace DataLayer
{
    public class SqlData
    {
        private List<Accounts> accounts { get; set; }

        string connectionString 
            = "Data Source=MiyuAki\\SQLEXPRESS;Initial Catalog = followblock; Integrated Security = True:";

        static SqlConnection sqlconnection;

        public SqlData()
        {
            sqlconnection = new SqlConnection(connectionString);
        }

         
        public Accounts GetAccountByUsername(string userName)
        {
            foreach (var username in accounts)
            {
                if (username.Username == userName)
                { return username; }
            }
            return new Accounts();
        }

        public Accounts GetAccountByStudentNo(string studentNo)
        {
            Accounts account = null;

            using (sqlconnection)
            {
                string statement = "SELECT * FROM Accounts WHERE StudentNo = @StudentNo";
                  
                using (SqlCommand command = new SqlCommand(statement, sqlconnection))
                {
                    command.Parameters.AddWithValue("@StudentNo", studentNo);
                    try
                    {
                        connectionString.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                account = new Accounts();
                                {
                                    StudentNo = reader["StudentNo"].ToString();
                                    Username = reader["Username"].ToString();
                                    Password = reader["Password"].ToString();
                                    Course = reader["Course"].ToString();
                                    Section = reader["Section"].ToString();

                                };
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Error" + ex.Message);
                    }
                }
            }
            return account;
        }

        public bool InsertFollowing(string loggedInStudentNo, string followingName)
        {
            // Connect to the database and insert a new record into the Following table
            if (IsFollowing(loggedInStudentNo, followingName))
            {
                Console.WriteLine("Already following");
                return false;
            }
            Accounts followingAcc = GetAccountByStudentNo(followingName);
            if (followingAcc == null)
            {
                Console.WriteLine("Account not found");
                return false;
            }

            using (sqlconnection)
            {
                string statement = @"INSERT INTO Following ( StudentNo, FollowingName, FollowingCourse, FollowingSection )
                                    VALUES (@LoggedInStudentNo, @FollowingName, @FollowingCourse, @FollowingSection)";

                using (SqlCommand command = new SqlCommand(statement, sqlconnection))
                {
                    command.Parameters.AddWithValue(@"LoggedInStudentNo" , loggedInStudentNo);
                    command.Parameters.AddWithValue(@"FollowingName", followingAcc.Username);
                    command.Parameters.AddWithValue(@"FollowingCourse", followingAcc.Course);
                    command.Parameters.AddWithValue(@"FollowingSection", followingAcc.Section);

                    try
                    {
                        connectionString.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error" + ex.Message);
                        return false;
                    }

                }
            }

            Console.WriteLine("You're following" + followingAcc.Username);
            return true;

        }

        public void InsertFollower(string followingStudentNo, string loggedInStudentNo)
        {
            // Connect to the database and insert a new record into the Follower table
            Accounts followerAcc = GetAccountByStudentNo(loggedInStudentNo);

            if (followerAcc == null)
            {
                Console.WriteLine("Account not found");
                return;
            }

            using(sqlconnection)
            {
                string statement = @"INSERT INTO Follower ( StudentNo, FollowerName, FollowerCourse, FollowerSection)
                                    VALUES (@FollowingStudentNo, @FollowerName, @FollowerCourse, @FollowerSection)";

                using (SqlCommand command = new SqlCommand(statement, sqlconnection))
                {
                    command.Parameters.AddWithValue(@"FollowingStudentNo", followingStudentNo);
                    command.Parameters.AddWithValue(@"FollowerName", followerAcc.Username);
                    command.Parameters.AddWithValue(@"FollowerCourse", followerAcc.Course);
                    command.Parameters.AddWithValue(@"FollowerSection", followerAcc.Section);

                    try
                    {
                        connectionString.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error" + ex.Message);
                    }
                }
            }

            Console.WriteLine("You're following" + followingStudentNo);
        }

        private bool IsFollowing(string loggedInStudentNo, string followingName)
        {
            using (sqlconnection)
            {
                string statement = @"SELECT COUNT (*) FROM Following WHERE StudentNo = @LoggedInStudent AND FollowingName = @FollowingName";

                using (SqlCommand command = new SqlCommand(statement, sqlconnection))
                {
                    command.Parameters.AddWithValue("@LoggedInStudentNo", loggedInStudentNo);
                    command.Parameters.AddWithValue("@FollowingName", followingName);

                    try
                    {
                        sqlconnection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;  
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error" + ex.Message);
                        return false;
                    }

                }

            }
        }

        public void InsertBlocked(Blocked blocked)
        {
            // Connect to the database and insert a new record into the Blocked table
        }
    }

}
