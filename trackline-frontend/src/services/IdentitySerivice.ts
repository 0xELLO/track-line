import { AxiosError } from "axios";
import { IJwtResponse } from "../domain/IJwtResponce";
import httpClient from "./HttpClient";

// TODO: axios error to api error

export class IdentityService{

    async refreshToken(): Promise<any>{
        let jwt = localStorage.getItem("jwt");
        let refreshToken = localStorage.getItem("refreshToken");
        
        try{
            let response = await httpClient.post('/identity/Account/refreshtoken', {
                jwt: jwt,
                refreshToken: refreshToken
            });

            let resData = response.data as IJwtResponse; ;

            localStorage.setItem("jwt", resData.token);
            localStorage.setItem("refreshToken", resData.refreshToken);
            return {
                status: response.status,
                data: response.data
            }
        }
        catch (e) {
            let restApiError  = ((e as AxiosError).response?.data);
            let response = {
                status: (e as AxiosError).response!.status,
            }
            return response;
        }
    }

    async register(email: string, password: string): Promise<any>{
        localStorage.removeItem("jwt");
        localStorage.removeItem("refreshToken");
        try{
            let loginInfo = {
                email,
                password
            }

            let response = await httpClient.post('/identity/Account/Register', loginInfo);

            let resData = response.data as IJwtResponse;
            localStorage.setItem("jwt", resData.token);
            localStorage.setItem("refreshToken", resData.refreshToken);

            return {
                status: response.status,
                data: resData
            }

        } catch (e) {
            let restApiError  = ((e as AxiosError).response?.data);
            let response = {
                status: (e as AxiosError).response!.status,
            }
            return response;
        }
    }

    async login(email: string, password: string): Promise<any>{
        localStorage.removeItem("jwt");
        localStorage.removeItem("refreshToken");
        try{
            let loginInfo = {
                email,
                password
            }

            let response = await httpClient.post('/identity/Account/Register', loginInfo);

            let resData = response.data as IJwtResponse;
            localStorage.setItem("jwt", resData.token);
            localStorage.setItem("refreshToken", resData.refreshToken);

            return {
                status: response.status,
                data: resData
            }

        } catch (e) {
            let restApiError  = ((e as AxiosError).response?.data);
            let response = {
                status: (e as AxiosError).response!.status,
            }
            return response;
        }
    }
}
