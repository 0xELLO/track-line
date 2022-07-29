import { AxiosError } from "axios";
import { Cookies } from "react-cookie";
import { IHeadListItem } from "../../domain/IHeadListItem";
import httpClient from "../HttpClient";

const path = "/list/HeadList"
const cookies = new Cookies();

export default class HeadListService {

    async getHeadLists() : Promise<IHeadListItem[] | null>{
        try {
            let jwt = cookies.get("jwt")
            let  response = await httpClient.get(path + "/GetHeadLists", {
                headers: {
                    "Authorization": "bearer " + jwt
                }
            })
            console.log(response.data)
            return response.data as IHeadListItem[] 
        } catch(e) {
            console.log((e as AxiosError))
            console.log("error")
            return null
        }
    }

    async postHeadList(headListItem: IHeadListItem) : Promise<IHeadListItem | null>{
        try {
            let jwt = cookies.get("jwt")
            let  response = await httpClient.post(path + "/PostHEadList", headListItem, {
                headers: {
                    "Authorization": "bearer " + jwt
                }
            })
            return response.data as IHeadListItem
        } catch(e) {
            console.log((e as AxiosError))
            console.log("error")
            return null
        }
    }


}
