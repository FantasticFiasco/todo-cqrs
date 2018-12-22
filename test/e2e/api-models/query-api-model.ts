import { request } from 'graphql-request';
import { Variables } from 'graphql-request/dist/src/types';

import { Todo } from './todo';

export class QueryApiModel {
  constructor(private readonly url: string) {
  }

  public async get(id: string): Promise<Todo> {
    const query = `query wrapper($id: String!) {
      todo(id: $id) {
        id,
        title,
        isCompleted
      }
    }`;

    const variables: Variables = {
      id,
    };

    const response = await request<IGetResponse>(this.url, query, variables);
    return response.todo;
  }

  public async getAll(): Promise<Todo[]> {
    const query = `{
      todos {
        id,
        title,
        isCompleted
      }
    }`;

    const response = await request<IGetAllResponse>(this.url, query);
    return response.todos.sort((a, b) => a.title.localeCompare(b.title));
  }
}

interface IGetResponse {
  todo: Todo;
}

interface IGetAllResponse {
  todos: Todo[];
}
