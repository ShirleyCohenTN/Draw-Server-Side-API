using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Draw.EncryptDecrypt;



namespace Draw.Models
{
    public class UsersDB
    {
        string strCon = ConfigurationManager.ConnectionStrings["ShirleyServer"].ConnectionString;


        public UsersDB()
        {

        }


        //List of ALL USERS
        public List<Users> GetAllUsers()
        {
            List<Users> ul = new List<Users>();
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand("SELECT * FROM Users", con);
            comm.Connection.Open();
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
            comm.Connection.Close();
            return ul;
        }




        //BY EMAIL AND PASS

        public Users GetUserByEmailAndPassword(string email, string pass)
        {
            Users u = null;
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand(
            $" SELECT * FROM Users " +
            $" WHERE email=@email AND pass=@pass", con);
            comm.Connection.Open();
            comm.Parameters.AddWithValue("@email", email);
          //comm.Parameters.AddWithValue("@pass", CommonMethods.ConvertToEncrypt(pass));
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
            comm.Connection.Close();
            return u;
        }




        //BY EMAIL
        public Users GetUserByEmail(string email)
        {
            Users u = null;
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand(
                $" SELECT * FROM Users " +
                $" WHERE email=@email", con);
            comm.Connection.Open();
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
            comm.Connection.Close();
            return u;
        }





        //BY ID
        public Users GetUserByID(int user_id)
        {
            Users u = null;
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand(
                $" SELECT * FROM Users " +
                $" WHERE user_id=@user_id", con);
            comm.Connection.Open();
            comm.Parameters.AddWithValue("@user_id", user_id);

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
            comm.Connection.Close();
            return u;
        }





        //INSERT USER
        public int InsertUserToDb(Users val)
        {
            //in case thers already a user with such email
            if (GetUserByEmail(val.Email) != null) return -1;

            string strComm =
                 $" INSERT INTO Users(first_name, last_name, email, pass) VALUES(" +
                 $" N'{val.First_Name}'," +
                 $" N'{val.Last_Name}'," +
                 $" N'{val.Email}'," +
               //  $" N'{CommonMethods.ConvertToEncrypt(val.Pass)}'); ";
                 $" N'{CommonMethods.HashString(val.Pass)}'); ";



            strComm +=
                " SELECT SCOPE_IDENTITY() AS[SCOPE_IDENTITY]; ";

            return ExcReaderInsertUser(strComm);
        }



        public int ExcReaderInsertUser(string comm2Run)
        {
            int UserID = -1;
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand(comm2Run, con);
            comm.Connection.Open();
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                UserID = int.Parse(reader["SCOPE_IDENTITY"].ToString());
            }
            comm.Connection.Close();
            return UserID;
        }





        //DELETE BY ID
        public int DeleteUserByID(int user_id)
        {
            string strComm =
                    $" DELETE Users " +
                    $" WHERE user_id={user_id}";

            return ExcNonQ(strComm);
        }



        private int ExcNonQ(string comm2Run)
        {
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand(comm2Run, con);
            comm.Connection.Open();
            int res = comm.ExecuteNonQuery();
            comm.Connection.Close();
            return res;
        }



        public List<Users> ExcReader(string comm2Run)
        {
            List<Users> ul = new List<Users>();
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand(comm2Run, con);
            comm.Connection.Open();
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
            comm.Connection.Close();
            return ul;
        }

    }
}