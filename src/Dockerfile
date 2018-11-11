FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish -c Release -o ./out ./todo-web/Todo.Web.csproj

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/todo-web/out .
ENTRYPOINT ["dotnet", "Todo.Web.dll"]
