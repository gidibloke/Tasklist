using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webapp.Attributes;
using Webapp.LookupModels;
using Webapp.Models;

namespace Webapp.ViewModels
{
    public class TasksViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Subject")]
        [StringLength(30)]
        [Required]
        public string Subject { get; set; }

        [Display(Name = "Description")]
        [StringLength(500)]
        [Required]

        public string Description { get; set; }

        public string AssigningUserId { get; set; }

        [Display(Name = "Assign to (start typing name)")]

        public string AssignedToUserId { get; set; }
        [Display(Name = "Assign to (start typing name)")]

        public string AutoCompletebox { get; set; }

        [Display(Name = "")]
        public DateTime? DateAssigned { get; set; }

        [Display(Name = "Select status")]
        public int? TaskStatusId { get; set; }
        public IEnumerable<SelectListItem> Status { get; set; }
        public IEnumerable<SelectListItem> Priority { get; set; }


        [Display(Name = "Date completed")]
        public DateTime? DateCompleted { get; set; }

        [Display(Name = "Select Priority?")]
        [Required]
        public int PriorityId { get; set; }

        [Required]
        [Display(Name = "Date Due")]
        [FutureOnlyDates]
        [DataType(DataType.Date)]
        public DateTime DateDue { get; set; }

    }

    public class TasksViewModelValidator : AbstractValidator<TasksViewModel>
    {
        public TasksViewModelValidator()
        {
            When(x => x.DateCompleted != null, () =>
            {
                RuleFor(x => x.DateCompleted).GreaterThanOrEqualTo(x => x.DateAssigned).WithMessage("Date completed cannot be before date assigned");
            });
            When(x => x.DateCompleted != null && x.DateAssigned != null, () =>
            {
                //Depends on the spec. Users might need to enter tasks that have been completed before it was assigned for record purpose
                RuleFor(x => x.DateCompleted).GreaterThanOrEqualTo(x => x.DateAssigned).WithMessage("Date completed cannot be before the assigned date");
            });
        }
    }
}
