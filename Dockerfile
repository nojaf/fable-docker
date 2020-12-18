FROM mcr.microsoft.com/dotnet/sdk:5.0

# NodeJS 14.X
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash
RUN apt-get install -y nodejs

# Yarn
RUN npm install -g yarn