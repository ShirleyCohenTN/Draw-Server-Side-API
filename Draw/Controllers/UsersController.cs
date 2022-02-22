using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Draw.Models;
using Microsoft.AspNetCore.Cors;

namespace Draw.Controllers
{
    public class UsersController : ApiController
    {

        private UsersDB _usersDB = new UsersDB();


        //[EnableCors( "*", "*", "*")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_usersDB.GetAllUsers());
            }
            catch (Exception ex)
            {
                return BadRequest("could not get all the users!\n  -- " + ex.Message);
            }
        }


        //GET BY ID
        [Route("{id:int:min(1)}", Name = "GetUserByID")]
        public IHttpActionResult Get(int user_id)
        {
            try
            {
                Users u = _usersDB.GetUserByID(user_id);
                if (u != null)
                {
                    return Ok(u);
                }
                return Content(HttpStatusCode.NotFound, $"User with id {user_id} was not found!!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        //[DisableCors]
        [HttpGet]
        public IHttpActionResult GET(string email, string pass)
        {
            try
            {
                return Ok(_usersDB.GetUserByEmailAndPassword(email, pass));
            }
            catch (TypeInitializationException e)
            {

                return Content(HttpStatusCode.NotFound, e.InnerException);
            }

        }




        [Route(Name = "GetUserByEmail")]
        public IHttpActionResult Get(string email)
        {
            try
            {
                Users res = _usersDB.GetUserByEmail(email);
                if (res == null)
                {
                    return Content(HttpStatusCode.NotFound, $"user with email= {email} was not found!");
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // POST CREATE NEW USER
        public IHttpActionResult Post([FromBody] Users User2Insert)
        {

            try
            {
                int res = _usersDB.InsertUserToDb(User2Insert);
                if (res == -1)
                {
                    return Content(HttpStatusCode.BadRequest, $"User id = {User2Insert.User_ID} was not created in the DB!!!");
                }
                User2Insert.User_ID = res;
                //gives error 400 but it creats a new User:
                //return Created(new Uri(Url.Link("GetUserByID", new { user_id = res })), User2Insert);
                
                //gives status 200 but it doesnt change the right ID, it gives the previous one
                return Get(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        //DELETE
        public IHttpActionResult Delete(int user_id)
        {
            try
            {
                Users u = _usersDB.GetUserByID(user_id);
                if (u != null)
                {
                    int res = _usersDB.DeleteUserByID(user_id);
                    if (res == 1)
                    {
                        return Ok();
                    }
                    return Content(HttpStatusCode.BadRequest, $"User with id {user_id} exsits but could not be deleted!!!");
                }
                return Content(HttpStatusCode.NotFound, "User with id = " + user_id + " was not found to delete!!!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
