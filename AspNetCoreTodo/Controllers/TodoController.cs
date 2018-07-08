using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        public async Task<IActionResult> Index()
        {
            // get to-do items from database
            var items = await _todoItemService.GetIncompleteItemsAsync();

            // put items into model
            var model = new TodoViewModel()
            {
                Items = items
            };

            // render view using the model
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var successful = await _todoItemService.AddItemAsync(newItem);

            if(!successful)
            {
                return BadRequest("Could not add item.");
            }

            return RedirectToAction("Index");
        }
    }
}