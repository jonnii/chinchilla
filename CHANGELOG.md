# CHANGELOG

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