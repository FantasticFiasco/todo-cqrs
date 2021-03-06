import { expect } from 'chai';

import { condition, MutationApiModel, QueryApiModel, Todo } from './api-models';

describe('queries', () => {
  const url = 'http://localhost:8080/graphql';
  const mutation = new MutationApiModel(url);
  const query = new QueryApiModel(url);

  beforeEach('given an empty todo list', async () => {
    const todos = await query.getAll();

    for (const todo of todos) {
      await mutation.remove(todo.id);
    }
  });

  it('should get one item', async () => {
    const title = 'Buy cheese';
    const id = await mutation.add(title);
    await condition(async () => await query.getAll().then((result) => result.length === 1));

    const actual = await query.get(id);

    const expected = new Todo(id, title, false);
    expect(actual).to.deep.equal(expected);
  });

  it('should get all items', async () => {
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
});

function sleep(ms: number) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}
