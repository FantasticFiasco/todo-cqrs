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

    await mutation.add(title);

    const actual = await query.getAll();
    expect(actual).to.have.length(1);
    expect(actual[0]).to.have.property('id');
    expect(actual[0].title).to.equal(title);
    expect(actual[0].isCompleted).to.equal(false);
  });

  it('should add two items given empty list', async () => {
    const firstTitle = 'Buy cheese';
    const secondTitle = 'Wash the car';

    await mutation.add(firstTitle);
    await mutation.add(secondTitle);

    const actual = await query.getAll();
    expect(actual).to.have.length(2);
    expect(actual[1]).to.have.property('id');
    expect(actual[1].title).to.equal(firstTitle);
    expect(actual[1].isCompleted).to.equal(false);
    expect(actual[0]).to.have.property('id');
    expect(actual[0].title).to.equal(secondTitle);
    expect(actual[0].isCompleted).to.equal(false);
  });

});
