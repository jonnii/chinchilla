// using Chinchilla.Integration.Features.Messages;
// using Chinchilla.Serializers.MsgPack;
// using NUnit.Framework;

// namespace Chinchilla.Integration.Features
// {
//     [TestFixture]
//     public class CustomSerializersFeature : WithApi
//     {
//         [Test]
//         public void ShouldHaveDefaultSerializer()
//         {
//             var settings = new DepotSettings
//             {
//                 MessageSerializers =
//                 {
//                     Default = new MessagePackMessageSerializer()
//                 }
//             };

//             using (var bus = Depot.Connect("localhost/integration", settings))
//             {
//                 HelloWorldMessage lastReceived = null;

//                 bus.Subscribe((HelloWorldMessage hwm) =>
//                 {
//                     lastReceived = hwm;
//                 });

//                 bus.Publish(new HelloWorldMessage { Message = "subscribe!" });

//                 WaitForDelivery();

//                 Assert.That(lastReceived, Is.Not.Null);
//                 Assert.That(lastReceived.Message, Is.EqualTo("subscribe!"));
//             }
//         }

//         [Test]
//         public void ShouldCustomizePublisherWithContentType()
//         {
//             var settings = new DepotSettings();
//             settings.MessageSerializers.Register(new MessagePackMessageSerializer());

//             using (var bus = Depot.Connect("localhost/integration", settings))
//             {
//                 HelloWorldMessage lastReceived = null;

//                 bus.Subscribe((HelloWorldMessage hwm) =>
//                 {
//                     lastReceived = hwm;
//                 });

//                 using (var publisher = bus.CreatePublisher<HelloWorldMessage>(
//                     p => p.SerializeWith("application/x-msgpack")))
//                 {
//                     publisher.Publish(new HelloWorldMessage { Message = "subscribe!" });
//                     WaitForDelivery();
//                 }

//                 Assert.That(lastReceived, Is.Not.Null);
//                 Assert.That(lastReceived.Message, Is.EqualTo("subscribe!"));
//             }
//         }
//     }
// }
