# spa-stack

This is just a little proof of concept to help me learn F# and [polymer js](http://polymer-project.org). It should be useful as a reference for anyone interested in learning to do ASP.NET Core development with F#, especially anyone coming from a C# background. I'm just starting to learn F# and functional programming but I am very interested in it and want to try to use it as much as possible in my future projects, so I wanted to create a sample project to illustrate basic concepts in a working reference implementation so I can refer to it later when applying the same approach in my real projects.

## Points of Interest Demonstrated in the back end for this Project

** F# and C# code with equivalent functionality. ASP.NET Core Web API, models, and data access, working examples of a To Do list back end in both languages providing a nice commparison for C# developers to see both a C# and an F# implementation.

** F# and C# interoperability - the main web app is C# but mainly serves as a compostion root, with both C# and F# class libraries providing the actual functionality.

** I chose C# for the main web app and am putting the F# code into class libraries, because there is no F# support for Razor. So it is better to use C# for the main app in case you want to also use MVC with Razor Views. At the moment I'm only using api controllers and html but I'm cosidering adding some MVC parts later to illustrate a hybrid approach to building websites with parts of a site implemented with MVC but linking to SPA style "apps" hosted together. I tend to think that content centric sites are better implemented with MVC whereas web "apps" are better with a SPA approach. I don't like it when the "content" of a site cannot be read at all unless javascript is enabled. When I do research on the web I protect myself with the uMatix plugin for FireFox which blocks scripts and other things unless you specifically enable it for a domain. So I tend to land on new sites with javascript disabled and it annoys me if I have to enable it to read content. I digress, but that is why I like server side MVC better for content.

## F# Tooling Gotchas you need to know about

As you may know the current project system with .xproj and project.json is temporary and will be going away in a future tooling update from Microsoft. The best ideas from the current project system are going to roll back into the .csproj project system and I assume also the .fsproj project type for F#. The current tooling has a lot of issues especially in regard to working with F#.

It is currently possible to develop in F# for ASP.NET Core but there are some pain points you need to know about, all of which will hopefully be resolved when the new tooling is available.

** Visual Studio 2015 will report numerous F# errors that are not true both in the error list and in intellisense. You must ignore those completely. The only way to know about the true errors is to build the project. You can only trust the errors reported in the build output. This is tedious because you have to build to check your code for errors you cannot rely on the tooling.

** When you add an F# .fs file to a project, you have to manually add it to the list in project.json to have the file included in the compiled assembly. Example:

    "buildOptions": {
        "debugType": "portable",
        "compilerName": "fsc",
        "compile": {
            "includeFiles": [
                "Controllers.fs"
            ]
        }
    }
	
** When you make a change in the code of the class library projects, you have to rebuild the whole solution before you see the changes in reflected in the application UI, changes in the class library ar enot automatically noticed in the main web app.

## Points of Interest Demonstrated in the Front end for this Project

I know all the buzz these days is about reactjs, angularjs, and aurelia, but myself I am more drawn towards web components and polymer with vanillajs. Web components are emerging web browser standards and therefore part of the web platform. It seems more future proof to learn and build things with web standards technology than to use the trendy js framework of the month or year.

So my front end stack of choice is web components and my back end stack is ASP.NET Core with C# and F#

Currently there is a basic polymer starter kit ui.  One page of the app has a ToDo list polymer component that talks to a web api written in C#. Another page has the same ToDo polymer component but points to a different version of the api implemented in F#.

I didn't write much of the current front end code demonstrated here, it is mostly borrowed from other samples. But, I plan to add more pages in the sample app to learn how to use and write polymer components, so there will likely be more bits and pieces illustrated later.


## Credits

Special thanks to [Ruben Bartelink](https://twitter.com/rbartelink) who helped a lot with the F# code. He helped me resolve things that I was having toruble figuring out the syntax for and he helped make the F# code more idiomatic to how an F# developer would implement things as opposed to my initial work towards translating the C# version into F#

[Fiyaz Bin Hasan](https://github.com/fiyazbinhasan) wrote the simple-todo polymer component and I based my initial work on his [sample for ASP.NET Core here](https://github.com/fiyazbinhasan/Polymer-With-ASP.NET-CORE-WEB-API). His example used Entity Framework. I refactored his C# code into something a little more abstracted, and I used my [NoDb project](https://github.com/joeaudette/NoDb) for storage instead of Entity Framework, then began work translating the code to F# with lots of help from Ruben Bartelink. I made a few tweaks in the simple-todo polymer component but it is essentially as he created it.
