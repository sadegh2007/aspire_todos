import {Outlet, useNavigate} from "react-router-dom";
import axios from "axios";
import {RemoveLoginData} from "../services/StorageService.ts";

const AppLayout = () => {
    const navigate = useNavigate();
    
    axios.interceptors.response.use(
        (res) => {
            return res;
        },
        (error) => {
            if (error.status === 401) {
                RemoveLoginData();
                return navigate('/login')
            }
            
            return Promise.reject(error);
        });
    
    return (
        <main>
            <Outlet />
        </main>
    )
}

export default AppLayout