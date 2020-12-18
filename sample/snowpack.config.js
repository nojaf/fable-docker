module.exports = {
    mount: {
        public: "/",
        src: "/_dist_"
    },
    plugins: [
        ["@snowpack/plugin-run-script", {
            "cmd": "dotnet fable ./src/App.fsproj",
            "watch": "dotnet fable watch ./src/App.fsproj"
        }]
    ],
    devOptions: {
        port:8080,
        hostname: "0.0.0.0",
        output: "stream"
    },
    install: ["react-dom"]
};