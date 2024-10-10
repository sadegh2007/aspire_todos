import React from "react";
import {Navigate} from "react-router-dom";
import {GetAccessToken} from "../services/StorageService";

type Props = {
    children: React.ReactNode,
}

const ProtectedRoute = ({children}: Props) => {
    const login = GetAccessToken();

    if (!login) {
        return <Navigate to='/login' />
    }

    return children;
}

export default ProtectedRoute;