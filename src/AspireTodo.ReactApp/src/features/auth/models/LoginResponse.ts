import {AuthUserModel} from "./AuthUserModel";

interface LoginResponse {
    accessToken: string;
    tokenType: string;
    refreshToken: string;
    expires: number;
    user:  AuthUserModel
}

export default LoginResponse;