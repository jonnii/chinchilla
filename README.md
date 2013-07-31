chinchilla [![endorse](http://api.coderwall.com/jonnii/endorsecount.png)](http://coderwall.com/jonnii)
==========

Rabbits are furry and lovable, and what's more lovable and furry than a Chinchilla?
What does this have to do with anything? Well, this is a RabbitMQ library to help 
you be more awesome with RabbitMQ when you need to be awesome, which is every day.

Show me the goods
=================
 
````
// connect to a rabbitmq server
var bus = Depot.Connect("server")

// publish a message
bus.Publish(new HelloWorldMessage());

// publish a lot of messages
using (var publisher = bus.CreatePublisher<HelloWorldMessage>())
{
    // this will use a publisher dedicated to this message type
    publisher.Publish(new HelloWorldMessage());
}

// subscribe to messages
bus.Subscribe((HelloWorldMessage message) => { Console.WriteLine("WOAH!") });

// subscribe with a message consumer
bus.Subscribe(new HelloWorldMessageConsumer());

// subscribe and process messages with a number of workers
bus.Subscribe((HelloWorldMessage message) => {}, 
     o => o.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 5));

// subscribe and process messages on the task factory
bus.Subscribe((HelloWorldMessage message) => {}, 
    o => o.DeliverUsing<TaskDeliveryStrategy>());
````

Samples?
========

If you're interested in more sample code, there are samples available in the `src\` directory.

Alternatives choices?
=====================

There are plenty of fantastic RabbitMQ libraries out there and you should evaluate
each and every one of them before making a choice. I suggest looking at:

* EasyNetQ (http://easynetq.com/)
* MassTransit (http://masstransit-project.com/)
* Burrow.NET (https://github.com/vanthoainguyen/Burrow.NET)
* RabbitBus (https://github.com/derekgreer/rabbitBus)

Each one is slightly different, and depending on your needs might fit your requirements better.

Contributors
============

 * @jonnii (https://github.com/jonnii)
 * @cocowalla (https://github.com/cocowalla)

Features
========

 * [Custom serializers](README.md#custom-serializers)
 * [Integration with DI Containers](README.md#integration-with-di-containers)
 * [Integration with Logging Libraries](README.md#integration-with-logging-libraries)

## Custom serializers

By default Chinchilla will serialize your messages as json with the content type of `application/json`,
which is great for inspecting messages on the wire and for interop with other languages. However there
are situations where you will want to serialize to another format, for example a binary format, to 
optimize payload size.

You can change the default serializer on the `DepotSettings` you use to create your depot:

````
var settings = new DepotSettings
{
    MessageSerializers =
    {
        Default = new MessagePackMessageSerializer()
    }
};

var depot = Depot.Connect("connection-string", settings);
````

It's also possible to create a publisher with a custom serializer:

````
var settings = new DepotSettings();

// First register the custom serializer
settings.MessageSerializers.Register(new MessagePackMessageSerializer());

var bus = Depot.Connect("localhost/integration", settings)

// create a publisher with the custom content type
var publisher = bus.CreatePublisher<TMessage>(
    p => p.SerializeWith("application/x-msgpack"))

publisher.Publish(...);
````

Chinchilla decides which serializer to use to deserialize an incoming message based 
upon the `content-type` of that message. Registering a custom serializer is enough 
to be able to subscribe to message with that format.

## Integration with DI Containers

Integration with a dependency injection container is the recommended way of working with Chinchilla.

## Integration with Logging Libraries

By default chinchilla will not write to the console or log any output, but it can be configured to 
log its internal messages anywhere you want by setting `Logger.Factory` to an ILoggerFactory before
creating an instance of IBus. If you are using NLog or log4net then there are already pre-built packages:

 * [Chinchilla.Logging.NLog](https://www.nuget.org/packages/Chinchilla.Logging.NLog/)
 * [Chinchilla.Logging.Log4Net](https://www.nuget.org/packages/Chinchilla.Logging.Log4Net/)
