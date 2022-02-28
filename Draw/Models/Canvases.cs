using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Draw.Models
{
    public class Canvases
    {
        public int Canvas_ID { get; set; }

        public int User_ID { get; set; }

        public string Canvas_Path { get; set; }

        public string Canvas_Coordinates { get; set; }
    



        // constructor
        public Canvases() { }


        public Canvases(int canvas_id, int user_id, string canvas_path, string canvas_coordinates)
        {
            Canvas_ID = canvas_id; 
            User_ID = user_id;
            Canvas_Path = canvas_path;
            Canvas_Coordinates = canvas_coordinates;
        }

        public override string ToString()
        {
            return $"{Canvas_ID}, {User_ID}, {Canvas_Path}, {Canvas_Coordinates}"; ;
        }
    }
}