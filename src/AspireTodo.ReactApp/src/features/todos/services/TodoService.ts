import PaginateResponse from "../../../common/PaginateResponse.ts";
import TodoModel from "../models/TodoModel.ts";
import {ApiRequest} from "../../../services/ApiService.ts";
import RequestMethod from "../../../common/enums/RequestMethod.ts";
import UpsertTodoRequest from "../requests/UpsertTodoRequest.ts";

export const TodosListApiRequest = async ({ page = 1, pageSize = 10 }: { page?: number, pageSize?: number }): Promise<PaginateResponse<TodoModel>> => {
    return await ApiRequest('/todos/api/todos', RequestMethod.GET, {
        page,
        pageSize,
    })
}

export const CreateTodoApiRequest = async (request: UpsertTodoRequest): Promise<TodoModel> => {
    return await ApiRequest('/todos/api/todos', RequestMethod.POST, request);
}

export const UpdateTodoApiRequest = async (id: number, request: UpsertTodoRequest): Promise<TodoModel> => {
    return await ApiRequest(`/todos/api/todos/${id}`, RequestMethod.PUT, request);
}

export const MarkTodoAsCompletedApiRequest = async (id: number, request: { isCompleted: boolean }): Promise<TodoModel> => {
    return await ApiRequest(`/todos/api/todos/${id}/completed`, RequestMethod.PUT, request);
}

export const RemoveTodoApiRequest = async (id: number): Promise<TodoModel> => {
    return await ApiRequest(`/todos/api/todos/${id}`, RequestMethod.DELETE);
}