# (ISC)² Interview Coding Exercise

This repository contains a coding exercise for use in the hiring process for Sitecore developers.

## Pub/Sub Pattern

The idea behind the exercise is to complete the service layer of a simple [pub/sub pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/PublishSubscribeChannel.html) along with an implementation of an interface to handle emitted messages on a channel.

## Instructions for Candidates

* To get started, clone or fork the repository, working off of the ```Unsolved``` branch.
* The ultimate goal of this task is to complete the pub/sub service layer and an implementation of the ```IHandleMessages``` interface.
    * The implementation (```IncomingLeadHandler```) should handle messages on its subscribed channel and insert any _valid_ leads handled.
    * For purposes of this exercise, we will consider any lead to be valid if it has a ```FirstName``` which is not ```null```.
* Complete the implementations of all incomplete methods in the following classes:
    * ```PubSubService```
    * ```IncomingLeadHandler```
* ```IncomingLeadHandlerTest``` contains a suite of integration tests which should pass if the implementation is to specification.
    * The tests make no assumptions about coding style or how you go about implementing the task, just that the outlined goals are achieved.
    * The only strict expectations are outlined in the method summary comments. The rest is up to you!
* This exercise is open book and you may use any resources online you wish to.
* When completed, push the solution to a Git repository of your own (Bitbucket, Github, GitLab, etc.) and provide the URL to your interviewer.