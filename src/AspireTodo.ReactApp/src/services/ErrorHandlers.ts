import {toast} from "react-toastify";
import {AxiosError} from "axios";


export const notify = (message: string, type: 'info' | 'success' | 'warning' | 'error' | 'default') => {
    toast(message, {
        type: type,
        theme: 'colored',
        hideProgressBar: true,
        className: 'text-sm rounded-xl'
    });
}

export const catchError = (err: any) => {
    const errors = err as AxiosError;
    handleError(errors.response);
}

// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
export const handleArrayErrors = (errors) => {
    Object.keys(errors).map((key) => {
        notify(errors[key][0], 'error');
        return true
    });
}

// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
export const handleError = (response, ) => {
    // const type =  response && response.type;
    const errors =  response && response.errors;
    const status =  response ? response.status : 500;
    const data =  response && response.data;

    if (data && data.detail) {
        notify(data.detail, 'error');
        return false;
    }

    if (Array.isArray(data)) {
        data.forEach((e) => {
            if (e.errorMessage) {
                notify(e.errorMessage, 'error');
            }
        })
        return false;
    }

    if (data && data.errors) {
        handleArrayErrors(data.errors);
        return false;
    }

    if (status === 404) {
        notify('Not Found.', 'error');
        return false;
    }

    if (status === 500 && data && data.detail) {
        notify(data.detail, 'error');
        return false;
    }

    // for server errors - 500
    if (status === 500) {
        notify('SERVER ERROR', 'error');
        return false;
    }

    // to handle form errors that come from server
    if (errors) {
        handleArrayErrors(errors);
        return false;
    }
}