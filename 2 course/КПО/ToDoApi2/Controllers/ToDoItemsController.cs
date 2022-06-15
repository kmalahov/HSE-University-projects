using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ToDoApi2.Models;

namespace ToDoApi2.Controllers
{
    public class ToDoItemsController : ApiController
    {
        private ToDoContext db = new ToDoContext();

        // GET: api/ToDoItems
        public IQueryable<ToDoItem> GetTodoItems()
        {
            return db.TodoItems;
        }

        // GET: api/ToDoItems/5
        [ResponseType(typeof(ToDoItem))]
        public IHttpActionResult GetToDoItem(long id)
        {
            ToDoItem toDoItem = db.TodoItems.Find(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return Ok(toDoItem);
        }

        // PUT: api/ToDoItems/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutToDoItem(long id, ToDoItem toDoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != toDoItem.Id)
            {
                return BadRequest();
            }

            db.Entry(toDoItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ToDoItems
        [ResponseType(typeof(ToDoItem))]
        public IHttpActionResult PostToDoItem(ToDoItem toDoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TodoItems.Add(toDoItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = toDoItem.Id }, toDoItem);
        }

        // DELETE: api/ToDoItems/5
        [ResponseType(typeof(ToDoItem))]
        public IHttpActionResult DeleteToDoItem(long id)
        {
            ToDoItem toDoItem = db.TodoItems.Find(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            db.TodoItems.Remove(toDoItem);
            db.SaveChanges();

            return Ok(toDoItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ToDoItemExists(long id)
        {
            return db.TodoItems.Count(e => e.Id == id) > 0;
        }
    }
}