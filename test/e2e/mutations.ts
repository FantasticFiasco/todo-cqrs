import { expect } from 'chai';

import { condition, MutationApiModel, QueryApiModel, Todo } from './api-models';

describe('mutations', () => {

  const url = 'http://localhost:8080/graphql';
  const mutation = new MutationApiModel(url);
  const query = new QueryApiModel(url);

  beforeEach('given an empty todo list', async () => {
    const todos = await query.getAll();

    for (const todo of todos) {
      await mutation.remove(todo.id);
    }

    await condition(async () => await query.getAll().then((result) => result.length === 0));
  });

  it('should add item given empty list', async () => {
    const title = 'Buy cheese';

    const id = await mutation.add(title);

    await condition(async () => await query.getAll().then((result) => result.length === 1));
    const actual = await query.getAll();
    const expected = [
      new Todo(id, title, false),
    ];

    expect(actual).to.deep.equal(expected);
  });

  it('should add two items given empty list', async () => {
    const firstTitle = 'Buy cheese';
    const secondTitle = 'Wash the car';

    const firstId = await mutation.add(firstTitle);
    const secondId = await mutation.add(secondTitle);

    await condition(async () => await query.getAll().then((result) => result.length === 2));
    const actual = await query.getAll();
    const expected = [
      new Todo(firstId, firstTitle, false),
      new Todo(secondId, secondTitle, false),
    ];
    expect(actual).to.deep.equal(expected);
  });

  it('should complete item given two items in the list', async () => {
    const firstTitle = 'Buy cheese';
    const secondTitle = 'Wash the car';
    const firstId = await mutation.add(firstTitle);
    const secondId = await mutation.add(secondTitle);

    await mutation.complete(firstId);

    await condition(async () => await query.get(firstId).then((result) => result.isCompleted === true));
    const actual = await query.getAll();
    const expected = [
      new Todo(firstId, firstTitle, true),
      new Todo(secondId, secondTitle, false),
    ];
    expect(actual).to.deep.equal(expected);
  });

  it('should incomplete item given two items in the list', async () => {
    const firstTitle = 'Buy cheese';
    const secondTitle = 'Wash the car';
    const firstId = await mutation.add(firstTitle);
    const secondId = await mutation.add(secondTitle);
    await mutation.complete(firstId);
    await condition(async () => await query.get(firstId).then((result) => result.isCompleted === true));

    await mutation.incomplete(firstId);

    await condition(async () => await query.get(firstId).then((result) => result.isCompleted === false));
    const actual = await query.getAll();
    const expected = [
      new Todo(firstId, firstTitle, false),
      new Todo(secondId, secondTitle, false),
    ];
    expect(actual).to.deep.equal(expected);
  });

  it('should remove incomplete item', async () => {
    const title = 'Buy cheese';
    const id = await mutation.add(title);
    await condition(async () => await query.getAll().then((result) => result.length === 1));

    await mutation.remove(id);

    await condition(async () => await query.getAll().then((result) => result.length === 0));
    const actual = await query.getAll();
    const expected = [];
    expect(actual).to.deep.equal(expected);
  });

  it('should remove completed item', async () => {
    const title = 'Buy cheese';
    const id = await mutation.add(title);
    await mutation.complete(id);
    await condition(async () => await query.get(id).then((result) => result.isCompleted === true));

    await mutation.remove(id);

    await condition(async () => await query.getAll().then((result) => result.length === 0));
    const actual = await query.getAll();
    const expected = [];
    expect(actual).to.deep.equal(expected);
  });

  it('should rename item', async () => {
    const title = 'Buy cheese';
    const id = await mutation.add(title);
    await condition(async () => await query.getAll().then((result) => result.length === 1));

    const newTitle = 'Apply for 6-month tax extension';
    await mutation.rename(id, newTitle);

    await condition(async () => await query.get(id).then((result) => result.title === newTitle));
    const actual = await query.getAll();
    const expected = [
      new Todo(id, newTitle, false),
    ];
    expect(actual).to.deep.equal(expected);
  });

});

function t(): string {
  const today = new Date();
  return today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
}
