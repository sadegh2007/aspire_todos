import TodoModel from "../models/TodoModel.ts";
import {TodoListItem} from "./TodoListItem.tsx";

type TodosListProps = {
    todos: TodoModel[],
    onRemoved: (todo: TodoModel) => void
    onUpdated: (todo: TodoModel) => void
}

const TodosList = (props: TodosListProps) => {
    return (
        <table className='table table-striped'>
            <tbody>
            {
                props.todos.map(todo => <TodoListItem onUpdated={props.onUpdated} key={todo.id} todo={todo} onRemoved={props.onRemoved}/>)
            }
            </tbody>
        </table>
    )
}

export default TodosList;