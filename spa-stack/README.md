# spa-stack

This is just a little proof of concept to help me learn F# and polymer js

I know all the buzz these days is about reactjs, angularjs, and aurelia, but myself I am more drawn towards web components and polymer with vanillajs. Web components are emerging web browser standards and therefore part of the web platform. It seems more future proof to learn and build things with web standards technology than to use the trendy js framework of the month or year.

So my front end stack is web components and my back end stack is ASP.NET Core with C# and F#
I'm just starting to learn F# and functional programming but I am very interested in it and want to try to use it as much as possible.

Currently there is a basic polymer starter kit ui with a ToDo list compponent that talks to a web api written in C#.
So far I have translated the C# ToDoController into F# but the methods are not async even though they call async methods on the Command and Queries objects. I'm having trouble figuring out the right syntax to get it working

Note that this proof of concept shows that you can use F# currently with .NET Core and ASP.NET Core
However there are some pretty bad well known tooling issues and VS reports a lot of errors, the compiler shows that it builds with no errors so that is what is true, but it makes it harder to know whether the code is correct at any given time unless you re-compile. ie you can only trust the errors in the build output at the moment. You also can't set break points or step through the F# code in VS.
The tooling issues are temporary and Microsoft is working to solve them.

Also note that since the F# controller is in a class library referenced by the main web app, you have to actually rebuild the whole solution before changes in the F# controller code are reflected in the app, this messed me up for a while because I would make code changes and nothing changed in the browser.

I chose C# for the main web app and am putting the F# code into class libraries, because there is no F# support for Razor. So it is better to use C# for the main app in case you want to also use MVC with Razor Views. At the moment I'm only using api controllers and html
