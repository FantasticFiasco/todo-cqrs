FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY app-sql/event-store-sql/EventStore.Sql.csproj ./app-sql/event-store-sql/
COPY app-sql/todo-web/Todo.Web.csproj ./app-sql/todo-web/
COPY shared/cqrs/Cqrs.csproj ./shared/cqrs/
COPY shared/todo/Todo.csproj ./shared/todo/
COPY shared/todo-events/Todo.Events.csproj ./shared/todo-events/
COPY shared/todo-readmodel/Todo.ReadModel.csproj ./shared/todo-readmodel/
COPY shared/todo-web-graphql/Todo.Web.GraphQL.csproj ./shared/todo-web-graphql/
RUN dotnet restore ./app-sql/todo-web/Todo.Web.csproj

COPY . ./
RUN dotnet publish -c Release -o out ./app-sql/todo-web/Todo.Web.csproj

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/app-sql/todo-web/out .
ENTRYPOINT ["dotnet", "Todo.Web.dll"]
