# spa-stack

This is just a little proof of concept to help me learn F# and polymer js

Currently there is a basic polymer starter kit ui with a ToDo list compponent that talks to a web api written in C#.
I'm in the process of translating the C# ToDoController into F# but not finished yet

Note that this proof of concept shows that you can use F# currently with .NET Core and ASP.NET Core
However there are some pretty bad well known tooling issues and VS reports a lot of errors, the compiler shows that it builds with no errors so that is what is true, but it makes it harder to know whether the code is correct at any given time unless you re-compile.
ie you can only trust the errors in the build output at the moment.
The tooling issues are temporary and Microsoft is working to solve them

I chose C# for the main web app and am putting the F# code into class libraries, because there is no F# support for Razor. So it is better to use C# for the main app in case you want to also use MVC with Razor Views. At the moment I'm only using api controllers and html
