using AutoMapper;
using FluentValidation;
using System;
using System.Data.Entity;
using Unity;
using Webapp.Interfaces;
using Webapp.Models;
using Webapp.Repositories;
using Webapp.Services;
using Webapp.ViewModels;

namespace Webapp
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();
            container.RegisterType<DbContext, ApplicationDbContext>();
            container.RegisterType<IFileUplaod, FileUploadService>();
            container.RegisterType<ITasks, TasksRepository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tasks, TasksViewModel>().ReverseMap();
            });
            var mapper = configuration.CreateMapper();
            container.RegisterInstance(mapper);
            var validators = AssemblyScanner.FindValidatorsInAssemblyContaining<TasksViewModelValidator>();
            validators.ForEach(validator => container.RegisterType(validator.InterfaceType, validator.ValidatorType));

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}