import TodoForm from "../components/TodoForm.tsx";
import TodosList from "../components/TodosList.tsx";
import {useEffect, useState} from "react";
import TodoModel from "../models/TodoModel.ts";
import {catchError} from "../../../services/ErrorHandlers.ts";
import {TodosListApiRequest} from "../services/TodoService.ts";

const HomePage = () => {
    const [todos, setTodos] = useState<TodoModel[]>([]);
    const [totalCount, setTotalCount] = useState<number>(0);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchData()
    }, []);
    
    const onCreated = (todo: TodoModel) => {
        setTodos(prev => ([todo, ...prev]));
    }

    const onUpdated = (todo: TodoModel) => {
        const newTodos = todos.map(t => {
            if (t.id == todo.id) return todo;
            return t;
        })
        
        setTodos(newTodos);
    }
    
    const onRemoved = (todo: TodoModel) => {
        setTodos(prev => (prev.filter(x => x.id != todo.id)));
        fetchData();
    }

    const fetchData = async () => {
        setLoading(true);

        try {
            const result = await TodosListApiRequest({});
            setTodos(result.data);
            setTotalCount(result.count)
        } catch (error) {
            catchError(error)
        }

        setLoading(false);
    }

    return <div className='w-screen h-screen flex items-center justify-center'>
        <div className="bg-base-300 w-[90%] lg:w-[40%] rounded-xl p-5">
            <TodoForm onCreated={onCreated} />
            <div className="divider py-4">Todos List</div>
            {
                loading 
                    ? <div className='w-full text-center'><span className='loading loading-spinner'/></div> 
                    : <TodosList onUpdated={onUpdated} onRemoved={onRemoved} todos={todos}/>
            }
            <div className="divider"></div>
            <p className='mt-0 text-gray-600 font-bold text-sm'>Count: { totalCount }</p>
        </div>
    </div>
}

export default HomePage;