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
    //[EnableCors("*", "*", "*")]
    public class CanvasesController : ApiController
    {
      
            private CanvasesDB _canvasesDB = new CanvasesDB();
            private UsersDB _usersDB = new UsersDB();

            //GET
            public IHttpActionResult Get()
            {
                try
                {
                    return Ok(_canvasesDB.GetAllCanvases());
                }
                catch (TypeInitializationException ex)
                {
                    return BadRequest("could not get all the canvases!\n  -- " + ex.InnerException);
                }
            }



            //GET CANVASES BY USER ID (THE PUBLISHER)
            public IHttpActionResult Get(int user_id)
            {
                try
                {
                    return Ok(_canvasesDB.GetAllCanvasesByUser(user_id));
                }
                catch (Exception ex)
                {
                    return BadRequest($"could not get all the Canvases of the id publisher {user_id}!\n  -- " + ex.Message);
                }
            }



            


            // POST - CREATE NEW CANVAS
            [Route("api/uploadCanvas")]
            public IHttpActionResult Post([FromBody] Canvases Canvas2Insert)
            {

                try
                {
                    int res = _canvasesDB.InsertCanvasToDb(Canvas2Insert);
                    if (res == -1)
                    {
                        return Content(HttpStatusCode.BadRequest, $"Canvas canvas_id = {Canvas2Insert.Canvas_ID} was not created in the DB!!!");
                    }

                    Canvas2Insert.Canvas_ID = res;

                    

                    //return Created(new Uri(Url.Link("GetFosterByID", new { id = res })), Foster2Insert);
                    return Created(new Uri(Request.RequestUri.AbsoluteUri + res), Canvas2Insert);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }



            //UPDATE - PUT (update the canvas drawing)
            public IHttpActionResult Put(Canvases Canvas2Update)
            {
                try
                {
                    Canvases c = _canvasesDB.GetCanvasByID(Canvas2Update.Canvas_ID);
                    if (c != null)
                    {
                        int res = _canvasesDB.UpdateCanvas(Canvas2Update);
                        if (res == 1)
                        {
                            return Ok();
                        }
                        return Content(HttpStatusCode.NotModified, $"Canvas with id {Canvas2Update.Canvas_ID} exsits but could not be modified!!!");
                    }
                    return Content(HttpStatusCode.NotFound, "Canvas with id = " + Canvas2Update.Canvas_ID + " was not found to update!!!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                    throw;
                }
            }





            //DELETE CANVAS BY CANVAS_ID
            public IHttpActionResult Delete(int canvas_id)
            {
                try
                {
                    Canvases c = _canvasesDB.GetCanvasByID(canvas_id);
                    if (c != null)
                    {
                        int res = _canvasesDB.DeleteCanvasByID(canvas_id);
                        if (res == 1)
                        {
                            return Ok();
                        }
                        return Content(HttpStatusCode.BadRequest, $"Canvas with canvas_id {canvas_id} exists but could not be deleted!!!");
                    }
                    return Content(HttpStatusCode.NotFound, "Canvas with canvas_id = " + canvas_id + " was not found to delete!!!");

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

           



        }
    }

