using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBoardApp.Web.ViewModels.Home
{
    public class HomeViewModel
    {
        public int AllTasksCount { get; init; }

        public List<HomeBoardModel> BoardsWithTasksCount { get; init; } = null!;
        public int UserTasksCount { get; init; }
    }
}
