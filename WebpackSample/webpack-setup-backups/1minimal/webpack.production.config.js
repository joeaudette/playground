const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const merge = require('webpack-merge');
const webPackStrip = require('strip-loader');
const devConfig = require('./webpack.config.js');
const isProduction = true;

const stripLoader = {
    test: [/\.js$/],
    exclude: /node_modules/,
    loader: webPackStrip.loader('console.log')
}

var devFiltered = devConfig;
devFiltered.plugins = []; //remove dev plugins

const overrides = {
    output: {
        filename: '[name].bundle.min.js',
    },
    module: {
        loaders: [stripLoader],
    },
    plugins: [
        new ExtractTextPlugin({
            filename: '[name].bundle.min.css',
            allChunks: true,
        }),
        new webpack.LoaderOptionsPlugin({
            minimize: true
        }),
        new webpack.optimize.UglifyJsPlugin(),
    ]
      
};

const merged = merge(devFiltered, overrides);

module.exports = merged;
