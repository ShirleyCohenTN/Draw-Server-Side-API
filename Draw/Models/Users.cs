using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Draw.Models
{
    public class Users
    {
        public int User_ID {get;set;}
        public string First_Name {get;set;}
        public string Last_Name { get;set;}
        public string Email {get;set;}  
        public string Pass {get;set;}   

        
        
        // constructor
        public Users() { }


        public Users(int user_id, string first_name, string last_name, string email, string pass)
        {
            User_ID = user_id;
            First_Name = first_name;
            Last_Name = last_name;  
            Email = email;  
            Pass = pass;    
        }

        public override string ToString()
        {
            return $"{User_ID}, {First_Name}, {Last_Name}, {Email}, {Pass}"; ;
        }

    }
}