using TaskBoardApp.Data;
using TaskBoardApp.Extensions;
using TaskBoardApp.Web.ViewModels.Home;

namespace TaskBoardApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    public class HomeController : Controller
    {
        private readonly TaskBoardDbContext dbContext;
        public HomeController(TaskBoardDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var taskBoards = dbContext
                .Boards
                .Select(b => b.Name)
                .Distinct();

            var tasksCount = new List<HomeBoardModel>();

            foreach (var boardName in taskBoards)
            {
                var tasksInBoard = dbContext.Tasks.Count(t => t.Board.Name == boardName);

                tasksCount.Add(new HomeBoardModel()
                {
                    BoardName = boardName,
                    TasksCount = tasksInBoard
                });
            }

            var userTasksCount = -1;

            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = User.GetId();
                userTasksCount = dbContext.Tasks.Count(t => t.OwnerId == currentUserId);
            }

            var homeModel = new HomeViewModel()
            {
                AllTasksCount = dbContext.Tasks.Count(),
                BoardsWithTasksCount = tasksCount,
                UserTasksCount = userTasksCount
            };

            return View(homeModel);
        }
    }
}
