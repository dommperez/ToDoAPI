using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.DATA.EF;
using ToDoAPI.API.Models;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoController : ApiController
    { 
        ToDoEntities db = new ToDoEntities();

        //Read 
        //api/ToDo
        //public IHttpActionResult GetToDoItems()
        //{
        //    List<ToDoItemViewModel> todoItems = db.TodoItems.Include("Category").Select(testc => new ToDoItemViewModel()
        //    {

        //    }).ToList<ToDoItemViewModel>();
        //}
    }
}
