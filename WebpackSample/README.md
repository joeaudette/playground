## Webpack Sample

This sample has cloudscribe Core and Simple Content using NoDb with pre-configured data.
You can login with admin@admin.com password is admin

There are a couple of great things about this sample. It shows how to use webpack for client development, with hot module reloading, and using the sass version of bootstrap which is great for doing custom bootstrap themes. You can more easily customize the colors, and you can leave out parts of bootstrap that you are not using to reduce the amount of css. I pretty much do all my client proejcts with bootstrap-sass. Best of all, I think this sample shows how you can easily integrate SPA (Single Page App) style apps into a larger MVC web site. When you need to have multiple apps all tied together in one web property this provides a great approach. SPA style apps usually have just a little markup for an app element like and empty div, and the some javascript that loads the app into the app element. In SimpleContent you can disable the CKeditor, which makes it easier to put in your app markup because CKeditor doesn't let you put in an empty div. Then the developer tools for SimpleContent pages allow you to add any needed scripts and css. There are already several sample SPA apps setup that you can see just by running the WebApp.

This sample has webpack setup with typescript and with bootstrap-sass and it has webpack hot module reloading.

For example you can edit the sass such as changing colors in the _variables.scss file in the scss folder with the site runnning and just refresh the page to see your changes.

You can also edit the CleintApp1/Main.ts file which has a simple greeter, you can change the greeting and refresh the page and it appears immediately due to the hot module reloading. Similarly you can edit any of the typescript code in the react sample and refresh the page to see the changes immediately. Webpack hot module reloading rocks!

## Vanilla Typescript app sample

There is a bassic hellow world vanilla typescript client app. It is hosted in a simplecontent page and is already in the menu.


## React/Redux/Typescript sample

There is also a react/redux/typescript app also hosted in a simplecontent page, already in the menu when you run the app.

## Fable Sample

There is also a Fable app example which allows using F# for client side development that is then compiled to javascript.
This one is also already in the menu but the sample will get a 404 for the js unless you compile the F# into js by following these steps.

This entry is commented out in the webpack.config.js file because you have to run the Fable daemon for it work.
If you want to try it uncommenting it, you first need to do a couple of things.

1 open a command window in the root of the solution and run the command:

    ./.paket/paket.exe install then: dotnet restore --no-cache
	
2 cd into the FableApp folder and then run the command:

    dotnet fable start
	
now you can uncomment the line in webpack.config.js for the entry: 'fable-app': '../FableApp/FableApp.fsproj',

### Why Fable 

As you may know the javascript world is a bit of a wild wild west, javascript the language has good parts and bad parts and it requires a lot of knowledge to avoid the bad parts. Typescript is a language that aims to improve javascript but it doesn't eliminate the bad parts of javascript.

F# on the other hand is a great functional first language with no bad parts so it offers an opportunity to completely avoid writing client side code in javascript.
Your F# code will get compiled into javascript but you can write in F#.

If you have been wanting to learn F#, using it for client side development presents an opportunity to ease into it without going all in on full stack F#.

I also forsee that web assembly is going to change the web development world in the near future. I expect that we will see whole new tool chains that allow us to write client side code in C# and F# and the code will compile to web assembly instead of javascript.
When that happens, existing javascript code will begin to be considered legacy code and I think most people will move away from using the javascript language for web development. So my thinking is that if I can start writing client side code in F# today, then perhaps much of that code will still be useful later when I can compile it to webassmble instead of compiling it to javascript with Fable.

Note that paket is a package manager used widely in the F# community.
