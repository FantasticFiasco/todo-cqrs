# TodoCQRS - TodoMVC using CQRS and event sourcing

[![Build Status](https://dev.azure.com/fantasticfiasco/Todo%20CQRS/_apis/build/status/FantasticFiasco.todo-cqrs)](https://dev.azure.com/fantasticfiasco/Todo%20CQRS/_build/latest?definitionId=2)

## Table of contents

- [Introduction](#introduction)
- [What you will end up with](#what-you-will-end-up-with)
- [Acceptance criteria](#acceptance-criteria)
- [Implementations](#implementations)

## Introduction

This repository contains various implementations meeting the acceptance criteria of [TodoMVC](http://todomvc.com/), built using CQRS and event sourcing. It starts with a very basic in-memory event store, but then gradually becomes more complex with increasing requirements.

These are the implementations, ordered according to complexity:

1. [Single process using in-memory event store](#single-process-using-in-memory-event-store)
1. [Single process using SQL event store](#single-process-using-sql-event-store)

## What you will end up with

All implementations will expose the same GraphQL playground on [http://localhost:8080/ui/playground](http://localhost:8080/ui/playground). Using the playground you will be able to query and mutate state using GraphQL, as shown below.

![alt text](./doc/resources/create-todo.png "Create todo")

![alt text](./doc/resources/get-todos.png "Create todo")

## Acceptance criteria

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

## Implementations

Before running any of the implementations, please make sure [Docker](https://www.docker.com/community-edition#/download) and [Docker Compose](https://docs.docker.com/compose/install) are installed.

### Single process using in-memory event store

#### Requirements

- State does not need to be durable, we can live with having it being lost if the application is terminated

#### Solution

The code needed to fulfill the requirements can be found in `TodoCQRS.InMemory.sln`. It contains a very basic in-memory event store that holds all published events. The read model is also held in memory, in the same process as the event store.

Run the following command in the root of the repository to start the application.

```bash
$ docker-compose -f ./docker-compose.app-in-memory.yml up
```

### Single process using SQL event store

#### Requirements

- State must be durable, we must retain state even if application is terminated

#### Solution

The code needed to fulfill the requirements can be found in `TodoCQRS.Sql.sln`. It has replaced the in-memory event store with one that persists events in a PostgreSQL database, thus living up to the requirements of being durable. The read model is still being held in memory, thus if the application is terminated, all evens will have to be replayed to get the current state of the application.

Run the following command in the root of the repository to start the application.

```bash
$ docker-compose -f ./docker-compose.app-sql.yml up
```
