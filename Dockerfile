FROM mcr.microsoft.com/dotnet/core/sdk:3.0

RUN dotnet tool install -g Paket
RUN dotnet tool install -g fake-cli

# Workaround for https://github.com/dotnet/cli/issues/9321
ENV PATH="/root/.dotnet/tools:${PATH}"

# NodeJS 10.X
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash
RUN apt-get install -y nodejs

# Yarn
RUN npm install -g yarn