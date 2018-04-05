# ~~Brighter, Darker,~~ Mediatr and FluentValidation

a project to explore usage of ~~Brighter, Darker~~ Mediatr, and FluentValidation in asp.net core

My approach is to start with a standard asp.net core app with identity and try to refactor the AccountController to use Brighter for commands, Darker for Queries and FluentValidation to validate viewmodels.

Using the architectural approach seems like a great solution for moving logic out of controllers, and making "thin" controllers. In this POC I'm refactoring the AccountController which is not really that "heavy" to begin with, but I have some other projects with "heavy" controllers that I would like to refactor so experimenting with the patterns here.

The idea of FluentValidation is basically separation of concerns, ie separating validation logic from the models. Here I want to verify that I can still use the client side validation when I change the server side to use FluentValidation

## Update

after starting out with Darker and then trying to use Brighter, I found that the interfaces for Brighter don't allow me to return a result from a command which I need  to do. Some info on the pros/cons of returning values from command processing [on stackoverflow](https://stackoverflow.com/questions/43433318/cqrs-command-return-values)

This got me to revisit using [Mediatr](https://github.com/jbogard/MediatR/wiki) instead of Brighter/Darker, it seems more popular on github as well and was recommended by a colleague.

Found a good github thread with an [example returning a result from a command](https://github.com/jbogard/MediatR/issues/141)
