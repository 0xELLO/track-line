import { AxiosError } from "axios";
import { Cookies } from "react-cookie";
import { IApiResponse } from "../domain/IApiResponse";
import { IJwtResponse } from "../domain/IJwtResponce";
import ILogin from "../domain/ILogin";
import { IRestApiErrorResponse } from "../domain/IRestApiErrorResponse";
import httpClient from "./HttpClient";

const cookies = new Cookies();
// TODO: axios error to api error
export class IdentityService{

    async refreshToken(): Promise<any>{
        let jwt = cookies.get("jwt");
        let refreshToken = cookies.get("refreshToken");
        
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

    async login(email: string, password: string): Promise<IApiResponse>{
        try{
            let loginInfo = {
                email,
                password
            } as ILogin
            
            let response = await httpClient.post('/identity/Account/LogIn', loginInfo);
            let resData = response.data as IJwtResponse;

            cookies.set("jwt", resData.token, {secure: true, sameSite: 'none'});
            cookies.set("refreshToken", resData.refreshToken, {secure: true, sameSite: 'none'});

            return {
                status: response.status,
                data: resData
            } as IApiResponse

        } catch (e) {
            let restApiError  = ((e as AxiosError).response?.data as IRestApiErrorResponse);
            let response = {
                status: (e as AxiosError).response!.status,
                data: restApiError
            }
            return response;
        }
    }
}
