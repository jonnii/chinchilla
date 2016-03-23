chinchilla
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

For more code samples check out the [features](README.md#features) section below.

There are also code samples and integration tests available in the `src\` directory which
show additional usage patterns.

Alternative choices?
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

 * @jonnii (https://github.com/jonnii) [![endorse](http://api.coderwall.com/jonnii/endorsecount.png)](http://coderwall.com/jonnii)
 * @cocowalla (https://github.com/cocowalla)
 * @jamescrowley (https://github.com/jamescrowley)
 * @tanir (https://github.com/tanir)

Builds
======

Builds are run using a [teamcity instance provided by codebetter](http://teamcity.codebetter.com/).

Feature Overview
================

Below is a basic overview of the features that are available in Chinchilla. It assumes a 
working knowledge of the RabbitMQ concept such as Exchanges, Queues, Routing and Virtual Hosts.
If you're unsure what any of those terms means then I suggest you read the 
[getting started guide](http://www.rabbitmq.com/getstarted.html) on the RabbitMQ website.

 * [Connecting to RabbitMQ](README.md#connecting-to-rabbitmq)
 * [Publishers](README.md#publishers)
 * [Pluggable Topologies](README.md#pluggable-topologies)
 * [Custom Routing](README.md#custom-routing)
 * [Subscribers](README.md#subscribers)
 * [Error Handling](README.md#error-handling)
 * [Request Response](README.md#request-response)
 * [Consumers](README.md#consumers)
 * [Custom serializers](README.md#custom-serializers)
 * [Integration with DI Containers](README.md#integration-with-di-containers)
 * [Integration with Logging Libraries](README.md#integration-with-logging-libraries)
 
## Connecting to RabbitMQ

Before you do anything else you'll need to create an instance of an `IBus`, which can be done
by using the `Depot`. Here's the smallest example possible of doing that:

````
// Create a bus!
IBus bus = Depot.Connect("localhost");
````

More often than not you won't be connecting to a server, but to a virtual host:

````
// Use a virtual host
IBus bus = Depot.Connect("server-name/my-awesome-virtual-host");
````

You can use any connection string that is supported by the RabbitMQ client library:

````
IBus bus = Depot.Connect("amqp://user:pass@host:10000/vhost");
````

One thing to keep in mind is that `IBus` implements `IDisposable`, which means you 
should make sure you dispose of it correctly. This will ensure all your messages
are published, all your subscribers are done processing any messages they are 
currently working on and all the resources are cleaned up.

### SSL

You can connect to an [SSL-enabled RabbitMQ server](http://www.rabbitmq.com/ssl.html) by providing the default connection factory with a configured instance of `RabbitMQ.Client.SslOption`:

````
var sslConfig = new SslOption
{
    Enabled = true,
  
    // Path to client certificate, if client authentication is required by the server
    CertPath = "my-cert.p12"
};

var settings = new DepotSettings()
{
    ConnectionFactoryBuilder = () => new DefaultConnectionFactory()
    {
        SslOptions = sslConfig     
    }
};

IBus bus = Depot.Connect("server-name:5671", settings);
````


## Publishers

To publish a message you'll first need to create a message type to publish. For example:

````
public class CustomerOrderMessage
{
   public string Name { get; set; }
   public int TableNumber { get; set; }
   public string Starter { get; set; }
   public string Main { get; set; }
   public string Dessert { get; set; }
}
````

You can use any type as a message as long as it's serializable by the configured default serializer. Most
of the time that just means you'll need a public default constructor.

Let's publish a customer order:

````
var customerOrder = new CustomerOrderMessage
{
    Name = "Bob Jones",
    TableNumber = 5,
    Starter = "Neeps, tatties and haggis",
    Main = "Haggis, neeps and tatties",
    Dessert = "Deep Fried Mars Bar"
};

bus.Publish(customerOrder);
````

When using the `Publish` method on `IBus` Chinchilla will create a single use publisher. If you need to publish
many messages it's recommended that you create a typed publisher:

````
var publisher = bus.CreatePublisher<CustomerOrderMessage>();
publisher.Publish(customerOrder);
````

Typed publishers implement `IDisposable`, so make sure you dispose of them properly when you're done with them.

It's also possible to configure your publishers when you create them, for example:

````
// publish to a named exchange
bus.CreatePublisher<CustomerOrderMessage>(o => o.PublishOn("order-exchange"));

// publish using a custom router (see more below about custom routing)
bus.CreatePublisher<CustomerOrderMessage>(o => o.RouteWith<CustomRouter>());

// use a different serialize for this publisher
bus.CreatePublisher<CustomerOrderMessage>(o => o.SerializeWith("application/x-msgpack"));
````

There are more examples of customizing a publisher below for specific use cases below.

## Pluggable Topologies

Simply put, a "Topology" is the answer to "How are my RabbitMQ exchanges/queues configured?". Chinchilla
lets you configure your topology in code and have that applied to the server. Infact, it comes with a few default
topologies, which work out of the box to support things like Pub/Sub and Request/Response.

There are many reasons why you might want to customize your topology, for example you may want to setup custom routing
or even setup `AutoDelete` consumers for elastic distribution of work loads. Whatever your use case it's straight 
forward to customize this part of Chinchilla. 

An example of a custom topology is the `AutoDeleteSubscribeTopologyBuilder`:

````
public class AutoDeleteSubscribeTopologyBuilder : IMessageTopologyBuilder
{
    // We're going to have to set up the topology on a per end point basis
    // as each subscriber can have multiple endpoints
    public IMessageTopology Build(IEndpoint endpoint)
    {
        // Define our new topology
        var topology = new MessageTopology();

        // We're going to use a topic based exchange
        var exchange = topology.DefineExchange(endpoint.MessageType, ExchangeType.Topic);

        // By not supplying a queue name the Queue will be `AutoDelete`
        topology.SubscribeQueue = topology.DefineQueue();
        
        // Bind the subscribe queue to the exchange
        topology.SubscribeQueue.BindTo(exchange);

        return topology;
    }
}
````

If you want to configure RabbitMQ manually this is also possible, you can choose to just configure the
`PublishExchange` and `SubscribeQueue` and omit any bindings.

## Custom Routing

One of the most powerful features of RabbitMQ is the topic based routing, which will route your message to a
queue based upon the message's routing key. Chinchilla offers a few different ways of changing the routing key.
The easiest of which is to implement `IHasRoutingKey` in your message:

````
public class PriceMessage : IHasRoutingKey
{
    public PriceMessage() { }

    public PriceMessage(string ticker, int price)
    {
        Ticker = ticker;
        Price = price;
    }

    public string Ticker { get; set; }

    public int Price { get; set; }

    public string RoutingKey
    {
        get { return "prices." + Ticker; }
    }
}
````

Alternatively you can make a custom router:

````
public class CustomRouter : DefaultRouter
{
    public override string Route<TMessage>(TMessage message)
    {
        var order = message as CustomOrderMessage;
        if(order != null)
        {
            return order.TableNumber;
        } 

        return base.Route(message);
    }
}
````

## Subscribers

There are a few ways to subscribe to messages using Chinchilla, but the easiest way is to call `Subscribe` on an
instance of `IBus`, this will create a consumer that subscribes on a queue with a name that matches the type you're
subscibing to:

````
// Subscribe to a message using a lambda
bus.Subscribe((CustomerOrderMessage message) => { 
    Console.WriteLine("We got an order for table number: {0}", message.TableNumber);
});

// Subscribe to a message using a handler method
bus.Subscribe(OnCustomerMessage);
````

You can also customize a subscription:

````
// Subscribe on a queue with a custom name
bus.Subscribe(OnCustomerMessage, o => 
    o.SubscribeOn("weird-customer-messages"));

// Deliver messages to the subscription using a custom delivery strategy
bus.Subscribe(OnCustomerMessage, o => 
    o.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 5));
````

Chinchilla also lets you subscribe on multiple endpoints, which is an abstraction over having a single
RabbitMq consumer with multiple consumer queues:

````
bus.Subscribe(OnCustomerMessage, o => o.SubscribeOn("queue-1", "queue-2"));
````

This subscription will attempt to take a message off of `queue-1`, and if no messages are available on 
that queue will then attempt to take to a message off of `queue-2`. This is a useful feature for simulating
priority queues with RabbitMq.

## Error Handling

If during the course of processing a message your handler throws an exception then Chinchilla
will put that message on a queue named `ErrorQueue` along with information about the cause of the problem, such
as the stack trace. If you want to change the way failures are handled you can change the failure strategy:

````
// Change the fault strategy on a subscription
bus.Subscribe(OnCustomerMessage, 
    o => o.OnFailure<CustomFaultStrategy>());
    
// Change the fault strategy on a subscription and configure it
bus.Subscribe(OnCustomerMessage, 
    o => o.OnFailure<CustomFaultStrategy>(s => s.NotifyByEmail = true));
````

If you have enabled publisher confirms then it's also possible to change the strategy used for failed 
publishes. For example:

````
bus.CreatePublisher<CustomerOrderMessage>(
	o => o.Confirm(true).OnFailure<RetryOnFailures>())

public class RetryOnFailures : IPublisherFailureStrategy<HelloWorldMessage>
{
    public void OnFailure(IPublisher<CustomerOrderMessage> publisher, CustomerOrderMessage failedMessage, IPublishReceipt receipt)
    {
		// publish the message again...
        publisher.Publish(failedMessage);
    }
}
````

## Request Response

Request/Response is one of the more common use cases with RabbitMq, it is used for RPC and can be used to replace
an unreliable transport like HTTP with a durable queued backed transport. 

The easiest way to do request/response is right off of an instance of `IBus`:

````
bus.Request<MyRequestMessage, MyResponseMessage>(
    new MyRequestMessage("where am i?"),
    (MyResponseMessage response) => { 
        // I got a response!
        Console.WriteLine(response);
    });
````

Both your Request and Response message must implement `ICorrelated`.

If you plan on sending a lot of request/response messages it might make sense to use a Requester:

````
var requester = bus.CreateRequester<CapitalizeMessage, CapitalizedMessage>();
````

Requesters implement `IDisposable` and will need to be disposed of properly for Chinchilla to shutdown cleanly.

## Consumers

What is a consumer? Consumers are good for consuming messages.

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

### Alternative Serialization
The following are alternatives are available on Nuget:
* [Chinchilla.Serialziers.JsonNET](https://www.nuget.org/packages/Chinchilla.Serializers.JsonNET/)
* [Chinchilla.Serializers.MsgPack](https://www.nuget.org/packages/Chinchilla.Serializers.MsgPack/)
* [Chinchilla.Serializers.Protobuf*](https://www.nuget.org/packages/Chinchilla.Serializers.Protobuf/)

*Protobuf serialization requires the registration of the protoc generated classes with the serializer.
````
var serializers = new MessageSerializers();
serializers.Default = ProtobufMessageSerializer.Create(s =>
{
	s.Register(PriceMessage.Parser);
	s.Register(ConnectMessage.Parser);
});
````


## Integration with DI Containers

Integration with a dependency injection container is the recommended way of working with Chinchilla.

## Integration with Logging Libraries

By default chinchilla will not write to the console or log any output, but it can be configured to 
log its internal messages anywhere you want by setting `Logger.Factory` to an ILoggerFactory before
creating an instance of IBus. If you are using NLog or log4net then there are already pre-built packages:

 * [Chinchilla.Logging.NLog](https://www.nuget.org/packages/Chinchilla.Logging.NLog/)
 * [Chinchilla.Logging.Log4Net](https://www.nuget.org/packages/Chinchilla.Logging.Log4Net/)


[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/jonnii/chinchilla/trend.png)](https://bitdeli.com/free "Bitdeli Badge")

