﻿using TaskBoardApp.Web.ViewModels.Task;

namespace TaskBoardApp.Web.ViewModels.Board
{
    public class BoardAllViewModel
    {
        public string Name { get; set; } = null!;

        public IEnumerable<TaskViewModel> Tasks { get; set; } = null!;
    }
}
