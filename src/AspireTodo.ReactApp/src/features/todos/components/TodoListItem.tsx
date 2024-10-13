import TodoModel from "../models/TodoModel.ts";
import {useState} from "react";
import {MarkTodoAsCompletedApiRequest, RemoveTodoApiRequest} from "../services/TodoService.ts";
import {catchError} from "../../../services/ErrorHandlers.ts";
import TodoForm from "./TodoForm.tsx";
import AnimateTableRowItem from "../../../components/AnimateTableRowItem.tsx";

type TodoListItemProps = {
    todo: TodoModel,
    onRemoved: (todo: TodoModel) => void,
    onUpdated: (todo: TodoModel) => void
}

export const TodoListItem = ({todo, onRemoved, onUpdated}: TodoListItemProps) => {
    const [isComplete, setIsComplete] = useState<boolean>(todo.isCompleted);
    const [loading, setLoading] = useState<boolean>(false);
    const [inEditMode, setInEditMode] = useState<boolean>(false);

    const checkCompleted = async () => {
        setLoading(true);

        try {
            await MarkTodoAsCompletedApiRequest(todo.id, {isCompleted: !todo.isCompleted});
            setIsComplete(!todo.isCompleted);
            todo.isCompleted = !todo.isCompleted;
        } catch (error) {
            catchError(error);
        }

        setLoading(false);
    }

    const onRemove = async () => {
        setLoading(true);

        try {
            if (confirm('Are you sure you want to remove this item?')) {
                await RemoveTodoApiRequest(todo.id);
                onRemoved(todo);
            }
        } catch (e) {
            catchError(e)
            setLoading(false);
        }
    }

    const onUpdatedTodo = (editedTodo: TodoModel) => {
        setInEditMode(false);
        onUpdated(editedTodo);
    }

    return (
        <AnimateTableRowItem className='flex items-center' key={todo.id}>
                {
                    !inEditMode &&
                    <input
                        disabled={loading || inEditMode}
                        type="radio"
                        className="radio focus:outline-0"
                        onClick={() => checkCompleted()}
                        defaultChecked={isComplete}
                    />
                }

                <div className={`${!inEditMode ? 'mx-4' : ''} flex flex-col w-full`}>
                    {
                        inEditMode
                            ? <TodoForm onCancelEdit={() => setInEditMode(false)} isEdit todo={todo}
                                        onUpdated={onUpdatedTodo}/>
                            :
                            <>
                                <h2 className={`font-bold text-nowrap ${isComplete ? 'completed' : ''}`}>{todo.title}</h2>
                                {
                                    todo.summery && <small>{todo.summery}</small>
                                }
                            </>
                    }
                </div>
                {
                    inEditMode ? <></> : <DefaultActionButtons loading={loading} onEdit={() => setInEditMode(true)}
                                                               onRemove={onRemove}/>
                }
        </AnimateTableRowItem>
    )
}

const DefaultActionButtons =
    ({loading, onRemove, onEdit}: { loading: boolean, onRemove: () => Promise<void>, onEdit: () => void }) => {
        return (
            <div className="flex gap-1">
                <button onClick={onRemove} disabled={loading}
                        className='focus:outline-0 btn btn-square btn-sm btn-error'>
                    {
                        loading ? <span className='loading loading-sm loading-spinner'></span> :
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"
                                 fill="none"
                                 stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"
                                 className="icon icon-tabler icons-tabler-outline icon-tabler-trash">
                                <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                                <path d="M4 7l16 0"/>
                                <path d="M10 11l0 6"/>
                                <path d="M14 11l0 6"/>
                                <path d="M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12"/>
                                <path d="M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3"/>
                            </svg>
                    }
                </button>
                <button onClick={onEdit} disabled={loading}
                        className='focus:outline-0 btn btn-square btn-sm btn-success'>
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none"
                         stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"
                         className="icon icon-tabler icons-tabler-outline icon-tabler-pencil">
                        <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                        <path d="M4 20h4l10.5 -10.5a2.828 2.828 0 1 0 -4 -4l-10.5 10.5v4"/>
                        <path d="M13.5 6.5l4 4"/>
                    </svg>
                </button>
            </div>
        )
    }