'use strict';

const path = require('path');

const bundleFolder = "./wwwroot/assets/";

module.exports = {
    mode: 'development',
    entry: [
        "./ReactApp/index.js"
    ],
    devtool: "source-map",
    output: {
        filename: "bundle.js",
        publicPath: 'assets/',
        path: path.resolve(__dirname, bundleFolder)
    },
    module: {
        rules: [
            {
                test: /\.jsx?$/,
                exclude: /(node_modules)/,
                loader: "babel-loader",
                query: {
                    presets: ["@babel/preset-env", "@babel/preset-react"]
                }
            }
        ]
    },
    plugins: [
    ]
};