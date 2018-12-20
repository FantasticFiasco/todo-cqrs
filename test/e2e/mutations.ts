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

  it('empty list can have item added', async () => {
    const title = 'Buy cheese';

    await mutation.add(title);

    const actual = await query.getAll();
    expect(actual).to.have.length(1);
    const x = actual[0].id;
    expect(x).length.to.be.greaterThan(0);
  });

});
