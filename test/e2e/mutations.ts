import { expect } from 'chai';

import { MutationApiModel, QueryApiModel, Todo } from './api-models';

describe('mutations', () => {

  const url = 'http://localhost:5000/graphql';
  const mutation = new MutationApiModel(url);
  const query = new QueryApiModel(url);

  beforeEach('given an empty todo list', async () => {
    const todos = await query.getAll();

    for (const todo of todos) {
      await mutation.remove(todo.id);
    }
  });

  it('should add item given empty list', async () => {
    const title = 'Buy cheese';

    const id = await mutation.add(title);

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

    const actual = await query.getAll();
    const expected = [
      new Todo(secondId, secondTitle, false),
      new Todo(firstId, firstTitle, false),
    ];
    expect(actual).to.deep.equal(expected);
  });

  it('should complete item given two items in the list', async () => {
    const firstTitle = 'Buy cheese';
    const secondTitle = 'Wash the car';
    const firstId = await mutation.add(firstTitle);
    const secondId = await mutation.add(secondTitle);

    await mutation.complete(firstId);

    const actual = await query.getAll();
    const expected = [
      new Todo(secondId, secondTitle, false),
      new Todo(firstId, firstTitle, true),
    ];
    expect(actual).to.deep.equal(expected);
  });

  it('should incomplete item given two items in the list', async () => {
    const firstTitle = 'Buy cheese';
    const secondTitle = 'Wash the car';
    const firstId = await mutation.add(firstTitle);
    const secondId = await mutation.add(secondTitle);
    await mutation.complete(firstId);

    await mutation.incomplete(firstId);

    const actual = await query.getAll();
    const expected = [
      new Todo(secondId, secondTitle, false),
      new Todo(firstId, firstTitle, false),
    ];
    expect(actual).to.deep.equal(expected);
  });

  it('should remove incomplete item', async () => {
    const title = 'Buy cheese';
    const id = await mutation.add(title);

    await mutation.remove(id);

    const actual = await query.getAll();
    const expected = [];
    expect(actual).to.deep.equal(expected);
  });

  it('should remove completed item', async () => {
    const title = 'Buy cheese';
    const id = await mutation.add(title);
    await mutation.complete(id);

    await mutation.remove(id);

    const actual = await query.getAll();
    const expected = [];
    expect(actual).to.deep.equal(expected);
  });

  it('should rename item', async () => {
    const title = 'Buy cheese';
    const id = await mutation.add(title);

    const newTitle = 'Apply for 6-month tax extension';
    await mutation.rename(id, newTitle);

    const actual = await query.getAll();
    const expected = [
      new Todo(id, newTitle, false),
    ];
    expect(actual).to.deep.equal(expected);
  });

});
