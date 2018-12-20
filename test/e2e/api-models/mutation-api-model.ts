import { request } from 'graphql-request';

import { Todo } from './todo';

export class MutationApiModel {
  constructor(private readonly url: string) {
  }

  public async add(title: string): Promise<string> {
    const mutation = `mutation {
      addTodo(title: "${title}") {
        id
      }
    }`;

    const response: IAddResponse = await request(this.url, mutation);
    return response.addTodo.id;
  }

  public async complete(id: string): Promise<void> {
    const mutation = `mutation {
      completeTodo(id: "${id}") {
        id
      }
    }`;

    await request(this.url, mutation);
  }

  public async remove(id: string): Promise<void> {
    const mutation = `mutation {
      removeTodo(id: "${id}") {
        id
      }
    }`;

    await request(this.url, mutation);
  }
}

interface IAddResponse {
  addTodo: {
    id: string;
  };
}
