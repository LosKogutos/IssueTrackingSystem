using IssueTrackingSystem.Services;
using IssueTrackingSystem.Services.Interfaces;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace IssueTrackingSystem
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<ISpaceService, SpaceService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}