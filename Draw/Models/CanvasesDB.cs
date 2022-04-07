using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace Draw.Models
{
    public class CanvasesDB
    {

        public SqlConnection connect()
        {
            string conStr = ConfigurationManager.ConnectionStrings["ShirleyServer"].ConnectionString;
            //string conStr = ConfigurationManager.ConnectionStrings["YarinServer"].ConnectionString;
            SqlConnection con = new SqlConnection(conStr);
            con.Open();
            return con;
        }

        public CanvasesDB()
        {

        }


        //List of ALL Canvases
        public List<Canvases> GetAllCanvases()
        {
            List<Canvases> cl = new List<Canvases>();
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand("SELECT * FROM Canvases", con);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                Canvases c = new Canvases(
                    (int)reader["canvas_id"],
                    (int)reader["user_id"],
                    (string)reader["canvas_path"]
                    );
                cl.Add(c);
            }
            con.Close();
            return cl;
        }

        //BY USER_ID(the one who published it)
        //LIST OF CANVASES THAT WERE CREATED BY A SPESIFIC USER
        public List<Canvases> GetAllCanvasesByUser(int user_id)
        {
            List<Canvases> cl = new List<Canvases>();
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand(
                $" SELECT * FROM Canvases " +
                $" WHERE user_id=@id", con);
            comm.Parameters.AddWithValue("@id", user_id);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                Canvases c = new Canvases(
                    (int)reader["canvas_id"],
                    (int)reader["user_id"],
                    (string)reader["canvas_path"]
                    );
                cl.Add(c);
            }
            con.Close();
            return cl;
        }

        //GET CANVAS BY CANVAS_ID
        public Canvases GetCanvasByID(int canvas_id)
        {
            Canvases c = new Canvases();
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand(
                $" SELECT * FROM Canvases " +
                $" WHERE canvas_id=@canvasId", con);
            comm.Parameters.AddWithValue("@canvasId", canvas_id);

            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                c = new Canvases(
                    (int)reader["canvas_id"],
                    (int)reader["user_id"],
                    (string)reader["canvas_path"]
                   );
            }
            con.Close();
            return c;
        }

        //INSERT POST
        public int InsertCanvasToDb(Canvases val)
        {
            string strComm =
          $" INSERT INTO Canvases(user_id, canvas_path) VALUES(" +
          $" @id," +
          $" @path); ";

            strComm +=
                " SELECT SCOPE_IDENTITY() AS[SCOPE_IDENTITY]; ";

            return ExcReaderInsertCanvas(strComm, val);
        }

        public int ExcReaderInsertCanvas(string comm2Run, Canvases val)
        {
            //int CanvasID = -10;
            int CanvasID = -1;
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand(comm2Run, con);
            comm.Parameters.AddWithValue("@id", val.User_ID);
            comm.Parameters.AddWithValue("@path", val.Canvas_Path);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                CanvasID = int.Parse(reader["SCOPE_IDENTITY"].ToString());
            }
            con.Close();
            return CanvasID;
        }


        //DELETE BY ID
        public int DeleteCanvasByID(int canvas_id)
        {
            string strComm =
                    $" DELETE Canvases " +
                    $" WHERE canvas_id=@id";

            return ExcNonQ(strComm, canvas_id);
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
        private int ExcNonQUpdate(string comm2Run, Canvases c)
        {
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand(comm2Run, con);
            comm.Parameters.AddWithValue("@id", c.Canvas_ID);
            comm.Parameters.AddWithValue("@path", c.Canvas_Path);
            int res = comm.ExecuteNonQuery();
            con.Close();
            return res;
        }
        //UPDATE Canvas drawing 
        public int UpdateCanvas(Canvases c)
        {
            string strComm =
                  $" UPDATE Canvases SET " +
                  $" canvas_path=@path  " +
                  $" WHERE canvas_id=@id";

            return ExcNonQUpdate(strComm, c);
        }

        public List<Canvases> ExcReader(string comm2Run)
        {
            List<Canvases> cl = new List<Canvases>();
            SqlConnection con = connect();
            SqlCommand comm = new SqlCommand(comm2Run, con);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                Canvases c = new Canvases(
                    (int)reader["canvas_id"],
                    (int)reader["user_id"],
                    (string)reader["canvas_path"]
                    );
                cl.Add(c);
            }
            con.Close();
            return cl;
        }






    }




}
