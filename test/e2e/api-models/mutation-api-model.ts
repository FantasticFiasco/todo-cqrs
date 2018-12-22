import { request } from 'graphql-request';
import { Variables } from 'graphql-request/dist/src/types';

export class MutationApiModel {
  constructor(private readonly url: string) {
  }

  public async add(title: string): Promise<string> {
    const mutation = `mutation wrapper($title: String!)  {
      addTodo(title: $title) {
        id
      }
    }`;

    const variables: Variables = {
      title,
    };

    const response: IAddResponse = await request(this.url, mutation, variables);
    return response.addTodo.id;
  }

  public async complete(id: string): Promise<void> {
    const mutation = `mutation wrapper($id: String!) {
      completeTodo(id: $id) {
        id
      }
    }`;

    const variables: Variables = {
      id,
    };

    await request(this.url, mutation, variables);
  }

  public async incomplete(id: string): Promise<void> {
    const mutation = `mutation wrapper($id: String!) {
      incompleteTodo(id: $id) {
        id
      }
    }`;

    const variables: Variables = {
      id,
    };

    await request(this.url, mutation, variables);
  }

  public async rename(id: string, newTitle: string): Promise<void> {
    const mutation = `mutation wrapper($id: String!, $newTitle: String!) {
      renameTodo(id: $id, newTitle: $newTitle) {
        id
      }
    }`;

    const variables: Variables = {
      id,
      newTitle,
    };

    await request(this.url, mutation, variables);
  }

  public async remove(id: string): Promise<void> {
    const mutation = `mutation wrapper($id: String!) {
      removeTodo(id: $id) {
        id
      }
    }`;

    const variables: Variables = {
      id,
    };

    await request(this.url, mutation, variables);
  }
}

interface IAddResponse {
  addTodo: {
    id: string;
  };
}
