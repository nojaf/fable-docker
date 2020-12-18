# Fable 2.6 Docker image sample

>docker run -it --rm -v "${PWD}:/app" -w "/app" -p "8080:8080"  nojaf/fable:2.6 bash

Inside the container:

>dotnet tool restore

>npm install

>npm start