import { request } from 'graphql-request';

export class MutationApiModel {
  constructor(private readonly url: string) {
  }

  public async add(title: string): Promise<void> {
    const mutation = `mutation {
      addTodo(title: "${title}") {
        id,
        title
      }
    }`;

    return await request(this.url, mutation);
  }

  public async remove(id: string): Promise<void> {
    const mutation = `mutation {
      removeTodo(id: "${id}") {
        id
      }
    }`;

    return await request(this.url, mutation);
  }
}
