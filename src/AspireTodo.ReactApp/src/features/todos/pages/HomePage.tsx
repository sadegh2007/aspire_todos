import TodoForm from "../components/TodoForm.tsx";
import TodosList from "../components/TodosList.tsx";
import {startTransition, useEffect, useState} from "react";
import TodoModel from "../models/TodoModel.ts";
import {catchError} from "../../../services/ErrorHandlers.ts";
import {TodosListApiRequest} from "../services/TodoService.ts";

const HomePage = () => {
    const [todos, setTodos] = useState<TodoModel[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchData()
    }, []);
    
    const onCreated = (todo: TodoModel) => {
        startTransition(() => {
            setTodos(prev => ([todo, ...prev]));
        })
    }

    const onUpdated = (todo: TodoModel) => {
        const newTodos = todos.map(t => {
            if (t.id == todo.id) return todo;
            return t;
        });
        
        setTodos(newTodos);
    }
    
    const onRemoved = (todo: TodoModel) => {
        startTransition(() => {
            setTodos(prev => (prev.filter(x => x.id != todo.id)));
        })
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

    return <div className='w-screen h-screen flex items-center justify-center'>
        <div className="bg-base-300 w-[90%] lg:w-[40%] rounded-xl p-5">
            <TodoForm onCreated={onCreated} />
            <div className="divider text-gray-400 font-bold py-4">Todos List</div>
            {
                loading 
                    ? <div className='w-full text-center'><span className='loading loading-spinner'/></div> 
                    : <TodosList onUpdated={onUpdated} onRemoved={onRemoved} todos={todos}/>
            }
            {/*<div className="divider"></div>*/}
            {/*<p className='mt-0 text-gray-600 font-bold text-sm'>Count: { totalCount }</p>*/}
        </div>
    </div>
}

export default HomePage;