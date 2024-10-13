import TodoForm from "../components/TodoForm.tsx";
import TodosList from "../components/TodosList.tsx";
import {useEffect, useState} from "react";
import TodoModel from "../models/TodoModel.ts";
import {catchError, notify} from "../../../services/ErrorHandlers.ts";
import {TodosListApiRequest} from "../services/TodoService.ts";
import * as signalR from "@microsoft/signalr";
import {GetAccessToken} from "../../../services/StorageService.ts";

const HomePage = () => {
    const [todos, setTodos] = useState<TodoModel[]>([]);
    const [loading, setLoading] = useState(true);
    const [connection, setConnection] = useState<signalR.HubConnection|undefined>();

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(() => {
                    // console.log('Connected!');
                    connection.on('ReceiveMessage', (data: {  type: string, data?: any  }) => {
                        console.log(data)
                        if (data.type == 'TodoCreated') {
                            notify("Todo created successfully", "success");
                            setTodos(prev => ([data.data.todo, ...prev]));
                        }
                        if (data.type == 'FailedUserUpdateTodosCount') {
                            notify(
                                data.data.message ?? "There is problem to add todo, please try again.",
                                "error"
                            );
                        }
                        if (data.type == 'TodoRemoved') {
                            setTodos(prev => (prev.filter(todo => todo.id != data.data.todoId)));
                            
                            notify(
                                "Todo Removed Successfully.",
                                "success"
                            );
                        }
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }

        return () => {}
    }, [connection]);

    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${import.meta.env.VITE_API_BASE_PATH}/notifications/todosHub`, {
                accessTokenFactory(): string | Promise<string> {
                    return GetAccessToken()!;
                },
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets,
            })
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
        
        fetchData();
    }, []);
    
    const onCreated = (_todo: TodoModel) => {
        // startTransition(() => {
        //     setTodos(prev => ([todo, ...prev]));
        // })
    }

    const onUpdated = (todo: TodoModel) => {
        const newTodos = todos.map(t => {
            if (t.id == todo.id) return todo;
            return t;
        });
        
        setTodos(newTodos);
    }
    
    const onRemoved = (_todo: TodoModel) => {
        // startTransition(() => {
        //     setTodos(prev => (prev.filter(x => x.id != todo.id)));
        // })
    }

    const fetchData = async () => {
        setLoading(true);

        try {
            const result = await TodosListApiRequest({});
            setTodos(result.data);
        } catch (error) {
            catchError(error)
        }

        setLoading(false);
    }

    return <div className='w-screen h-screen flex flex-col items-center justify-center'>
        <div className="bg-base-300 w-[90%] lg:w-[40%] rounded-xl p-5">
            <h1 className='text-2xl font-bold text-center mb-4'>Todo App</h1>
            <div className="divider"></div>
            <TodoForm onCreated={onCreated} />
            {
                loading 
                    ? <div className='w-full mt-3 text-center'><span className='loading loading-spinner'/></div> 
                    : <TodosList onUpdated={onUpdated} onRemoved={onRemoved} todos={todos}/>
            }
            {/*<div className="divider"></div>*/}
            {/*<p className='mt-0 text-gray-600 font-bold text-sm'>Count: { totalCount }</p>*/}
        </div>
    </div>
}

export default HomePage;