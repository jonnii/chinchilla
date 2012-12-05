# CHANGELOG

## 0.1.4

  * better shutdown handling
  * worker pool will wait until all jobs are finished before terminating
  * each subscription gets one connection, not one connection per endpoint

## 0.1.3

  * delay starting delivery queues until a subscription is started

## 0.1.2

  * creating a subscription with an invalid delivery strategy will cause the subscription not to start