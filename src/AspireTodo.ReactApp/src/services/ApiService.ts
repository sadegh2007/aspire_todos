import RequestMethod from "../common/enums/RequestMethod.ts";
import axios, {AxiosHeaders} from "axios";
import {GetAccessToken} from "./StorageService.ts";

export const ApiRequest = async <T>(path: string, method: RequestMethod = RequestMethod.GET, body?: Record<string, any> | FormData): Promise<T> => {

    const headers = new AxiosHeaders({
        "Content-Type": "application/json",
        "Accept": "application/json",
    });
    
    const token = GetAccessToken();
    if (token) {
        headers.setAuthorization("Bearer " + token);
    }

    const { data } = await axios.request({
        method,
        baseURL: import.meta.env.VITE_API_BASE_PATH,
        url: path,
        data: method != RequestMethod.GET ? body : null,
        params: method == RequestMethod.GET ? body : null,
        headers
    });

    return data;
}