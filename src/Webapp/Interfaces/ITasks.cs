using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapp.Dtos;
using Webapp.Helpers;
using Webapp.Models;
using Webapp.ViewModels;

namespace Webapp.Interfaces
{
    public interface ITasks
    {
        Task<Result<List<TasksViewModel>>> GetAllTasksAsync(string userId);
        Task<Result<TasksViewModel>> GetTaskAsync(Guid taskId);
        Task<Result<Tasks>> GetTaskEntityAsync(Guid taskId);
        void AddTasks(TasksViewModel task);
        void UpdateTasks(Tasks task);
        void RemoveTasks(Tasks tasks);

        //Task<Result<DashboardDto>> GetDashboardAsync();
        Task<bool> SaveChangesAsync();
        Task<Result<List<AutocompleteDto>>> GetUsers(string FirstName);
        void Populatedropdown(TasksViewModel data);
        Task<Result<List<TaskIndexDto>>> GetTaskIndexAsync(string userId);
    }
}

