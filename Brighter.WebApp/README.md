# Brighter, Darker, and FluentValidation

a project to explore usage of Brighter, Darker, and FluentValidation in asp.net core

My approach is to start with a standard asp.net core app with identity and try to refactor the AccountController to use Brighter for commands, Darker for Queries and FluentValidation to validate viewmodels.

Using the architectural approach provided by Brighter and Darker seems like a great solution for moving logic out of controllers, and making "thin" controllers. In this POC I'm refactoring the AccountController which is not really that "heavy" to begin with, but I have some other projects with "heavy" controllers that I would like to refactor so experimenting with the patterns here.

The idea of FluentValidation is basically separation of concerns, ie separating validation logic from the models. Here I want to verify that I can still use the client side validation when I change the server side to use FluentValidation