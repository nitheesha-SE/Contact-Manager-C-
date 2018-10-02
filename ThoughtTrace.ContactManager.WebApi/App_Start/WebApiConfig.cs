using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json.Serialization;
using ThoughTrace.ContactManager.Data;
using ThoughTrace.ContactManager.Data.Model;

namespace ThoughtTrace.ContactManager.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(context => SeedData())
                .SingleInstance();

            builder.Register(context => GroupData())
                .SingleInstance();

            builder.RegisterType<InMemoryContactRepository>()
                .As<IContactRepository>().InstancePerRequest();

            builder.RegisterType<InMemoryGroupRepository>()
                .As<IGroupRepository>().InstancePerRequest();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            // Web API routes
            config.MapHttpAttributeRoutes();
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private static ConcurrentDictionary<Guid, Contact> SeedData()
        {
            var store = new ConcurrentDictionary<Guid, Contact>();

            AddContact(store, "sayidjarrah@example.com", "Sayid", "Jarrah");
            AddContact(store, "jackshephard@example.com", "Jack", "Shephard");
            AddContact(store, "4815162342@example.com", "Hugo", "Reyes");
            AddContact(store, "johnlocke@example.com", "John", "Locke");
            AddContact(store, "jamesford@example.com", "James", "Ford");
            AddContact(store, "kateausten@example.com", "Kate", "Austen");
            AddContact(store, "michaeldawson@example.com", "Michael", "Dawson");
            AddContact(store, "waltlloyd@example.com", "Walt", "Lloyd");
            AddContact(store, "charliepace@example.com", "Charlie", "Pace");
            AddContact(store, "desmondhume@example.com", "Desmond", "Hume");
            AddContact(store, "clairelittleton@example.com", "Claire", "Littleton");
            AddContact(store, "kwon@example.com", "Jin-Soo", "Kwon");
            AddContact(store, "kwon@example.com", "Sun-Hwa", "Kwon");
            AddContact(store, "boonecarlyle@example.com", "Boone", "Carlyle");
            AddContact(store, "benjaminlinus@example.com", "Benjamin", "Linus");

            return store;
        }

        private static void AddContact(ConcurrentDictionary<Guid, Contact> store, string email, string firstName, string lastName)
        {
            var guid = Guid.NewGuid();
            store.TryAdd(guid,
                new Contact { Email = email, FirstName = firstName, LastName = lastName, Id = guid });
        }

        private static ConcurrentDictionary<Guid, Group> GroupData()
        {
            var store = new ConcurrentDictionary<Guid, Group>();
            List<Contact> contacts = new List<Contact>();
            contacts.Add(new Contact { Email = "nithi@gmail.com", FirstName = "nithi", LastName = "reddy", Id = Guid.NewGuid() });

            AddGroup(store, "group1", "sample description", contacts);

            return store;
        }

        private static void AddGroup(ConcurrentDictionary<Guid, Group> store, string name, string description, List<Contact> contacts)
        {
            var guid = Guid.NewGuid();
            store.TryAdd(guid, new Group { Name = name, Description = description, contacts = contacts, Id = guid });
        }
    }
}
