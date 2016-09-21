using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSharp.Models;
using Microsoft.AspNetCore.Routing;

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

        [HttpGet("{id}", Name = "GetTodo")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await queries.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(item.Id)) item.Id = Guid.NewGuid().ToString();

            //RouteValueDictionary rv = new RouteValueDictionary();
           //rv.Add("id",v)

            await commands.Add(item);
            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ToDoItem item)
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

        //[HttpPatch("{id}")]
        //public async Task<IActionResult> Update([FromBody] ToDoItem item, string id)
        //{
        //    if (item == null)
        //    {
        //        return BadRequest();
        //    }

        //    var todo = await queries.Find(id);
        //    if (todo == null)
        //    {
        //        return NotFound();
        //    }

        //    item.Id = todo.Id;

        //    await commands.Update(item);
        //    return new NoContentResult();
        //}

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
