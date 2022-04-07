using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Draw.EncryptDecrypt;
using System.Text.RegularExpressions;




namespace Draw.Models
{
    public class UsersDB
    {
        public SqlConnection connect()
        {
            string conStr = ConfigurationManager.ConnectionStrings["ShirleyServer"].ConnectionString;
            //string conStr = ConfigurationManager.ConnectionStrings["YarinServer"].ConnectionString;
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            return con;
        }




        public UsersDB()
        {

        }


        //List of ALL USERS
        public List<Users> GetAllUsers()
        {
            List<Users> ul = new List<Users>();
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand("SELECT * FROM Users", con);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                Users u = new Users(
                    (int)reader["user_id"],
                    (string)reader["first_name"],
                    (string)reader["last_name"],
                    (string)reader["email"],
                    (string)reader["pass"]
                    );
                ul.Add(u);
            }
            con.Close();
            return ul;
        }



        //BY EMAIL AND PASS

        public Users GetUserByEmailAndPassword(string email, string pass)
        {
            Users u = null;
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand(
            $" SELECT * FROM Users " +
            $" WHERE email=@email AND pass=@pass", con);
            comm.Parameters.AddWithValue("@email", email);
            comm.Parameters.AddWithValue("@pass", CommonMethods.HashString(pass));
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                u = new Users(
                    (int)reader["user_id"],
                    (string)reader["first_name"],
                    (string)reader["last_name"],
                    (string)reader["email"],
                    CommonMethods.HashString((string)reader["pass"])
                    );
            }
            con.Close();
            return u;
        }



        //BY EMAIL
        public Users GetUserByEmail(string email)
        {
            Users u = null;
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand(null, con);
            comm.CommandText = $"SELECT * FROM Users WHERE email=@email";
            comm.Parameters.AddWithValue("@email", email);

            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                u = new Users(
                    (int)reader["user_id"],
                    (string)reader["first_name"],
                    (string)reader["last_name"],
                    (string)reader["email"],
                    (string)reader["pass"]
                    );
            }
            con.Close();
            return u;
        }



        //BY ID
        public Users GetUserByID(int user_id)
        {
            Users u = null;
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand(
                $" SELECT * FROM Users " +
                $" WHERE user_id='{user_id}'", con);
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                u = new Users(
                    (int)reader["user_id"],
                    (string)reader["first_name"],
                    (string)reader["last_name"],
                    (string)reader["email"],
                    (string)reader["pass"]
                    );
            }
            con.Close();
            return u;
        }




        //INSERT USER
      
        static bool isValidDomain(string email)
        {
            return email.Split('@')[1].Split('.').Length > 1;
        }

        static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                return addr.Address == trimmedEmail && isValidDomain(email);
            }
            catch
            {
                return false;
            }
        }
        public bool validateUser(Users val)
        {
            if ( val.Email == "" || val.Pass == "" || val.First_Name == "" || val.Last_Name == "")
                return false;
            if (!IsValidEmail(val.Email))
                return false;
            if (!validateFieldRegex(val.Pass, @"\A(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{7,14}\Z"))
                return false;
            if (GetUserByEmail(val.Email) != null)
                return false;
            return true;
        }

        private bool validateFieldRegex(string field, string regexStr)
        {
            Regex rgField = new Regex(regexStr);
            return rgField.IsMatch(field);

        }
        public int InsertUserToDb(Users val)
        {
            if (!validateUser(val))
                return -1;

            string strComm =
                 $" INSERT INTO Users(first_name, last_name, email, pass) VALUES(" +
                 $" @first," +
                 $" @last," +
                 $" @mail," +
                 $" @pass); ";

            strComm +=
                " SELECT SCOPE_IDENTITY() AS[SCOPE_IDENTITY]; ";

            ;

            return ExcReaderInsertUser(strComm, val);
        }





        public int ExcReaderInsertUser(string comm2Run, Users val)
        {
            int UserID = -1;
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand(comm2Run, con);

            comm.Parameters.AddWithValue("@first", val.First_Name);
            comm.Parameters.AddWithValue("@last", val.Last_Name);
            comm.Parameters.AddWithValue("@mail", val.Email);
            comm.Parameters.AddWithValue("@pass", CommonMethods.HashString(val.Pass));
            SqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                UserID = int.Parse(reader["SCOPE_IDENTITY"].ToString());
            }
            con.Close();
            return UserID;
        }




        //DELETE USER BY ID
       
        public int DeleteUserByID(int user_id)
        {
            string strComm =
                    $" DELETE Users " +
                    $" WHERE user_id=@id";

            return ExcNonQ(strComm, user_id);
        }

        private int ExcNonQ(string comm2Run, int id)
        {
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand(comm2Run, con);
            comm.Parameters.AddWithValue("@id", id);
            int res = comm.ExecuteNonQuery();
            con.Close();
            return res;
        }


        public List<Users> ExcReader(string comm2Run)
        {
            List<Users> ul = new List<Users>();
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand(comm2Run, con);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())

            {
                Users u = new Users(
                    (int)reader["user_id"],
                    (string)reader["first_name"],
                    (string)reader["last_name"],
                    (string)reader["email"],
                    (string)reader["pass"]
                    );
                ul.Add(u);
            }
            con.Close();
            return ul;
        }

    }
}