import {useForm} from "react-hook-form";
import { z } from 'zod'
import { zodResolver } from '@hookform/resolvers/zod'
import {catchError} from "../../../services/ErrorHandlers.ts";
import {useEffect, useState} from "react";
import {LoginApiRequest} from "../services/AccountService.ts";
import LoginRequest from "../requests/LoginRequest.ts";
import {GetAccessToken, SetLoginData} from "../../../services/StorageService.ts";
import {useNavigate} from "react-router-dom";

const LoginSchema = z.object({
    phoneNumber: z.string().min(11),
    password: z.string().min(6)
});

type LoginSchemaType = z.infer<typeof LoginSchema>;

const LoginPage = () => {
    const [loading, setLoading] = useState<boolean>(false);
    
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<LoginSchemaType>({
        resolver: zodResolver(LoginSchema),
    });
    
    const navigate = useNavigate();

    useEffect(() => {
        if (GetAccessToken()) {
            navigate('/', { replace: true });
        }
    }, []);
    
    const onSubmit = async (values: LoginRequest) => {
        setLoading(true);
        try {
            const data = await LoginApiRequest(values);
            SetLoginData(data);
            
            navigate('/', { replace: true });
        }
        catch (e) {
            catchError(e);
        }
        setLoading(false);
    }
    
    return <div className='w-screen h-screen flex justify-center items-center'>
        <div>
            <form onSubmit={handleSubmit(onSubmit)} className='bg-base-300 w-96 rounded-xl p-5'>
                <label className="form-control w-full">
                    <div className="label">
                        <span className="label-text">Phone Number</span>
                    </div>
                    <input
                        {...register('phoneNumber')}
                        type="text" 
                        placeholder="Phone Number"
                        className={`focus:outline-0 input input-bordered ${errors.phoneNumber ? 'input-error' : ''} w-full`}/>
                    {
                        errors.phoneNumber &&
                        <div className="label">
                            <span className="label-text-alt">{ errors.phoneNumber.message }</span>
                        </div>
                    }
                </label>

                <label className="form-control w-full">
                    <div className="label">
                        <span className="label-text">Password</span>
                    </div>
                    <input
                        {...register('password')}    
                        type="text" 
                        placeholder="Password"
                        className={`focus:outline-0 input input-bordered ${errors.password ? 'input-error' : ''} w-full`}/>
                    {
                        errors.password &&
                        <div className="label">
                            <span className="label-text-alt">{ errors.password.message }</span>
                        </div>
                    }
                </label>
                
                <button disabled={loading} className='mt-4 btn btn-primary w-full block' >
                    {loading && <span className='loading loading-spinner'></span>}
                    Login
                </button>
            </form>
        </div>
    </div>
}

export default LoginPage;