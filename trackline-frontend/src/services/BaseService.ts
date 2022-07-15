import httpClient from "./HttpClient";

// TODO : handle errors

export class BaseService<TEntity> {

    constructor(private path: string) {
    }

    async getAll(): Promise<TEntity[]> {
        let jwt = localStorage.getItem("jwt");
        console.log(jwt)

        let response = await httpClient.get(`/${this.path}`, {
            headers: {
                "Authorization": "bearer " + jwt
            }
        });

        console.log(response);
        let res = response.data as TEntity[];
        return res;
    }

    async add(entity: TEntity): Promise<TEntity> {
        let jwt = localStorage.getItem("jwt");

        let response = await httpClient.post(`/${this.path}`, entity,
        {
            headers: {
                "Authorization": "bearer " + jwt
            }
        });

        let res = response.data as TEntity;
        return res;
    }

    async get(id : string): Promise<TEntity> {
        let jwt = localStorage.getItem("jwt");
        console.log(jwt)

        let response = await httpClient.get(`/${this.path}/${id}`, {
            headers: {
                "Authorization": "bearer " + jwt
            }
        });
        console.log(response);
        let res = response.data as TEntity;
        return res;
    }

}
