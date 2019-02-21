'use strict';

const path = require('path');
const webpack = require('webpack');
const bundleFolder = "./wwwroot/assets/";

module.exports = (_, argv = { mode: 'development' }) => ({
    mode: argv.mode,
    entry: [
        "./ReactApp/index.jsx"
    ],
    devtool: (argv.mode === 'production' ? false : "source-map"),
    output: {
        filename: "bundle.js",
        publicPath: 'assets/',
        path: path.resolve(__dirname, bundleFolder)
    },
    plugins: [
        new webpack.DefinePlugin({
            NODE_ENV: JSON.stringify(argv.mode || 'development')
        })
    ],
    performance: {
        maxEntrypointSize: 512000,
        maxAssetSize: 512000
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
    resolve: { extensions: ['.js', '.jsx'] },
});
