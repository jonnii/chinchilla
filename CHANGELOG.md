# CHANGELOG

# Next

  * Bug fix for memory leak with ad-hoc publishing

# 0.3.12

  * Change json serializers to not wrap messages in a Message envelope

# 0.3.11

  * [jonrad] Ability to passing settings to the json net serializer
  * [bieri] Add a protobuf message serializer
  * [jonrad] Make consumers subscriber handle explicit interfaces (better F# support!)

# 0.3.10

  * Bump RabbitMQ Dependency

# 0.3.9

  * Ability to specify username/password in admin client

# 0.3.8
  * Bump RabbitMQ Dependency
  * Bump All Dependencies!!
  * Upgrade to .net 4.5.2

# 0.3.7

  * [thingylab] Fix nuspec references to rabbitmq client
  * [thingylab] Throw exception when starting subscription twice

# 0.3.6

  * Upgrade RabbitMQ Client

# 0.3.5

  * Upgrade RabbitMQ Client

# 0.3.4

  * Publishing without patch level version

# 0.3.3.0

 * Remove the PCL redirects from the app.config in the api package to try to appease the PCL gods

# 0.3.2.0

 * Upgrade RabbitMQ Client

# 0.3.1.0

 * Upgrade RabbitMQ Client
 * Upgrade MsgPack
 * Upgrade SimpleJson

# 0.3.0.0

 * [mzabolotko] Remove speakeasy dependency on api client, and make client a portable library
 * [BUG-#31] [tanir] Fault strategies cause exception when iterating delivery listeners multiple times

# 0.2.1.0

 * Interface serializers
 * Set username/machine on connection properties
 * Add an icon for the packages

# 0.2.0.0

 * Upgrade to .NET 4.5
 * Headers are now exposed on IDelivery
 * Messages can now be rejected (nack'd) by throwing a MessageRejectedException.

# 0.1.14.2

 * Fail all receipts on a publisher reconnect with a failure reason
 * Rename subscriber faults => subscriber failure strategy
 * Add the concept of a publisher failure handler strategy to handle confirmed messages that are nacked

# 0.1.14.1

  * Fix a bug with request/response cause message to end up on the error queue incorrectly
  * Change delivery to have a list of delivery listeners, so we can hang things a delivery happening.

# 0.1.14

  * Add an async version of Request/Response so you don't have to use the callbacks

# 0.1.13.1

  * Fix a bug when registering a serializer that overwrites an already registered serializer with the same content-type

# 0.1.13

  * Published messages will default to being marked as persistent, you can make them transient by marking them as ITransient
  * Json.NET is now no longer a dependency, serialization is now handled by SimpleJson
  * Added a new package for serialization with Json.NET

# 0.1.12.8

  * [cocowalla] updated logging API to support named loggers
  * [cocowalla] added logging integration for log4net
  * [cocowalla] added logging integration for NLog

# 0.1.12.7

  * [cocowalla] Don't attempt reconnect on auth errors

# 0.1.12.6

  * update to rabbitmq client 3.1
  * [cocowalla] default connection factory will retry connection with a delay if broker cannot be reached
  * [cocowalla] add support for setting ssl options on default connection factory

# 0.1.12.5

  * [cocowalla] bug fix for per message ttl
  * added contributor cocowalla (https://github.com/cocowalla)

# 0.1.12.4

  * update packages

# 0.1.12.3

  * update packages

# 0.1.12.2

  * update packages

# 0.1.12.1

  * add auto delete subscriber topology

# 0.1.12

  * ability to get a subscription by name from the bus

# 0.1.11

  * add support for messages with a custom timeout
  * add sample showing how to use timeouts

# 0.1.10.2

  * update to latest rabbit mq client

# 0.1.10.1

  * update to latest rabbit mq client

## 0.1.10

  * small interface changes
  * worker control, the ability to pause/resume a worker
  * give subscriptions a name

## 0.1.9.1

  * give workers a name so they can be more easily identified

## 0.1.9

  * add support for publisher confirms
  * publisher confirms are enabled by default

## 0.1.8

  * add ability to set correlation ids on messages
  * add ability to send requests with response handlers (rpc)
  * added additional chinchilla.api functions
  * thread safety fixes

## 0.1.7

  * task delivery strategy is now the default when creating a subscription
  * remove immediate delivery strategy

## 0.1.6.2

  * expose more state for subscriptions
  * expose subscription state on the bus

## 0.1.6

  * add the ability to query for the state of subscriptions
  * remove multi subscription, it's a bad idea

## 0.1.5

  * ability to change the message serializer globally and per publisher

## 0.1.4

  * better shutdown handling
  * worker pool will wait until all jobs are finished before terminating
  * each subscription gets one connection, not one connection per endpoint

## 0.1.3

  * delay starting delivery queues until a subscription is started

## 0.1.2

  * creating a subscription with an invalid delivery strategy will cause the subscription not to start
