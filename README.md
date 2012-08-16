chinchilla [![endorse](http://api.coderwall.com/jonnii/endorsecount.png)](http://coderwall.com/jonnii)
==========

Rabbits are furry and lovable, and what's more lovable and furry than a Chinchilla?
What does this have to do with anything? Well, this is a RabbitMQ library to help 
you be more awesome with RabbitMQ when you need to be awesome, which is every day.

Show me the goods
=================
 
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

Samples?
========

There is currently one sample which demonstrates publishing and subscribing to ticker/price updates, there
will be more samples soon... I promise.

Alternatives choices?
=====================

There are plenty of fantastic RabbitMQ libraries out there and you should evaluate
each and every one of them before making a choice. I suggest looking at:

EasyNetQ (http://easynetq.com/)
MassTransit (http://masstransit-project.com/)
Burrow.NET (https://github.com/vanthoainguyen/Burrow.NET)
RabbitBus (https://github.com/derekgreer/rabbitBus)

Each one is slightly different, and depending on your needs might fit your requirements better.