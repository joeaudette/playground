## Webpack Sample

This sample has cloudscribe Core and Simple Content using NoDb with pre-configured data.
You can login with admin@admin.com password is admin

This sample has webpack setup with typescript and with bootstrap-sass and it has webpack hot module reloading.

For example you can edit the sass such as the _variables.scss file in the scss folder with the site runnning and just refresh the page to see your changes.

You can also edit the CleintApp1/Main.ts file which has a simple greeter, you can change the greeting and refresh the page and it appears immediately.

## Vanilla Typescript app sample

The typescript client app is hosted in a simplecontent page and is already in the menu.


## React/Redux/Typescript sample

There is also a react app also hosted in a simplecontent page, already in the menu when you run the app.

## Fable Sample

There is also a Fable app example which allows using F# for client side development that is then compiled to javascript.
This one is also already in the menu but the sample will get a 404 for the js unless you compile the F# into js by following these steps.

This entry is commented out in the webpack.config.js file because you have to run the Fable daemon for it work.
If you want to try it uncommenting it, you first need to do a couple of things.

1 open a command window in the root of the solution and run the command:

    `./.paket/paket.exe install` then: `dotnet restore --no-cache`
	
2 cd into the FableApp fodler and then run the command:

    `dotnet fable start`
	
now you can uncomment the line in webpack.config.js for the entry: 'fable-app': '../FableApp/FableApp.fsproj',

### Why Fable 

As you may know the javascript world is a bit of a wild wild west, javaqscript the language has good parts and bad parts and it requires a lot of knowledge to avoid the bad parts. Typescript is a language that aims to improve javascript but it doesn't eliminate the bad parts of javascript.

F# on the other hand is a great functional first language with no bad parts so it offers an opportunity to completely avoid writing client side code in javascript. 
Your F# code will get compiled into javascript but you can write in F#.

If you have been wanting to learn F#, using it for client side development presents and opportunity to ease into it without going all in on full stack F#.

I also forsee that web assembly is going to change the web development world in the near future. I expect that we will see whole new toolchains that allow us to write client side code in C# and F# and the code will compile to web assembly instead of javascript.
When that happens, existing javascript code will begin to be considered legacy code and I think most people will move away from using the javascript language for web development. So my thinking is that if I can start writing client side code in F# today, then perhaps much of that code will still be useful later when I can compile it to webassmble instead of compiling it to javascript with Fable.
