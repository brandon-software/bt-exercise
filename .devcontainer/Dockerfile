FROM mcr.microsoft.com/devcontainers/dotnet:1-8.0-bookworm

# [Optional] Uncomment this section to install additional OS packages.
# RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
#     && apt-get -y install --no-install-recommends <your-package-list-here>

# [Optional] Uncomment this line to install global node packages.
# RUN su vscode -c "source /usr/local/share/nvm/nvm.sh && npm install -g <your-package-here>" 2>&1


# install dev tools
RUN dotnet tool install -g dotnet-ef
RUN dotnet tool install -g dotnet-reportgenerator-globaltool

# RUN dotnet tool install -g dotnet-sonarscanner
# RUN dotnet tool install -g dotnet-stryker
# RUN dotnet tool install -g dotnet-stryker-sonarqube

EXPOSE 4200
