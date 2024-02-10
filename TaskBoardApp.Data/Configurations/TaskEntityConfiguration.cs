namespace TaskBoardApp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System.Diagnostics.Metrics;
    using System;
    using Task = TaskBoardApp.Data.Models.Task;
    public class TaskEntityConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasOne(t => t.Board)
                .WithMany(b => b.Tasks)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(this.GenerateTasks());
        }

        private ICollection<Task> GenerateTasks()
        {
            ICollection<Task> tasks = new HashSet<Task>()
            {
                new Task()
                {
                    Title = "Improve CSS styles",
                    Description = "Implement better styling for all public pages",
                    CreatedOn = DateTime.Now.AddDays(-200),
                    OwnerId = "55a451d4-bdb8-42d2-817d-aea8cce4240c",
                    BoardId = 1,
                },

                new Task()
                {
                    Title = "Android Client App",
                    Description = "Create Android client app for the TaskBoard RESTful API",
                    CreatedOn = DateTime.Now.AddMonths(-5),
                    OwnerId = "55a451d4-bdb8-42d2-817d-aea8cce4240c",
                    BoardId = 1,
                },

                new Task()
                {
                    Title = "Desktop Client App",
                    Description = "Create Windows Forms desktop app client for the TaskBoard RESTful API",
                    CreatedOn = DateTime.Now.AddMonths(-1),
                    OwnerId = "e3751391-c68a-4753-8bc7-9461604e3032",
                    BoardId = 2,
                },

                new Task()
                {
                    Title = "Create Tasks",
                    Description = "Implement [Create Task] page for adding new tasks",
                    CreatedOn = DateTime.Now.AddYears(-1),
                    OwnerId = "e3751391-c68a-4753-8bc7-9461604e3032",
                    BoardId = 3,
                },
            };
            return tasks;
        }
    }
}
