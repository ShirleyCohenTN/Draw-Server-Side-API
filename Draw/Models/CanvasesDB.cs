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

        string strCon = ConfigurationManager.ConnectionStrings["ShirleyServer"].ConnectionString;


        public CanvasesDB()
        {

        }


        //List of ALL Canvases
        public List<Canvases> GetAllCanvases()
        {
            List<Canvases> cl = new List<Canvases>();
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand("SELECT * FROM Canvases", con);
            comm.Connection.Open();
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                Canvases c = new Canvases(
                    (int)reader["canvas_id"],
                    (int)reader["user_id"],
                    (string)reader["canvas_path"],
                    (string)reader["canvas_coordinates"]
                    );
                cl.Add(c);
            }
            comm.Connection.Close();
            return cl;
        }






        //BY USER_ID(the one who published it)
        //LIST OF CANVASES THAT WERE CREATED BY A SPESIFIC USER
        public List<Canvases> GetAllCanvasesByUser(int user_id)
        {
            List<Canvases> cl = new List<Canvases>();
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand(
                $" SELECT * FROM Canvases " +
                $" WHERE user_id='{user_id}' ", con);
            comm.Connection.Open();
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                Canvases c = new Canvases(
                    (int)reader["canvas_id"],
                    (int)reader["user_id"],
                    (string)reader["canvas_path"],
                    (string)reader["canvas_coordinates"]
                    );
                cl.Add(c);
            }
            comm.Connection.Close();
            return cl;
        }




        //GET CANVAS BY CANVAS_ID
        public Canvases GetCanvasByID(int canvas_id)
        {
            Canvases c = new Canvases();
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand(
                $" SELECT * FROM Canvases " +
                $" WHERE canvas_id='{canvas_id}' ", con);
            comm.Connection.Open();
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                c = new Canvases(
                    (int)reader["canvas_id"],
                    (int)reader["user_id"],
                    (string)reader["canvas_path"],
                    (string)reader["canvas_coordinates"]
                   );
            }
            comm.Connection.Close();
            return c;
        }





        //INSERT POST
        public int InsertCanvasToDb(Canvases val)
        {

            string strComm =
                 $" INSERT INTO Canvases(user_id, canvas_path, canvas_coordinates) VALUES(" +
                 $" {val.User_ID}," +
                 $" N'{val.Canvas_Path}'," +
                 $" N'{val.Canvas_Coordinates}'); ";

            strComm +=
                " SELECT SCOPE_IDENTITY() AS[SCOPE_IDENTITY]; ";

            return ExcReaderInsertCanvas(strComm);
        }



        public int ExcReaderInsertCanvas(string comm2Run)
        {
            //int CanvasID = -10;
            int CanvasID = -1;
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand(comm2Run, con);
            comm.Connection.Open();
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                CanvasID = int.Parse(reader["SCOPE_IDENTITY"].ToString());
            }
            comm.Connection.Close();
            return CanvasID;
        }





        //DELETE BY ID
        public int DeleteCanvasByID(int canvas_id)
        {
            string strComm =
                    $" DELETE Canvases " +
                    $" WHERE canvas_id={canvas_id}";

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




        //UPDATE Canvas drawing 
        public int UpdateCanvas(Canvases c)
        {
            string strComm =
                  $" UPDATE Canvases SET " +
                  $" canvas_path='{c.Canvas_Path}' , " +
                  $" canvas_coordinates='{c.Canvas_Coordinates}'  " +
                  $" WHERE canvas_id={c.Canvas_ID}";

            return ExcNonQ(strComm);
        }



        public List<Canvases> ExcReader(string comm2Run)
        {
            List<Canvases> cl = new List<Canvases>();
            SqlConnection con = new SqlConnection(strCon);
            SqlCommand comm = new SqlCommand(comm2Run, con);
            comm.Connection.Open();
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                Canvases c = new Canvases(
                    (int)reader["canvas_id"],
                    (int)reader["user_id"],
                    (string)reader["canvas_path"],
                    (string)reader["canvas_coordinates"]
                    );
                cl.Add(c);
            }
            comm.Connection.Close();
            return cl;
        }






    }




}
