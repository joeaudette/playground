﻿const path = require('path');
const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");

//const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;
const fableUtils = require("fable-utils");
var babelOptions = fableUtils.resolveBabelOptions({
    presets: [["es2015", { "modules": false }]],
    plugins: ["transform-runtime"]
});
const isProduction = false;


const config = {
    entry: {
        //'fable-app': '../FableApp/FableApp.fsproj',
        'app-react': './app-react/boot-client.tsx',
        'app-react-server': './app-react/boot-server.tsx',
        'clientapp1': ['./ClientApp1/Main.ts'],
        'mainstyle': './scss/style.scss'
    },
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, 'wwwroot/dist/'),
        publicPath: 'dist/'
    },
    module: {
        rules: [
            {
                test: /\.fs(x|proj)?$/,
                use: {
                    loader: "fable-loader",
                    options: {
                        babel: babelOptions,
                        define: isProduction ? [] : ["DEBUG"]
                    }
                }
            },
            {
                test: /\.css$/,
                use: ExtractTextPlugin.extract({
                    fallback: "style-loader",
                    use: "css-loader"
                }),
                exclude: /node_modules/
            },
            { 
                test: /\.(sass|scss)$/,
                use: ExtractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: ['css-loader', 'sass-loader']
                }),
                
                
            },
            //{
            //    test: /\.tsx?$/,
            //    include: /ClientApp/,
            //    use: 'awesome-typescript-loader?silent=true'
            //},
            {
                test: /\.tsx?$/,
                loader: 'ts-loader',
                exclude: /node_modules/,
            },
        ]
    },
    resolve: {
        modules: [path.join(__dirname,"./node_modules/")],
        extensions: [".tsx", ".ts", ".js", ".scss"]
    },
    devtool: 'source-map',
    plugins: [
        new webpack.HotModuleReplacementPlugin(),
        new webpack.NamedModulesPlugin(),
        new ExtractTextPlugin({
            filename: '[name].bundle.css',
            allChunks: true,
        }),
       
       
    ]
};

module.exports = config;