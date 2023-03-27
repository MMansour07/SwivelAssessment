using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Swivel.Core.Interfaces;
using Swivel.Core.Model;
using Swivel.Data.Identity;
using Swivel.Data.Repositories;
using Swivel.Data.Unity;
using Swivel.Service;
using Swivel.Service.Infrastructure;
using Swivel.Service.Interfaces;
using Swivel.Service.Services;
using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;

namespace Swivel.Webclient
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

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
        }

        [Obsolete]
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            container.RegisterType<DbContext, DataContext>();

            container.RegisterType<UserManager<User>>();

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IUserStore<User>, UserStore<User>>();

            container.RegisterType<IAuthRepository, AuthRepository>();

            container.RegisterType<IJobRepository, JobRepository>();

            container.RegisterType<IMediaRepository, MediaRepository>();

            container.RegisterType<IUnitOfWork, UnitOfWork>();

            container.RegisterType<IAuthService, AuthService>();

            container.RegisterType<IJobService, JobService>();

            container.RegisterInstance<IMapper>(MappingProfile.Mapper);

        }
    }
}