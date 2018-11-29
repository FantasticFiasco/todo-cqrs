# TodoCQRS - TodoMVC using CQRS and event sourcing

[![Build Status](https://dev.azure.com/fantasticfiasco/Todo%20CQRS/_apis/build/status/FantasticFiasco.todo-cqrs)](https://dev.azure.com/fantasticfiasco/Todo%20CQRS/_build/latest?definitionId=2)

## Table of contents

- [Introduction](#introduction)
- [What you will end up with](#what-you-will-end-up-with)
- [Implementations](#implementations)
- [TodoMVC acceptance criteria](#todomvc-acceptance-criteria)

## Introduction

This repository contain various implementations meeting the acceptance criteria of [TodoMVC](http://todomvc.com/), built using CQRS and event sourcing. It starts with a very basic in-memory event store, but then gradually becomes more complex with increasing requirements.

These are the implementations, ordered according to complexity:

1. [Single process using a in-memory event store](#single-process-using-a-in-memory-event-store)
1. [Single process using a SQL event store](#single-process-using-a-sql-event-store)

## What you will end up with

All implementations will expose the GraphQL playground on [http://localhost:8080/ui/playground](http://localhost:8080/ui/playground). Using the playground you will be able to execute GraphQL mutations to modify the state, and GraphQL queries to view the state, as demonstrated below.

![alt text](./doc/resources/create-todo.png "Create todo")

![alt text](./doc/resources/get-todos.png "Create todo")


## Implementations

Before running any of the implementations, please make sure [Docker](https://www.docker.com/community-edition#/download) and [Docker Compose](https://docs.docker.com/compose/install) are installed.

### Single process using a in-memory event store

The solution `TodoCQRS.InMemory.sln` in the root of the repository contains the C# code needed to meet the requirements.

#### Acceptance criteria

- State does not need to be durable, we can live with having to rebuild it if the application is terminated

#### Running the application

Run the following command in the root of the repository:

```bash
$ docker-compose -f .\docker-compose.app-in-memory.yml up
```

### Single process using a SQL event store

The solution `TodoCQRS.Sql.sln` in the root of the repository contains the C# code needed to meet the requirements.

#### Acceptance criteria

- State must be durable, we must retain state even if application is terminated

#### Running the application

Run the following command in the root of the repository:

```bash
$ docker-compose -f .\docker-compose.app-sql.yml up
```

## TodoMVC acceptance criteria

### Empty list can have item added

Given an empty Todo list<br/>
When I add a Todo for 'Buy cheese'<br/>
Then only that item is listed

### Empty list can have two items added

Given an empty Todo list<br/>
When I add Todos for 'Buy cheese' & 'Wash the car'<br/>
Then only those items are listed

### Item completion changes the list

Given a Todo list with items 'Buy cheese' & 'Wash the car'<br/>
When the first item is marked as complete<br/>
Then only those items are listed<br/>
And only the second item is listed as active

### Item incompletion changes the list

Given a Todo list with items 'Buy cheese' & 'Wash the car'<br/>
And the first item is completed<br/>
When the first item is marked as incomplete<br/>
Then only those items are listed<br/>
And both items are listed as active

### Incomplete items can be removed

Given a Todo list with a single item 'Buy cheese'<br/>
When the item is removed<br/>
Then nothing is listed

### Complete items can be removed

Given a Todo list with a single item 'Buy cheese'<br/>
And the item is completed<br/>
When the item is removed<br/>
Then nothing is listed

### Editing can change the text of an item

Given a Todo list with a single item 'Buy cheese'<br/>
When the item changed to 'Apply for 6-month tax extension'<br/>
Then only the revised item is listed
