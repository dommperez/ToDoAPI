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
        public IHttpActionResult GetTodo()
        {
            //Create list to house resources 
            List<ToDoItemViewModel> todo = db.TodoItems.Include("Category").Select(t => new ToDoItemViewModel()
            {
                //This section of code is translating the database Resource objects to the Data Transfer Object. This is called abstraction, as we are adding a layer which consuming apps will access instead of accessing the domain models directly.
                TodoId = t.TodoId,
                Action = t.Action,
                Done = t.Done,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }

            }).ToList<ToDoItemViewModel>();

            //Check on the results and if there are no results we will send back to the consuming app a 404
            if (todo.Count == 0)
            {
                return NotFound();//404 error
            }

            return Ok(todo);
        }//end GetResources

        //api/Resources/id
        //Details
        public IHttpActionResult GetResource(int id)
        {
            //Createa new ResourceViewModel object and assign it the value of the appropriate
            //resource from the db
            ToDoItemViewModel todo = db.TodoItems.Include("Category").Where(t => t.TodoId == id).Select(t => new ToDoItemViewModel()
            {
                //copy our assignments from above - GetResources
                TodoId = t.TodoId,
                Action = t.Action,
                Done = t.Done,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            }).FirstOrDefault();

            //Check that there is a resource and return the resource with an OK (200) response
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        //api/Resources (HttpPost)
        public IHttpActionResult PostResource(ToDoItemViewModel todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            TodoItem newTodo = new TodoItem()
            {
                Action = todo.Action,
                Done = todo.Done
            };

            db.TodoItems.Add(newTodo);
            db.SaveChanges();
            return Ok(newTodo);
        }//end PostResource

        //api/Resources (HttpPut)
        public IHttpActionResult PutResource(ToDoItemViewModel todo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Data");//scopeless if... this is one of the few good examples of scopeless, with just one line of code in the scope this if 

            //get the resource from the db that we want to edit
            TodoItem existingTodo = db.TodoItems.Where(t => t.TodoId == todo.TodoId).FirstOrDefault();

            //If the resource isnt null, then we will reassign its values from the consuming applications request
            if (existingTodo != null)
            {
                existingTodo.TodoId = todo.TodoId;
                existingTodo.Action = todo.Action;
                existingTodo.Done = todo.Done;
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end PutResource

        //api/Resource (HttpDelete)
        public IHttpActionResult DeleteResource(int id)
        {
            TodoItem todo = db.TodoItems.Where(t => t.TodoId == id).FirstOrDefault();

            if (todo != null)
            {
                db.TodoItems.Remove(todo);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//end DeleteResource

        //We use the Dispose() below to dispose of any connections to the db after we are done
        //with them. Best Practive 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();//dispose the db connection
            }
            base.Dispose(disposing);//dispose the instance of the controller
        }

    }
}
