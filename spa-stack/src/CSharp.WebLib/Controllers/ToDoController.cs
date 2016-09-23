using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSharp.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.JsonPatch;

namespace CSharp.WebLib.Controllers
{
    [Route("api/[controller]")]
    public class ToDoController : Controller
    {
        public ToDoController(
            IToDoCommands commands,
            IToDoQueries queries
            )
        {
            this.commands = commands;
            this.queries = queries;
        }

        private IToDoCommands commands;
        private IToDoQueries queries;

        [HttpGet]
        public async Task<IEnumerable<ToDoItem>> Get()
        {
            return await queries.GetAll();
        }

        [HttpGet("{id}", Name = "GetToDo")]
        public async Task<IActionResult> Get(string id)
        {
            var item = await queries.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ToDoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(item.Id)) item.Id = Guid.NewGuid().ToString();
            
            await commands.Add(item);
            //The CreatedAtRoute method is intended to return a URI to the newly created 
            // resource when you invoke a POST method to store some new object.
            return CreatedAtRoute("GetToDo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ToDoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = await queries.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            await commands.Update(item);
            return new NoContentResult();
        }

        //http://benfoster.io/blog/aspnet-core-json-patch-partial-api-updates
        // this method is not currently used from the client
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody] JsonPatchDocument<ToDoItem> patch)
        {
            if (patch == null)
            {
                return BadRequest();
            }

            var todo = await queries.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            var patched = todo.Copy();

            patch.ApplyTo(patched, ModelState);

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            await commands.Update(patched);

            var model = new
            {
                original = todo,
                patched = patched
            };

            return Ok(model);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var todo = await queries.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            await commands.Remove(todo);
            return new NoContentResult();
        }

    }
}
