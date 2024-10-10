import {ApiRequest} from "../../../services/ApiService.ts";
import LoginResponse from "../models/LoginResponse.ts";
import LoginRequest from "../requests/LoginRequest.ts";
import RequestMethod from "../../../common/enums/RequestMethod.ts";
import RegisterRequest from "../requests/RegisterRequest.ts";

export const LoginApiRequest = async (request: LoginRequest): Promise<LoginResponse> => {
    return await ApiRequest('/users/api/account/login', RequestMethod.POST, request)
}

export const RegisterApiRequest = async (request: RegisterRequest): Promise<LoginResponse> => {
    return await ApiRequest('/users/api/account/register', RequestMethod.POST, request)
}