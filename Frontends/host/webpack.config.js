const HtmlWebPackPlugin = require("html-webpack-plugin");
const ModuleFederationPlugin = require("webpack/lib/container/ModuleFederationPlugin");
const { VueLoaderPlugin } = require("vue-loader");
const Dotenv = require("dotenv-webpack");
module.exports = (_, argv) => ({
	output: {
		publicPath: "http://localhost:8080/",
	},

	resolve: {
		extensions: [".tsx", ".ts", ".vue", ".jsx", ".js", ".json"],
	},

	devServer: {
		port: 8080,
		historyApiFallback: true,
	},

	module: {
		rules: [
			{
				test: /\.vue$/,
				loader: "vue-loader",
			},
			{
				test: /\.tsx?$/,
				use: [
					"babel-loader",
					{
						loader: "ts-loader",
						options: {
							transpileOnly: true,
							appendTsSuffixTo: ["\\.vue$"],
							happyPackMode: true,
						},
					},
				],
			},
			{
				test: /\.(css|s[ac]ss)$/i,
				use: ["style-loader", "css-loader", "postcss-loader"],
			},
		],
	},

	plugins: [
		new VueLoaderPlugin(),
		new ModuleFederationPlugin({
			name: "host",
			filename: "remoteEntry.js",
			remotes: {
				maintenance_frontend:
					"maintenance_frontend@http://localhost:8081/remoteEntry.js",
				tracking_frontend:
					"tracking_frontend@http://localhost:8082/remoteEntry.js",
				assets_frontend:
					"assets_frontend@http://localhost:8083/remoteEntry.js",
			},
			exposes: {},
			shared: require("./package.json").dependencies,
		}),
		new HtmlWebPackPlugin({
			template: "./src/index.html",
		}),
		new Dotenv(),
	],
});
