import LoginResponse from "../features/auth/models/LoginResponse.ts";

export const GetAccessToken = (): string|undefined => {
  return GetLoginData()?.accessToken;
}

export const SetLoginData = (data: LoginResponse) => {
  localStorage.setItem(GenKeyName("login-data"), JSON.stringify(data));
}

export const GetLoginData = (): LoginResponse|undefined => {
  const data = localStorage.getItem(GenKeyName("login-data"));
  
  if (!data) {
      return undefined;
  }
  
  return JSON.parse(data);
}

const GenKeyName = (key: string) => {
  return `app-${key}`;
}