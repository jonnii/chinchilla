// using System.Threading;
// using Chinchilla.Api;
// using NUnit.Framework;

// namespace Chinchilla.Integration.Features
// {
//     public class WithApi : WithLogging
//     {
//         public readonly VirtualHost IntegrationVHost = new VirtualHost("integration");

//         protected IRabbitAdmin admin;

//         [SetUp]
//         public void ResetVirtualHost()
//         {
//             admin = new RabbitAdmin("http://localhost:15672/api");
//             admin.Delete(IntegrationVHost);
//             admin.Create(IntegrationVHost);
//             admin.Create(IntegrationVHost, new User("guest"), Permission.All);
//         }

//         protected void WaitForDelivery()
//         {
//             Thread.Sleep(500);
//         }
//     }
// }