using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoardApp.Data;
using TaskBoardApp.Extensions;
using TaskBoardApp.Services.Interfaces;
using TaskBoardApp.Web.ViewModels.Board;
using TaskBoardApp.Web.ViewModels.Task;
using Task = TaskBoardApp.Data.Models.Task;

namespace TaskBoardApp.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly IBoardService boardService;
        private readonly ITaskService taskService;
        private readonly TaskBoardDbContext dbContext;

        public TaskController(IBoardService boardService, ITaskService taskService, TaskBoardDbContext dbContext)
        {
            this.boardService = boardService;
            this.taskService = taskService;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            TaskFormModel viewModel = new TaskFormModel()
            {
                AllBoards = await this.boardService.AllForSelectAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AllBoards = await this.boardService.AllForSelectAsync();
                return View(model);
            }
            bool boardExists = await this.boardService.ExistsByIdAsync(model.BoardId);
            if (!boardExists)
            {
                ModelState.AddModelError(nameof(model.BoardId), "Selected board does not exist!");
                model.AllBoards = await this.boardService.AllForSelectAsync();
                return this.View(model);
            }
            
            string currentUserId = this.User.GetId();//GetId() is an extension method from TaskBoardApp.Extensions

            await this.taskService.AddAsync(currentUserId, model);

            return this.RedirectToAction("All", "Board");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                TaskDetailsViewModel viewModel = await this.taskService.GetForDetailsByIdAsync(id);
                return this.View(viewModel);
            }
            catch (Exception )
            {
                return this.RedirectToAction("All","Board");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var task = await dbContext.Tasks.FindAsync(id);

            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = User.GetId(); //GetId() is an extension method from TaskBoardApp.Extensions

            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            TaskFormModel taskModel = new TaskFormModel()
            {
                Title = task.Title,
                Description = task.Description,
                BoardId = task.BoardId,
                AllBoards = await this.boardService.AllForSelectAsync()
            };

            return View(taskModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, TaskFormModel model)
        {
            var task = await dbContext.Tasks.FindAsync(id);

            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = User.GetId(); //GetId() is an extension method from TaskBoardApp.Extensions

            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                model.AllBoards = await this.boardService.AllForSelectAsync();
                return View(model);
            }

            task.Title = model.Title;
            task.Description = model.Description;
            task.BoardId = model.BoardId;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("All", "Board");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var task = await dbContext.Tasks.FindAsync(id);

            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = User.GetId(); //GetId() is an extension method from TaskBoardApp.Extensions

            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }
            TaskViewModel taskModel = new TaskViewModel()
            {
                Id = task.Id.ToString(),
                Title = task.Title,
                Description = task.Description,
            };
            return View(taskModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(TaskViewModel model)
        {
            var task = await dbContext.Tasks.FindAsync(Guid.Parse(model.Id));

            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = User.GetId(); //GetId() is an extension method from TaskBoardApp.Extensions

            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            dbContext.Tasks.Remove(task);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("All", "Board");
        }
    }
}
