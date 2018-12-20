import { request } from 'graphql-request';

import { Todo } from './todo';

export class QueryApiModel {
  constructor(private readonly url: string) {
  }

  public async getAll(): Promise<Todo[]> {
    const query = `{
      todos {
        id,
        title
      }
    }`;

    const response: IGetAllResponse = await request(this.url, query);
    return response.todos;
  }
}

interface IGetAllResponse {
  todos: Todo[];
}
